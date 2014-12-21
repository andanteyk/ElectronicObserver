using Codeplex.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {
	
	public sealed class Configuration {

		#region Singleton

		private static readonly Configuration instance = new Configuration();

		public static Configuration Instance {
			get { return instance; }
		}

		#endregion


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



			public ConfigConnection() {

				Port = 40620;
				SaveReceivedData = false;
				SaveDataFilter = "";
				SaveDataPath = System.Environment.GetFolderPath( Environment.SpecialFolder.Desktop ) + @"\EOAPI";
				SaveRequest = false;
				SaveResponse = true;
				SaveSWF = false;
				SaveOtherFile = false;

			}

		}
		public ConfigConnection Connection { get; private set; }


		/// <summary>
		/// ログの設定を扱います。
		/// </summary>
		public class ConfigLog : ConfigPartBase {

			public int LogLevel { get; set; }
			public bool SaveLogFlag { get; set; }

			public ConfigLog() {
				LogLevel = 1;
				SaveLogFlag = true;
			}

		}
		public ConfigLog Log { get; private set; }


		/// <summary>
		/// 動作の設定を扱います。
		/// </summary>
		public class ConfigControl : ConfigPartBase {

			public int ConditionBorder { get; set; }

			public ConfigControl() {
				ConditionBorder = 40;
			}
		}
		public ConfigControl Control { get; private set; }


		public string Version { get { return "0.0"; } }


		//undone
		private Configuration() {
			Connection = new ConfigConnection();
			Log = new ConfigLog();
			Control = new ConfigControl();
		}


		/// <summary>
		/// 設定ダイアログを、現在の設定で初期化します。
		/// </summary>
		/// <param name="dialog">設定するダイアログ。</param>
		public void GetConfiguration( DialogConfiguration dialog ) {

			//[通信]
			dialog.Connection_Port.Value = Connection.Port;
			dialog.Connection_SaveReceivedData.Checked = Connection.SaveReceivedData;
			dialog.Connection_SaveDataFilter.Text = Connection.SaveDataFilter;
			dialog.Connection_SaveDataPath.Text = Connection.SaveDataPath;
			dialog.Connection_SaveRequest.Checked = Connection.SaveRequest;
			dialog.Connection_SaveResponse.Checked = Connection.SaveResponse;
			dialog.Connection_SaveSWF.Checked = Connection.SaveSWF;
			dialog.Connection_SaveOtherFile.Checked = Connection.SaveOtherFile;

			//[ログ]
			dialog.Log_LogLevel.Value = Log.LogLevel;
			dialog.Log_SaveLogFlag.Checked = Log.SaveLogFlag;

			//[動作]
			dialog.Control_ConditionBorder.Value = Control.ConditionBorder;

			//finalize
			dialog.UpdateParameter();

		}


		/// <summary>
		/// 設定ダイアログの情報から、設定を変更します。
		/// </summary>
		/// <param name="dialog">設定元のダイアログ。</param>
		public void SetConfiguration( DialogConfiguration dialog ) {

			//[通信]
			if ( Connection.Port != (ushort)dialog.Connection_Port.Value ) {
				Connection.Port = (ushort)dialog.Connection_Port.Value;
				APIObserver.Instance.Stop();
				ushort port = (ushort)APIObserver.Instance.Start( (int)dialog.Connection_Port.Value );
			}
			Connection.SaveReceivedData = dialog.Connection_SaveReceivedData.Checked;
			Connection.SaveDataFilter = dialog.Connection_SaveDataFilter.Text;
			Connection.SaveDataPath = dialog.Connection_SaveDataPath.Text.Trim( @"\ """.ToCharArray() );
			Connection.SaveRequest = dialog.Connection_SaveRequest.Checked;
			Connection.SaveResponse = dialog.Connection_SaveResponse.Checked;
			Connection.SaveSWF = dialog.Connection_SaveSWF.Checked;
			Connection.SaveOtherFile = dialog.Connection_SaveOtherFile.Checked;

			
			//[ログ]
			Log.LogLevel = (int)dialog.Log_LogLevel.Value;
			Log.SaveLogFlag = dialog.Log_SaveLogFlag.Checked;

			//[動作]
			Control.ConditionBorder = (int)dialog.Control_ConditionBorder.Value;

		}


		//fixme: 以下、いろいろ書きなおすべき

		private const string SaveFileName = @"Settings\Configuration.json";


		public void Load() {

			string path = SaveFileName;

			try {

				using ( StreamReader sr = new StreamReader( path ) ) {

					dynamic json = DynamicJson.Parse( sr.ReadToEnd() );

					Connection = json.Connection;
					Log = json.Log;
					Control = json.Control;

				}

			} catch ( Exception ex ) {

				ElectronicObserver.Utility.Logger.Add( 3, "設定ファイル " + path + " の読み込みに失敗しました。" + ex.Message );
			}


		}


		public void Save() {

			string path = SaveFileName;

			try {

				string data = DynamicJson.Serialize( this );

				using ( StreamWriter sw = new StreamWriter( path ) ) {
					sw.Write( data );
				}

			} catch ( Exception ex ) {

				ElectronicObserver.Utility.Logger.Add( 3, "設定ファイル " + path + " の書き込みに失敗しました。" + ex.Message );
			}

		}


	}
}
