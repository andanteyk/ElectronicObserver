using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	/// <summary>
	/// ブラウザを表示するフォームです。
	/// </summary>
	/// <remarks>thx KanColleViewer!</remarks>
	public partial class FormBrowser : DockContent {

		
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


		public FormBrowser( FormMain parent ) {
			InitializeComponent();

			StyleSheetApplied = false;

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser] );
		}


		private void FormBrowser_Load( object sender, EventArgs e ) {

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Observer.APIObserver.Instance.APIList["api_start2"].ResponseReceived += 
				( string apiname, dynamic data ) => Invoke( new Observer.APIReceivedEventHandler( InitialAPIReceived ), apiname, data );


			if ( Utility.Configuration.Config.FormBrowser.IsEnabled )
				NavigateToLogInPage();
		}

		

		void ConfigurationChanged() {
			SizeAdjuster.AutoScroll = Utility.Configuration.Config.FormBrowser.IsScrollable;
			ApplyZoom();	
		}

		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		void InitialAPIReceived( string apiname, dynamic data ) {
			ApplyStyleSheet();
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
		
			int x = Browser.Location.X, y = Browser.Location.Y;
			bool isScrollable = Utility.Configuration.Config.FormBrowser.IsScrollable;

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
		public void ApplyStyleSheet() {

			if ( !Utility.Configuration.Config.FormBrowser.AplliesStyleSheet )
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

					ss.cssText = Utility.Configuration.Config.FormBrowser.StyleSheet;


					StyleSheetApplied = true;
				}



			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "スタイルシートの適用に失敗しました。" );
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
		/// 艦これのログインページを開きます。
		/// </summary>
		public void NavigateToLogInPage() {
			Navigate( Utility.Configuration.Config.FormBrowser.LogInPageURL );
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
			ApplyZoom( Utility.Configuration.Config.FormBrowser.ZoomRate );
		}


		/// <summary>
		/// ズームを適用します。
		/// </summary>
		/// <param name="zoomRate">拡大率。%指定で 10-1000</param>
		public void ApplyZoom( int zoomRate ) {

			// fixme: ページが未ロードのとき例外を吐くので一時しのぎ；回避できるなら回避すること
			try {

                if (zoomRate < 10)
                    zoomRate = 10;
                if (zoomRate > 1000)
                    zoomRate = 1000;

                var wb = Browser.ActiveXInstance as SHDocVw.IWebBrowser2;
                if (wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED) return;

                var dpi = ElectronicObserver.Window.Support.ScreenHelper.GetSystemDpi();
                var zoomFactor = dpi.ScaleX + (zoomRate / 100.0 - 1.0);
                var percentage = (int)(zoomFactor * 100);

                object pin = percentage;
                object pout = null;

                wb.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout);

                if (StyleSheetApplied)
                {
                    //Browser.Size = Browser.MinimumSize = new Size( (int)( KanColleSize.Width * zoomRate / 100.0 ), (int)( KanColleSize.Height * zoomRate / 100.0 ) );
                    Browser.Size = Browser.MinimumSize = new Size(
                        (int)(KanColleSize.Width * (zoomFactor / dpi.ScaleX)),
                        (int)(KanColleSize.Height * (zoomFactor / dpi.ScaleY))
                        );
                }

			} catch ( Exception ex ) {

				Utility.Logger.Add( 3, "ズームの適用に失敗しました。" + ex.Message );
			}
			
		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		public void SaveScreenShot() {

			string path = Utility.Configuration.Config.FormBrowser.ScreenShotPath;

			if ( !System.IO.Directory.Exists( path ) ) {
				System.IO.Directory.CreateDirectory( path );
			}

			string ext;
			System.Drawing.Imaging.ImageFormat format;

			switch ( Utility.Configuration.Config.FormBrowser.ScreenShotFormat ) {
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
				DateTimeHelper.GetTimeStamp(),
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


				Utility.Logger.Add( 2, string.Format( "スクリーンショットを {0} に保存しました。", path ) );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "スクリーンショットの保存時にエラーが発生しました。" );
			}


		}



		protected override string GetPersistString() {
			return "Browser";
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
