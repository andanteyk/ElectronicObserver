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
				return SoftwareNameJapanese + "一二型改六";
			}
		}

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish {
			get {
				return "1.2.6";
			}
		}


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime {
			get {
				return DateTimeHelper.CSVStringToTime( "2015/05/18 20:00:00" );
			}
		}


		/// <summary>
		/// 魔改版本
		/// </summary>
		public static double MakaiVersion {
			get { return 519.1400; }
		}


		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri( "https://www.dropbox.com/s/vk073iw1wvktq4d/version.txt?dl=1" );
		private static readonly Uri uri_kai = new Uri( "http://106.187.38.41/version.txt" );

		public static void CheckUpdate() {

			if ( !Utility.Configuration.Config.Life.CheckUpdateInformation )
				return;

			if ( client == null ) {
				client = new System.Net.WebClient();
				client.CachePolicy = new System.Net.Cache.RequestCachePolicy( System.Net.Cache.RequestCacheLevel.NoCacheNoStore );
				client.Encoding = Encoding.GetEncoding( 936 );
				client.DownloadStringCompleted += DownloadStringKaiCompleted;
			}

			if ( !client.IsBusy )
				client.DownloadStringAsync( uri_kai );
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
				using ( var sr = new System.IO.StringReader( e.Result ) ) {

					string versionText = sr.ReadLine();
					string showText = sr.ReadLine();
					string description = sr.ReadToEnd();
					double version;
					bool show;

					if ( double.TryParse( versionText, out version ) && bool.TryParse( showText, out show ) ) {
						if ( version > MakaiVersion) {

							if ( show ) {
								// 有更新
								Utility.Logger.Add( 3, "发现新的版本！ : " + version.ToString( "F4" ) );

								var result = System.Windows.Forms.MessageBox.Show(
									string.Format( "发现新的版本：{0:F4}\r\n更新内容 : \r\n{1}\r\n需要前往LGA查看吗？\r\n（点“取消”停止以后检查版本更新）",
									version, description ),
									"更新情报", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
									System.Windows.Forms.MessageBoxDefaultButton.Button1 );


								if ( result == System.Windows.Forms.DialogResult.Yes ) {

									System.Diagnostics.Process.Start( "http://bbs.ngacn.cc/read.php?tid=8093743" );

								} else if ( result == System.Windows.Forms.DialogResult.Cancel ) {

									Utility.Configuration.Config.Life.CheckUpdateInformation = false;

								}
							} else {

								Utility.Logger.Add( 2, string.Format( "发现小版本变动：{0:F4}", version ) );
							}

						} else {

							Utility.Logger.Add( 1, "正在使用的为最新版本。" );

						}
					}
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "更新情报处理失败。" );
			}
		}

		private static void DownloadStringCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e ) {

			if ( e.Error != null ) {

				Utility.ErrorReporter.SendErrorReport( e.Error, "アップデート情報の取得に失敗しました。" );
				return;

			}

			if ( e.Result.StartsWith( "<!DOCTYPE html>" ) ) {

				Utility.Logger.Add( 3, "アップデート情報の URI が無効です。" );
				return;

			}


			try {

				using ( var sr = new System.IO.StringReader( e.Result ) ) {

					DateTime date = DateTimeHelper.CSVStringToTime( sr.ReadLine() );
					string version = sr.ReadLine();
					string description = sr.ReadToEnd();

					if ( UpdateTime < date ) {

						Utility.Logger.Add( 3, "新しいバージョンがリリースされています！ : " + version );

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format( "新しいバージョンがリリースされています！ : {0}\r\n更新内容 : \r\n{1}\r\nダウンロードページを開きますか？\r\n(キャンセルすると以降表示しません)",
							version, description ),
							"アップデート情報", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1 );


						if ( result == System.Windows.Forms.DialogResult.Yes ) {

							System.Diagnostics.Process.Start( "http://electronicobserver.blog.fc2.com/" );

						} else if ( result == System.Windows.Forms.DialogResult.Cancel ) {

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					} else {

						Utility.Logger.Add( 1, "お使いのバージョンは最新です。" );

					}

				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "アップデート情報の処理に失敗しました。" );
			}

		}

	}

}
