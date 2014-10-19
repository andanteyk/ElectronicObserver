using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.SaveData;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormMain : Form {

		#region Properties
		#endregion


		#region Events

		public event EventHandler UpdateTimerTick = delegate { };

		#endregion


		#region Forms

		public FormFleet[] fFleet;
		public FormDock fDock;
		public FormArsenal fArsenal;
		public FormHeadquarters fHeadquarters;
		public FormInformation fInformation;
		public FormCompass fCompass;

		#endregion




		public FormMain() {
			InitializeComponent();
		}



		private void FormMain_Load( object sender, EventArgs e ) {

			ElectronicObserver.Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data ) );


			Utility.Logger.Add( 1, "七四式電子観測儀 を起動しています…" );


			ResourceManager.Instance.Load();
			SaveDataMaster.Instance.Load();


			APIObserver.Instance.Start( 40620 );


			MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();

			//form init
			//注：一度全てshowしないとイベントを受け取れないので注意	
			fFleet = new FormFleet[4];
			for ( int i = 0; i < fFleet.Length; i++ ) {
				fFleet[i] = new FormFleet( this, i + 1 );
			}
	
			fDock = new FormDock( this );
			
			fArsenal = new FormArsenal( this );
			
			fHeadquarters = new FormHeadquarters( this );

			fInformation = new FormInformation( this );

			fCompass = new FormCompass( this );


			WindowPlacementManager.LoadWindowPlacement( this, WindowPlacementManager.WindowPlacementConfigPath );
			LoadSubWindowsLayout( @"Settings\layout.xml" );		//fixme: パスの一元化


			UIUpdateTimer.Start();

			Utility.Logger.Add( 1, "起動処理が完了しました。" );
		}

		




		private void StripMenu_Debug_LoadAPIFromFile_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					if ( dialog.IsResponse ) {
						APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
					}
					if ( dialog.IsRequest ) {
						APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
					}

				}

			}

		}



		private void UIUpdateTimer_Tick( object sender, EventArgs e ) {

			UpdateTimerTick( this, new EventArgs() );


			{
				DateTime now = DateTime.Now;
				StripStatus_Clock.Text = string.Format( "{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second );
			}
		}


		private void FormMain_FormClosing( object sender, FormClosingEventArgs e ) {
			//todo: 後々「終了しますか？」表示をつける


			Utility.Logger.Add( 1, "七四式電子観測儀 を終了しています…" );

			WindowPlacementManager.SaveWindowPlacement( this, WindowPlacementManager.WindowPlacementConfigPath );
			SaveSubWindowsLayout( @"Settings\layout.xml" );		//fixme: パスの一元化

		}

		private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {

			APIObserver.Instance.Stop();
			SaveDataMaster.Instance.Save();

			Utility.Logger.Add( 1, "終了処理が完了しました。" );
		}



		private IDockContent GetDockContentFromPersistString( string persistString ) {

			switch ( persistString ) {
				case "Fleet #1":
					return fFleet[0];
				case "Fleet #2":
					return fFleet[1];
				case "Fleet #3":
					return fFleet[2];
				case "Fleet #4":
					return fFleet[3];
				case "Dock":
					return fDock;
				case "Arsenal":
					return fArsenal;
				case "HeadQuarters":
					return fHeadquarters;
				case "Information":
					return fInformation;
				case "Compass":
					return fCompass;
				default:
					return null;
			}
		}


		private void LoadSubWindowsLayout( string path ) {

			try {

				if ( File.Exists( path ) ) {

					for ( int i = 0; i < fFleet.Length; i++ ) {
						fFleet[i].Show( MainDockPanel, DockState.Document );
						fFleet[i].DockPanel = null;
					}
					fDock.Show( MainDockPanel, DockState.Document );
					fDock.DockPanel = null;
					fArsenal.Show( MainDockPanel, DockState.Document );
					fArsenal.DockPanel = null;
					fHeadquarters.Show( MainDockPanel, DockState.Document );
					fHeadquarters.DockPanel = null;
					fInformation.Show( MainDockPanel, DockState.Document );
					fInformation.DockPanel = null;
					fCompass.Show( MainDockPanel, DockState.Document );
					fCompass.DockPanel = null;

					MainDockPanel.LoadFromXml( path, new DeserializeDockContent( GetDockContentFromPersistString ) );

					//一度全ウィンドウを読み込むことでフォームを初期化する
					foreach ( var x in MainDockPanel.Contents ) {
						if ( x.DockHandler.DockState == DockState.Hidden ) {
							x.DockHandler.Show( MainDockPanel );
							x.DockHandler.Hide();
						} else {
							x.DockHandler.Activate();
						}
						
					}

					if ( MainDockPanel.Contents.Count > 0 )
						MainDockPanel.Contents.First().DockHandler.Activate();


				} else {

					//とりあえず全ウィンドウを表示してから隠しておく
					for ( int i = 0; i < fFleet.Length; i++ ) {
						fFleet[i].Show( MainDockPanel );
					}
					fDock.Show( MainDockPanel );
					fArsenal.Show( MainDockPanel );
					fHeadquarters.Show( MainDockPanel );
					fInformation.Show( MainDockPanel );
					fCompass.Show( MainDockPanel );

					foreach ( var x in MainDockPanel.Contents ) {
						x.DockHandler.Hide();
					}

				}

			} catch ( Exception e ) {

				Utility.Logger.Add( 3, "サブウィンドウ レイアウトの復元に失敗しました。\r\n" + e.Message );

			}

		}


		private void SaveSubWindowsLayout( string path ) {

			try {

				string parent = Directory.GetParent( path ).FullName;
				if ( !Directory.Exists( parent ) ) {
					Directory.CreateDirectory( parent );
				}

				MainDockPanel.SaveAsXml( path );

			} catch ( Exception e ) {
				
				Utility.Logger.Add( 3, "サブウィンドウ レイアウトの保存に失敗しました。\r\n" + e.Message );
			}

		}



		void Logger_LogAdded( Utility.Logger.LogData data ) {

			StripStatus_Information.Text = data.Message;

		}




		#region フォーム表示
		
		private void StripMenu_View_Fleet_1_Click( object sender, EventArgs e ) {
			fFleet[0].Show( MainDockPanel );
		}

		private void StripMenu_View_Fleet_2_Click( object sender, EventArgs e ) {
			fFleet[1].Show( MainDockPanel );
		}

		private void StripMenu_View_Fleet_3_Click( object sender, EventArgs e ) {
			fFleet[2].Show( MainDockPanel ); 
		}

		private void StripMenu_View_Fleet_4_Click( object sender, EventArgs e ) {
			fFleet[3].Show( MainDockPanel );
		}

		private void StripMenu_View_Dock_Click( object sender, EventArgs e ) {
			fDock.Show( MainDockPanel );
		}

		private void StripMenu_View_Arsenal_Click( object sender, EventArgs e ) {
			fArsenal.Show( MainDockPanel );
		}

		private void StripMenu_View_Headquarters_Click( object sender, EventArgs e ) {
			fHeadquarters.Show( MainDockPanel );
		}

		private void StripMenu_View_Information_Click( object sender, EventArgs e ) {
			fInformation.Show( MainDockPanel );
		}

		private void StripMenu_View_Compass_Click( object sender, EventArgs e ) {
			fCompass.Show( MainDockPanel );
		}

		#endregion

	

		



	

		

	}
}
