using BrowserLib;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser {
	/// <summary>
	/// ブラウザを表示するフォームです。
	/// </summary>
	/// <remarks>thx KanColleViewer!</remarks>
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	public partial class FormBrowser : Form, IBrowser {
		// FormBrowserHostの通信サーバ
		private string ServerUri;

		// FormBrowserの通信サーバ
		private PipeCommunicator<IBrowserHost> BrowserHost;

		private BrowserLib.BrowserConfiguration Configuration;

		// 親プロセスが生きているか定期的に確認するためのタイマー
		private Timer HeartbeatTimer = new Timer();
		private IntPtr HostWindow;

		private readonly Size KanColleSize = new Size( 800, 480 );

		private bool _styleSheetApplied;
		/// <summary>
		/// スタイルシートの変更が適用されているか
		/// </summary>
		private bool StyleSheetApplied {
			get { return _styleSheetApplied; }
			set {

				if ( value ) {
					//Browser.Anchor = AnchorStyles.None;
					ApplyZoom();
					SizeAdjuster_SizeChanged( null, new EventArgs() );

				} else {
					SizeAdjuster.SuspendLayout();
					//Browser.Anchor = AnchorStyles.Top | AnchorStyles.Left;
					Browser.Location = new Point( 0, 0 );
					Browser.MinimumSize = new Size( 0, 0 );
					Browser.Size = SizeAdjuster.Size;
					SizeAdjuster.ResumeLayout();
				}

				_styleSheetApplied = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="serverUri">ホストプロセスとの通信用URL</param>
		public FormBrowser( string serverUri ) {
			InitializeComponent();

			ServerUri = serverUri;
			StyleSheetApplied = false;
		}

		[DllImport( "user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true )]
		private static extern uint GetWindowLong( IntPtr hwnd, int nIndex );

		[DllImport( "user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true )]
		private static extern uint SetWindowLong( IntPtr hwnd, int nIndex, uint dwNewLong );

		private const int GWL_STYLE = ( -16 );
		private const uint WS_CHILD = 0x40000000;
		private const uint WS_VISIBLE = 0x10000000;

		private void FormBrowser_Load( object sender, EventArgs e ) {
			SetWindowLong( this.Handle, GWL_STYLE, WS_CHILD );

			// ホストプロセスに接続
			BrowserHost = new PipeCommunicator<IBrowserHost>(
				this, typeof( IBrowser ), ServerUri + "Browser", "Browser" );
			BrowserHost.Connect( ServerUri + "/BrowserHost" );
			BrowserHost.Faulted += BrowserHostChannel_Faulted;

			Configuration = BrowserHost.Proxy.Configuration;

			// キーメッセージの送り先はFormBrowserHost
			// FormBrowserHostを別ウィンドウに切り離すとショートカットキーが効かなくなる
			// メインウィンドウに送れば切り離してても効くようになると思うが、
			// 元からそういう仕様だったから変更していない
			Application.AddMessageFilter( new KeyMessageGrabber( BrowserHost.Proxy.HWND ) );

			// ウィンドウの親子設定＆ホストプロセスから接続してもらう
			BrowserHost.Proxy.ConnectToBrowser( this.Handle );

			// 親ウィンドウが生きているか確認 
			HeartbeatTimer.Tick += (EventHandler)(( sender2, e2 ) => {
				BrowserHost.AsyncRemoteRun( () => { HostWindow = BrowserHost.Proxy.HWND; } );
			});
			HeartbeatTimer.Interval = 2000; // 2秒ごと　
			HeartbeatTimer.Start();
		}

		void BrowserHostChannel_Faulted( Exception e ) {
			// 親と通信できなくなったら終了する
			if ( !BrowserHost.Closed ) {
				BrowserHost.Close();
				Application.Exit();
			}
		}

		private void FormBrowser_FormClosed( object sender, FormClosedEventArgs e ) {
			BrowserHost.Close();
		}

		public void ConfigurationChanged( BrowserLib.BrowserConfiguration conf ) {
			Configuration = conf;
			SizeAdjuster.AutoScroll = Configuration.IsScrollable;
			ApplyZoom();
		}

		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		public void InitialAPIReceived() {
			ApplyZoom();
		}


		private void SizeAdjuster_SizeChanged( object sender, EventArgs e ) {

			if ( !StyleSheetApplied ) {
				Browser.Location = new Point( 0, 0 );
				Browser.Size = SizeAdjuster.Size;
				return;
			}

			/*/
			Utility.Logger.Add( 1, string.Format( "SizeChanged: BR ({0},{1}) {2}x{3}, PA {4}x{5}, CL {6}x{7}",
				Browser.Location.X, Browser.Location.Y, Browser.Width, Browser.Height, SizeAdjuster.Width, SizeAdjuster.Height, ClientSize.Width, ClientSize.Height ) );
			//*/

			//スタイルシート適用時はセンタリング
			CenteringBrowser();
		}

		private void CenteringBrowser() {
			int x = Browser.Location.X, y = Browser.Location.Y;
			bool isScrollable = Configuration.IsScrollable;

			if ( !isScrollable || Browser.Width <= SizeAdjuster.Width ) {
				x = ( SizeAdjuster.Width - Browser.Width ) / 2;
			}
			if ( !isScrollable || Browser.Height <= SizeAdjuster.Height ) {
				y = ( SizeAdjuster.Height - Browser.Height ) / 2;
			}

			//if ( x != Browser.Location.X || y != Browser.Location.Y )
			Browser.Location = new Point( x, y );
		}

		private void Browser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {

			StyleSheetApplied = false;
			ApplyStyleSheet();

		}

		/// <summary>
		/// スタイルシートを適用します。
		/// </summary>
		private void ApplyStyleSheet() {

			try {

				var document = Browser.Document;
				if ( document == null ) return;

				var gameframe = document.GetElementById( "game_frame" );
				if ( gameframe == null ) {
					if ( document.Url.AbsolutePath.Contains( ".swf?" ) )
						gameframe = document.Body;
				}

				if ( gameframe == null ) return;


				var target = gameframe.Document;

				if ( target != null ) {
					mshtml.IHTMLStyleSheet ss = ( (mshtml.IHTMLDocument2)target.DomDocument ).createStyleSheet( "", 0 );

					ss.cssText = Configuration.StyleSheet;


					StyleSheetApplied = true;
				}



			} catch ( Exception ex ) {

				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.SendErrorReport( ex.ToString(), "スタイルシートの適用に失敗しました。" ) );
			}

		}

		/// <summary>
		/// 指定した URL のページを開きます。
		/// </summary>
		public void Navigate( string url ) {
			StyleSheetApplied = false;
			Browser.Navigate( url );
		}

		/// <summary>
		/// ブラウザを再読み込みします。
		/// </summary>
		public void RefreshBrowser() {
			Browser.Refresh( WebBrowserRefreshOption.Completely );
		}

		/// <summary>
		/// ズームを適用します。
		/// </summary>
		public void ApplyZoom() {
			ApplyZoom( Configuration.ZoomRate );
		}

		/// <summary>
		/// ズームを適用します。
		/// </summary>
		/// <param name="zoomRate">拡大率。%指定で 10-1000</param>
		public void ApplyZoom( int zoomRate ) {

			try {

				if ( zoomRate < 10 )
					zoomRate = 10;
				if ( zoomRate > 1000 )
					zoomRate = 1000;

				var wb = Browser.ActiveXInstance as SHDocVw.IWebBrowser2;
				if ( wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED ) return;

				// 読み込み中は例外を吐くので避ける
				if ( wb.Busy ) return;

				object pin = zoomRate;
				object pout = null;

				wb.ExecWB( SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout );

				if ( StyleSheetApplied ) {
					Browser.Size = Browser.MinimumSize = new Size( (int)( KanColleSize.Width * zoomRate / 100.0 ), (int)( KanColleSize.Height * zoomRate / 100.0 ) );
					CenteringBrowser();
				}

			} catch ( Exception ex ) {
				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.AddLog( 3, "ズームの適用に失敗しました。" + ex.Message ));
			}

		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		public void SaveScreenShot( string path, int screenShotFormat, string timestamp ) {
			if ( !System.IO.Directory.Exists( path ) ) {
				System.IO.Directory.CreateDirectory( path );
			}

			string ext;
			System.Drawing.Imaging.ImageFormat format;

			switch ( screenShotFormat ) {
				case 1:
					ext = "jpg";
					format = System.Drawing.Imaging.ImageFormat.Jpeg;
					break;
				case 2:
				default:
					ext = "png";
					format = System.Drawing.Imaging.ImageFormat.Png;
					break;
			}


			SaveScreenShot( string.Format( "{0}\\{1}.{2}",
				path,
				timestamp,
				ext ), format );

		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		/// <param name="path">保存先。</param>
		/// <param name="format">画像のフォーマット。</param>
		private void SaveScreenShot( string path, System.Drawing.Imaging.ImageFormat format ) {

			var wb = Browser;

			try {

				var document = wb.Document.DomDocument as HTMLDocument;
				if ( document == null ) {
					throw new InvalidOperationException( "Document が取得できませんでした。" );
				}


				IViewObject viewobj = null;
				int width = 0, height = 0;


				if ( document.url.Contains( ".swf?" ) ) {

					viewobj = document.getElementsByTagName( "embed" ).item( 0, 0 ) as IViewObject;
					if ( viewobj == null ) {
						throw new InvalidOperationException( "embed 要素の取得に失敗しました。" );
					}

					width = ( (HTMLEmbed)viewobj ).clientWidth;
					height = ( (HTMLEmbed)viewobj ).clientHeight;

				} else {

					var gameFrame = document.getElementById( "game_frame" ).document as HTMLDocument;
					if ( gameFrame == null ) {
						throw new InvalidOperationException( "game_frame 要素の取得に失敗しました。" );
					}

					bool foundflag = false;

					for ( int i = 0; i < document.frames.length; i++ ) {

						var provider = document.frames.item( i ) as IServiceProvider;
						if ( provider == null ) continue;

						object ppvobj;
						provider.QueryService( typeof( SHDocVw.IWebBrowserApp ).GUID, typeof( SHDocVw.IWebBrowser2 ).GUID, out ppvobj );

						var _wb = ppvobj as SHDocVw.IWebBrowser2;
						if ( _wb == null ) continue;

						var iframe = _wb.Document as HTMLDocument;
						if ( iframe == null ) continue;


						var swf = iframe.getElementById( "externalswf" );
						if ( swf == null ) continue;

						Func<dynamic, bool> isvalid = target => {

							if ( target == null ) return false;
							viewobj = target as IViewObject;
							if ( viewobj == null ) return false;
							width = int.Parse( target.width );
							height = int.Parse( target.height );
							return true;
						};

						if ( !isvalid( swf as HTMLEmbed ) && !isvalid( swf as HTMLObjectElement ) )
							continue;

						foundflag = true;

						break;
					}


					if ( !foundflag ) {
						throw new InvalidOperationException( "対象の swf が見つかりませんでした。" );
					}
				}


				if ( viewobj != null ) {

					using ( var image = new Bitmap( width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb ) ) {


						var rect = new RECT { left = 0, top = 0, width = width, height = height };
						var device = new DVTARGETDEVICE { tdSize = 0 };

						using ( var g = Graphics.FromImage( image ) ) {
							var hdc = g.GetHdc();
							viewobj.Draw( 1, 0, IntPtr.Zero, device, IntPtr.Zero, hdc, rect, null, IntPtr.Zero, IntPtr.Zero );
							g.ReleaseHdc( hdc );
						}

						image.Save( path, format );
					}

				}


				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.AddLog( 2, string.Format( "スクリーンショットを {0} に保存しました。", path ) ) );

			} catch ( Exception ex ) {

				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.SendErrorReport( ex.ToString(), "スクリーンショットの保存時にエラーが発生しました。" ) );
			}


		}

		public void SetProxy( int port ) {
			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}", port ), "<local>" );
		}
	}

	/// <summary>
	/// キーボードメッセージを親ウィンドウに届ける
	/// 別プロセスの子ウィンドウにフォーカスがあるとキーボードショートカットが効かなくなるため、
	/// キー関連のメッセージのコピーをホスト側に送る
	/// </summary>
	internal class KeyMessageGrabber : IMessageFilter {
		private IntPtr TargetWnd;

		[DllImport( "user32.dll" )]
		private static extern bool PostMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_SYSKEYDOWN = 0x0104;
		private const int WM_SYSKEYUP = 0x0105;

		public KeyMessageGrabber( IntPtr targetWnd ) {
			TargetWnd = targetWnd;
		}

		public bool PreFilterMessage( ref Message m ) {
			switch ( m.Msg ) {
				case WM_KEYDOWN:
				case WM_KEYUP:
				case WM_SYSKEYDOWN:
				case WM_SYSKEYUP:
					PostMessage( TargetWnd, m.Msg, m.WParam, m.LParam );
					break;
			}
			return false;
		}
	}

	#region struct

	[ComImport(), Guid( "0000010d-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	internal interface IViewObject {
		[PreserveSig]
		int Draw(
			[In, MarshalAs( UnmanagedType.U4 )] int dwDrawAspect,
			int lindex,
			IntPtr pvAspect,
			[In] DVTARGETDEVICE ptd,
			IntPtr hdcTargetDev,
			IntPtr hdcDraw,
			[In] RECT lprcBounds,
			[In] RECT lprcWBounds,
			IntPtr pfnContinue,
			[In] IntPtr dwContinue );
	}

	[StructLayout( LayoutKind.Sequential )]
	internal class DVTARGETDEVICE {
		public ushort tdSize;
		public uint tdDeviceNameOffset;
		public ushort tdDriverNameOffset;
		public ushort tdExtDevmodeOffset;
		public ushort tdPortNameOffset;
		public byte tdData;
	}

	[StructLayout( LayoutKind.Sequential )]
	internal class RECT {
		public int left;
		public int top;
		public int width;
		public int height;
	}

	[ComImport, Guid( "6d5140c1-7436-11ce-8034-00aa006009fa" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), ComVisible( false )]
	internal interface IServiceProvider {
		[return: MarshalAs( UnmanagedType.I4 )]
		[PreserveSig]
		int QueryService( ref Guid guidService, ref Guid riid, [MarshalAs( UnmanagedType.Interface )] out object ppvObject );
	}

	#endregion
}
