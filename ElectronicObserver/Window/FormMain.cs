using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Plugins;
using ElectronicObserver.Window.Support;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
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

		public List<IPluginHost> Plugins { get; private set; }

		public FormFleet[] fFleet;
		public FormShipGroup fShipGroup;
		public FormBrowserHost fBrowser;
		public FormWindowCapture fWindowCapture;

		public Form Browser { get { return fBrowser; } }

		#endregion


		private const string LAYOUT_FILE1 = @"Settings\WindowLayout.zip";
		private const string LAYOUT_FILE2 = @"Settings\WindowLayout2.zip";

		public FormMain() {
			this.BackColor = Utility.Configuration.Config.UI.BackColor.ColorData;
			this.ForeColor = Utility.Configuration.Config.UI.ForeColor.ColorData;

			InitializeComponent();
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

			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameJapanese + " 开始启动…" );


			this.Text = SoftwareInformation.VersionJapanese + "（迷彩型）";

			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();


			Icon = ResourceManager.Instance.AppIcon;
			StripMenu_Tool_PluginManager.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];

			APIObserver.Instance.Start( Utility.Configuration.Config.Connection.Port, this );


			MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();


			SubForms = new List<DockContent>();

			//form init
			//注：一度全てshowしないとイベントを受け取れないので注意	
			fFleet = new FormFleet[4];
			for ( int i = 0; i < fFleet.Length; i++ ) {
				SubForms.Add( fFleet[i] = new FormFleet( this, i + 1 ) );
			}

			SubForms.Add( fShipGroup = new FormShipGroup( this ) );
			SubForms.Add( fBrowser = new FormBrowserHost( this ) );
			SubForms.Add( fWindowCapture = new FormWindowCapture( this ) );

			var path = this.GetType().Assembly.Location;
			path = path.Substring( 0, path.LastIndexOf( '\\' ) + 1 ) + "Plugins";
			if ( Directory.Exists( path ) )
			{
				bool flag = false;
				Plugins = new List<IPluginHost>();

				foreach ( var file in Directory.GetFiles( path, "*.dll", SearchOption.TopDirectoryOnly ) )
				{
					try
					{
						var assembly = Assembly.LoadFile( file );
						var pluginType = assembly.GetExportedTypes().FirstOrDefault( t => t.GetInterface( typeof(IPluginHost).FullName ) != null );
						if ( pluginType != null )
						{
							var plugin = (IPluginHost)assembly.CreateInstance( pluginType.FullName );
							Plugins.Add( plugin );

							if ( !flag )
							{
								var sep = new ToolStripSeparator();
								StripMenu_View.DropDownItems.Add( sep );
								flag = true;
							}

							if ( plugin.PluginType == PluginType.DockContent )
							{
								List<DockContent> plugins = new List<DockContent>();
								foreach ( var type in assembly.GetTypes().Where( t => t.BaseType == typeof( DockContent ) ) )
								{
									if ( type != null )
									{
										var form = (DockContent)type.GetConstructor( new[] { typeof( FormMain ) } ).Invoke( new object[] { this } );
										plugins.Add( form );
									}
								}
								if ( plugins.Count == 1 )
								{
									var p = plugins[0];
									var item = new ToolStripMenuItem
									{
										Text = plugin.MenuTitle ?? p.Text,
										Tag = p
									};
									item.Click += menuitem_Click;
									StripMenu_View.DropDownItems.Add( item );
								}
								else if ( plugins.Count > 1 )
								{
									var item = new ToolStripMenuItem
									{
										Text = plugin.MenuTitle ?? plugins.First().Text
									};
									foreach ( var p in plugins )
									{
										var subItem = new ToolStripMenuItem
										{
											Text = p.Text,
											Tag = p
										};
										subItem.Click += menuitem_Click;
										item.DropDownItems.Add( subItem );
									}
									StripMenu_View.DropDownItems.Add( item );
								}

								SubForms.AddRange( plugins );
							}

							// service
							else if ( plugin.PluginType == PluginType.Service )
							{
								if ( await plugin.RunService( this ) )
								{
									Utility.Logger.Add( 2, string.Format( "服务 {0}({1}) 已加载。", plugin.MenuTitle, plugin.Version ) );
								}
								else
								{
									Utility.Logger.Add( 3, string.Format( "服务 {0}({1}, {2}) 加载时返回异常结果。", plugin.MenuTitle, plugin.Version, pluginType.Name ) );
								}
							}

							// dialog
							else if ( plugin.PluginType == PluginType.Dialog )
							{
								var item = new ToolStripMenuItem
								{
									Text = plugin.MenuTitle,
									Tag = plugin
								};
								item.Click += dialogPlugin_Click;
								StripMenu_Tool.DropDownItems.Add( item );
							}
						}
					}
					catch ( ReflectionTypeLoadException refEx )
					{
						Utility.ErrorReporter.SendLoadErrorReport( refEx, "载入插件时出错：" + file.Substring( file.LastIndexOf( '\\' ) + 1 ) );
					}
					catch ( Exception ex )
					{
						Utility.ErrorReporter.SendErrorReport( ex, "载入插件时出错：" + file.Substring( file.LastIndexOf( '\\' ) + 1 ) );
					}
				}
			}

			// layout
			LoadLayout( Configuration.Config.Life.LayoutFilePath );

			string lower = Configuration.Config.Life.LayoutFilePath.ToLower();
			if ( lower == LAYOUT_FILE1.ToLower() ) {
				StripMenu_File_Layout1.Checked = true;
			} else if ( lower == LAYOUT_FILE2.ToLower() ) {
				StripMenu_File_Layout2.Checked = true;
			}

			ConfigurationChanged();		//設定から初期化

			SoftwareInformation.CheckUpdate();

			// 完了通知（ログインページを開く）
			fBrowser.InitializeApiCompleted();

			UIUpdateTimer.Start();

			Utility.Logger.Add( 2, "启动处理完毕。" );
		}


		void dialogPlugin_Click( object sender, EventArgs e )
		{
			var plugin = (IPluginHost)( (ToolStripMenuItem)sender ).Tag;
			if ( plugin != null )
			{
				try
				{
					plugin.GetToolWindow().Show( this );
				}
				catch ( ObjectDisposedException ) { }
				catch ( Exception ex )
				{
					Utility.ErrorReporter.SendErrorReport( ex, string.Format( "插件显示出错：{0}({1})", plugin.MenuTitle, plugin.Version ) );
				}
			}
		}


		void menuitem_Click( object sender, EventArgs e )
		{
			var f = ((ToolStripMenuItem)sender).Tag as DockContent;
			if ( f != null )
			{
				f.Show( this.MainDockPanel );
			}
		}



		private void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

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


			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameJapanese + " 即将结束…" );

			UIUpdateTimer.Stop();

			fBrowser.CloseBrowser();

			if ( !Directory.Exists( "Settings" ) )
				Directory.CreateDirectory( "Settings" );

			SystemEvents.OnSystemShuttingDown();


			SaveLayout( Configuration.Config.Life.LayoutFilePath );

		}

		private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {

			// TODO 不确定影响
			//NotifierManager.Instance.ApplyToConfiguration();
			Utility.Configuration.Instance.Save();
			APIObserver.Instance.Stop();
			RecordManager.Instance.Save();
			KCDatabase.Instance.Save();


			Utility.Logger.Add( 2, "结束处理完毕。" );

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
				case "ShipGroup":
					return fShipGroup;
				case "Browser":
					return fBrowser;
				case "WindowCapture":
					return fWindowCapture;
				default:
					if ( persistString.StartsWith( FormIntegrate.PREFIX ) )
					{
						return FormIntegrate.FromPersistString( this, persistString );
					}
					else
					{
						var form = SubForms.FirstOrDefault( f => f.GetPersistString() == persistString );
						if ( form != null )
						{
							return form;
						}
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

				Utility.ErrorReporter.SendErrorReport( ex, "子窗口布局恢复失败。" );
			}

		}


		private void SaveSubWindowsLayout( Stream stream ) {

			try {

				MainDockPanel.SaveAsXml( stream, Encoding.UTF8 );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "子窗口布局保存失败。" );
			}

		}



		private bool LoadLayout( string path ) {

			try {

				using ( var stream = File.OpenRead( path ) ) {

					using ( var archive = new ZipFile( stream ) ) {

						WindowPlacementManager.LoadWindowPlacement( this, archive.GetInputStream( archive.GetEntry( "WindowPlacement.xml" ) ) );
						LoadSubWindowsLayout( archive.GetInputStream( archive.GetEntry( "SubWindowLayout.xml" ) ) );

					}
				}


				Utility.Logger.Add( 2, "窗口布局已恢复。" );
				return true;

			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "窗口布局文件不存在。" ) );
				MessageBox.Show( "已初始化布局。\r\n请在「显示」菜单中添加想要的窗口。", "窗口布局文件不存在",
					MessageBoxButtons.OK, MessageBoxIcon.Information );

				fBrowser.Show( MainDockPanel );

			} catch ( DirectoryNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "窗口布局文件不存在。" ) );
				MessageBox.Show( "已初始化布局。\r\n请在「显示」菜单中添加想要的窗口。", "窗口布局文件不存在",
					MessageBoxButtons.OK, MessageBoxIcon.Information );

				fBrowser.Show( MainDockPanel );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "窗口布局恢复失败。" );
			}

			return false;
		}

		private void SaveLayout( string path ) {

			try {

				CreateParentDirectories( path );

				using ( var stream = File.Open( path, FileMode.Create ) )
				using ( var zipStream = new ZipOutputStream( stream ) ) {

					var entry = new ZipEntry( "SubWindowLayout.xml" );
					entry.DateTime = DateTime.Now;
					zipStream.PutNextEntry( entry );
					SaveSubWindowsLayout( zipStream );

					entry = new ZipEntry( "WindowPlacement.xml" );
					entry.DateTime = DateTime.Now;
					zipStream.PutNextEntry( entry );
					WindowPlacementManager.SaveWindowPlacement( this, zipStream );

					zipStream.CloseEntry();

				}


				Utility.Logger.Add( 2, "窗口布局已保存。" );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "窗口布局保存失败。" );
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

		private void StripMenu_Tool_PluginManager_Click( object sender, EventArgs e )
		{
			new DialogPlugins( this ).Show( this );
		}

		private void StripMenu_Tool_CopyEOBrowserExecute_Click( object sender, EventArgs e ) {

			var eobrowser = Path.Combine( Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location ), "EOBrowser.exe" );
			var parameter = "net.pipe://localhost/" + System.Diagnostics.Process.GetCurrentProcess().Id + "/ElectronicObserver";

			string path = eobrowser + " \"" + parameter + "\"";

			MessageBox.Show( this, "已复制以下启动参数至剪贴板：\r\n" + path + "\r\n请打开“运行”对话框粘贴执行。", "启动 EOBrowser", MessageBoxButtons.OK, MessageBoxIcon.Information );
			Clipboard.SetText( path );

		}



		private void StripMenu_Tool_EquipmentList_Click( object sender, EventArgs e ) {

			new DialogEquipmentList().Show( this );

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

		private void StripMenu_Tool_ResourceChart_Click( object sender, EventArgs e ) {

			new Dialog.DialogResourceChart().Show( this );

		}

		private bool SwitchLayout( string path )
		{
			if ( !File.Exists( path ) ) {
				MessageBox.Show( string.Format( "布局文件 {0} 不存在。", path ), "切换布局", MessageBoxButtons.OK, MessageBoxIcon.Warning );
				return false;
			}


			Utility.Configuration.Config.Life.LayoutFilePath = path;
			return LoadLayout( path );
		}

		private void StripMenu_File_Layout1_Click( object sender, EventArgs e )
		{
			if ( SwitchLayout( LAYOUT_FILE1 ) ) {
				StripMenu_File_Layout1.Checked = true;
				StripMenu_File_Layout2.Checked = false;
			}
		}

		private void StripMenu_File_Layout2_Click( object sender, EventArgs e )
		{
			if ( SwitchLayout( LAYOUT_FILE2 ) ) {
				StripMenu_File_Layout1.Checked = false;
				StripMenu_File_Layout2.Checked = true;
			}
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
