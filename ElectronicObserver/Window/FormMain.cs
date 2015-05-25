using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormMain : Form {

		#region Properties

		public DockPanel MainPanel { get { return MainDockPanel; } }
		public FormWindowCapture WindowCapture { get { return fWindowCapture; } }

		#endregion


		#region Forms

		public List<DockContent> SubForms { get; private set; }

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
		public FormBrowserHost fBrowser;
		public FormWindowCapture fWindowCapture;

		#endregion




		public FormMain() {
            SuspendLayout();
			this.BackColor = Utility.Configuration.Config.UI.BackColor.ColorData;
			this.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;

			InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
        }

		private async void FormMain_Load( object sender, EventArgs e ) {

			//Utility.Configuration.Instance.Load();

			Utility.Modify.ModifyConfiguration.Instance.LoadSettings();


			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => {
				if ( InvokeRequired ) {
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data );
				} else {
					Logger_LogAdded( data );
				}
			} );
			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameJapanese + " を起動しています…" );


			this.Text = SoftwareInformation.VersionJapanese + "（迷彩型）";

			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();
			NotifierManager.Instance.Initialize( this );


			Icon = ResourceManager.Instance.AppIcon;

			APIObserver.Instance.Start( Utility.Configuration.Config.Connection.Port, this );


			MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();


			SubForms = new List<DockContent>();

			//form init
			//注：一度全てshowしないとイベントを受け取れないので注意	
			fFleet = new FormFleet[4];
			for ( int i = 0; i < fFleet.Length; i++ ) {
				SubForms.Add( fFleet[i] = new FormFleet( this, i + 1 ) );
			}

			SubForms.Add( fDock = new FormDock( this ) );
			SubForms.Add( fArsenal = new FormArsenal( this ) );
			SubForms.Add( fHeadquarters = new FormHeadquarters( this ) );
			SubForms.Add( fInformation = new FormInformation( this ) );
			SubForms.Add( fCompass = new FormCompass( this ) );
			SubForms.Add( fLog = new FormLog( this ) );
			SubForms.Add( fQuest = new FormQuest( this ) );
			SubForms.Add( fBattle = new FormBattle( this ) );
			SubForms.Add( fFleetOverview = new FormFleetOverview( this ) );
			SubForms.Add( fShipGroup = new FormShipGroup( this ) );
			SubForms.Add( fBrowser = new FormBrowserHost( this ) );
			SubForms.Add( fWindowCapture = new FormWindowCapture( this ) );

			LoadLayout( Configuration.Config.Life.LayoutFilePath );

			ConfigurationChanged();		//設定から初期化

			SoftwareInformation.CheckUpdate();

			// デバッグ: 開始時にAPIリストを読み込む
			if ( Configuration.Config.Debug.LoadAPIListOnLoad ) {

				try {

					await Task.Factory.StartNew( () => LoadAPIList( Configuration.Config.Debug.APIListPath ) );

				} catch ( Exception ex ) {

					Utility.Logger.Add( 3, "API読み込みに失敗しました。" + ex.Message );
				}
			}

			// 完了通知（ログインページを開く）
			fBrowser.InitializeApiCompleted();

			UIUpdateTimer.Start();

			Utility.Logger.Add( 2, "起動処理が完了しました。" );
		}



		private void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			StripMenu_Debug.Enabled = StripMenu_Debug.Visible = c.Debug.EnableDebugMenu;
			StripStatus.Visible = c.Life.ShowStatusBar;

			TopMost = c.Life.TopMost;

			Font = c.UI.MainFont;
			//StripMenu.Font = Font;
			StripStatus.Font = Font;
			MainDockPanel.Skin.AutoHideStripSkin.TextFont = Font;
			MainDockPanel.Skin.DockPaneStripSkin.TextFont = Font;

			MainDockPanel.AllowEndUserDocking =
			MainDockPanel.AllowSplitterDrag = !c.Life.IsLocked;
			StripMenu_File_Layout_Lock.Checked = c.Life.IsLocked;

			// color theme
			foreach ( var f in SubForms ) {
				if ( f is FormShipGroup ) {
					f.BackColor = SystemColors.Control;
					f.ForeColor = SystemColors.ControlText;
				} else {
					f.BackColor = this.BackColor;
					f.ForeColor = this.ForeColor;
				}
			}
		}






		private void StripMenu_Debug_LoadAPIFromFile_Click( object sender, EventArgs e ) {

			/*/
			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
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
			/*/
			new DialogLocalAPILoader2().Show( this );
			//*/
		}



		private void UIUpdateTimer_Tick( object sender, EventArgs e ) {

			SystemEvents.OnUpdateTimerTick();

			// 東京標準時で表示
			DateTime now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId( DateTime.UtcNow, "Tokyo Standard Time" );
			StripStatus_Clock.Text = now.ToString( "HH:mm:ss" );
			StripStatus_Clock.ToolTipText = now.ToString( "yyyy/MM/dd (ddd)" );
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

			fBrowser.CloseBrowser();

			if ( !Directory.Exists( "Settings" ) )
				Directory.CreateDirectory( "Settings" );

			SystemEvents.OnSystemShuttingDown();


			SaveLayout( Configuration.Config.Life.LayoutFilePath );

		}

		private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {

			NotifierManager.Instance.ApplyToConfiguration();
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
				case "Browser":
					return fBrowser;
				case "WindowCapture":
					return fWindowCapture;
				default:
					if ( persistString.StartsWith( FormIntegrate.PREFIX ) ) {
						return FormIntegrate.FromPersistString( this, persistString );
					}
					return null;
			}
		}



		private void LoadSubWindowsLayout( Stream stream ) {

			try {

				if ( stream != null ) {

					// 取り込んだウィンドウは一旦デタッチして閉じる
					fWindowCapture.CloseAll();

					foreach ( var f in SubForms ) {
						f.Show( MainDockPanel, DockState.Document );
						f.DockPanel = null;
					}

					MainDockPanel.LoadFromXml( stream, new DeserializeDockContent( GetDockContentFromPersistString ) );

					/* 一度全ウィンドウを読み込むことでフォームを初期化する
					foreach ( var x in MainDockPanel.Contents ) {
						if ( x.DockHandler.DockState == DockState.Hidden ) {
							x.DockHandler.Show( MainDockPanel );
							x.DockHandler.Hide();
						} else {
							x.DockHandler.Activate();
						}
					}
					//*/

					// checkme: このコードの存在意義
					/*/
					if ( MainDockPanel.Contents.Count > 0 )
						MainDockPanel.Contents.First().DockHandler.Activate();
					//*/

					fWindowCapture.AttachAll();

				} else {

					foreach ( var f in SubForms )
						f.Show( MainDockPanel );


					foreach ( var x in MainDockPanel.Contents ) {
						x.DockHandler.Hide();
					}
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "サブウィンドウ レイアウトの復元に失敗しました。" );
			}

		}


		private void SaveSubWindowsLayout( Stream stream ) {

			try {

				MainDockPanel.SaveAsXml( stream, Encoding.UTF8 );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "サブウィンドウ レイアウトの保存に失敗しました。" );
			}

		}



		private void LoadLayout( string path ) {

			try {

				using ( var stream = File.OpenRead( path ) ) {

					using ( var archive = new ZipArchive( stream, ZipArchiveMode.Read ) ) {

						WindowPlacementManager.LoadWindowPlacement( this, archive.GetEntry( "WindowPlacement.xml" ).Open() );
						LoadSubWindowsLayout( archive.GetEntry( "SubWindowLayout.xml" ).Open() );

					}
				}


				Utility.Logger.Add( 2, "ウィンドウ レイアウトを復元しました。" );

			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "ウィンドウ レイアウト ファイルは存在しません。" ) );
				MessageBox.Show( "レイアウトが初期化されました。\r\n「表示」メニューからお好みのウィンドウを追加してください。", "ウィンドウ レイアウト ファイルが存在しません",
					MessageBoxButtons.OK, MessageBoxIcon.Information );

				fBrowser.Show( MainDockPanel );

			} catch ( DirectoryNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "ウィンドウ レイアウト ファイルは存在しません。" ) );
				MessageBox.Show( "レイアウトが初期化されました。\r\n「表示」メニューからお好みのウィンドウを追加してください。", "ウィンドウ レイアウト ファイルが存在しません",
					MessageBoxButtons.OK, MessageBoxIcon.Information );

				fBrowser.Show( MainDockPanel );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ レイアウトの復元に失敗しました。" );
			}

		}

		private void SaveLayout( string path ) {

			try {

				CreateParentDirectories( path );

				using ( var stream = File.Open( path, FileMode.Create ) )
				using ( var archive = new ZipArchive( stream, ZipArchiveMode.Create ) ) {

					using ( var layoutstream = archive.CreateEntry( "SubWindowLayout.xml" ).Open() ) {
						SaveSubWindowsLayout( layoutstream );
					}
					using ( var placementstream = archive.CreateEntry( "WindowPlacement.xml" ).Open() ) {
						WindowPlacementManager.SaveWindowPlacement( this, placementstream );
					}
				}


				Utility.Logger.Add( 2, "ウィンドウ レイアウトを保存しました。" );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ レイアウトの保存に失敗しました。" );
			}

		}

		private void CreateParentDirectories( string path ) {

			var parents = Path.GetDirectoryName( path );

			if ( !String.IsNullOrEmpty( parents ) ) {
				Directory.CreateDirectory( parents );
			}

		}



		void Logger_LogAdded( Utility.Logger.LogData data ) {

			StripStatus_Information.Text = data.Message;

		}


		private void StripMenu_Help_Version_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogVersion() ) {
				dialog.ShowDialog( this );
			}

		}

		private void StripMenu_File_Configuration_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfiguration( Utility.Configuration.Config ) ) {
				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					dialog.ToConfiguration( Utility.Configuration.Config );
					Utility.Configuration.Instance.OnConfigurationChanged();

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



		private async void StripMenu_Debug_LoadInitialAPI_Click( object sender, EventArgs e ) {

			using ( OpenFileDialog ofd = new OpenFileDialog() ) {

				ofd.Title = "APIリストをロード";
				ofd.Filter = "API List|*.txt|File|*";
				ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					try {

						await Task.Factory.StartNew( () => LoadAPIList( ofd.FileName ) );

					} catch ( Exception ex ) {

						MessageBox.Show( "API読み込みに失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );

					}

				}

			}

		}



		private void LoadAPIList( string path ) {

			string parent =  Path.GetDirectoryName( path );

			using ( StreamReader sr = new StreamReader( path ) ) {
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
								if ( isRequest ) {
									Invoke( (Action)( () => {
										APIObserver.Instance.LoadRequest( "/kcsapi/" + line, sr2.ReadToEnd() );
									} ) );
								} else {
									Invoke( (Action)( () => {
										APIObserver.Instance.LoadResponse( "/kcsapi/" + line, sr2.ReadToEnd() );
									} ) );
								}
							}

							//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
						}
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
				new DialogAlbumMasterShip().Show( this );
			}

		}

		private void StripMenu_Tool_AlbumMasterEquipment_Click( object sender, EventArgs e ) {

			if ( KCDatabase.Instance.MasterEquipments.Count == 0 ) {
				MessageBox.Show( "装備データが読み込まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

			} else {
				new DialogAlbumMasterEquipment().Show( this );
			}

		}

        private void StripMenu_Tool_ResourcesGraph_Click(object sender, EventArgs e)
        {
            try
            {
                new DialogResourcesGraph().Show(this);
            }
            catch (System.ObjectDisposedException)
            {
                //catch and do nothing. window was disposed by inner logic.
            }
        }

		private void StripMenu_Tool_CopyEOBrowserExecute_Click( object sender, EventArgs e ) {

			var eobrowser = Path.Combine( Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location ), "EOBrowser.exe" );
			var parameter = "net.pipe://localhost/" + System.Diagnostics.Process.GetCurrentProcess().Id + "/ElectronicObserver";

			string path = eobrowser + " \"" + parameter + "\"";

			MessageBox.Show( this, "已复制以下启动参数至剪贴板：\r\n" + path + "\r\n请打开“运行”对话框粘贴执行。", "启动 EOBrowser", MessageBoxButtons.OK, MessageBoxIcon.Information );
			Clipboard.SetText( path );

		}

		private async void StripMenu_Debug_DeleteOldAPI_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "古いAPIデータを削除します。\r\n本当によろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				try {

					int count = await Task.Factory.StartNew( () => DeleteOldAPI() );

					MessageBox.Show( "削除が完了しました。\r\n" + count + " 個のファイルを削除しました。", "削除成功", MessageBoxButtons.OK, MessageBoxIcon.Information );

				} catch ( Exception ex ) {

					MessageBox.Show( "削除に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}


			}

		}

		private int DeleteOldAPI() {


			//適当極まりない
			int count = 0;

			var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

			foreach ( string s in Directory.EnumerateFiles( Utility.Configuration.Config.Connection.SaveDataPath, "*.json", SearchOption.TopDirectoryOnly ) ) {

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
				for ( int i = 0; i < l2.Count - 1; i++ ) {
					File.Delete( l2[i].Value );
					count++;
				}
			}

			return count;
		}



		private void StripMenu_Tool_EquipmentList_Click( object sender, EventArgs e ) {

			new DialogEquipmentList().Show( this );

		}


		private async void StripMenu_Debug_RenameShipResource_Click( object sender, EventArgs e ) {

			if ( KCDatabase.Instance.MasterShips.Count == 0 ) {
				MessageBox.Show( "艦船データが読み込まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			if ( MessageBox.Show( "通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
				"対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
				"続行しますか？", "艦船リソースをリネーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
				== System.Windows.Forms.DialogResult.Yes ) {

				string path = null;

				using ( FolderBrowserDialog dialog = new FolderBrowserDialog() ) {
					dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
					if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
						path = dialog.SelectedPath;
					}
				}

				if ( path == null ) return;



				try {

					int count = await Task.Factory.StartNew( () => RenameShipResource( path ) );

					MessageBox.Show( string.Format( "リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count ), "処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information );


				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "艦船リソースのリネームに失敗しました。" );
					MessageBox.Show( "艦船リソースのリネームに失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

				}



			}

		}


		private int RenameShipResource( string path ) {

			int count = 0;

			foreach ( var p in Directory.EnumerateFiles( path, "*", SearchOption.AllDirectories ) ) {

				string name = Path.GetFileName( p );

				foreach ( var ship in KCDatabase.Instance.MasterShips.Values ) {

					if ( name.Contains( ship.ResourceName ) ) {

						name = name.Replace( ship.ResourceName, ship.NameWithClass ).Replace( ' ', '_' );

						try {

							File.Move( p, Path.Combine( Path.GetDirectoryName( p ), name ) );
							count++;
							break;

						} catch ( IOException ) {
							//ファイルが既に存在する：＊にぎりつぶす＊
						}

					}

				}

			}

			foreach ( var p in Directory.EnumerateDirectories( path, "*", SearchOption.AllDirectories ) ) {

				string name = Path.GetFileName( p );		//GetDirectoryName だと親フォルダへのパスになってしまうため

				foreach ( var ship in KCDatabase.Instance.MasterShips.Values ) {

					if ( name.Contains( ship.ResourceName ) ) {

						name = name.Replace( ship.ResourceName, ship.NameWithClass ).Replace( ' ', '_' );

						try {

							Directory.Move( p, Path.Combine( Path.GetDirectoryName( p ), name ) );
							count++;
							break;

						} catch ( IOException ) {
							//フォルダが既に存在する：＊にぎりつぶす＊
						}
					}

				}

			}


			return count;
		}


		private void StripMenu_Help_Help_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "外部ブラウザでオンラインヘルプを開きます。\r\nよろしいですか？", "ヘルプ",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
				== System.Windows.Forms.DialogResult.Yes ) {

				System.Diagnostics.Process.Start( "https://github.com/andanteyk/ElectronicObserver/wiki" );
			}

		}


		private void SeparatorWhitecap_Click( object sender, EventArgs e ) {
			new DialogWhitecap().Show( this );
		}



		private void StripMenu_File_Layout_Load_Click( object sender, EventArgs e ) {

			LoadLayout( Utility.Configuration.Config.Life.LayoutFilePath );

		}

		private void StripMenu_File_Layout_Save_Click( object sender, EventArgs e ) {

			SaveLayout( Utility.Configuration.Config.Life.LayoutFilePath );

		}

		private void StripMenu_File_Layout_Lock_Click( object sender, EventArgs e ) {

			bool locked = !StripMenu_File_Layout_Lock.Checked;

			MainDockPanel.AllowEndUserDocking =
			MainDockPanel.AllowSplitterDrag = !locked;

			Utility.Configuration.Config.Life.IsLocked = locked;

			StripMenu_File_Layout_Lock.Checked = locked;

		}

		private void StripMenu_File_Layout_Open_Click( object sender, EventArgs e ) {

			using ( var dialog = new OpenFileDialog() ) {

				dialog.Filter = "Layout Archive|*.zip|File|*";
				dialog.Title = "レイアウト ファイルを開く";


				PathHelper.InitOpenFileDialog( Utility.Configuration.Config.Life.LayoutFilePath, dialog );

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					Utility.Configuration.Config.Life.LayoutFilePath = PathHelper.GetPathFromOpenFileDialog( dialog );
					LoadLayout( Utility.Configuration.Config.Life.LayoutFilePath );

				}

			}

		}




		private void StripMenu_Browser_ScreenShot_Click( object sender, EventArgs e ) {

			fBrowser.SaveScreenShot();

		}

		private void StripMenu_Browser_Refresh_Click( object sender, EventArgs e ) {

			fBrowser.RefreshBrowser();

		}

		private void StripMenu_Browser_NavigateToLogInPage_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "ログインページへ移動します。\r\nよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question )
				== System.Windows.Forms.DialogResult.Yes ) {

				fBrowser.NavigateToLogInPage();
			}
		}

		private void StripMenu_Browser_Navigate_Click( object sender, EventArgs e ) {

			using ( var dialog = new Window.Dialog.DialogTextInput( "移動先の入力", "移動先の URL を入力してください。" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					fBrowser.Navigate( dialog.InputtedText );
				}
			}
		}


		private void StripMenu_Browser_Zoom_Decr20_Click( object sender, EventArgs e ) {

			Utility.Configuration.Config.FormBrowser.ZoomRate =
				Math.Max( Utility.Configuration.Config.FormBrowser.ZoomRate - 20, 10 );

			fBrowser.ApplyZoom();
		}

		private void StripMenu_Browser_Zoom_Incr20_Click( object sender, EventArgs e ) {

			Utility.Configuration.Config.FormBrowser.ZoomRate =
				Math.Min( Utility.Configuration.Config.FormBrowser.ZoomRate + 20, 1000 );

			fBrowser.ApplyZoom();
		}


		private void StripMenu_Browser_Zoom_Click( object sender, EventArgs e ) {

			int zoom;

			if ( sender == StripMenu_Browser_Zoom_25 )
				zoom = 25;
			else if ( sender == StripMenu_Browser_Zoom_50 )
				zoom = 50;
			else if ( sender == StripMenu_Browser_Zoom_75 )
				zoom = 75;
			else if ( sender == StripMenu_Browser_Zoom_100 )
				zoom = 100;
			else if ( sender == StripMenu_Browser_Zoom_150 )
				zoom = 150;
			else if ( sender == StripMenu_Browser_Zoom_200 )
				zoom = 200;
			else if ( sender == StripMenu_Browser_Zoom_250 )
				zoom = 250;
			else if ( sender == StripMenu_Browser_Zoom_300 )
				zoom = 300;
			else if ( sender == StripMenu_Browser_Zoom_400 )
				zoom = 400;
			else
				zoom = 100;

			Utility.Configuration.Config.FormBrowser.ZoomRate = zoom;

			fBrowser.ApplyZoom();
		}

		private void StripMenu_Browser_Zoom_DropDownOpening( object sender, EventArgs e ) {

			StripMenu_Browser_Zoom_Current.Text = string.Format( "現在: {0}%",
				Utility.Configuration.Config.FormBrowser.ZoomRate );

		}

		private void StripMenu_Browser_AppliesStyleSheet_CheckedChanged( object sender, EventArgs e ) {

			Utility.Configuration.Config.FormBrowser.AppliesStyleSheet = StripMenu_Browser_AppliesStyleSheet.Checked;
			fBrowser.ConfigurationChanged();

		}

		private void StripMenu_Browser_DropDownOpening( object sender, EventArgs e ) {

			StripMenu_Browser_AppliesStyleSheet.Checked = Utility.Configuration.Config.FormBrowser.AppliesStyleSheet;
		}

		private void StripMenu_WindowCapture_AttachAll_Click( object sender, EventArgs e ) {
			fWindowCapture.AttachAll();
		}

		private void StripMenu_WindowCapture_DetachAll_Click( object sender, EventArgs e ) {
			fWindowCapture.DetachAll();
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

		private void StripMenu_View_Browser_Click( object sender, EventArgs e ) {
			fBrowser.Show( MainDockPanel );
		}

		private void StripMenu_WindowCapture_SubWindow_Click( object sender, EventArgs e ) {
			fWindowCapture.Show( MainDockPanel );
		}

		#endregion




	}
}
