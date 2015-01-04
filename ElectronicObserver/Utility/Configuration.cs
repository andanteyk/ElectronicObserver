using Codeplex.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {
	
	
	public sealed class Configuration {


		private static readonly Configuration instance = new Configuration();

		public static Configuration Instance {
			get { return instance; }
		}

		
		private const string SaveFileName = @"Settings\Configuration.xml";


		public delegate void ConfigurationChangedEventHandler();
		public event ConfigurationChangedEventHandler ConfigurationChanged = delegate { };


		[DataContract( Name = "Configuration" )]
		public class ConfigurationData : DataStorage {

			public class ConfigPartBase {
				//reserved
			}


			/// <summary>
			/// 通信の設定を扱います。
			/// </summary>
			public class ConfigConnection : ConfigPartBase {

				/// <summary>
				/// ポート
				/// </summary>
				public ushort Port { get; set; }

				/// <summary>
				/// 通信内容を保存するか
				/// </summary>
				public bool SaveReceivedData { get; set; }

				/// <summary>
				/// 通信内容保存：フィルタ
				/// </summary>
				public string SaveDataFilter { get; set; }

				/// <summary>
				/// 通信内容保存：保存先
				/// </summary>
				public string SaveDataPath { get; set; }

				/// <summary>
				/// 通信内容保存：Requestを保存するか
				/// </summary>
				public bool SaveRequest { get; set; }

				/// <summary>
				/// 通信内容保存：Responseを保存するか
				/// </summary>
				public bool SaveResponse { get; set; }

				/// <summary>
				/// 通信内容保存：SWFを保存するか
				/// </summary>
				public bool SaveSWF { get; set; }

				/// <summary>
				/// 通信内容保存：その他ファイルを保存するか
				/// </summary>
				public bool SaveOtherFile { get; set; }

				/// <summary>
				/// 通信内容保存：バージョンを追加するか
				/// </summary>
				public bool ApplyVersion { get; set; }


				public ConfigConnection() {

					Port = 40620;
					SaveReceivedData = false;
					SaveDataFilter = "";
					SaveDataPath = System.Environment.CurrentDirectory + @"\EOAPI";
					SaveRequest = false;
					SaveResponse = true;
					SaveSWF = false;
					SaveOtherFile = false;
					ApplyVersion = false;
				}

			}
			/// <summary>通信</summary>
			[DataMember]
			public ConfigConnection Connection { get; private set; }


			public class ConfigUI : ConfigPartBase {

				/// <summary>
				/// メインフォント
				/// </summary>
				public SerializableFont MainFont { get; set; }

				/// <summary>
				/// サブフォント
				/// </summary>
				public SerializableFont SubFont { get; set; }


				public ConfigUI() {
					//*/
					MainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					SubFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					//*/
				}
			}
			/// <summary>UI</summary>
			[DataMember]
			public ConfigUI UI { get; private set; }


			/// <summary>
			/// ログの設定を扱います。
			/// </summary>
			public class ConfigLog : ConfigPartBase {

				/// <summary>
				/// ログのレベル
				/// </summary>
				public int LogLevel { get; set; }

				/// <summary>
				/// ログを保存するか
				/// </summary>
				public bool SaveLogFlag { get; set; }

				/// <summary>
				/// エラーレポートを保存するか
				/// </summary>
				public bool SaveErrorReport { get; set; }

				public ConfigLog() {
					LogLevel = 1;
					SaveLogFlag = true;
					SaveErrorReport = true;
				}

			}
			/// <summary>ログ</summary>
			[DataMember]
			public ConfigLog Log { get; private set; }


			/// <summary>
			/// 動作の設定を扱います。
			/// </summary>
			public class ConfigControl : ConfigPartBase {

				/// <summary>
				/// 疲労度ボーダー
				/// </summary>
				public int ConditionBorder { get; set; }

				public ConfigControl() {
					ConditionBorder = 40;
				}
			}
			/// <summary>動作</summary>
			[DataMember]
			public ConfigControl Control { get; private set; }


			/// <summary>
			/// デバッグの設定を扱います。
			/// </summary>
			public class ConfigDebug : ConfigPartBase {

				/// <summary>
				/// デバッグメニューを有効にするか
				/// </summary>
				public bool EnableDebugMenu { get; set; }

				public ConfigDebug() {
					EnableDebugMenu = false;
				}
			}
			/// <summary>デバッグ</summary>
			[DataMember]
			public ConfigDebug Debug { get; private set; }


			/// <summary>
			/// 起動と終了の設定を扱います。
			/// </summary>
			public class ConfigLife : ConfigPartBase {

				/// <summary>
				/// 終了時に確認するか
				/// </summary>
				public bool ConfirmOnClosing { get; set; }

				public ConfigLife() {
					ConfirmOnClosing = true;
				}
			}
			/// <summary>起動と終了</summary>
			[DataMember]
			public ConfigLife Life { get; private set; }



			[DataMember]
			public string Version {
				get { return SoftwareInformation.VersionEnglish; }
				set { }	//readonly
			}


			public override void Initialize() {

				Connection = new ConfigConnection();
				UI = new ConfigUI();
				Log = new ConfigLog();
				Control = new ConfigControl();
				Debug = new ConfigDebug();
				Life = new ConfigLife();

			}
		}
		private static ConfigurationData _config;

		public static ConfigurationData Config {
			get { return _config; }
		}
		


		private Configuration()
			: base() {

			_config = new ConfigurationData();
		}



		/// <summary>
		/// 設定ダイアログを、現在の設定で初期化します。
		/// </summary>
		/// <param name="dialog">設定するダイアログ。</param>
		public void GetConfiguration( DialogConfiguration dialog ) {

			//[通信]
			dialog.Connection_Port.Value = _config.Connection.Port;
			dialog.Connection_SaveReceivedData.Checked = _config.Connection.SaveReceivedData;
			dialog.Connection_SaveDataFilter.Text = _config.Connection.SaveDataFilter;
			dialog.Connection_SaveDataPath.Text = _config.Connection.SaveDataPath;
			dialog.Connection_SaveRequest.Checked = _config.Connection.SaveRequest;
			dialog.Connection_SaveResponse.Checked = _config.Connection.SaveResponse;
			dialog.Connection_SaveSWF.Checked = _config.Connection.SaveSWF;
			dialog.Connection_SaveOtherFile.Checked = _config.Connection.SaveOtherFile;
			dialog.Connection_ApplyVersion.Checked = _config.Connection.ApplyVersion;

			//[UI]
			dialog.UI_MainFont.Font = _config.UI.MainFont.FontData;
			dialog.UI_MainFont.Text = _config.UI.MainFont.SerializeFontAttribute;
			dialog.UI_SubFont.Font = _config.UI.SubFont.FontData;
			dialog.UI_SubFont.Text = _config.UI.SubFont.SerializeFontAttribute;

			//[ログ]
			dialog.Log_LogLevel.Value = _config.Log.LogLevel;
			dialog.Log_SaveLogFlag.Checked = _config.Log.SaveLogFlag;
			dialog.Log_SaveErrorReport.Checked = _config.Log.SaveErrorReport;

			//[動作]
			dialog.Control_ConditionBorder.Value = _config.Control.ConditionBorder;

			//[デバッグ]
			dialog.Debug_EnableDebugMenu.Checked = _config.Debug.EnableDebugMenu;

			//[起動と終了]
			dialog.Life_ConfirmOnClosing.Checked = _config.Life.ConfirmOnClosing;

			//finalize
			dialog.UpdateParameter();

		}


		/// <summary>
		/// 設定ダイアログの情報から、設定を変更します。
		/// </summary>
		/// <param name="dialog">設定元のダイアログ。</param>
		public void SetConfiguration( DialogConfiguration dialog ) {

			//[通信]
			if ( _config.Connection.Port != (ushort)dialog.Connection_Port.Value ) {
				_config.Connection.Port = (ushort)dialog.Connection_Port.Value;
				APIObserver.Instance.Stop();
				ushort port = (ushort)APIObserver.Instance.Start( (int)dialog.Connection_Port.Value );
			}
			_config.Connection.SaveReceivedData = dialog.Connection_SaveReceivedData.Checked;
			_config.Connection.SaveDataFilter = dialog.Connection_SaveDataFilter.Text;
			_config.Connection.SaveDataPath = dialog.Connection_SaveDataPath.Text.Trim( @"\ """.ToCharArray() );
			_config.Connection.SaveRequest = dialog.Connection_SaveRequest.Checked;
			_config.Connection.SaveResponse = dialog.Connection_SaveResponse.Checked;
			_config.Connection.SaveSWF = dialog.Connection_SaveSWF.Checked;
			_config.Connection.SaveOtherFile = dialog.Connection_SaveOtherFile.Checked;
			_config.Connection.ApplyVersion = dialog.Connection_ApplyVersion.Checked;
			
			//[UI]
			_config.UI.MainFont = dialog.UI_MainFont.Font;
			_config.UI.SubFont = dialog.UI_SubFont.Font;

			//[ログ]
			_config.Log.LogLevel = (int)dialog.Log_LogLevel.Value;
			_config.Log.SaveLogFlag = dialog.Log_SaveLogFlag.Checked;
			_config.Log.SaveErrorReport = dialog.Log_SaveErrorReport.Checked;

			//[動作]
			_config.Control.ConditionBorder = (int)dialog.Control_ConditionBorder.Value;

			//[デバッグ]
			_config.Debug.EnableDebugMenu = dialog.Debug_EnableDebugMenu.Checked;

			//[起動と終了]
			_config.Life.ConfirmOnClosing = dialog.Life_ConfirmOnClosing.Checked;


			ConfigurationChanged();
		}


		public void Load() {
			var temp = (ConfigurationData)_config.Load( SaveFileName );
			if ( temp != null )
				_config = temp;
		}

		public void Save() {
			_config.Save( SaveFileName );
		}
	}


}
