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
                return LoadResources.getter("SoftwareInformation_1");
			}
		}

		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish {
			get {
                return LoadResources.getter("SoftwareInformation_2");
			}
		}

		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese {
			get {
                return SoftwareNameJapanese + LoadResources.getter("SoftwareInformation_3");
			}
		}

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish {
			get {
                return LoadResources.getter("SoftwareInformation_4");
			}
		}


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime {
			get {
				return DateTimeHelper.CSVStringToTime( "2015/04/11 18:00:00" );
			}
		}



		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri( "https://www.dropbox.com/s/vk073iw1wvktq4d/version.txt?dl=1" );

		public static void CheckUpdate() {

			if ( !Utility.Configuration.Config.Life.CheckUpdateInformation )
				return;

			if ( client == null ) { 
				client = new System.Net.WebClient();
				client.Encoding = new System.Text.UTF8Encoding( false );
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if ( !client.IsBusy )
				client.DownloadStringAsync( uri );
		}

		private static void DownloadStringCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e ) {

			if ( e.Error != null ) {

                Utility.ErrorReporter.SendErrorReport(e.Error, LoadResources.getter("SoftwareInformation_7"));
				return;

			}

			if ( e.Result.StartsWith( "<!DOCTYPE html>" ) ) {

                Utility.Logger.Add(3, LoadResources.getter("SoftwareInformation_8"));
				return;

			}


			try {

				using ( var sr = new System.IO.StringReader( e.Result ) ) {

					DateTime date = DateTimeHelper.CSVStringToTime( sr.ReadLine() );
					string version = sr.ReadLine();
					string description = sr.ReadToEnd();

					if ( UpdateTime < date ) {

                        Utility.Logger.Add(3, LoadResources.getter("SoftwareInformation_9") + version);

						var result = System.Windows.Forms.MessageBox.Show(
                            string.Format(LoadResources.getter("SoftwareInformation_5"),
							version, description ),
                            LoadResources.getter("SoftwareInformation_6"), System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1 );


						if ( result == System.Windows.Forms.DialogResult.Yes ) {

							System.Diagnostics.Process.Start( "http://electronicobserver.blog.fc2.com/" );

						} else if ( result == System.Windows.Forms.DialogResult.Cancel ) {

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					} else {

                        Utility.Logger.Add(1, LoadResources.getter("SoftwareInformation_10"));

					}

				}

			} catch ( Exception ex ) {

                Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("SoftwareInformation_11"));
			}
			
		}

	}

}
