using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
				return SoftwareNameJapanese + "二一型改四";
			}
		}

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish {
			get {
				return "2.1.4";
			}
		}


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime {
			get {
				return DateTimeHelper.CSVStringToTime( "2016/02/11 02:00:00" );
			}
		}



		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri( "https://ci.appveyor.com/api/projects/tsanie/electronicobserver/branch/net40" );
		private const string ARTIFACTS = "https://ci.appveyor.com/project/tsanie/electronicobserver/branch/net40/artifacts";

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

		/// <summary>
		/// 比较版本，例 1.0.1.2.makai
		/// </summary>
		/// <returns>
		/// 0 - 相同
		/// 1 - 小变动
		/// 2 - 大变动
		/// </returns>
		private static int CompareVersion( string verRemote, string verLocal )
		{
			if ( verRemote == verLocal )
				return 0;

			int t;
			int[] vr = verRemote.Split( '.' ).Select( s => int.TryParse( s, out t ) ? t : 0 ).ToArray();

			if ( vr.Length != 5 ) {
				// 远程版本不合法也不进行升级通知
				return 0;
			}

			int[] vl = verLocal.Split( '.' ).Select( s => int.TryParse( s, out t ) ? t : 0 ).ToArray();

			if ( vr.Length != 5 || vl.Length != 5 || vr.Length != vl.Length )
				return 2;

			// 2.0.0.0 -> 1.0.0.0
			if ( vr[0] > vl[0] ) {
				return 2;
			} else if ( vr[0] == vl[0] ) {

				// 2.1.0.0 -> 2.0.0.0
				if ( vr[1] > vl[1] ) {
					return 2;
				} else if ( vr[1] == vl[1] ) {

					// 2.1.1.0 -> 2.1.0.0
					if ( vr[2] > vl[2] ) {
						return 2;
					} else if ( vr[2] == vl[2] ) {

						// 2.1.1.9 -> 2.1.1.0
						if ( vr[3] > vl[3] ) {
							return 1;
						}
					}
				}
			}

			return 0;
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
				{
					var attr = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyInformationalVersionAttribute ), true ).FirstOrDefault();
					string verLocal = attr == null ? null : ( (AssemblyInformationalVersionAttribute)attr ).InformationalVersion;
					int compare = CompareVersion( ver, verLocal );

					if ( compare == 0 ) {
						// 最新
						Utility.Logger.Add( 1, "正在使用的为最新版本。" );
						return;
					}

					// 判断是否为重要更新
					else if ( compare >= 2 ) {

						Utility.Logger.Add( 3, "发现新的版本！: " + ver );

						string message = build.message;
						string extend = build.messageExtended() ? build.messageExtended : null;
						if ( extend != null ) {
							extend = extend.Replace( " ", "\r\n" );
						}

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format( "发现新的版本: {0}\r\n更新内容 : \r\n{1}\r\n{2}\r\n\r\n需要打开下载页面吗？\r\n（点“取消”停止以后检查版本更新）",
							ver, message, extend ),
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
