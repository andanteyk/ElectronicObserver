﻿using ElectronicObserver.Notifier;
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


		public DialogConfiguration() {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			CustomInitialize();
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
			textCacheFolder_TextChanged(null, EventArgs.Empty);

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
			Connection_SaveDataFilter.Text = config.Connection.SaveDataFilter;
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

			//[UI]
			UI_MainFont.Font = config.UI.MainFont.FontData;
			UI_MainFont.Text = config.UI.MainFont.SerializeFontAttribute;
			UI_SubFont.Font = config.UI.SubFont.FontData;
			UI_SubFont.Text = config.UI.SubFont.SerializeFontAttribute;

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

			//[起動と終了]
			Life_ConfirmOnClosing.Checked = config.Life.ConfirmOnClosing;
			Life_TopMost.Checked = this.TopMost = config.Life.TopMost;		//メインウィンドウに隠れないように
			Life_LayoutFilePath.Text = config.Life.LayoutFilePath;
			Life_CheckUpdateInformation.Checked = config.Life.CheckUpdateInformation;
			Life_ShowStatusBar.Checked = config.Life.ShowStatusBar;
			Life_AutoScaleDpi.Checked = config.UI.AutoScaleDpi;

			//[サブウィンドウ]
			FormFleet_ShowAircraft.Checked = config.FormFleet.ShowAircraft;
			FormFleet_SearchingAbilityMethod.SelectedIndex = config.FormFleet.SearchingAbilityMethod;
			FormFleet_IsScrollable.Checked = config.FormFleet.IsScrollable;
			FormFleet_FixShipNameWidth.Checked = config.FormFleet.FixShipNameWidth;
			FormFleet_ShortenHPBar.Checked = config.FormFleet.ShortenHPBar;
			FormFleet_ShowNextExp.Checked = config.FormFleet.ShowNextExp;
			FormFleet_BlinkHPBar.Checked = config.UI.NotExpeditionBlink;
			FormFleet_TextProficiency.Checked = config.FormFleet.ShowTextProficiency;
			FormFleet_ShowEquipmentLevel.Checked = config.FormFleet.ShowEquipmentLevel;
			FormFleet_AirSuperiorityMethod.SelectedIndex = config.FormFleet.AirSuperiorityMethod;

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

			// [缓存]
			textCacheFolder.Text = config.CacheSettings.CacheFolder;
			checkCache.Checked = config.CacheSettings.CacheEnabled;


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
				config.Connection.EnableSslUpstreamProxy = Connection_EnableSslUpstreamProxy.Checked;
				config.Connection.UpstreamProxyAddress = Connection_UpstreamProxyHost.Text;
				config.Connection.UpstreamProxyPort = (ushort)Connection_UpstreamProxyPort.Value;

				if ( changed ) {
					APIObserver.Instance.Stop();
					APIObserver.Instance.Start( config.Connection.Port, this );
				}

			}

			//[UI]
			config.UI.MainFont = UI_MainFont.Font;
			config.UI.SubFont = UI_SubFont.Font;

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

			//[起動と終了]
			config.Life.ConfirmOnClosing = Life_ConfirmOnClosing.Checked;
			config.Life.TopMost = Life_TopMost.Checked;
			config.Life.LayoutFilePath = Life_LayoutFilePath.Text;
			config.Life.CheckUpdateInformation = Life_CheckUpdateInformation.Checked;
			config.Life.ShowStatusBar = Life_ShowStatusBar.Checked;
			config.UI.AutoScaleDpi = Life_AutoScaleDpi.Checked;

			//[サブウィンドウ]
			config.FormFleet.ShowAircraft = FormFleet_ShowAircraft.Checked;
			config.FormFleet.SearchingAbilityMethod = FormFleet_SearchingAbilityMethod.SelectedIndex;
			config.FormFleet.IsScrollable = FormFleet_IsScrollable.Checked;
			config.FormFleet.FixShipNameWidth = FormFleet_FixShipNameWidth.Checked;
			config.FormFleet.ShortenHPBar = FormFleet_ShortenHPBar.Checked;
			config.FormFleet.ShowNextExp = FormFleet_ShowNextExp.Checked;
			config.UI.NotExpeditionBlink = FormFleet_BlinkHPBar.Checked;
			config.FormFleet.ShowTextProficiency = FormFleet_TextProficiency.Checked;
			config.FormFleet.ShowEquipmentLevel = FormFleet_ShowEquipmentLevel.Checked;
			config.FormFleet.AirSuperiorityMethod = FormFleet_AirSuperiorityMethod.SelectedIndex;

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
			config.FormBrowser.ShowURL = FormBrowser_ShowURL.Checked;
			config.FormBrowser.ModifyCookieRegion = FormBrowser_ModifyCookieRegion.Checked;
			config.FormBrowser.FlashQuality = FormBrowser_FlashQuality.Text;
			config.FormBrowser.FlashWmode = FormBrowser_FlashWMode.Text;


			// [缓存]
			if (checkCache.Checked)
			{
				if ( !config.CacheSettings.CacheEnabled || config.CacheSettings.CacheFolder != textCacheFolder.Text ) {
					Utility.Logger.Add( 2, string.Format( "缓存设置更新。“{0}”", textCacheFolder.Text ) );
				}
			} else if ( config.CacheSettings.CacheEnabled ) {
				Utility.Logger.Add( 2, string.Format( "缓存已关闭。" ) );
			}

			config.CacheSettings.CacheEnabled = checkCache.Checked;
			config.CacheSettings.CacheFolder = textCacheFolder.Text;

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

		private void buttonCacheFolderBrowse_Click( object sender, EventArgs e )
		{
			textCacheFolder.Text = PathHelper.ProcessFolderBrowserDialog( textCacheFolder.Text, FolderBrowser );
		}

		private void textCacheFolder_TextChanged( object sender, EventArgs e )
		{
			if ( Directory.Exists( textCacheFolder.Text ) )
			{
				textCacheFolder.BackColor = SystemColors.Window;
				ToolTipInfo.SetToolTip( textCacheFolder, null );
			}
			else
			{
				textCacheFolder.BackColor = Color.MistyRose;
				ToolTipInfo.SetToolTip( textCacheFolder, "指定的文件夹不存在。" );
			}
		}



		#region - Added config pages -

		private void CustomInitialize()
		{
			this.tabPageCache = new System.Windows.Forms.TabPage();
			this.labelCache = new System.Windows.Forms.Label();
			this.textCacheFolder = new System.Windows.Forms.TextBox();
			this.buttonCacheFolderBrowse = new System.Windows.Forms.Button();
			this.checkCache = new System.Windows.Forms.CheckBox();

			this.tabControl1.SuspendLayout();
			this.tabPageCache.SuspendLayout();
			this.tabControl1.Controls.Add( this.tabPageCache );
			// 
			// tabPageCache
			// 
			this.tabPageCache.Controls.Add( this.buttonCacheFolderBrowse );
			this.tabPageCache.Controls.Add( this.textCacheFolder );
			this.tabPageCache.Controls.Add( this.labelCache );
			this.tabPageCache.Controls.Add( this.checkCache );
			this.tabPageCache.Location = new System.Drawing.Point( 4, 44 );
			this.tabPageCache.Name = "tabPageCache";
			this.tabPageCache.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPageCache.Size = new System.Drawing.Size( 392, 211 );
			this.tabPageCache.TabIndex = 8;
			this.tabPageCache.Text = "缓存";
			this.tabPageCache.UseVisualStyleBackColor = true;
			// 
			// labelCache
			// 
			this.labelCache.AutoSize = true;
			this.labelCache.Location = new System.Drawing.Point( 8, 9 );
			this.labelCache.Name = "labelCache";
			this.labelCache.Size = new System.Drawing.Size( 103, 15 );
			this.labelCache.TabIndex = 0;
			this.labelCache.Text = "缓存文件夹路径：";
			// 
			// textCacheFolder
			// 
			this.textCacheFolder.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.textCacheFolder.Location = new System.Drawing.Point( 118, 6 );
			this.textCacheFolder.Name = "textCacheFolder";
			this.textCacheFolder.Size = new System.Drawing.Size( 199, 23 );
			this.textCacheFolder.TabIndex = 1;
			this.textCacheFolder.TextChanged += new System.EventHandler( this.textCacheFolder_TextChanged );
			// 
			// buttonCacheFolderBrowse
			// 
			this.buttonCacheFolderBrowse.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.buttonCacheFolderBrowse.Location = new System.Drawing.Point( 323, 6 );
			this.buttonCacheFolderBrowse.Name = "buttonCacheFolderBrowse";
			this.buttonCacheFolderBrowse.Size = new System.Drawing.Size( 61, 23 );
			this.buttonCacheFolderBrowse.TabIndex = 2;
			this.buttonCacheFolderBrowse.Text = "浏览";
			this.buttonCacheFolderBrowse.UseVisualStyleBackColor = true;
			this.buttonCacheFolderBrowse.Click += new System.EventHandler( this.buttonCacheFolderBrowse_Click );
			// 
			// checkCache
			// 
			this.checkCache.AutoSize = true;
			this.checkCache.Location = new System.Drawing.Point( 8, 38 );
			this.checkCache.Name = "checkCache";
			this.checkCache.Size = new System.Drawing.Size( 139, 19 );
			this.checkCache.TabIndex = 3;
			this.checkCache.Text = "启用缓存";
			this.checkCache.UseVisualStyleBackColor = true;
			//
			// End
			//
			this.tabControl1.ResumeLayout( false );
			this.tabPageCache.ResumeLayout( false );
		}

		// custom config
		private System.Windows.Forms.TabPage tabPageCache;
		private System.Windows.Forms.Label labelCache;
		private System.Windows.Forms.TextBox textCacheFolder;
		private System.Windows.Forms.Button buttonCacheFolderBrowse;
		private System.Windows.Forms.CheckBox checkCache;
	}

	#endregion
}
