using BrowserLib;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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


		private const string BrowserExeName = "Browser.exe";

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

		/// <summary>
		/// 初期化ステージカウント
		/// デバッグ用初期APIロードが完了した後に、艦これページを開くようにするため
		/// APIロードの完了で+1、ブラウザ起動の完了で+1、最終的に2になる
		/// </summary>
		private int initializeCompletionCount = 0;



		public FormBrowserHost( FormMain parent ) {
			InitializeComponent();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser] );
		}

		public void InitializeApiCompleted() {
			++initializeCompletionCount;
			if ( initializeCompletionCount == 2 ) { // ブラウザ起動も完了していたら実行
				if ( Utility.Configuration.Config.FormBrowser.IsEnabled ) {
					NavigateToLogInPage();
				}
			}
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
			Browser.AsyncRemoteRun( () => Browser.Proxy.ConfigurationChanged( Configuration ) );
		}


		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		void InitialAPIReceived( string apiname, dynamic data ) {
			if ( initializeCompletionCount < 2 ) return; // 未初期化状態なので、まだ
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
			ApplyZoom( Utility.Configuration.Config.FormBrowser.ZoomRate );
		}


		/// <summary>
		/// ズームを適用します。
		/// </summary>
		/// <param name="zoomRate">拡大率。%指定で 10-1000</param>
		public void ApplyZoom( int zoomRate ) {
			Browser.AsyncRemoteRun( () => Browser.Proxy.ApplyZoom( zoomRate ) );
		}

		/// <summary>
		/// スタイルシートを適用します。
		/// </summary>
		public void ApplyStyleSheet() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.ApplyStyleSheet() );
		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		public void SaveScreenShot() {
			Browser.AsyncRemoteRun( () => Browser.Proxy.SaveScreenShot( Utility.Configuration.Config.FormBrowser.ScreenShotPath,
				Utility.Configuration.Config.FormBrowser.ScreenShotFormat,
				DateTimeHelper.GetTimeStamp() ) );
		}

		protected override string GetPersistString() {
			return "Browser";
		}


		public void SendErrorReport( string exceptionName, string message ) {
			Utility.ErrorReporter.SendErrorReport( new Exception( exceptionName ), message );
		}

		public void AddLog( int priority, string message ) {
			Utility.Logger.Add( priority, message );
		}

		public BrowserLib.BrowserConfiguration Configuration {
			get {
				BrowserLib.BrowserConfiguration conf = new BrowserLib.BrowserConfiguration();
				conf.IsScrollable = Utility.Configuration.Config.FormBrowser.IsScrollable;
				conf.LogInPageURL = Utility.Configuration.Config.FormBrowser.LogInPageURL;
				conf.StyleSheet = Utility.Configuration.Config.FormBrowser.StyleSheet;
				conf.ZoomRate = Utility.Configuration.Config.FormBrowser.ZoomRate;
				conf.AppliesStyleSheet = Utility.Configuration.Config.FormBrowser.AppliesStyleSheet;
				return conf;
			}
		}

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern uint SetParent( IntPtr hWndChild, IntPtr hWndNewParent );

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern bool MoveWindow( IntPtr hwnd, int x, int y, int cx, int cy, bool repaint );

		public void ConnectToBrowser( IntPtr hwnd ) {
			BrowserWnd = hwnd;

			// 子ウィンドウに設定
			SetParent( BrowserWnd, this.Handle );
			MoveWindow( BrowserWnd, 0, 0, this.Width, this.Height, true );

			// デッドロックするので非同期で処理
			BeginInvoke( (Action)( () => {
				// ブラウザプロセスに接続
				Browser.Connect( ServerUri + "Browser/Browser" );
				Browser.Faulted += Browser_Faulted;

				ConfigurationChanged();

				Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

				Observer.APIObserver.Instance.APIList["api_start2"].ResponseReceived +=
					( string apiname, dynamic data ) => InitialAPIReceived( apiname, data );

				// プロキシをセット
				Browser.AsyncRemoteRun( () =>
					Browser.Proxy.SetProxy( Observer.APIObserver.Instance.ProxyPort ) );
				Observer.APIObserver.Instance.ProxyStarted += () => {
					Browser.AsyncRemoteRun( () =>
						Browser.Proxy.SetProxy( Observer.APIObserver.Instance.ProxyPort ) );
				};

				++initializeCompletionCount;
				if ( initializeCompletionCount == 2 ) { // APIロードも完了していたら実行
					if ( Utility.Configuration.Config.FormBrowser.IsEnabled ) {
						NavigateToLogInPage();
					}
				}
			} ) );
		}

		void Browser_Faulted( Exception e ) {
			if ( Browser.Proxy == null ) {
				Utility.Logger.Add( 3, "ブラウザプロセスが予期せず終了しました。" );
			} else {
				Utility.ErrorReporter.SendErrorReport( e, "通信エラー" );
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
		}

		private void FormBrowserHost_Resize( object sender, EventArgs e ) {
			if ( BrowserWnd != IntPtr.Zero ) {
				MoveWindow( BrowserWnd, 0, 0, this.Width, this.Height, true );
			}
		}

		/// <summary>
		/// キーメッセージの送り先
		/// </summary>
		public IntPtr HWND {
			get { return this.Handle; }
		}
	}

}
