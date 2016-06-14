using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
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

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogConfiguration : Form {

		private static readonly string RegistryPathMaster = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";
		private static readonly string RegistryPathBrowserVersion = @"FEATURE_BROWSER_EMULATION\";
		private static readonly string RegistryPathGPURendering = @"FEATURE_GPU_RENDERING\";

		private static readonly int DefaultBrowserVersion = 7000;
		private static readonly bool DefaultGPURendering = false;


		private System.Windows.Forms.Control _UIControl;

		private Dictionary<SyncBGMPlayer.SoundHandleID, SyncBGMPlayer.SoundHandle> BGMHandles;




		public DialogConfiguration() {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			// 加载索敌式列表
			FormFleet_SearchingAbilityMethod.Items.AddRange( Configuration.Config.FormFleet.SearchingAbilities.Split( ';' ) );

			this.ResumeLayoutForDpiScale();
		}

		public DialogConfiguration( Configuration.ConfigurationData config )
			: this() {

			FromConfiguration( config );
		}


		private void Connection_SaveReceivedData_CheckedChanged( object sender, EventArgs e ) {

			Connection_PanelSaveData.Enabled = Connection_SaveReceivedData.Checked;

		}

		private void Connection_UseUpstreamProxy_CheckedChanged( object sender, EventArgs e ) {

		}

		private void Connection_EnableSslUpstreamProxy_CheckedChanged( object sender, EventArgs e ) {

			Connection_UpstreamProxySSLHost.Visible =
			Connection_UpstreamProxySSLPort.Visible = Connection_EnableSslUpstreamProxy.Checked;

			if ( Connection_EnableSslUpstreamProxy.Checked ) {
				Connection_UseUpstreamProxy.Checked = true;
			}

		}


		private void Connection_SaveDataPath_TextChanged( object sender, EventArgs e ) {

			if ( Directory.Exists( Connection_SaveDataPath.Text ) ) {
				Connection_SaveDataPath.BackColor = SystemColors.Window;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, null );
			} else {
				Connection_SaveDataPath.BackColor = Color.MistyRose;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, "指定的文件夹不存在。" );
			}
		}


		/// <summary>
		/// パラメータの更新をUIに適用します。
		/// </summary>
		internal void UpdateParameter() {

			Connection_SaveReceivedData_CheckedChanged( null, new EventArgs() );
			Connection_SaveDataPath_TextChanged( null, new EventArgs() );

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

			_UIControl = Owner;

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





		private void Life_LayoutFilePathSearch_Click( object sender, EventArgs e ) {

			Life_LayoutFilePath.Text = PathHelper.ProcessOpenFileDialog( Life_LayoutFilePath.Text, LayoutFileBrowser );

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
			Connection_SaveDataPath.Text = config.Connection.SaveDataPath;
			Connection_SaveRequest.Checked = config.Connection.SaveRequest;
			Connection_SaveResponse.Checked = config.Connection.SaveResponse;
			Connection_SaveSWF.Checked = config.Connection.SaveSWF;
			Connection_SaveOtherFile.Checked = config.Connection.SaveOtherFile;
			Connection_ApplyVersion.Checked = config.Connection.ApplyVersion;
			Connection_RegisterAsSystemProxy.Checked = config.Connection.RegisterAsSystemProxy;
			Connection_EnableSslUpstreamProxy.Checked = config.Connection.EnableSslUpstreamProxy;
			Connection_UseUpstreamProxy.Checked = config.Connection.UseUpstreamProxy;
			Connection_UpstreamProxyHost.Text = config.Connection.UpstreamProxyAddress;
			Connection_UpstreamProxyPort.Value = config.Connection.UpstreamProxyPort;
            if ( config.Connection.UpstreamProxyPortSSL == 0 ) {
                Connection_UpstreamProxySSLHost.Text = config.Connection.UpstreamProxyAddress;
                Connection_UpstreamProxySSLPort.Value = config.Connection.UpstreamProxyPort;
            } else {
                Connection_UpstreamProxySSLHost.Text = config.Connection.UpstreamProxyAddressSSL;
                Connection_UpstreamProxySSLPort.Value = config.Connection.UpstreamProxyPortSSL;
            }
			Connection_UpstreamProxySSLHost.Visible =
			Connection_UpstreamProxySSLPort.Visible = config.Connection.EnableSslUpstreamProxy;


			//[UI]
			UI_MainFont.Font = config.UI.MainFont.FontData;
			UI_MainFont.Text = config.UI.MainFont.SerializeFontAttribute;
			UI_SubFont.Font = config.UI.SubFont.FontData;
			UI_SubFont.Text = config.UI.SubFont.SerializeFontAttribute;
			UI_BarColorMorphing.Checked = config.UI.BarColorMorphing;

			comboUITheme.SelectedIndex = config.UI.ThemeID;
			colorBackColor.SelectedColor = config.UI.BackColor.ColorData;
			colorForeColor.SelectedColor = config.UI.ForeColor.ColorData;
			colorSubForeColor.SelectedColor = config.UI.SubForeColor.ColorData;
			colorHightlightColor.SelectedColor = config.UI.HighlightColor.ColorData;
			colorHightlightForeColor.SelectedColor = config.UI.HighlightForeColor.ColorData;
			colorLineColor.SelectedColor = config.UI.LineColor.ColorData;
			colorButtonBackColor.SelectedColor = config.UI.ButtonBackColor.ColorData;

			colorFailedColor.SelectedColor = config.UI.FailedColor.ColorData;
			colorEliteColor.SelectedColor = config.UI.EliteColor.ColorData;
			colorFlagshipColor.SelectedColor = config.UI.FlagshipColor.ColorData;
			colorLateModelEliteColor.SelectedColor = config.UI.LateModelEliteColor.ColorData;
			colorLateModelFlagshipColor.SelectedColor = config.UI.LateModelFlagshipColor.ColorData;
			colorLateModelColor.SelectedColor = config.UI.LateModelColor.ColorData;

			colorHp0Color.SelectedColor = config.UI.Hp0Color.ColorData;
			colorHp25Color.SelectedColor = config.UI.Hp25Color.ColorData;
			colorHp50Color.SelectedColor = config.UI.Hp50Color.ColorData;
			colorHp75Color.SelectedColor = config.UI.Hp75Color.ColorData;
			colorHp100Color.SelectedColor = config.UI.Hp100Color.ColorData;
			colorHpIncrementColor.SelectedColor = config.UI.HpIncrementColor.ColorData;
			colorDecrementColor.SelectedColor = config.UI.HpDecrementColor.ColorData;
			colorHpBackgroundColor.SelectedColor = config.UI.HpBackgroundColor.ColorData;
			numericHpBackgroundOffset.Value = config.UI.HpBackgroundOffset;
			numericHpThickness.Value = config.UI.HpThickness;

			colorFleetReadyColor.SelectedColor = config.UI.FleetReadyColor.ColorData;
			colorFleetExpeditionColor.SelectedColor = config.UI.FleetExpeditionColor.ColorData;
			colorFleetSortieColor.SelectedColor = config.UI.FleetSortieColor.ColorData;
			colorFleetNotReadyColor.SelectedColor = config.UI.FleetNotReadyColor.ColorData;
			colorFleetDamageColor.SelectedColor = config.UI.FleetDamageColor.ColorData;

			colorQuestOrganization.SelectedColor = config.UI.QuestOrganization.ColorData;
			colorQuestSortie.SelectedColor = config.UI.QuestSortie.ColorData;
			colorQuestExercise.SelectedColor = config.UI.QuestExercise.ColorData;
			colorQuestExpedition.SelectedColor = config.UI.QuestExpedition.ColorData;
			colorQuestSupplyDocking.SelectedColor = config.UI.QuestSupplyDocking.ColorData;
			colorQuestArsenal.SelectedColor = config.UI.QuestArsenal.ColorData;
			colorQuestRenovated.SelectedColor = config.UI.QuestRenovated.ColorData;
			colorQuestForeColor.SelectedColor = config.UI.QuestForeColor.ColorData;

			//[ログ]
			Log_LogLevel.Value = config.Log.LogLevel;
			Log_SaveLogFlag.Checked = config.Log.SaveLogFlag;
			Log_SaveErrorReport.Checked = config.Log.SaveErrorReport;
			Log_FileEncodingID.SelectedIndex = config.Log.FileEncodingID;
			Log_ShowSpoiler.Checked = config.Log.ShowSpoiler;
			Log_AutoSave.Checked = config.Log.AutoSave;
			Log_AutoSaveMinutes.Value = config.Log.AutoSaveMinutes;

			//[動作]
			Control_ConditionBorder.Value = config.Control.ConditionBorder;
			Control_RecordAutoSaving.SelectedIndex = config.Control.RecordAutoSaving;
			Control_UseSystemVolume.Checked = config.Control.UseSystemVolume;
			Control_PowerEngagementForm.SelectedIndex = config.Control.PowerEngagementForm - 1;


			//[起動と終了]
			Life_ConfirmOnClosing.Checked = config.Life.ConfirmOnClosing;
			Life_TopMost.Checked = this.TopMost = config.Life.TopMost;		//メインウィンドウに隠れないように
			Life_LayoutFilePath.Text = config.Life.LayoutFilePath;
			Life_CheckUpdateInformation.Checked = config.Life.CheckUpdateInformation;
			Life_ShowStatusBar.Checked = config.Life.ShowStatusBar;
			Life_ClockFormat.SelectedIndex = config.Life.ClockFormat;
			Life_AutoScaleDpi.Checked = config.UI.AutoScaleDpi;
			Life_LockLayout.Checked = config.Life.IsLocked;
			Life_CanCloseFloatWindowInLock.Checked = config.Life.CanCloseFloatWindowInLock;
            Life_ScreenDock.Checked = config.Life.CanScreenDock;

            //[サブウィンドウ]
            FormFleet_ShowAircraft.Checked = config.FormFleet.ShowAircraft;
			FormFleet_SearchingAbilityMethod.SelectedIndex = config.FormFleet.SearchingAbilityMethod;
			FormFleet_IsScrollable.Checked = config.FormFleet.IsScrollable;
			FormFleet_FixShipNameWidth.Checked = config.FormFleet.FixShipNameWidth;
			FormFleet_ShortenHPBar.Checked = config.FormFleet.ShortenHPBar;
			FormFleet_ShowNextExp.Checked = config.FormFleet.ShowNextExp;
			FormFleet_EquipmentLevelVisibility.SelectedIndex = (int)config.FormFleet.EquipmentLevelVisibility;
			FormFleet_BlinkHPBar.Checked = config.UI.NotExpeditionBlink;
			FormFleet_TextProficiency.Checked = config.FormFleet.ShowTextProficiency;
			FormFleet_AirSuperiorityMethod.SelectedIndex = config.FormFleet.AirSuperiorityMethod;
			FormFleet_ShowAnchorageRepairingTimer.Checked = config.FormFleet.ShowAnchorageRepairingTimer;
			FormFleet_BlinkAtCompletion.Checked = config.FormFleet.BlinkAtCompletion;

			FormShipGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
			FormShipGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;
			FormShipGroup_ShipNameSortMethod.SelectedIndex = config.FormShipGroup.ShipNameSortMethod;

			FormBrowser_IsEnabled.Checked = config.FormBrowser.IsEnabled;
			FormBrowser_ZoomRate.Value = config.FormBrowser.ZoomRate;
			FormBrowser_ZoomFit.Checked = config.FormBrowser.ZoomFit;
			FormBrowser_LogInPageURL.Text = config.FormBrowser.LogInPageURL;
			FormBrowser_ScreenShotFormat_JPEG.Checked = config.FormBrowser.ScreenShotFormat == 1;
			FormBrowser_ScreenShotFormat_PNG.Checked = config.FormBrowser.ScreenShotFormat == 2;
			FormBrowser_ScreenShotPath.Text = config.FormBrowser.ScreenShotPath;
			FormBrowser_ConfirmAtRefresh.Checked = config.FormBrowser.ConfirmAtRefresh;
			FormBrowser_AppliesStyleSheet.Checked = config.FormBrowser.AppliesStyleSheet;
			FormBrowser_ShowURL.Checked = config.FormBrowser.ShowURL;
			FormBrowser_ModifyCookieRegion.Checked = config.FormBrowser.ModifyCookieRegion;
			{
				FormBrowser_BrowserVersion.Enabled = false;
				FormBrowser_GPURendering.Enabled = false;

				Microsoft.Win32.RegistryKey reg = null;
				try {

					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathBrowserVersion );
					if ( reg == null ) {
						FormBrowser_BrowserVersion.Text = DefaultBrowserVersion.ToString();

					} else {
						FormBrowser_BrowserVersion.Text = ( reg.GetValue( FormBrowserHost.BrowserExeName ) ?? DefaultBrowserVersion ).ToString();
					}
					FormBrowser_BrowserVersion.Enabled = true;
					if ( reg != null )
						reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( RegistryPathMaster + RegistryPathGPURendering );
					if ( reg == null ) {
						FormBrowser_GPURendering.Checked = DefaultGPURendering;

					} else {
						int? gpu = reg.GetValue( FormBrowserHost.BrowserExeName ) as int?;
						FormBrowser_GPURendering.Checked = gpu != null ? gpu != 0 : DefaultGPURendering;
					}
					FormBrowser_GPURendering.Enabled = true;

				} catch ( Exception ) {

					FormBrowser_BrowserVersion.Text = DefaultBrowserVersion.ToString();
					FormBrowser_GPURendering.Checked = DefaultGPURendering;

					//Utility.Logger.Add( 3, "注册表读取失败。" + ex.Message );

				} finally {
					if ( reg != null )
						reg.Close();

				}

				FormBrowser_ApplyRegistry.Enabled = FormBrowser_DeleteRegistry.Enabled =
					FormBrowser_BrowserVersion.Enabled || FormBrowser_GPURendering.Enabled;
			}
			FormBrowser_FlashQuality.Text = config.FormBrowser.FlashQuality;
			FormBrowser_FlashWMode.Text = config.FormBrowser.FlashWmode;

			FormCompass_CandidateDisplayCount.Value = config.FormCompass.CandidateDisplayCount;

			//[データベース]

			//[BGM]
			BGMPlayer_Enabled.Checked = config.BGMPlayer.Enabled;
			BGMHandles = config.BGMPlayer.Handles.ToDictionary( h => h.HandleID );
			BGMPlayer_SyncBrowserMute.Checked = config.BGMPlayer.SyncBrowserMute;
			UpdateBGMPlayerUI();

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
				config.Connection.SaveDataPath = Connection_SaveDataPath.Text.Trim( @"\ """.ToCharArray() );
				config.Connection.SaveRequest = Connection_SaveRequest.Checked;
				config.Connection.SaveResponse = Connection_SaveResponse.Checked;
				config.Connection.SaveSWF = Connection_SaveSWF.Checked;
				config.Connection.SaveOtherFile = Connection_SaveOtherFile.Checked;
				config.Connection.ApplyVersion = Connection_ApplyVersion.Checked;

				changed |= config.Connection.RegisterAsSystemProxy != Connection_RegisterAsSystemProxy.Checked;
				config.Connection.RegisterAsSystemProxy = Connection_RegisterAsSystemProxy.Checked;

				changed |= config.Connection.UseUpstreamProxy != Connection_UseUpstreamProxy.Checked;
				config.Connection.UseUpstreamProxy = Connection_UseUpstreamProxy.Checked;
				changed |= config.Connection.EnableSslUpstreamProxy != Connection_EnableSslUpstreamProxy.Checked;
				config.Connection.EnableSslUpstreamProxy = Connection_EnableSslUpstreamProxy.Checked;
                if ( config.Connection.EnableSslUpstreamProxy ) {
                    changed |= config.Connection.UpstreamProxyAddressSSL != Connection_UpstreamProxySSLHost.Text;
                    config.Connection.UpstreamProxyAddressSSL = Connection_UpstreamProxySSLPort.Value == 0 ? Connection_UpstreamProxyHost.Text : Connection_UpstreamProxySSLHost.Text;
                    changed |= config.Connection.UpstreamProxyPortSSL != (ushort)Connection_UpstreamProxySSLPort.Value;
                    config.Connection.UpstreamProxyPortSSL = Connection_UpstreamProxySSLPort.Value == 0 ? (ushort)Connection_UpstreamProxyPort.Value : (ushort)Connection_UpstreamProxySSLPort.Value;
                }

                changed |= config.Connection.UpstreamProxyAddress != Connection_UpstreamProxyHost.Text;
				config.Connection.UpstreamProxyAddress = Connection_UpstreamProxyHost.Text;
				changed |= config.Connection.UpstreamProxyPort != (ushort)Connection_UpstreamProxyPort.Value;
				config.Connection.UpstreamProxyPort = (ushort)Connection_UpstreamProxyPort.Value;

				if ( changed ) {
					APIObserver.Instance.Stop();
					APIObserver.Instance.Start( config.Connection.Port, _UIControl );
				}

			}

			//[UI]
			config.UI.MainFont = UI_MainFont.Font;
			config.UI.SubFont = UI_SubFont.Font;
			config.UI.BarColorMorphing = UI_BarColorMorphing.Checked;

			config.UI.ThemeID = comboUITheme.SelectedIndex;
			config.UI.BackColor = colorBackColor.SelectedColor;
			config.UI.ForeColor = colorForeColor.SelectedColor;
			config.UI.SubForeColor = colorSubForeColor.SelectedColor;
			config.UI.HighlightColor = colorHightlightColor.SelectedColor;
			config.UI.HighlightForeColor = colorHightlightForeColor.SelectedColor;
			config.UI.LineColor = colorLineColor.SelectedColor;
			config.UI.ButtonBackColor = colorButtonBackColor.SelectedColor;

			config.UI.FailedColor = colorFailedColor.SelectedColor;
			config.UI.EliteColor = colorEliteColor.SelectedColor;
			config.UI.FlagshipColor = colorFlagshipColor.SelectedColor;
			config.UI.LateModelEliteColor = colorLateModelEliteColor.SelectedColor;
			config.UI.LateModelFlagshipColor = colorLateModelFlagshipColor.SelectedColor;
			config.UI.LateModelColor = colorLateModelColor.SelectedColor;

			config.UI.Hp0Color = colorHp0Color.SelectedColor;
			config.UI.Hp25Color = colorHp25Color.SelectedColor;
			config.UI.Hp50Color = colorHp50Color.SelectedColor;
			config.UI.Hp75Color = colorHp75Color.SelectedColor;
			config.UI.Hp100Color = colorHp100Color.SelectedColor;
			config.UI.HpIncrementColor = colorHpIncrementColor.SelectedColor;
			config.UI.HpDecrementColor = colorDecrementColor.SelectedColor;
			config.UI.HpBackgroundColor = colorHpBackgroundColor.SelectedColor;
			config.UI.HpBackgroundOffset = (int)numericHpBackgroundOffset.Value;
			config.UI.HpThickness = (int)numericHpThickness.Value;

			config.UI.FleetReadyColor = colorFleetReadyColor.SelectedColor;
			config.UI.FleetExpeditionColor = colorFleetExpeditionColor.SelectedColor;
			config.UI.FleetSortieColor = colorFleetSortieColor.SelectedColor;
			config.UI.FleetNotReadyColor = colorFleetNotReadyColor.SelectedColor;
			config.UI.FleetDamageColor = colorFleetDamageColor.SelectedColor;

			config.UI.QuestOrganization = colorQuestOrganization.SelectedColor;
			config.UI.QuestSortie = colorQuestSortie.SelectedColor;
			config.UI.QuestExercise = colorQuestExercise.SelectedColor;
			config.UI.QuestExpedition = colorQuestExpedition.SelectedColor;
			config.UI.QuestSupplyDocking = colorQuestSupplyDocking.SelectedColor;
			config.UI.QuestArsenal = colorQuestArsenal.SelectedColor;
			config.UI.QuestRenovated = colorQuestRenovated.SelectedColor;
			config.UI.QuestForeColor = colorQuestForeColor.SelectedColor;

			//[ログ]
			config.Log.LogLevel = (int)Log_LogLevel.Value;
			config.Log.SaveLogFlag = Log_SaveLogFlag.Checked;
			config.Log.SaveErrorReport = Log_SaveErrorReport.Checked;
			config.Log.FileEncodingID = Log_FileEncodingID.SelectedIndex;
			config.Log.ShowSpoiler = Log_ShowSpoiler.Checked;
			if ( !config.Log.AutoSave && Log_AutoSave.Checked )
			{
				ElectronicObserver.Resource.Record.RecordManager.Instance.Save();
				ElectronicObserver.Data.KCDatabase.Instance.Save();
			}
			config.Log.AutoSave = Log_AutoSave.Checked;
			config.Log.AutoSaveMinutes = (int)Log_AutoSaveMinutes.Value;

			//[動作]
			config.Control.ConditionBorder = (int)Control_ConditionBorder.Value;
			config.Control.RecordAutoSaving = Control_RecordAutoSaving.SelectedIndex;
			config.Control.UseSystemVolume = Control_UseSystemVolume.Checked;
			config.Control.PowerEngagementForm = Control_PowerEngagementForm.SelectedIndex + 1;


			//[起動と終了]
			config.Life.ConfirmOnClosing = Life_ConfirmOnClosing.Checked;
			config.Life.TopMost = Life_TopMost.Checked;
			config.Life.LayoutFilePath = Life_LayoutFilePath.Text;
			config.Life.CheckUpdateInformation = Life_CheckUpdateInformation.Checked;
			config.Life.ShowStatusBar = Life_ShowStatusBar.Checked;
			config.Life.ClockFormat = Life_ClockFormat.SelectedIndex;
			config.UI.AutoScaleDpi = Life_AutoScaleDpi.Checked;
			config.Life.IsLocked = Life_LockLayout.Checked;
			config.Life.CanCloseFloatWindowInLock = Life_CanCloseFloatWindowInLock.Checked;
            config.Life.CanScreenDock = Life_ScreenDock.Checked;

            //[サブウィンドウ]
            config.FormFleet.ShowAircraft = FormFleet_ShowAircraft.Checked;
			config.FormFleet.SearchingAbilityMethod = FormFleet_SearchingAbilityMethod.SelectedIndex;
			config.FormFleet.IsScrollable = FormFleet_IsScrollable.Checked;
			config.FormFleet.FixShipNameWidth = FormFleet_FixShipNameWidth.Checked;
			config.FormFleet.ShortenHPBar = FormFleet_ShortenHPBar.Checked;
			config.FormFleet.ShowNextExp = FormFleet_ShowNextExp.Checked;
			config.FormFleet.EquipmentLevelVisibility = (Window.Control.ShipStatusEquipment.LevelVisibilityFlag)FormFleet_EquipmentLevelVisibility.SelectedIndex;
			config.UI.NotExpeditionBlink = FormFleet_BlinkHPBar.Checked;
			config.FormFleet.ShowTextProficiency = FormFleet_TextProficiency.Checked;
			config.FormFleet.AirSuperiorityMethod = FormFleet_AirSuperiorityMethod.SelectedIndex;
			config.FormFleet.ShowAnchorageRepairingTimer = FormFleet_ShowAnchorageRepairingTimer.Checked;
			config.FormFleet.BlinkAtCompletion = FormFleet_BlinkAtCompletion.Checked;

			config.FormShipGroup.AutoUpdate = FormShipGroup_AutoUpdate.Checked;
			config.FormShipGroup.ShowStatusBar = FormShipGroup_ShowStatusBar.Checked;
			config.FormShipGroup.ShipNameSortMethod = FormShipGroup_ShipNameSortMethod.SelectedIndex;

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
			config.FormBrowser.ShowURL = FormBrowser_ShowURL.Checked;
			config.FormBrowser.ModifyCookieRegion = FormBrowser_ModifyCookieRegion.Checked;
			config.FormBrowser.FlashQuality = FormBrowser_FlashQuality.Text;
			config.FormBrowser.FlashWmode = FormBrowser_FlashWMode.Text;

			config.FormCompass.CandidateDisplayCount = (int)FormCompass_CandidateDisplayCount.Value;

			//[データベース]

			//[BGM]
			config.BGMPlayer.Enabled = BGMPlayer_Enabled.Checked;
			for ( int i = 0; i < BGMPlayer_ControlGrid.Rows.Count; i++ ) {
				BGMHandles[(SyncBGMPlayer.SoundHandleID)BGMPlayer_ControlGrid[BGMPlayer_ColumnContent.Index, i].Value].Enabled = (bool)BGMPlayer_ControlGrid[BGMPlayer_ColumnEnabled.Index, i].Value;
			}
			config.BGMPlayer.Handles = new List<SyncBGMPlayer.SoundHandle>( BGMHandles.Values.ToList() );
			config.BGMPlayer.SyncBrowserMute = BGMPlayer_SyncBrowserMute.Checked;

		}


		private void UpdateBGMPlayerUI() {

			BGMPlayer_ControlGrid.Rows.Clear();

			var rows = new DataGridViewRow[BGMHandles.Count];

			int i = 0;
			foreach ( var h in BGMHandles.Values ) {
				var row = new DataGridViewRow();
				row.CreateCells( BGMPlayer_ControlGrid );
				row.SetValues( h.Enabled, h.HandleID, h.Path );
				rows[i] = row;
				i++;
			}

			BGMPlayer_ControlGrid.Rows.AddRange( rows );

			BGMPlayer_VolumeAll.Value = (int)BGMHandles.Values.Average( h => h.Volume );
		}


		private void FormBrowser_ApplyRegistry_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "确认写入注册表吗？\r\n＊需要重新启动以完全适用。", "确认",
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

					Utility.ErrorReporter.SendErrorReport( ex, "注册表写入失败。" );
					MessageBox.Show( "注册表写入失败。\r\n" + ex.Message, "错误", 
						MessageBoxButtons.OK, MessageBoxIcon.Error );

				} finally {
					if ( reg != null )
						reg.Close();
				}
			}

		}

		private void FormBrowser_DeleteRegistry_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "确认删除注册表项吗？\r\n＊需要重新启动以完全适用。", "确认",
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

					Utility.ErrorReporter.SendErrorReport( ex, "注册表项删除失败。" );
					MessageBox.Show( "注册表项删除失败。\r\n" + ex.Message, "错误",
						MessageBoxButtons.OK, MessageBoxIcon.Error );

				} finally {
					if ( reg != null )
						reg.Close();
				}
			}
		}




		// BGMPlayer
		private void BGMPlayer_ControlGrid_CellContentClick( object sender, DataGridViewCellEventArgs e ) {
			if ( e.ColumnIndex == BGMPlayer_ColumnSetting.Index ) {

				var handleID = (SyncBGMPlayer.SoundHandleID)BGMPlayer_ControlGrid[BGMPlayer_ColumnContent.Index, e.RowIndex].Value;

				using ( var dialog = new DialogConfigurationBGMPlayer( BGMHandles[handleID] ) ) {
					if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
						BGMHandles[handleID] = dialog.ResultHandle;
					}
				}

				UpdateBGMPlayerUI();
			}
		}

		private void BGMPlayer_ControlGrid_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex == BGMPlayer_ColumnContent.Index ) {
				e.Value = SyncBGMPlayer.SoundHandleIDToString( (SyncBGMPlayer.SoundHandleID)e.Value );
				e.FormattingApplied = true;
			}

		}

		//for checkbox
		private void BGMPlayer_ControlGrid_CurrentCellDirtyStateChanged( object sender, EventArgs e ) {
			if ( BGMPlayer_ControlGrid.Columns[BGMPlayer_ControlGrid.CurrentCellAddress.X] is DataGridViewCheckBoxColumn ) {
				if ( BGMPlayer_ControlGrid.IsCurrentCellDirty ) {
					BGMPlayer_ControlGrid.CommitEdit( DataGridViewDataErrorContexts.Commit );
				}
			}
		}

		private void BGMPlayer_SetVolumeAll_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "すべてのBGMに対して音量 " + (int)BGMPlayer_VolumeAll.Value + " を適用します。\r\nよろしいですか？\r\n", "音量一括設定の確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 ) == System.Windows.Forms.DialogResult.Yes ) {

				foreach ( var h in BGMHandles.Values ) {
					h.Volume = (int)BGMPlayer_VolumeAll.Value;
				}

				UpdateBGMPlayerUI();
			}

		}


	}

}
