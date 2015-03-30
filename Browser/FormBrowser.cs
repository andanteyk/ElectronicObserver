﻿using BrowserLib;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

		private readonly Size KanColleSize = new Size( 800, 480 );



		// FormBrowserHostの通信サーバ
		private string ServerUri;

		// FormBrowserの通信サーバ
		private PipeCommunicator<IBrowserHost> BrowserHost;

		private BrowserLib.BrowserConfiguration Configuration;

		// 親プロセスが生きているか定期的に確認するためのタイマー
		private Timer HeartbeatTimer = new Timer();
		private IntPtr HostWindow;

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
		/// 艦これが読み込まれているかどうか
		/// </summary>
		private bool IsKanColleLoaded { get; set; }

		private VolumeManager _volumeManager;



		/// <summary>
		/// </summary>
		/// <param name="serverUri">ホストプロセスとの通信用URL</param>
		public FormBrowser( string serverUri ) {
			InitializeComponent();

			ServerUri = serverUri;
			StyleSheetApplied = false;
			_volumeManager = new VolumeManager( (uint)System.Diagnostics.Process.GetCurrentProcess().Id );
			Browser.ReplacedKeyDown += Browser_ReplacedKeyDown;
		}

		
		private void FormBrowser_Load( object sender, EventArgs e ) {
			SetWindowLong( this.Handle, GWL_STYLE, WS_CHILD );

			// ホストプロセスに接続
			BrowserHost = new PipeCommunicator<IBrowserHost>(
				this, typeof( IBrowser ), ServerUri + "Browser", "Browser" );
			BrowserHost.Connect( ServerUri + "/BrowserHost" );
			BrowserHost.Faulted += BrowserHostChannel_Faulted;


			ConfigurationChanged( BrowserHost.Proxy.Configuration );


			// ウィンドウの親子設定＆ホストプロセスから接続してもらう
			BrowserHost.Proxy.ConnectToBrowser( this.Handle );

			// 親ウィンドウが生きているか確認 
			HeartbeatTimer.Tick += (EventHandler)( ( sender2, e2 ) => {
				BrowserHost.AsyncRemoteRun( () => { HostWindow = BrowserHost.Proxy.HWND; } );
			} );
			HeartbeatTimer.Interval = 2000; // 2秒ごと　
			HeartbeatTimer.Start();


			BrowserHost.AsyncRemoteRun( () => BrowserHost.Proxy.GetIconResource() );
		}

		void Exit() {
			if ( !BrowserHost.Closed ) {
				BrowserHost.Close();
				HeartbeatTimer.Stop();
				Application.Exit();
			}
		}

		void BrowserHostChannel_Faulted( Exception e ) {
			// 親と通信できなくなったら終了する
			Exit();
		}

		public void CloseBrowser() {
			HeartbeatTimer.Stop();
			// リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
			BeginInvoke( (Action)( () => Exit() ) );
		}

		public void ConfigurationChanged( BrowserLib.BrowserConfiguration conf ) {
			Configuration = conf;

			SizeAdjuster.AutoScroll = Configuration.IsScrollable;
			ApplyZoom();
			ToolMenu_Other_AppliesStyleSheet.Checked = Configuration.AppliesStyleSheet;
			ToolMenu.Dock = (DockStyle)Configuration.ToolMenuDockStyle;
			ToolMenu.Visible = Configuration.IsToolMenuVisible;

		}

		private void ConfigurationUpdated() {
			BrowserHost.AsyncRemoteRun( () => BrowserHost.Proxy.ConfigurationUpdated( Configuration ) );
		}

		private void AddLog( int priority, string message ) {
			BrowserHost.AsyncRemoteRun( () => BrowserHost.Proxy.AddLog( priority, message ) );
		}


		public void InitialAPIReceived() {

			IsKanColleLoaded = true;

			//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
			ApplyStyleSheet();
			ApplyZoom();

			//起動直後はまだ音声が鳴っていないのでミュートできないため、この時点で有効化
			SetMuteIcon();
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


		private void Browser_Navigating( object sender, WebBrowserNavigatingEventArgs e ) {

			IsKanColleLoaded = false;

		}

		private void Browser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {

			StyleSheetApplied = false;
			ApplyStyleSheet();

			ApplyZoom();
		}

		/// <summary>
		/// スタイルシートを適用します。
		/// </summary>
		public void ApplyStyleSheet() {

			if ( !Configuration.AppliesStyleSheet )
				return;

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
				if ( wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED || wb.Busy ) return;

				object pin = zoomRate;
				object pout = null;

				wb.ExecWB( SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout );

				if ( StyleSheetApplied ) {
					Browser.Size = Browser.MinimumSize = new Size( (int)( KanColleSize.Width * zoomRate / 100.0 ), (int)( KanColleSize.Height * zoomRate / 100.0 ) );
					CenteringBrowser();
				}

				ToolMenu_Other_Zoom_Current.Text = string.Format( "現在: {0}%", zoomRate );

			} catch ( Exception ex ) {
				AddLog( 3, "ズームの適用に失敗しました。" + ex.Message );
			}

		}



		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		/// <param name="folderPath">保存するフォルダへのパス。</param>
		/// <param name="screenShotFormat">スクリーンショットのフォーマット。1=jpg, 2=png</param>
		public void SaveScreenShot( string folderPath, int screenShotFormat ) {
			if ( !System.IO.Directory.Exists( folderPath ) ) {
				System.IO.Directory.CreateDirectory( folderPath );
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


			SaveScreenShot( string.Format(
				"{0}\\{1:yyyyMMdd_HHmmssff}.{2}",
				folderPath,
				DateTime.Now,
				ext ), format );

		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		/// <param name="path">保存先。</param>
		/// <param name="format">画像のフォーマット。</param>
		private void SaveScreenShot( string path, System.Drawing.Imaging.ImageFormat format ) {

			var wb = Browser;

			if ( !IsKanColleLoaded ) {
				AddLog( 3, string.Format( "艦これが読み込まれていないため、スクリーンショットを撮ることはできません。" ) );
				return;
			}

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
							if ( !int.TryParse( target.width, out width ) ) return false;
							if ( !int.TryParse( target.height, out height ) ) return false;
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


				AddLog( 2, string.Format( "スクリーンショットを {0} に保存しました。", path ) );

			} catch ( Exception ex ) {

				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.SendErrorReport( ex.ToString(), "スクリーンショットの保存時にエラーが発生しました。" ) );
			}


		}


		public void SetProxy( int port ) {
			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}", port ), "<local>" );
		}


		public void SetIconResource( byte[] canvas ) {

			string[] keys = new string[] {
				"Browser_ScreenShot",
				"Browser_Zoom",
				"Browser_ZoomIn",
				"Browser_ZoomOut",
				"Browser_Unmute",
				"Browser_Mute",
				"Browser_Refresh",
				"Browser_Navigate",
				"Browser_Other",
			};
			int unitsize = 16 * 16 * 4;

			for ( int i = 0; i < keys.Length; i++ ) {
				Bitmap bmp = new Bitmap( 16, 16, PixelFormat.Format32bppArgb );

				if ( canvas != null ) {
					BitmapData bmpdata = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb );
					Marshal.Copy( canvas, unitsize * i, bmpdata.Scan0, unitsize );
					bmp.UnlockBits( bmpdata );
				}

				Icons.Images.Add( keys[i], bmp );
			}


			ToolMenu_ScreenShot.Image = ToolMenu_Other_ScreenShot.Image =
				Icons.Images["Browser_ScreenShot"];
			ToolMenu_Zoom.Image = ToolMenu_Other_Zoom.Image =
				Icons.Images["Browser_Zoom"];
			ToolMenu_Other_Zoom_Increment.Image =
				Icons.Images["Browser_ZoomIn"];
			ToolMenu_Other_Zoom_Decrement.Image =
				Icons.Images["Browser_ZoomOut"];
			ToolMenu_Refresh.Image = ToolMenu_Other_Refresh.Image =
				Icons.Images["Browser_Refresh"];
			ToolMenu_NavigateToLogInPage.Image = ToolMenu_Other_NavigateToLogInPage.Image =
				Icons.Images["Browser_Navigate"];
			ToolMenu_Other.Image =
				Icons.Images["Browser_Other"];
			SetMuteIcon();
		}


		private void SetMuteIcon() {

			bool mute;
			bool isEnabled;

			try {
				mute = _volumeManager.IsMute;
				isEnabled = true;

			} catch ( Exception ) {
				// 音量データ取得不能時
				mute = false;
				isEnabled = false;
			}

			ToolMenu_Mute.Image = ToolMenu_Other_Mute.Image =
				Icons.Images[mute ? "Browser_Mute" : "Browser_Unmute"];


			ToolMenu_Mute.Enabled = ToolMenu_Other_Mute.Enabled =
				isEnabled;
		}


		private void ToolMenu_Other_ScreenShot_Click( object sender, EventArgs e ) {
			SaveScreenShot( Configuration.ScreenShotPath, Configuration.ScreenShotFormat );
		}

		private void ToolMenu_Other_Zoom_Decrement_Click( object sender, EventArgs e ) {
			Configuration.ZoomRate = Math.Max( Configuration.ZoomRate - 20, 10 );
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Increment_Click( object sender, EventArgs e ) {
			Configuration.ZoomRate = Math.Min( Configuration.ZoomRate + 20, 1000 );
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Click( object sender, EventArgs e ) {

			int zoom;

			if ( sender == ToolMenu_Other_Zoom_25 )
				zoom = 25;
			else if ( sender == ToolMenu_Other_Zoom_50 )
				zoom = 50;
			else if ( sender == ToolMenu_Other_Zoom_75 )
				zoom = 75;
			else if ( sender == ToolMenu_Other_Zoom_100 )
				zoom = 100;
			else if ( sender == ToolMenu_Other_Zoom_150 )
				zoom = 150;
			else if ( sender == ToolMenu_Other_Zoom_200 )
				zoom = 200;
			else if ( sender == ToolMenu_Other_Zoom_250 )
				zoom = 250;
			else if ( sender == ToolMenu_Other_Zoom_300 )
				zoom = 300;
			else if ( sender == ToolMenu_Other_Zoom_400 )
				zoom = 400;
			else
				zoom = 100;

			Configuration.ZoomRate = zoom;
			ApplyZoom();
			ConfigurationUpdated();
		}


		//ズームUIの使いまわし
		private void ToolMenu_Other_DropDownOpening( object sender, EventArgs e ) {
			var list = ToolMenu_Zoom.DropDownItems.Cast<ToolStripItem>().ToArray();
			ToolMenu_Other_Zoom.DropDownItems.AddRange( list );
		}

		private void ToolMenu_Zoom_DropDownOpening( object sender, EventArgs e ) {

			var list = ToolMenu_Other_Zoom.DropDownItems.Cast<ToolStripItem>().ToArray();
			ToolMenu_Zoom.DropDownItems.AddRange( list );
		}


		private void ToolMenu_Other_Mute_Click( object sender, EventArgs e ) {
			try {
				_volumeManager.ToggleMute();

			} catch ( Exception ) {
			}

			SetMuteIcon();
		}

		private void ToolMenu_Other_Refresh_Click( object sender, EventArgs e ) {

			if ( !Configuration.ConfirmAtRefresh || 
				MessageBox.Show( "再読み込みします。\r\nよろしいですか？", "確認",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.OK ) {

				RefreshBrowser();
			}
		}

		private void ToolMenu_Other_NavigateToLogInPage_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "ログインページへ移動します。\r\nよろしいですか？", "確認",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.OK ) {

				Navigate( Configuration.LogInPageURL );
			}

		}

		private void ToolMenu_Other_Navigate_Click( object sender, EventArgs e ) {
			BrowserHost.AsyncRemoteRun( () => BrowserHost.Proxy.RequestNavigation( Browser.Url.ToString() ) );
		}

		private void ToolMenu_Other_AppliesStyleSheet_Click( object sender, EventArgs e ) {
			Configuration.AppliesStyleSheet = ToolMenu_Other_AppliesStyleSheet.Checked;
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Alignment_Click( object sender, EventArgs e ) {

			if ( sender == ToolMenu_Other_Alignment_Top )
				ToolMenu.Dock = DockStyle.Top;
			else if ( sender == ToolMenu_Other_Alignment_Bottom )
				ToolMenu.Dock = DockStyle.Bottom;
			else if ( sender == ToolMenu_Other_Alignment_Left )
				ToolMenu.Dock = DockStyle.Left;
			else
				ToolMenu.Dock = DockStyle.Right;

			Configuration.ToolMenuDockStyle = (int)ToolMenu.Dock;

			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Alignment_Invisible_Click( object sender, EventArgs e ) {
			ToolMenu.Visible =
			Configuration.IsToolMenuVisible = false;
			ConfigurationUpdated();
		}

		private void SizeAdjuster_Click( object sender, EventArgs e ) {
			ToolMenu.Visible =
			Configuration.IsToolMenuVisible = true;
			ConfigurationUpdated();
		}

		private void ContextMenuTool_ShowToolMenu_Click( object sender, EventArgs e ) {
			ToolMenu.Visible =
			Configuration.IsToolMenuVisible = true;
			ConfigurationUpdated();
		}

		private void ContextMenuTool_Opening( object sender, CancelEventArgs e ) {

			if ( IsKanColleLoaded || ToolMenu.Visible )
				e.Cancel = true;
		}


		private void ToolMenu_ScreenShot_Click( object sender, EventArgs e ) {
			ToolMenu_Other_ScreenShot_Click( sender, e );
		}

		private void ToolMenu_Mute_Click( object sender, EventArgs e ) {
			ToolMenu_Other_Mute_Click( sender, e );
		}

		private void ToolMenu_Refresh_Click( object sender, EventArgs e ) {
			ToolMenu_Other_Refresh_Click( sender, e );
		}

		private void ToolMenu_NavigateToLogInPage_Click( object sender, EventArgs e ) {
			ToolMenu_Other_NavigateToLogInPage_Click( sender, e );
		}

		


		// ショートカットキーが反映されない問題の対策
		void Browser_ReplacedKeyDown( object sender, KeyEventArgs e ) {

			foreach ( var item in ToolMenu_Other.DropDownItems ) {

				ToolStripMenuItem menu = item as ToolStripMenuItem;

				if ( menu != null ) {
					if ( e.KeyData == menu.ShortcutKeys ) {
						menu.PerformClick();
						e.Handled = true;
					}
				}
			}

			/*// 有効にするとショートカットが完全に無効化できる代わりに提督コメント・艦隊名が入力不能に
			if ( IsKanColleLoaded )
				e.Handled = true;
			//*/
		}


		private void FormBrowser_Activated( object sender, EventArgs e ) {
			
			//System.Media.SystemSounds.Asterisk.Play();
			Browser.Focus();
		}

		private void ToolMenu_Other_Alignment_DropDownOpening( object sender, EventArgs e ) {

			foreach ( var item in ToolMenu_Other_Alignment.DropDownItems ) {
				var menu = item as ToolStripMenuItem;
				if ( menu != null ) {
					menu.Checked = false;
				}
			}

			switch ( (DockStyle)Configuration.ToolMenuDockStyle ) {
				case DockStyle.Top:
					ToolMenu_Other_Alignment_Top.Checked = true;
					break;
				case DockStyle.Bottom:
					ToolMenu_Other_Alignment_Bottom.Checked = true;
					break;
				case DockStyle.Left:
					ToolMenu_Other_Alignment_Left.Checked = true;
					break;
				case DockStyle.Right:
					ToolMenu_Other_Alignment_Right.Checked = true;
					break;
			}

			ToolMenu_Other_Alignment_Invisible.Checked = !Configuration.IsToolMenuVisible;
		}


		
		#region 呪文

		[DllImport( "user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true )]
		private static extern uint GetWindowLong( IntPtr hwnd, int nIndex );

		[DllImport( "user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true )]
		private static extern uint SetWindowLong( IntPtr hwnd, int nIndex, uint dwNewLong );

		private const int GWL_STYLE = ( -16 );
		private const uint WS_CHILD = 0x40000000;
		private const uint WS_VISIBLE = 0x10000000;

		#endregion

		
		
	}

	

	/// <summary>
	/// デフォルトのショートカットキーを無効化する WebBrowser です。
	/// WebBrowserShortCutEnabled = false だとメニューのショートカットキーが無効化されるため、
	/// わざわざ手動で実装しています。
	/// </summary>
	internal class ExtraWebBrowser : WebBrowser {

		public event KeyEventHandler ReplacedKeyDown = delegate { };


		public ExtraWebBrowser()
			: base() { }

		public override bool PreProcessMessage( ref Message msg ) {

			if ( msg.Msg == 0x100 ) {		//WM_KEYDOWN

				var e = new KeyEventArgs( (Keys)msg.WParam | ModifierKeys );
				ReplacedKeyDown( this, e );

				if ( e.Handled )
					return true;
			}

			return base.PreProcessMessage( ref msg );
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
