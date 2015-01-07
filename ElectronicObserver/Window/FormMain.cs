using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
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
		public event EventHandler SystemShuttingDown = delegate { };

		#endregion


		#region Forms

		public FormFleet[] fFleet;
		public FormDock fDock;
		public FormArsenal fArsenal;
		public FormHeadquarters fHeadquarters;
		public FormInformation fInformation;
		public FormCompass fCompass;
		public FormLog fLog;
		public FormQuest fQuest;
		public FormBattle fBattle;
		public FormFleetOverview fFleetOverview;
		public FormShipGroup fShipGroup;

		#endregion




		public FormMain() {
			InitializeComponent();
		}



		private void FormMain_Load( object sender, EventArgs e ) {

			Utility.Configuration.Instance.Load();


			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data ) );
			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameJapanese + " を起動しています…" );
			

			this.Text = SoftwareInformation.VersionJapanese;
			Font = Utility.Configuration.Config.UI.MainFont;
			//StripMenu.Font = Font;
			//StripStatus.Font = Font;
			MainDockPanel.Skin.AutoHideStripSkin.TextFont = Font;
			MainDockPanel.Skin.DockPaneStripSkin.TextFont = Font;


			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();


			APIObserver.Instance.Start( Utility.Configuration.Config.Connection.Port );	//fixme


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
			fLog = new FormLog( this );
			fQuest = new FormQuest( this );
			fBattle = new FormBattle( this );
			fFleetOverview = new FormFleetOverview( this );
			fShipGroup = new FormShipGroup( this );

			WindowPlacementManager.LoadWindowPlacement( this, WindowPlacementManager.WindowPlacementConfigPath );
			LoadSubWindowsLayout( @"Settings\layout.xml" );		//fixme: パスの一元化


			ConfigurationChanged();		//設定から初期化


			UIUpdateTimer.Start();

			Utility.Logger.Add( 2, "起動処理が完了しました。" );
		}



		private void ConfigurationChanged() {

			StripMenu_Debug.Enabled = StripMenu_Debug.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}






		private void StripMenu_Debug_LoadAPIFromFile_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
					if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

						if ( dialog.IsResponse ) {
							APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
						}
						if ( dialog.IsRequest ) {
							APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
						}

					}
				}
			}

		}



		private void UIUpdateTimer_Tick( object sender, EventArgs e ) {

			UpdateTimerTick( this, new EventArgs() );


			StripStatus_Clock.Text = DateTime.Now.ToString( "HH:mm:ss" );
		}


		private void FormMain_FormClosing( object sender, FormClosingEventArgs e ) {

			if ( Utility.Configuration.Config.Life.ConfirmOnClosing ) {
				if ( MessageBox.Show( SoftwareInformation.SoftwareNameJapanese + " を終了しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.No ) {
					e.Cancel = true;
					return;
				}
			}


			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameJapanese + " を終了しています…" );

			UIUpdateTimer.Stop();

			SystemShuttingDown( this, new EventArgs() );


			WindowPlacementManager.SaveWindowPlacement( this, WindowPlacementManager.WindowPlacementConfigPath );
			SaveSubWindowsLayout( @"Settings\layout.xml" );		//fixme: パスの一元化

		}

		private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {

			Utility.Configuration.Instance.Save();
			APIObserver.Instance.Stop();
			RecordManager.Instance.Save();
			KCDatabase.Instance.Save();


			Utility.Logger.Add( 2, "終了処理が完了しました。" );

			if ( Utility.Configuration.Config.Log.SaveLogFlag )
				Utility.Logger.Save( @"eolog.log" );

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
				case "Log":
					return fLog;
				case "Quest":
					return fQuest;
				case "Battle":
					return fBattle;
				case "FleetOverview":
					return fFleetOverview;
				case "ShipGroup":
					return fShipGroup;

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
					fLog.Show( MainDockPanel, DockState.Document );
					fLog.DockPanel = null;
					fQuest.Show( MainDockPanel, DockState.Document );
					fQuest.DockPanel = null;
					fBattle.Show( MainDockPanel, DockState.Document );
					fBattle.DockPanel = null;
					fFleetOverview.Show( MainDockPanel, DockState.Document );
					fFleetOverview.DockPanel = null;
					fShipGroup.Show( MainDockPanel, DockState.Document );
					fShipGroup.DockPanel = null;

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
					fLog.Show( MainDockPanel );
					fQuest.Show( MainDockPanel );
					fBattle.Show( MainDockPanel );
					fFleetOverview.Show( MainDockPanel );
					fShipGroup.Show( MainDockPanel );

					foreach ( var x in MainDockPanel.Contents ) {
						x.DockHandler.Hide();
					}

				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "サブウィンドウ レイアウトの復元に失敗しました。" );

			}

		}


		private void SaveSubWindowsLayout( string path ) {

			try {

				string parent = Directory.GetParent( path ).FullName;
				if ( !Directory.Exists( parent ) ) {
					Directory.CreateDirectory( parent );
				}

				MainDockPanel.SaveAsXml( path );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "サブウィンドウ レイアウトの保存に失敗しました。" );
			}

		}



		void Logger_LogAdded( Utility.Logger.LogData data ) {

			StripStatus_Information.Text = data.Message;

		}


		private void StripMenu_Help_Version_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogVersion() ) {
				dialog.ShowDialog();
			}

		}

		private void StripMenu_File_Configuration_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfiguration() ) {
				Utility.Configuration.Instance.GetConfiguration( dialog );

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					Utility.Configuration.Instance.SetConfiguration( dialog );

				}
			}
		}

		private void StripMenu_File_Close_Click( object sender, EventArgs e ) {
			Close();
		}


		private void StripMenu_File_SaveData_Save_Click( object sender, EventArgs e ) {

			RecordManager.Instance.Save();
		}

		private void StripMenu_File_SaveData_Load_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "セーブしていないレコードが失われる可能性があります。\r\nロードしますか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				RecordManager.Instance.Load();
			}

		}



		private void StripMenu_Debug_LoadInitialAPI_Click( object sender, EventArgs e ) {

			using ( OpenFileDialog ofd = new OpenFileDialog() ) {

				ofd.Title = "APIリストをロード";
				ofd.Filter = "API List|*.txt|File|*";
				ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					try {

						string parent = Path.GetDirectoryName( ofd.FileName );


						using ( StreamReader sr = new StreamReader( ofd.FileName ) ) {
							string line;
							while ( ( line = sr.ReadLine() ) != null ) {

								bool isRequest = false;
								{
									int slashindex = line.IndexOf( '/' );
									if ( slashindex != -1 ) {

										switch ( line.Substring( 0, slashindex ).ToLower() ) {
											case "q":
											case "request":
												isRequest = true;
												goto case "s";
											case "":
											case "s":
											case "response":
												line = line.Substring( Math.Min( slashindex + 1, line.Length ) );
												break;
										}

									}
								}

								if ( APIObserver.Instance.APIList.ContainsKey( line ) ) {
									APIBase api = APIObserver.Instance.APIList[line];

									if ( isRequest ? api.IsRequestSupported : api.IsResponseSupported ) {

										string[] files = Directory.GetFiles( parent, string.Format( "*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace( '/', '@' ) ), SearchOption.TopDirectoryOnly );

										if ( files.Length == 0 )
											continue;

										Array.Sort( files );

										using ( StreamReader sr2 = new StreamReader( files[files.Length - 1] ) ) {
											if ( isRequest )
												APIObserver.Instance.LoadRequest( "/kcsapi/" + line, sr2.ReadToEnd() );
											else
												APIObserver.Instance.LoadResponse( "/kcsapi/" + line, sr2.ReadToEnd() );
										}

										//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
									}
								}
							}

						}


					} catch ( Exception ex ) {

						MessageBox.Show( "API読み込みに失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );

					}

				}

			}

		}


		private void StripMenu_Debug_LoadRecordFromOld_Click( object sender, EventArgs e ) {

			if ( KCDatabase.Instance.MasterShips.Count == 0 ) {
				MessageBox.Show( "先に通常の api_start2 を読み込んでください。", "大変ご迷惑をおかけしております", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}


			using ( OpenFileDialog ofd = new OpenFileDialog() ) {

				ofd.Title = "旧 api_start2 からレコードを構築";
				ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					try {

						using ( StreamReader sr = new StreamReader( ofd.FileName ) ) {

							dynamic json = DynamicJson.Parse( sr.ReadToEnd().Remove( 0, 7 ) );

							foreach ( dynamic elem in json.api_data.api_mst_ship ) {
								if ( elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey( (int)elem.api_id ) && KCDatabase.Instance.MasterShips[(int)elem.api_id].Name == elem.api_name ) {
									RecordManager.Instance.ShipParameter.UpdateParameter( (int)elem.api_id, 1, (int)elem.api_tais[0], (int)elem.api_tais[1], (int)elem.api_kaih[0], (int)elem.api_kaih[1], (int)elem.api_saku[0], (int)elem.api_saku[1] );

									int[] defaultslot = Enumerable.Repeat( -1, 5 ).ToArray();
									( (int[])elem.api_defeq ).CopyTo( defaultslot, 0 );
									RecordManager.Instance.ShipParameter.UpdateDefaultSlot( (int)elem.api_id, defaultslot );
								}
							}
						}

					} catch ( Exception ex ) {

						MessageBox.Show( "API読み込みに失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );
					}
				}
			}
		}



		private void StripMenu_Tool_AlbumMasterShip_Click( object sender, EventArgs e ) {

			if ( KCDatabase.Instance.MasterShips.Count == 0 ) {
				MessageBox.Show( "艦船データが読み込まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

			} else {
				new DialogAlbumMasterShip().Show();
			}

		}

		private void StripMenu_Tool_AlbumMasterEquipment_Click( object sender, EventArgs e ) {

			if ( KCDatabase.Instance.MasterEquipments.Count == 0 ) {
				MessageBox.Show( "装備データが読み込まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

			} else {
				new DialogAlbumMasterEquipment().Show();
			}

		}


		private void StripMenu_Debug_DeleteOldAPI_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "古いAPIデータを削除します。\r\n本当によろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				try {

					//適当極まりない

					string[] files = Directory.GetFiles( Utility.Configuration.Config.Connection.SaveDataPath, "*.json", SearchOption.TopDirectoryOnly );

					var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

					foreach ( string s in files ) {

						int start = s.IndexOf( '@' );
						int end = s.LastIndexOf( '.' );

						start--;
						string key = s.Substring( start, end - start + 1 );
						string date = s.Substring( 0, start );


						if ( !apilist.ContainsKey( key ) ) {
							apilist.Add( key, new List<KeyValuePair<string, string>>() );
						}
						apilist[key].Add( new KeyValuePair<string, string>( date, s ) );
					}

					foreach ( var l in apilist.Values ) {
						var l2 = l.OrderBy( el => el.Key ).ToList();
						for ( int i = 0; i < l2.Count - 1; i++ )
							File.Delete( l2[i].Value );
						//System.Diagnostics.Debug.WriteLine( l2[i].Value );
					}


					MessageBox.Show( "削除が完了しました。", "削除成功", MessageBoxButtons.OK, MessageBoxIcon.Information );

				} catch ( Exception ex ) {

					MessageBox.Show( "削除に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}


			}

		}


		private void StripMenu_Tool_EquipmentList_Click( object sender, EventArgs e ) {

			new DialogEquipmentList().Show();

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

		private void StripMenu_View_Log_Click( object sender, EventArgs e ) {
			fLog.Show( MainDockPanel );
		}

		private void StripMenu_View_Quest_Click( object sender, EventArgs e ) {
			fQuest.Show( MainDockPanel );
		}

		private void StripMenu_View_Battle_Click( object sender, EventArgs e ) {
			fBattle.Show( MainDockPanel );
		}

		private void StripMenu_View_FleetOverview_Click( object sender, EventArgs e ) {
			fFleetOverview.Show( MainDockPanel );
		}

		private void StripMenu_View_ShipGroup_Click( object sender, EventArgs e ) {
			fShipGroup.Show( MainDockPanel );
		}

		#endregion

		

	}
}
