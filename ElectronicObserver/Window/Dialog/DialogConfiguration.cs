using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
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

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogConfiguration : Form {

		private static readonly string RegistryPathMaster = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";
		private static readonly string RegistryPathBrowserVersion = @"FEATURE_BROWSER_EMULATION\";
		private static readonly string RegistryPathGPURendering = @"FEATURE_GPU_RENDERING\";

		private static readonly int DefaultBrowserVersion = 7000;
		private static readonly bool DefaultGPURendering = false;


		public DialogConfiguration() {
			InitializeComponent();

		}

		public DialogConfiguration( Configuration.ConfigurationData config )
			: this() {

			FromConfiguration( config );
		}


		private void Connection_SaveReceivedData_CheckedChanged( object sender, EventArgs e ) {

			Connection_PanelSaveData.Enabled = Connection_SaveReceivedData.Checked;

		}


		private void Connection_SaveDataPath_TextChanged( object sender, EventArgs e ) {

			if ( Directory.Exists( Connection_SaveDataPath.Text ) ) {
				Connection_SaveDataPath.BackColor = SystemColors.Window;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, null );
			} else {
				Connection_SaveDataPath.BackColor = Color.MistyRose;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, "指定されたフォルダは存在しません。" );
			}
		}


		/// <summary>
		/// パラメータの更新をUIに適用します。
		/// </summary>
		internal void UpdateParameter() {

			Connection_SaveReceivedData_CheckedChanged( null, new EventArgs() );
			Connection_SaveDataPath_TextChanged( null, new EventArgs() );
			Debug_EnableDebugMenu_CheckedChanged( null, new EventArgs() );

		}



		private void Connection_SaveDataPathSearch_Click( object sender, EventArgs e ) {

			Connection_SaveDataPath.Text = PathHelper.ProcessFolderBrowserDialog( Connection_SaveDataPath.Text, FolderBrowser );

		}


		private void UI_MainFontSelect_Click( object sender, EventArgs e ) {

			FontSelector.Font = UI_MainFont.Font;

			if ( FontSelector.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

				SerializableFont font = new SerializableFont( FontSelector.Font );

				UI_MainFont.Text = font.SerializeFontAttribute;
				UI_MainFont.Font = font.FontData;

			}

		}


		private void UI_SubFontSelect_Click( object sender, EventArgs e ) {

			FontSelector.Font = UI_SubFont.Font;

			if ( FontSelector.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

				SerializableFont font = new SerializableFont( FontSelector.Font );

				UI_SubFont.Text = font.SerializeFontAttribute;
				UI_SubFont.Font = font.FontData;

			}

		}


		private void DialogConfiguration_Load( object sender, EventArgs e ) {

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration] );

		}

		private void DialogConfiguration_FormClosed( object sender, FormClosedEventArgs e ) {

			ResourceManager.DestroyIcon( Icon );

		}


		private void UI_MainFontApply_Click( object sender, EventArgs e ) {

			UI_MainFont.Font = SerializableFont.StringToFont( UI_MainFont.Text ) ?? UI_MainFont.Font;
		}

		private void UI_SubFontApply_Click( object sender, EventArgs e ) {

			UI_SubFont.Font = SerializableFont.StringToFont( UI_SubFont.Text ) ?? UI_SubFont.Font;
		}




		//ui
		private void Connection_OutputConnectionScript_Click( object sender, EventArgs e ) {

			string serverAddress = APIObserver.Instance.ServerAddress;
			if ( serverAddress == null ) {
				MessageBox.Show( "艦これに接続してから操作してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}

			using ( var dialog = new SaveFileDialog() ) {
				dialog.Filter = "Proxy Script|*.pac|File|*";
				dialog.Title = "自動プロキシ設定スクリプトを保存する";
				dialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
				dialog.FileName = System.IO.Directory.GetCurrentDirectory() + "\\proxy.pac";

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					try {

						using ( StreamWriter sw = new StreamWriter( dialog.FileName ) ) {

							sw.WriteLine( "function FindProxyForURL(url, host) {" );
							sw.WriteLine( "  if (/^" + serverAddress.Replace( ".", @"\." ) + "/.test(host)) {" );
							sw.WriteLine( "    return \"PROXY localhost:{0}; DIRECT\";", (int)Connection_Port.Value );
							sw.WriteLine( "  }" );
							sw.WriteLine( "  return \"DIRECT\";" );
							sw.WriteLine( "}" );

						}

						Clipboard.SetData( DataFormats.StringFormat, "file:///" + dialog.FileName.Replace( '\\', '/' ) );

						MessageBox.Show( "自動プロキシ設定スクリプトを保存し、設定用URLをクリップボードにコピーしました。\r\n所定の位置に貼り付けてください。",
							"作成完了", MessageBoxButtons.OK, MessageBoxIcon.Information );


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "自動プロキシ設定スクリプトの保存に失敗しました。" );
						MessageBox.Show( "自動プロキシ設定スクリプトの保存に失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );

					}

				}
			}

		}



		private void Notification_Expedition_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Expedition ) ) {
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Construction_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Construction ) ) {
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Repair_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Repair ) ) {
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Condition_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Condition ) ) {
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Damage_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Damage ) ) {
				dialog.ShowDialog( this );
			}
		}


		private void Life_LayoutFilePathSearch_Click( object sender, EventArgs e ) {

			Life_LayoutFilePath.Text = PathHelper.ProcessOpenFileDialog( Life_LayoutFilePath.Text, LayoutFileBrowser );

		}


		private void Debug_APIListPathSearch_Click( object sender, EventArgs e ) {

			Debug_APIListPath.Text = PathHelper.ProcessOpenFileDialog( Debug_APIListPath.Text, APIListBrowser );

		}


		private void Debug_EnableDebugMenu_CheckedChanged( object sender, EventArgs e ) {

			Debug_SealingPanel.Visible = Debug_EnableDebugMenu.Checked;
		}


		private void FormBrowser_ScreenShotPathSearch_Click( object sender, EventArgs e ) {

			FormBrowser_ScreenShotPath.Text = PathHelper.ProcessFolderBrowserDialog( FormBrowser_ScreenShotPath.Text, FolderBrowser );
		}





		/// <summary>
		/// 設定からUIを初期化します。
		/// </summary>
		public void FromConfiguration( Configuration.ConfigurationData config ) {

			//[通信]
			Connection_Port.Value = config.Connection.Port;
			Connection_SaveReceivedData.Checked = config.Connection.SaveReceivedData;
			Connection_SaveDataFilter.Text = config.Connection.SaveDataFilter;
			Connection_SaveDataPath.Text = config.Connection.SaveDataPath;
			Connection_SaveRequest.Checked = config.Connection.SaveRequest;
			Connection_SaveResponse.Checked = config.Connection.SaveResponse;
			Connection_SaveSWF.Checked = config.Connection.SaveSWF;
			Connection_SaveOtherFile.Checked = config.Connection.SaveOtherFile;
			Connection_ApplyVersion.Checked = config.Connection.ApplyVersion;
			Connection_RegisterAsSystemProxy.Checked = config.Connection.RegisterAsSystemProxy;
			Connection_UseUpstreamProxy.Checked = config.Connection.UseUpstreamProxy;
			Connection_UpstreamProxyPort.Value = config.Connection.UpstreamProxyPort;

			//[UI]
			UI_MainFont.Font = config.UI.MainFont.FontData;
			UI_MainFont.Text = config.UI.MainFont.SerializeFontAttribute;
			UI_SubFont.Font = config.UI.SubFont.FontData;
			UI_SubFont.Text = config.UI.SubFont.SerializeFontAttribute;

			//[ログ]
			Log_LogLevel.Value = config.Log.LogLevel;
			Log_SaveLogFlag.Checked = config.Log.SaveLogFlag;
			Log_SaveErrorReport.Checked = config.Log.SaveErrorReport;
			Log_FileEncodingID.SelectedIndex = config.Log.FileEncodingID;
			Log_ShowSpoiler.Checked = config.Log.ShowSpoiler;

			//[動作]
			Control_ConditionBorder.Value = config.Control.ConditionBorder;

			//[デバッグ]
			Debug_EnableDebugMenu.Checked = config.Debug.EnableDebugMenu;
			Debug_LoadAPIListOnLoad.Checked = config.Debug.LoadAPIListOnLoad;
			Debug_APIListPath.Text = config.Debug.APIListPath;

			//[起動と終了]
			Life_ConfirmOnClosing.Checked = config.Life.ConfirmOnClosing;
			Life_TopMost.Checked = this.TopMost = config.Life.TopMost;		//メインウィンドウに隠れないように
			Life_LayoutFilePath.Text = config.Life.LayoutFilePath;
			Life_CheckUpdateInformation.Checked = config.Life.CheckUpdateInformation;
			Life_ShowStatusBar.Checked = config.Life.ShowStatusBar;

			//[サブウィンドウ]
			FormArsenal_ShowShipName.Checked = config.FormArsenal.ShowShipName;

			FormFleet_ShowAircraft.Checked = config.FormFleet.ShowAircraft;
			FormFleet_SearchingAbilityMethod.SelectedIndex = config.FormFleet.SearchingAbilityMethod;
			FormFleet_IsScrollable.Checked = config.FormFleet.IsScrollable;
			FormFleet_FixShipNameWidth.Checked = config.FormFleet.FixShipNameWidth;
			FormFleet_ShortenHPBar.Checked = config.FormFleet.ShortenHPBar;
			FormFleet_ShowNextExp.Checked = config.FormFleet.ShowNextExp;

			FormQuest_ShowRunningOnly.Checked = config.FormQuest.ShowRunningOnly;
			FormQuest_ShowOnce.Checked = config.FormQuest.ShowOnce;
			FormQuest_ShowDaily.Checked = config.FormQuest.ShowDaily;
			FormQuest_ShowWeekly.Checked = config.FormQuest.ShowWeekly;
			FormQuest_ShowMonthly.Checked = config.FormQuest.ShowMonthly;

			FormShipGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
			FormShipGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;

			FormBrowser_IsEnabled.Checked = config.FormBrowser.IsEnabled;
			FormBrowser_ZoomRate.Value = config.FormBrowser.ZoomRate;
			FormBrowser_ZoomFit.Checked = config.FormBrowser.ZoomFit;
			FormBrowser_LogInPageURL.Text = config.FormBrowser.LogInPageURL;
			FormBrowser_ScreenShotFormat_JPEG.Checked = config.FormBrowser.ScreenShotFormat == 1;
			FormBrowser_ScreenShotFormat_PNG.Checked = config.FormBrowser.ScreenShotFormat == 2;
			FormBrowser_ScreenShotPath.Text = config.FormBrowser.ScreenShotPath;
			FormBrowser_ConfirmAtRefresh.Checked = config.FormBrowser.ConfirmAtRefresh;
			FormBrowser_AppliesStyleSheet.Checked = config.FormBrowser.AppliesStyleSheet;
			{
				Microsoft.Win32.RegistryKey reg = null;
				try {

					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathBrowserVersion );
					if ( reg == null ) {
						FormBrowser_BrowserVersion.Text = DefaultBrowserVersion.ToString();

					} else {
						FormBrowser_BrowserVersion.Text = ( reg.GetValue( FormBrowserHost.BrowserExeName ) ?? DefaultBrowserVersion ).ToString();
					}
					reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathGPURendering );
					if ( reg == null ) {
						FormBrowser_GPURendering.Checked = DefaultGPURendering;

					} else {
						int? gpu = reg.GetValue( FormBrowserHost.BrowserExeName ) as int?;
						FormBrowser_GPURendering.Checked = gpu != null ? gpu != 0 : DefaultGPURendering;
					}

				} catch ( Exception ex ) {

					FormBrowser_BrowserVersion.Text = DefaultBrowserVersion.ToString();
					FormBrowser_GPURendering.Checked = DefaultGPURendering;

					Utility.Logger.Add( 3, "レジストリからの読み込みに失敗しました。" + ex.Message );

				} finally {
					if ( reg != null )
						reg.Close();

				}
			}
			FormBrowser_FlashQuality.Text = config.FormBrowser.FlashQuality;
			FormBrowser_FlashWMode.Text = config.FormBrowser.FlashWMode;

			//finalize
			UpdateParameter();
		}



		/// <summary>
		/// UIをもとに設定を適用します。
		/// </summary>
		public void ToConfiguration( Configuration.ConfigurationData config ) {

			//[通信]
			{
				bool changed = false;

				changed |= config.Connection.Port != (ushort)Connection_Port.Value;
				config.Connection.Port = (ushort)Connection_Port.Value;

				config.Connection.SaveReceivedData = Connection_SaveReceivedData.Checked;
				config.Connection.SaveDataFilter = Connection_SaveDataFilter.Text;
				config.Connection.SaveDataPath = Connection_SaveDataPath.Text.Trim( @"\ """.ToCharArray() );
				config.Connection.SaveRequest = Connection_SaveRequest.Checked;
				config.Connection.SaveResponse = Connection_SaveResponse.Checked;
				config.Connection.SaveSWF = Connection_SaveSWF.Checked;
				config.Connection.SaveOtherFile = Connection_SaveOtherFile.Checked;
				config.Connection.ApplyVersion = Connection_ApplyVersion.Checked;

				changed |= config.Connection.RegisterAsSystemProxy != Connection_RegisterAsSystemProxy.Checked;
				config.Connection.RegisterAsSystemProxy = Connection_RegisterAsSystemProxy.Checked;

				config.Connection.UseUpstreamProxy = Connection_UseUpstreamProxy.Checked;
				config.Connection.UpstreamProxyPort = (ushort)Connection_UpstreamProxyPort.Value;

				if ( changed ) {
					APIObserver.Instance.Stop();
					APIObserver.Instance.Start( config.Connection.Port, this );
				}
			}

			//[UI]
			config.UI.MainFont = UI_MainFont.Font;
			config.UI.SubFont = UI_SubFont.Font;

			//[ログ]
			config.Log.LogLevel = (int)Log_LogLevel.Value;
			config.Log.SaveLogFlag = Log_SaveLogFlag.Checked;
			config.Log.SaveErrorReport = Log_SaveErrorReport.Checked;
			config.Log.FileEncodingID = Log_FileEncodingID.SelectedIndex;
			config.Log.ShowSpoiler = Log_ShowSpoiler.Checked;

			//[動作]
			config.Control.ConditionBorder = (int)Control_ConditionBorder.Value;

			//[デバッグ]
			config.Debug.EnableDebugMenu = Debug_EnableDebugMenu.Checked;
			config.Debug.LoadAPIListOnLoad = Debug_LoadAPIListOnLoad.Checked;
			config.Debug.APIListPath = Debug_APIListPath.Text;

			//[起動と終了]
			config.Life.ConfirmOnClosing = Life_ConfirmOnClosing.Checked;
			config.Life.TopMost = Life_TopMost.Checked;
			config.Life.LayoutFilePath = Life_LayoutFilePath.Text;
			config.Life.CheckUpdateInformation = Life_CheckUpdateInformation.Checked;
			config.Life.ShowStatusBar = Life_ShowStatusBar.Checked;

			//[サブウィンドウ]
			config.FormArsenal.ShowShipName = FormArsenal_ShowShipName.Checked;

			config.FormFleet.ShowAircraft = FormFleet_ShowAircraft.Checked;
			config.FormFleet.SearchingAbilityMethod = FormFleet_SearchingAbilityMethod.SelectedIndex;
			config.FormFleet.IsScrollable = FormFleet_IsScrollable.Checked;
			config.FormFleet.FixShipNameWidth = FormFleet_FixShipNameWidth.Checked;
			config.FormFleet.ShortenHPBar = FormFleet_ShortenHPBar.Checked;
			config.FormFleet.ShowNextExp = FormFleet_ShowNextExp.Checked;

			config.FormQuest.ShowRunningOnly = FormQuest_ShowRunningOnly.Checked;
			config.FormQuest.ShowOnce = FormQuest_ShowOnce.Checked;
			config.FormQuest.ShowDaily = FormQuest_ShowDaily.Checked;
			config.FormQuest.ShowWeekly = FormQuest_ShowWeekly.Checked;
			config.FormQuest.ShowMonthly = FormQuest_ShowMonthly.Checked;

			config.FormShipGroup.AutoUpdate = FormShipGroup_AutoUpdate.Checked;
			config.FormShipGroup.ShowStatusBar = FormShipGroup_ShowStatusBar.Checked;

			config.FormBrowser.IsEnabled = FormBrowser_IsEnabled.Checked;
			config.FormBrowser.ZoomRate = (int)FormBrowser_ZoomRate.Value;
			config.FormBrowser.ZoomFit = FormBrowser_ZoomFit.Checked;
			config.FormBrowser.LogInPageURL = FormBrowser_LogInPageURL.Text;
			if ( FormBrowser_ScreenShotFormat_JPEG.Checked )
				config.FormBrowser.ScreenShotFormat = 1;
			else
				config.FormBrowser.ScreenShotFormat = 2;
			config.FormBrowser.ScreenShotPath = FormBrowser_ScreenShotPath.Text;
			config.FormBrowser.ConfirmAtRefresh = FormBrowser_ConfirmAtRefresh.Checked;
			config.FormBrowser.AppliesStyleSheet = FormBrowser_AppliesStyleSheet.Checked;
			config.FormBrowser.FlashQuality = FormBrowser_FlashQuality.Text;
			config.FormBrowser.FlashWMode = FormBrowser_FlashWMode.Text;

		}


		private void FormBrowser_ApplyRegistry_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "レジストリに登録します。よろしいですか？\r\n＊完全に適用するには再起動が必要です。", "確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				Microsoft.Win32.RegistryKey reg = null;

				try {
					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( RegistryPathMaster + RegistryPathBrowserVersion );
					reg.SetValue( FormBrowserHost.BrowserExeName, int.Parse( FormBrowser_BrowserVersion.Text ), Microsoft.Win32.RegistryValueKind.DWord );
					reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( RegistryPathMaster + RegistryPathGPURendering );
					reg.SetValue( FormBrowserHost.BrowserExeName, FormBrowser_GPURendering.Checked ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord );

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "レジストリへの書き込みに失敗しました。" );
					MessageBox.Show( "レジストリへの書き込みに失敗しました。\r\n" + ex.Message, "エラー",
						MessageBoxButtons.OK, MessageBoxIcon.Error );

				} finally {
					if ( reg != null )
						reg.Close();
				}
			}

		}

		private void FormBrowser_DeleteRegistry_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "レジストリを削除します。よろしいですか？\r\n＊完全に適用するには再起動が必要です。", "確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				Microsoft.Win32.RegistryKey reg = null;

				try {
					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathBrowserVersion, true );
					reg.DeleteValue( FormBrowserHost.BrowserExeName );
					reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathGPURendering, true );
					reg.DeleteValue( FormBrowserHost.BrowserExeName );

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "レジストリの削除に失敗しました。" );
					MessageBox.Show( "レジストリの削除に失敗しました。\r\n" + ex.Message, "エラー",
						MessageBoxButtons.OK, MessageBoxIcon.Error );

				} finally {
					if ( reg != null )
						reg.Close();
				}
			}
		}

	}
}
