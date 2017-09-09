using BrowserLib;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	/// <summary>
	/// ブラウザのホスト側フォーム
	/// </summary>
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	public partial class FormBrowserHost : DockContent, IBrowserHost {


		public static readonly string BrowserExeName = "EOBrowser.exe";


		/// <summary>
		/// FormBrowserHostの通信サーバ
		/// </summary>
		private string ServerUri = "net.pipe://localhost/" + Process.GetCurrentProcess().Id + "/ElectronicObserver";

		/// <summary>
		/// FormBrowserとの通信インターフェース
		/// </summary>
		private PipeCommunicator<IBrowser> Browser;

		private Process BrowserProcess;

		private IntPtr BrowserWnd = IntPtr.Zero;



		[Flags]
		private enum InitializationStageFlag {
			InitialAPILoaded = 1,
			BrowserConnected = 2,
			SetProxyCompleted = 4,
			Completed = 7,
		}

		/// <summary>
		/// 初期化ステージカウント
		/// 完了したらログインページを開く (各処理が終わらないと正常にロードできないため)
		/// </summary>
		private InitializationStageFlag _initializationStage = 0;
		private InitializationStageFlag InitializationStage {
			get { return _initializationStage; }
			set {
				//AddLog( 1, _initializationStage + " -> " + value );
				if ( _initializationStage != InitializationStageFlag.Completed && value == InitializationStageFlag.Completed ) {
					if ( Utility.Configuration.Config.FormBrowser.IsEnabled ) {
						NavigateToLogInPage();
					}
				}

				_initializationStage = value;
			}
		}



		public FormBrowserHost( FormMain parent ) {
			InitializeComponent();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser] );
		}

		public void InitializeApiCompleted() {
			InitializationStage |= InitializationStageFlag.InitialAPILoaded;
		}

		private void FormBrowser_Load( object sender, EventArgs e ) {
			LaunchBrowserProcess();
		}


		private void LaunchBrowserProcess() {
			// 通信サーバ起動
			Browser = new PipeCommunicator<IBrowser>(
				this, typeof( IBrowserHost ), ServerUri, "BrowserHost" );

			try {
				// プロセス起動

				if ( System.IO.File.Exists( BrowserExeName ) )
					BrowserProcess = Process.Start( BrowserExeName, ServerUri );

				else	//デバッグ環境用 作業フォルダにかかわらず自分と同じフォルダのを参照する
					BrowserProcess = Process.Start(
						System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location ) + "\\" + BrowserExeName,
						ServerUri );

				// 残りはサーバに接続してきたブラウザプロセスがドライブする

			} catch ( Exception ex ) {
				Utility.ErrorReporter.SendErrorReport( ex, "ブラウザプロセスの起動に失敗しました。" );
				MessageBox.Show( "ブラウザプロセスの起動に失敗しました。\r\n" + ex.Message,
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		internal void ConfigurationChanged() {
			Font = Utility.Configuration.Config.UI.MainFont;
			Browser.AsyncRemoteRun( () => Browser.Proxy.ConfigurationChanged( Configuration ) );
		}


		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		void InitialAPIReceived( string apiname, dynamic data ) {
			if ( InitializationStage != InitializationStageFlag.Completed )		// 初期化が終わってから
				return;

			Browser.AsyncRemoteRun( () => Browser.Proxy.InitialAPIReceived() );
		}


		/// <summary>
		/// 指定した URL のページを開きます。
		/// </summary>
		public void Navigate( string url ) {
			Browser.AsyncRemoteRun( () => Browser.Proxy.Navigate( url ) );
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
			Browser.AsyncRemoteRun( () => Browser.Proxy.RefreshBrowser() );
		}

		/// <summary>
		/// ズームを適用します。
		/// </summary>
		public void ApplyZoom() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.ApplyZoom() );
		}

		/// <summary>
		/// スタイルシートを適用します。
		/// </summary>
		public void ApplyStyleSheet() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.ApplyStyleSheet() );
		}

		/// <summary>
		/// DMMによるページ更新ダイアログを非表示にします。
		/// </summary>
		public void DestroyDMMreloadDialog() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.DestroyDMMreloadDialog() );
		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		public void SaveScreenShot() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.SaveScreenShot() );
		}


		public void SendErrorReport( string exceptionName, string message ) {
			Utility.ErrorReporter.SendErrorReport( new Exception( exceptionName ), message );
		}

		public void AddLog( int priority, string message ) {
			Utility.Logger.Add( priority, message );
		}


		public BrowserLib.BrowserConfiguration Configuration {
			get {
				BrowserLib.BrowserConfiguration config = new BrowserLib.BrowserConfiguration();
				var c = Utility.Configuration.Config.FormBrowser;

				config.ZoomRate = c.ZoomRate;
				config.ZoomFit = c.ZoomFit;
				config.LogInPageURL = c.LogInPageURL;
				config.IsEnabled = c.IsEnabled;
				config.ScreenShotPath = c.ScreenShotPath;
				config.ScreenShotFormat = c.ScreenShotFormat;
				config.ScreenShotSaveMode = c.ScreenShotSaveMode;
				config.StyleSheet = c.StyleSheet;
				config.IsScrollable = c.IsScrollable;
				config.AppliesStyleSheet = c.AppliesStyleSheet;
				config.IsDMMreloadDialogDestroyable = c.IsDMMreloadDialogDestroyable;
				config.AvoidTwitterDeterioration = c.AvoidTwitterDeterioration;
				config.ToolMenuDockStyle = (int)c.ToolMenuDockStyle;
				config.IsToolMenuVisible = c.IsToolMenuVisible;
				config.ConfirmAtRefresh = c.ConfirmAtRefresh;

				return config;
			}
		}

		public void ConfigurationUpdated( BrowserLib.BrowserConfiguration config ) {

			var c = Utility.Configuration.Config.FormBrowser;

			c.ZoomRate = config.ZoomRate;
			c.ZoomFit = config.ZoomFit;
			c.LogInPageURL = config.LogInPageURL;
			c.IsEnabled = config.IsEnabled;
			c.ScreenShotPath = config.ScreenShotPath;
			c.ScreenShotFormat = config.ScreenShotFormat;
			c.ScreenShotSaveMode = config.ScreenShotSaveMode;
			c.StyleSheet = config.StyleSheet;
			c.IsScrollable = config.IsScrollable;
			c.AppliesStyleSheet = config.AppliesStyleSheet;
			c.IsDMMreloadDialogDestroyable = config.IsDMMreloadDialogDestroyable;
			c.AvoidTwitterDeterioration = config.AvoidTwitterDeterioration;
			c.ToolMenuDockStyle = (DockStyle)config.ToolMenuDockStyle;
			c.IsToolMenuVisible = config.IsToolMenuVisible;
			c.ConfirmAtRefresh = config.ConfirmAtRefresh;

			// volume
			if ( Utility.Configuration.Config.BGMPlayer.SyncBrowserMute ) {
				Utility.SyncBGMPlayer.Instance.IsMute = config.IsMute;
			}
		}

		public void GetIconResource() {

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

			byte[] canvas = new byte[unitsize * keys.Length];

			for ( int i = 0; i < keys.Length; i++ ) {
				Image img = ResourceManager.Instance.Icons.Images[keys[i]];
				if ( img != null ) {
					using ( Bitmap bmp = new Bitmap( img ) ) {

						BitmapData bmpdata = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
						Marshal.Copy( bmpdata.Scan0, canvas, unitsize * i, unitsize );
						bmp.UnlockBits( bmpdata );

					}
				}
			}

			Browser.AsyncRemoteRun( () => Browser.Proxy.SetIconResource( canvas ) );

		}


		public void RequestNavigation( string baseurl ) {

			using ( var dialog = new Window.Dialog.DialogTextInput( "移動先の入力", "移動先の URL を入力してください。" ) ) {
				dialog.InputtedText = baseurl;

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					Navigate( dialog.InputtedText );
				}
			}

		}


		public void ConnectToBrowser( IntPtr hwnd ) {
			BrowserWnd = hwnd;

			// 子ウィンドウに設定
			SetParent( BrowserWnd, this.Handle );
			MoveWindow( BrowserWnd, 0, 0, this.Width, this.Height, true );

			//キー入力をブラウザに投げる
			Application.AddMessageFilter( new KeyMessageGrabber( BrowserWnd ) );

			// デッドロックするので非同期で処理
			BeginInvoke( (Action)( () => {
				// ブラウザプロセスに接続
				Browser.Connect( ServerUri + "Browser/Browser" );
				Browser.Faulted += Browser_Faulted;

				ConfigurationChanged();

				Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

				APIObserver.Instance.APIList["api_start2"].ResponseReceived +=
					( string apiname, dynamic data ) => InitialAPIReceived( apiname, data );

				// プロキシをセット
				Browser.AsyncRemoteRun( () => Browser.Proxy.SetProxy( BuildDownstreamProxy() ) );
				APIObserver.Instance.ProxyStarted += () => {
					Browser.AsyncRemoteRun( () => Browser.Proxy.SetProxy( BuildDownstreamProxy() ) );
				};

				InitializationStage |= InitializationStageFlag.BrowserConnected;

			} ) );
		}

		private string BuildDownstreamProxy() {
			var config = Utility.Configuration.Config.Connection;

			if ( !string.IsNullOrEmpty( config.DownstreamProxy ) ) {
				return config.DownstreamProxy;

			} else if ( config.UseSystemProxy ) {
				return APIObserver.Instance.ProxyPort.ToString();

			} else if ( config.UseUpstreamProxy ) {
				return string.Format(
					"http=127.0.0.1:{0};https={1}:{2}",
					APIObserver.Instance.ProxyPort,
					config.UpstreamProxyAddress,
					config.UpstreamProxyPort );
			} else {
				return string.Format( "http=127.0.0.1:{0}", APIObserver.Instance.ProxyPort );
			}
		}


		public void SetProxyCompleted() {
			InitializationStage |= InitializationStageFlag.SetProxyCompleted;
		}


		void Browser_Faulted( Exception e ) {
			if ( Browser.Proxy == null ) {
				Utility.Logger.Add( 3, "ブラウザプロセスが予期せず終了しました。" );
			} else {
				Utility.ErrorReporter.SendErrorReport( e, "ブラウザプロセス間で通信エラーが発生しました。" );
			}
		}


		private void TerminateBrowserProcess() {
			if ( !BrowserProcess.WaitForExit( 2000 ) ) {
				try {
					// 2秒待って終了しなかったらKill
					BrowserProcess.Kill();
				} catch ( Exception ) {
					// プロセスが既に終了してた場合などに例外が出る
				}
			}
			BrowserWnd = IntPtr.Zero;
		}

		public void CloseBrowser() {

			try {

				if ( Browser == null ) {
					// ブラウザを開いていない場合はnullなので
					return;
				}
				if ( !Browser.Closed ) {
					// ブラウザプロセスが異常終了した場合などはnullになる
					if ( Browser.Proxy != null ) {
						Browser.Proxy.CloseBrowser();
					}
					Browser.Close();
					TerminateBrowserProcess();
				}

			} catch ( Exception ex ) {		//ブラウザプロセスが既に終了していた場合など

				Utility.ErrorReporter.SendErrorReport( ex, "ブラウザの終了中にエラーが発生しました。" );
			}

		}

		private void FormBrowserHost_Resize( object sender, EventArgs e ) {
			if ( BrowserWnd != IntPtr.Zero ) {
				MoveWindow( BrowserWnd, 0, 0, this.Width, this.Height, true );
			}
		}

		/// <summary>
		/// ハートビート用
		/// </summary>
		public IntPtr HWND {
			get { return this.Handle; }
		}

		protected override string GetPersistString() {
			return "Browser";
		}


		#region 呪文

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern uint SetParent( IntPtr hWndChild, IntPtr hWndNewParent );

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern bool MoveWindow( IntPtr hwnd, int x, int y, int cx, int cy, bool repaint );


		#endregion

	}


	/// <summary>
	/// 別プロセスのウィンドウにフォーカスがあるとキーボードショートカットが効かなくなるため、
	/// キー関連のメッセージのコピーを別のウィンドウに送る
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

}
