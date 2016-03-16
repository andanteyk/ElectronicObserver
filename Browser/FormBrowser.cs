using Browser.Win32;
using BrowserLib;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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

		private string FrameScript;
		private string PageScript;


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

#if !NOVOL
		private VolumeManager _volumeManager;
#endif


		private NumericUpDown ToolMenu_Other_Volume_VolumeControl {
			get { return (NumericUpDown)( (ToolStripControlHost)ToolMenu_Other_Volume.DropDownItems["ToolMenu_Other_Volume_VolumeControlHost"] ).Control; }
		}




		/// <summary>
		/// </summary>
		/// <param name="serverUri">ホストプロセスとの通信用URL</param>
		public FormBrowser( string serverUri ) {
			SuspendLayout();
			InitializeComponent();

			ServerUri = serverUri;
			StyleSheetApplied = false;
#if !NOVOL
			_volumeManager = new VolumeManager( (uint)System.Diagnostics.Process.GetCurrentProcess().Id );
#endif
			Browser.ReplacedKeyDown += Browser_ReplacedKeyDown;

			// 音量設定用コントロールの追加
			{
				var control = new NumericUpDown();
				control.Name = "ToolMenu_Other_Volume_VolumeControl";
				control.Maximum = 100;
				control.TextAlign = HorizontalAlignment.Right;
				control.Font = ToolMenu_Other_Volume.Font;

				control.ValueChanged += ToolMenu_Other_Volume_ValueChanged;
				control.Tag = false;

				var host = new ToolStripControlHost( control, "ToolMenu_Other_Volume_VolumeControlHost" );

				control.Size = new Size( host.Width - control.Margin.Horizontal, host.Height - control.Margin.Vertical );
				control.Location = new Point( control.Margin.Left, control.Margin.Top );


				ToolMenu_Other_Volume.DropDownItems.Add( host );
			}

			if ( File.Exists( "PageScript.js" ) ) {
				PageScript = File.ReadAllText( "PageScript.js" );
			}
			if ( string.IsNullOrEmpty( PageScript ) ) {
				PageScript = Properties.Resources.PageScript;
			}

			if ( File.Exists( "FrameScript.js" ) ) {
				FrameScript = File.ReadAllText( "FrameScript.js" );
			}
			if ( string.IsNullOrEmpty( FrameScript ) ) {
				FrameScript = Properties.Resources.FrameScript;
			}

			this.AutoScaleMode = AutoScaleMode.Dpi;
			this.AutoScaleDimensions = new SizeF( 96F, 96F );
			ResumeLayout();

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
			ToolMenu_Other_Zoom_Fit.Checked = Configuration.ZoomFit;
			ApplyZoom();
			ToolMenu_Other_AppliesStyleSheet.Checked = Configuration.AppliesStyleSheet;
			ToolMenu.Dock = (DockStyle)Configuration.ToolMenuDockStyle;
			ToolMenu.Visible = Configuration.IsToolMenuVisible;

			ToolMenu_Url.Visible = conf.ShowURL && ( ToolMenu.Dock == DockStyle.Top || ToolMenu.Dock == DockStyle.Bottom );

			ToolStripCustomizer.ToolStripRender.RendererTheme = (ToolStripCustomizer.ToolStripRenderTheme)Configuration.ThemeID;

			ToolStripCustomizer.ToolStripRender.SetRender( this.ContextMenuTool );
			ToolStripCustomizer.ToolStripRender.SetRender( this.ToolMenu );
		}

		private void ConfigurationUpdated() {
			ToolMenu_Url.Visible = Configuration.ShowURL && ( ToolMenu.Dock == DockStyle.Top || ToolMenu.Dock == DockStyle.Bottom );

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
			SetVolumeState();
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

			ApplyZoom();
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

			// note: ここを有効にすると別ページに切り替えた際にきちんとセーフティが働くが、代わりにまれに誤検知して撮影できなくなる時がある
			// 無効にするとセーフティは働かなくなるが誤検知がなくなる
			// セーフティを切ってでも誤検知しなくしたほうがいいので無効化

			//IsKanColleLoaded = false;

		}

		private void Browser_Navigated( object sender, WebBrowserNavigatedEventArgs e ) {

			ToolMenu_Url.Text = Browser.Url.ToString();

		}

		private void Browser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {
			ReplaceEmbedHtml();

			StyleSheetApplied = false;
			ApplyStyleSheet();

			ApplyZoom();

			ToolMenu_Url.Text = Browser.Url.ToString();
		}

		/// <summary>
		/// 应用embed元素
		/// </summary>
		private void ReplaceEmbedHtml() {
			if ( string.IsNullOrEmpty( Configuration.EmbedHtml ) )
				return;

			try {
				var document = Browser.Document;
				string url;
				if ( document != null && ( url = document.Url.ToString() ).Contains( ".swf?" ) ) {
					document.Body.InnerHtml = string.Format( Configuration.EmbedHtml, url, Configuration.FlashWmode, Configuration.FlashQuality );
				}
			} catch ( Exception ex ) {
				BrowserHost.AsyncRemoteRun( () =>
					BrowserHost.Proxy.SendErrorReport( ex.ToString(), "embed元素应用失败。" ) );
			}
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

				if ( document.Url.ToString().Contains( ".swf?" ) ) {

					var swf = document.Body.Children.OfType<HtmlElement>().FirstOrDefault( e => e.TagName == "EMBED" );
					if ( swf == null ) return;

					swf.SetAttribute( "width", "100%" );
					swf.SetAttribute( "height", "100%" );

				} else {
					var swf = getFrameElementById( document, "externalswf" );
					if ( swf == null ) return;

					// InvokeScriptは関数しか呼べないようなので、スクリプトをevalで渡す
					document.InvokeScript( "eval", new object[] { PageScript } );
					swf.Document.InvokeScript( "eval", new object[] { FrameScript } );
				}

				StyleSheetApplied = true;

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
			ToolMenu_Url.Text = url;
		}

		/// <summary>
		/// ブラウザを再読み込みします。
		/// </summary>
		public void RefreshBrowser() {
			//Browser.Refresh( WebBrowserRefreshOption.Completely );
			Browser.Navigate( Browser.Url );
		}

		/// <summary>
		/// ズームを適用します。
		/// </summary>
		public void ApplyZoom() {
			int zoomRate = Configuration.ZoomRate;
			bool fit = Configuration.ZoomFit && StyleSheetApplied;

			try {
				var wb = Browser.ActiveXInstance as SHDocVw.IWebBrowser2;
				if ( wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED || wb.Busy ) return;

				double zoomFactor;
				object pin;

				if ( fit ) {
					pin = 100;
					double rateX = (double)SizeAdjuster.Width / KanColleSize.Width;
					double rateY = (double)SizeAdjuster.Height / KanColleSize.Height;
					zoomFactor = Math.Min( rateX, rateY );
				} else {
					if ( zoomRate < 10 )
						zoomRate = 10;
					if ( zoomRate > 1000 )
						zoomRate = 1000;

					pin = zoomRate;
					zoomFactor = zoomRate / 100.0;
				}

				object pout = null;
				wb.ExecWB( SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout );

				if ( StyleSheetApplied ) {
					Browser.Size = Browser.MinimumSize = new Size(
						(int)( KanColleSize.Width * zoomFactor ),
						(int)( KanColleSize.Height * zoomFactor )
						);
					CenteringBrowser();
				}

				if ( fit ) {
					ToolMenu_Other_Zoom_Current.Text = string.Format( "現在: ぴったり" );
				} else {
					ToolMenu_Other_Zoom_Current.Text = string.Format( "現在: {0}%", zoomRate );
				}


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

		// ラッパークラスに戻す
		private static HtmlDocument WrapHTMLDocument( IHTMLDocument2 document ) {
			ConstructorInfo[] constructor = typeof( HtmlDocument ).GetConstructors(
				BindingFlags.NonPublic | BindingFlags.Instance );
			return (HtmlDocument)constructor[0].Invoke( new object[] { null, document } );
		}

		// 中のフレームからidにマッチする要素を返す
		private static HtmlElement getFrameElementById( HtmlDocument document, String id ) {
			foreach ( HtmlWindow frame in document.Window.Frames ) {

				// frameが別ドメインだとセキュリティ上の問題（クロスフレームスクリプティング）
				// からアクセスができないのでアクセスできるドキュメントに変換する
				IServiceProvider provider = (IServiceProvider)frame.DomWindow;
				object ppvobj;
				provider.QueryService( typeof( SHDocVw.IWebBrowserApp ).GUID, typeof( SHDocVw.IWebBrowser2 ).GUID, out ppvobj );
				var htmlDocument = WrapHTMLDocument( (IHTMLDocument2)( (SHDocVw.IWebBrowser2)ppvobj ).Document );
				var htmlElement = htmlDocument.GetElementById( id );
				if ( htmlElement == null )
					continue;

				return htmlElement;
			}

			return null;
		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		/// <param name="path">保存先。</param>
		/// <param name="format">画像のフォーマット。</param>
		private void SaveScreenShot( string path, System.Drawing.Imaging.ImageFormat format ) {

			var wb = Browser;

			//if ( !IsKanColleLoaded ) {
			//	AddLog( 3, string.Format( "艦これが読み込まれていないため、スクリーンショットを撮ることはできません。" ) );
			//	System.Media.SystemSounds.Beep.Play();
			//	return;
			//}

			try {
				IViewObject viewobj = null;
				//int width = 0, height = 0;

				if ( wb.Document.Url.ToString().Contains( ".swf?" ) ) {

					viewobj = wb.Document.GetElementsByTagName( "embed" )[0].DomElement as IViewObject;
					if ( viewobj == null ) {
						throw new InvalidOperationException( "embed 要素の取得に失敗しました。" );
					}

					//width = ( (HTMLEmbed)viewobj ).clientWidth;
					//height = ( (HTMLEmbed)viewobj ).clientHeight;

				} else {

					var swf = getFrameElementById( wb.Document, "externalswf" );
					if ( swf == null ) {
						viewobj = wb.Document.GetElementsByTagName( "embed" )[0].DomElement as IViewObject;
						if ( viewobj == null ) {
							throw new InvalidOperationException( "swf 对象未发现，并且获取 embed 元素失败。" );
						}
					}

					Func<dynamic, bool> isvalid = target => {

						if ( target == null ) return false;
						viewobj = target as IViewObject;
						if ( viewobj == null ) return false;
						//if ( !int.TryParse( target.width, out width ) ) return false;
						//if ( !int.TryParse( target.height, out height ) ) return false;
						return true;
					};

					if ( viewobj == null && !isvalid( swf.DomElement as HTMLEmbed ) && !isvalid( swf.DomElement as HTMLObjectElement ) ) {
						string name = null;
						if ( swf.DomElement != null ) {
							name = swf.DomElement.GetType().FullName;
						}
						throw new InvalidOperationException( string.Format( "swf对象无效，该对象为：{0}.", name ) );
					}
				}


				if ( viewobj != null ) {
					var rect = new RECT { left = 0, top = 0, width = KanColleSize.Width, height = KanColleSize.Height };

					using ( var image = new Bitmap( rect.width, rect.height, System.Drawing.Imaging.PixelFormat.Format24bppRgb ) ) {

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
				System.Media.SystemSounds.Beep.Play();

			}


		}


		public void SetProxy( string proxy ) {
			ushort port;
			if ( ushort.TryParse( proxy, out port ) ) {
				SetIESettings( "localhost:" + port );
			} else {
				SetIESettings( proxy );
			}

			//AddLog( 1, "setproxy:" + proxy );
		}

		private static void SetIESettings( string proxyUri ) {
			// ReSharper disable InconsistentNaming
			const int INTERNET_OPTION_PROXY = 38;
			const int INTERNET_OPEN_TYPE_PROXY = 3;
			// ReSharper restore InconsistentNaming

			INTERNET_PROXY_INFO proxyInfo;
			proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
			proxyInfo.proxy = Marshal.StringToHGlobalAnsi( proxyUri );
			proxyInfo.proxyBypass = Marshal.StringToHGlobalAnsi( "local" );

			var proxyInfoSize = Marshal.SizeOf( proxyInfo );
			var proxyInfoPtr = Marshal.AllocCoTaskMem( proxyInfoSize );
			Marshal.StructureToPtr( proxyInfo, proxyInfoPtr, true );

			NativeMethods.InternetSetOption( IntPtr.Zero, INTERNET_OPTION_PROXY, proxyInfoPtr, proxyInfoSize );
		}

		/// <summary>
		/// キャッシュを削除します。
		/// </summary>
		private void ClearCache() {

            // No more items have been found.
            const int ERROR_NO_MORE_ITEMS = 259;
            // Local variables
            int cacheEntryInfoBufferSizeInitial = 0;
            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            INTERNET_CACHE_ENTRY_INFOA internetCacheEntry;
            IntPtr enumHandle = IntPtr.Zero;
            bool returnValue = false;

            /*/ Delete the groups first.
            // Groups may not always exist on the system.
            // For more information, visit the following Microsoft Web site:
            // http://msdn.microsoft.com/library/?url=/workshop/networking/wininet/overview/cache.asp        
            // By default, a URL does not belong to any group. Therefore, that cache may become
            // empty even when the CacheGroup APIs are not used because the existing URL does not belong to any group.         
            enumHandle = FindFirstUrlCacheGroup( 0, CACHEGROUP_SEARCH_ALL, IntPtr.Zero, 0, ref groupId, IntPtr.Zero );
            // If there are no items in the Cache, you are finished.
            if ( enumHandle != IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() )
                return;
            // Loop through Cache Group, and then delete entries.
            while ( true ) {
                // Delete a particular Cache Group.
                returnValue = DeleteUrlCacheGroup( groupId, CACHEGROUP_FLAG_FLUSHURL_ONDELETE, IntPtr.Zero );
                if ( !returnValue && ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error() ) {
                    returnValue = FindNextUrlCacheGroup( enumHandle, ref groupId, IntPtr.Zero );
                }
                if ( !returnValue && ( ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() || ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error() ) )
                    break;
            }
            //*/

            // Start to delete URLs that do not belong to any group.
            enumHandle = FindFirstUrlCacheEntry( null, IntPtr.Zero, ref cacheEntryInfoBufferSizeInitial );
            if ( enumHandle == IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() )
                return;
            cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
            cacheEntryInfoBuffer = Marshal.AllocHGlobal( cacheEntryInfoBufferSize );
            enumHandle = FindFirstUrlCacheEntry( null, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial );
            while ( true ) {
                internetCacheEntry = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure( cacheEntryInfoBuffer, typeof( INTERNET_CACHE_ENTRY_INFOA ) );
                cacheEntryInfoBufferSizeInitial = cacheEntryInfoBufferSize;
                returnValue = DeleteUrlCacheEntry( internetCacheEntry.lpszLocalFileName );
                if ( !returnValue ) {
                    returnValue = FindNextUrlCacheEntry( enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial );
                }
                if ( !returnValue && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() ) {
                    break;
                }
                if ( !returnValue && cacheEntryInfoBufferSizeInitial > cacheEntryInfoBufferSize ) {
                    cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
                    cacheEntryInfoBuffer = Marshal.ReAllocHGlobal( cacheEntryInfoBuffer, (IntPtr)cacheEntryInfoBufferSize );
                    returnValue = FindNextUrlCacheEntry( enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial );
                }
            }
            Marshal.FreeHGlobal( cacheEntryInfoBuffer );

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

			SetVolumeState();
		}


		private void SetVolumeState() {

			bool mute;
			float volume;

			try {
#if !NOVOL
				mute = _volumeManager.IsMute;
				volume = _volumeManager.Volume * 100;
#else
				mute = false;
				isEnabled = false;
#endif

			} catch ( Exception ) {
				// 音量データ取得不能時
				mute = false;
				volume = 100;
			}

			ToolMenu_Mute.Image = ToolMenu_Other_Mute.Image =
				Icons.Images[mute ? "Browser_Mute" : "Browser_Unmute"];

			{
				var control = ToolMenu_Other_Volume_VolumeControl;
				control.Tag = false;
				control.Value = (decimal)volume;
				control.Tag = true;
			}

			Configuration.Volume = volume;
			Configuration.IsMute = mute;
			ConfigurationUpdated();
		}


		private void ToolMenu_Other_ScreenShot_Click( object sender, EventArgs e ) {
			SaveScreenShot( Configuration.ScreenShotPath, Configuration.ScreenShotFormat );
		}

		private void ToolMenu_Other_Zoom_Decrement_Click( object sender, EventArgs e ) {
			Configuration.ZoomRate = Math.Max( Configuration.ZoomRate - 20, 10 );
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Increment_Click( object sender, EventArgs e ) {
			Configuration.ZoomRate = Math.Min( Configuration.ZoomRate + 20, 1000 );
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
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
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Fit_Click( object sender, EventArgs e ) {
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked;
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
#if !NOVOL
			try {
				_volumeManager.ToggleMute();

			} catch ( Exception ) {
				System.Media.SystemSounds.Beep.Play();
			}
#endif

			SetVolumeState();
		}

		void ToolMenu_Other_Volume_ValueChanged( object sender, EventArgs e ) {

			var control = ToolMenu_Other_Volume_VolumeControl;

			try {
				if ( (bool)control.Tag )
					_volumeManager.Volume = (float)( control.Value / 100 );
				control.BackColor = SystemColors.Window;

			} catch ( Exception ) {
				control.BackColor = Color.MistyRose;

			}

		}


		private void ToolMenu_Other_Refresh_Click( object sender, EventArgs e ) {

			if ( !Configuration.ConfirmAtRefresh ||
				MessageBox.Show( "是否确定刷新网页？", "确认",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.OK ) {

				RefreshBrowser();
			}
		}

		private void ToolMenu_Other_NavigateToLogInPage_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "是否确认移动到初始页面？", "确认",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.OK ) {

				Navigate( Configuration.LogInPageURL );
			}

		}

		private void ToolMenu_Other_Navigate_Click( object sender, EventArgs e ) {
			BrowserHost.AsyncRemoteRun( () => BrowserHost.Proxy.RequestNavigation( Browser.Url == null ? null : Browser.Url.ToString() ) );
		}

		private void ToolMenu_Other_AppliesStyleSheet_Click( object sender, EventArgs e ) {
			Configuration.AppliesStyleSheet = ToolMenu_Other_AppliesStyleSheet.Checked;
			if ( Configuration.AppliesStyleSheet ) {
				ApplyStyleSheet();
			}
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


		private async void ToolMenu_Other_ClearCache_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "ブラウザのキャッシュを削除します。\nよろしいですか？", "キャッシュの削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question )
				== System.Windows.Forms.DialogResult.OK ) {

				try {

					await Task.Factory.StartNew( (Action)( () => ClearCache() ) );
					MessageBox.Show( "キャッシュの削除が完了しました。", "削除完了", MessageBoxButtons.OK, MessageBoxIcon.Information );

				} catch ( Exception ex ) {
					BrowserHost.Proxy.SendErrorReport( ex.Message, "清除缓存时出错。" );
				}

			}
		}




		private void SizeAdjuster_DoubleClick( object sender, EventArgs e ) {
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

            if (e.KeyData == Keys.F8)
            {
                if (Configuration.IsToolMenuVisible)
                    ToolMenu_Other_Alignment_Invisible_Click(ToolMenu_Other_Alignment_Invisible, null);
                else
                    ContextMenuTool_ShowToolMenu_Click(ContextMenuTool_ShowToolMenu, null);
                e.Handled = true;
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

		protected override void WndProc( ref Message m ) {

			if ( m.Msg == WM_ERASEBKGND )
				// ignore this message
				return;

			base.WndProc( ref m );
		}


		#region 呪文

		[DllImport( "user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true )]
		private static extern uint GetWindowLong( IntPtr hwnd, int nIndex );

		[DllImport( "user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true )]
		private static extern uint SetWindowLong( IntPtr hwnd, int nIndex, uint dwNewLong );

		private const int GWL_STYLE = ( -16 );
		private const uint WS_CHILD = 0x40000000;
		private const uint WS_VISIBLE = 0x10000000;
		private const int WM_ERASEBKGND = 0x14;


        //以下キャッシュ削除用呪文
        [StructLayout( LayoutKind.Explicit, Size = 80, CharSet = CharSet.Auto )]
        protected struct INTERNET_CACHE_ENTRY_INFOA {
            [FieldOffset( 0 )]
            public uint dwStructSize;
            [FieldOffset( 4 )]
            public IntPtr lpszSourceUrlName;
            [FieldOffset( 8 )]
            public IntPtr lpszLocalFileName;
            [FieldOffset( 12 )]
            public uint CacheEntryType;
            [FieldOffset( 16 )]
            public uint dwUseCount;
            [FieldOffset( 20 )]
            public uint dwHitRate;
            [FieldOffset( 24 )]
            public uint dwSizeLow;
            [FieldOffset( 28 )]
            public uint dwSizeHigh;
            [FieldOffset( 32 )]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            [FieldOffset( 40 )]
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
            [FieldOffset( 48 )]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            [FieldOffset( 56 )]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            [FieldOffset( 64 )]
            public IntPtr lpHeaderInfo;
            [FieldOffset( 68 )]
            public uint dwHeaderInfoSize;
            [FieldOffset( 72 )]
            public IntPtr lpszFileExtension;
            [FieldOffset( 76 )]
            public uint dwReserved;
            [FieldOffset( 76 )]
            public uint dwExemptDelta;
        }
        // For PInvoke: Initiates the enumeration of the cache groups in the Internet cache
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern IntPtr FindFirstUrlCacheGroup(
            int dwFlags,
            int dwFilter,
            IntPtr lpSearchCondition,
            int dwSearchCondition,
            ref long lpGroupId,
            IntPtr lpReserved );
        // For PInvoke: Retrieves the next cache group in a cache group enumeration
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern bool FindNextUrlCacheGroup(
            IntPtr hFind,
            ref long lpGroupId,
            IntPtr lpReserved );
        // For PInvoke: Releases the specified GROUPID and any associated state in the cache index file
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern bool DeleteUrlCacheGroup(
            long GroupId,
            int dwFlags,
            IntPtr lpReserved );
        // For PInvoke: Begins the enumeration of the Internet cache
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern IntPtr FindFirstUrlCacheEntry(
            [MarshalAs( UnmanagedType.LPTStr )] string lpszUrlSearchPattern,
            IntPtr lpFirstCacheEntryInfo,
            ref int lpdwFirstCacheEntryInfoBufferSize );
        // For PInvoke: Retrieves the next entry in the Internet cache
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern bool FindNextUrlCacheEntry(
            IntPtr hFind,
            IntPtr lpNextCacheEntryInfo,
            ref int lpdwNextCacheEntryInfoBufferSize );
        // For PInvoke: Removes the file that is associated with the source name from the cache, if the file exists
        [DllImport( @"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall )]
        protected static extern bool DeleteUrlCacheEntry(
            IntPtr lpszUrlName );

		#endregion

		private void ToolMenu_Mute_DropDownOpening( object sender, EventArgs e ) {
#if !NOVOL
			trackVolume.Value = (int)( _volumeManager.Volume * 100 );
			trackVolume.Visible = !trackVolume.Visible;
#endif
		}

		private void trackVolume_ValueChanged( object sender, EventArgs e ) {
#if !NOVOL
			_volumeManager.Volume = trackVolume.Value / 100f;
#endif
		}

		private void trackVolume_LostFocus( object sender, EventArgs e ) {
#if !NOVOL
			trackVolume.Visible = false;
#endif
		}


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
