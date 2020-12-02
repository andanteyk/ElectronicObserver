using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{

	/// <summary>
	/// ソフトウェアの情報を保持します。
	/// </summary>
	public static class SoftwareInformation
	{

		/// <summary>
		/// ソフトウェア名(日本語)
		/// </summary>
		public static string SoftwareNameJapanese => "七四式電子観測儀";


		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish => "ElectronicObserver";


		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese => SoftwareNameJapanese + "四六型改四 Joint Operation Mk.4";


		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish => "4.6.4";



		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime => DateTimeHelper.CSVStringToTime("2020/12/02 22:00:00");




		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri("https://ci.appveyor.com/api/projects/Tsukumoyarei/electronicobserver/branch/develop");
		public static string BuildVersion => "<BUILD_VERSION>";
		public static string BuildTime => "<BUILD_TIME>";

		public static void CheckUpdate()
		{

			if (!Utility.Configuration.Config.Life.CheckUpdateInformation)
				return;

			if (client == null)
			{
				client = new System.Net.WebClient
				{
					Encoding = new System.Text.UTF8Encoding(false)
				};
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if (!client.IsBusy)
				client.DownloadStringAsync(uri);
		}

		private static void DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
		{

			if (e.Error != null)
			{

				Utility.ErrorReporter.SendErrorReport(e.Error, "アップデート情報の取得に失敗しました。");
				return;

			}

			if (e.Result.StartsWith("<!DOCTYPE html>"))
			{

				Utility.Logger.Add(3, "アップデート情報の URI が無効です。");
				return;

			}


			try
			{
                
				var updateInfo = Codeplex.Data.DynamicJson.Parse(e.Result);
				{

					string version = updateInfo.build.version;
					string description = updateInfo.build.message;

					if ( version != BuildVersion ) {

						Utility.Logger.Add(3, "新しいバージョンがリリースされています！ : " + version);

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format("新しいバージョンがリリースされています！ : {0}\r\n更新内容 : \r\n{1}\r\nダウンロードページを開きますか？\r\n(キャンセルすると以降表示しません)",
							version, description),
							"アップデート情報", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1);


						if (result == System.Windows.Forms.DialogResult.Yes)
						{

							//System.Diagnostics.Process.Start("http://electronicobserver.blog.fc2.com/");
							var p = new System.Diagnostics.Process();
							p.StartInfo.FileName = "ElectronicObserverUpdater.exe";
							p.StartInfo.Arguments = version;
							p.Start();

						}
						else if (result == System.Windows.Forms.DialogResult.Cancel)
						{

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					}
					else
					{

						Utility.Logger.Add(2, "お使いのバージョンは最新です。");

					}

				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "アップデート情報の処理に失敗しました。");
			}

		}

	}

}
