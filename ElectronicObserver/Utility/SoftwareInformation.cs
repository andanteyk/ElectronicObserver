using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	/// <summary>
	/// ソフトウェアの情報を保持します。
	/// </summary>
	public static class SoftwareInformation {

		/// <summary>
		/// ソフトウェア名(日本語)
		/// </summary>
		public static string SoftwareNameJapanese {
			get {
				return "七四式電子観測儀";
			}
		}

		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish {
			get {
				return "ElectronicObserver";
			}
		}

		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese {
			get {
				return SoftwareNameJapanese + "一三型";
			}
		}

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish {
			get {
				return "1.3.0";
			}
		}


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime {
			get {
				return DateTimeHelper.CSVStringToTime( "2015/05/26 00:00:00" );
			}
		}


		/// <summary>
		/// 魔改版本
		/// </summary>
		public static double MakaiVersion {
			get { return 602.1614; }
		}

		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri( "https://ci.appveyor.com/api/projects/tsanie/electronicobserver/branch/net40" );
		private const string ARTIFACTS = "https://ci.appveyor.com/project/tsanie/electronicobserver/branch/net40/artifacts";
		private const string VERSION_FILE = ".version";

		public static void CheckUpdate() {

			if ( !Utility.Configuration.Config.Life.CheckUpdateInformation )
				return;

			if ( client == null ) {
				client = new System.Net.WebClient();
				client.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.NoCacheNoStore );
				client.Encoding = Encoding.UTF8;
				client.DownloadStringCompleted += DownloadStringKaiCompleted;
			}
			System.Net.ServicePointManager.Expect100Continue = false;

			if ( !client.IsBusy )
				client.DownloadStringAsync( uri );
		}

		private static void DownloadStringKaiCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e ) {

			if ( e.Error != null ) {

				Utility.ErrorReporter.SendErrorReport( e.Error, "更新情报获取失败。" );
				return;

			}

			if ( e.Result.StartsWith( "<!DOCTYPE html" ) ) {

				Utility.Logger.Add( 3, "更新情报URI无效。" );
				return;

			}

			try {

				var build = Codeplex.Data.DynamicJson.Parse( e.Result ).build;
				string ver = build.version;

				// 验证版本
				if ( !System.IO.File.Exists( VERSION_FILE ) ) {

					System.IO.File.WriteAllText( VERSION_FILE, ver );
					Utility.Logger.Add( 2, "已记录版本: " + ver );
					return;

				} else {
					string verLocal = System.IO.File.ReadAllText( VERSION_FILE );
					if ( verLocal == ver ) {
						// 最新
						Utility.Logger.Add( 1, "正在使用的为最新版本。" );
						return;
					}

					// 判断是否为重要更新
					string message = build.message;
					if ( message.StartsWith( "important:" ) ) {

						Utility.Logger.Add( 3, "发现新的版本！: " + ver );

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format( "发现新的版本: {0}\r\n更新内容 : \r\n{1}\r\n需要打开下载页面吗？\r\n（点“取消”停止以后检查版本更新）",
							ver, message ),
							"更新情报", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1 );


						if ( result == System.Windows.Forms.DialogResult.Yes ) {

							System.Diagnostics.Process.Start( ARTIFACTS );

						} else if ( result == System.Windows.Forms.DialogResult.Cancel ) {

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					} else {

						Utility.Logger.Add( 2, string.Format( "发现小版本变动：{0}", ver ) );
					}
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "更新情报处理失败。" );
			}
		}

	}

}
