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
	public partial class FormBrowserHost : DockContent, BrowserLib.IBrowserHost {
		// FormBrowserHostの通信サーバ
		private string ServerUri = "net.pipe://localhost/" + Process.GetCurrentProcess().Id + "/ElectronicObserver";

		private ServiceHost CommunicationServiceHost;

		private Process BrowserProcess;

		private IntPtr BrowserWnd = IntPtr.Zero;

		// FormBrowserとの通信インターフェース
		private BrowserLib.IBrowser Browser;

		public FormBrowserHost( FormMain parent ) {
			InitializeComponent();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser] );
		}


		private void FormBrowser_Load( object sender, EventArgs e ) {
			LaunchBrowserProcess();
		}

		private void LaunchBrowserProcess() {
			// 通信サーバ起動
			CommunicationServiceHost = new ServiceHost( this, new Uri[] { new Uri( ServerUri ) } );
			CommunicationServiceHost.AddServiceEndpoint(
				typeof( BrowserLib.IBrowserHost ), new NetNamedPipeBinding(), "BrowserHost" );
			CommunicationServiceHost.Open();

			try {
				// プロセス起動
				BrowserProcess = Process.Start( "Browser.exe", ServerUri );

				// 残りはサーバに接続してきたブラウザプロセスがドライブする

			} catch ( Exception ex ) {
				MessageBox.Show( "ブラウザプロセスの起動に失敗しました。\r\n" + ex.Message );
			}
		}

		private void ConfigurationChanged() {
			if ( Browser == null ) return;
			Task.Run( () => Browser.ConfigurationChanged( Configuration ) );
		}

		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		void InitialAPIReceived( string apiname, dynamic data ) {
			if ( Browser == null ) return;
			Task.Run( () => Browser.InitialAPIReceived() );
		}


		/// <summary>
		/// 指定した URL のページを開きます。
		/// </summary>
		public void Navigate( string url ) {
			if ( Browser == null ) return;
			Task.Run( () => Browser.Navigate( url ) );
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
			if ( Browser == null ) return;
			Task.Run( () => Browser.RefreshBrowser() );
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
			if ( Browser == null ) return;
			Task.Run( () => Browser.ApplyZoom( zoomRate ) );
		}


		/// <summary>
		/// スクリーンショットを保存します。
		/// </summary>
		public void SaveScreenShot() {
			if ( Browser == null ) return;
			Task.Run( () => Browser.SaveScreenShot( Utility.Configuration.Config.FormBrowser.ScreenShotPath,
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

			// 後は非同期で処理
			BeginInvoke( (Action)( () => {
				// ブラウザプロセスに接続
				ChannelFactory<BrowserLib.IBrowser> pipeFactory =
					new ChannelFactory<BrowserLib.IBrowser>(
						new NetNamedPipeBinding(), new EndpointAddress( ServerUri + "Browser/Browser" ) );
				Browser = pipeFactory.CreateChannel();
				( (IClientChannel)Browser ).Faulted += BrowserChannel_Faulted;

				ConfigurationChanged();

				Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

				Observer.APIObserver.Instance.APIList["api_start2"].ResponseReceived +=
					( string apiname, dynamic data ) => InitialAPIReceived( apiname, data );

				// プロキシをセット
				Task.Run( () => Browser.SetProxy( Observer.APIObserver.Instance.ProxyPort ) );
				Observer.APIObserver.Instance.ProxyStarted += () => {
					Task.Run( () => Browser.SetProxy( Observer.APIObserver.Instance.ProxyPort ) );
				};

				if ( Utility.Configuration.Config.FormBrowser.IsEnabled )
					NavigateToLogInPage();
			} ) );
		}

		void TerminateBrowserProcess() {
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

		void BrowserChannel_Faulted( object sender, EventArgs e ) {
			( (IClientChannel)Browser ).Abort();
			Browser = null;
			TerminateBrowserProcess();
			Utility.Logger.Add( 3, "ブラウザプロセスが予期せず終了しました" );
		}

		private void FormBrowserHost_FormClosed( object sender, FormClosedEventArgs e ) {
			( (IClientChannel)Browser ).Close();
			CommunicationServiceHost.Close();
			TerminateBrowserProcess();
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
