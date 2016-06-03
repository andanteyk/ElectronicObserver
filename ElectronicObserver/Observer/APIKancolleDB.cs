using ElectronicObserver.Utility;
using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicObserver.Observer {

	/// <summary>
	/// 艦これ統計データベースへのデータ送信処理を行います。
	/// </summary>
	/// <remarks>http://kancolle-db.net/</remarks>
	public class APIKancolleDB {


		private static readonly HashSet<string> apis = new HashSet<string>() {
			"/kcsapi/api_port/port",
			"/kcsapi/api_get_member/ship2",
			"/kcsapi/api_get_member/ship3",
			"/kcsapi/api_get_member/slot_item",
			"/kcsapi/api_get_member/kdock",
			"/kcsapi/api_get_member/mapinfo",
			"/kcsapi/api_req_hensei/change",
			"/kcsapi/api_req_kousyou/createship",
			"/kcsapi/api_req_kousyou/getship",
			"/kcsapi/api_req_kousyou/createitem",
			"/kcsapi/api_req_map/start",
			"/kcsapi/api_req_map/next",
			"/kcsapi/api_req_map/select_eventmap_rank",
			"/kcsapi/api_req_sortie/battle",
			"/kcsapi/api_req_battle_midnight/battle",
			"/kcsapi/api_req_battle_midnight/sp_midnight",
			"/kcsapi/api_req_sortie/night_to_day",
			"/kcsapi/api_req_sortie/battleresult",
			"/kcsapi/api_req_combined_battle/battle",
			"/kcsapi/api_req_combined_battle/airbattle",
			"/kcsapi/api_req_combined_battle/midnight_battle",
			"/kcsapi/api_req_combined_battle/battleresult",
			"/kcsapi/api_req_sortie/airbattle",
			"/kcsapi/api_req_combined_battle/battle_water",
			"/kcsapi/api_req_combined_battle/sp_midnight",
		};


		public APIKancolleDB() {

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			ConfigurationChanged();

			// to avoid http 417 error
			ServicePointManager.Expect100Continue = false;

		}

		private void ConfigurationChanged() {
			OAuth = Utility.Configuration.Config.Connection.SendKancolleOAuth;

			if ( Utility.Configuration.Config.Connection.UseUpstreamProxy ) {
				Proxy = new WebProxy( "127.0.0.1", Utility.Configuration.Config.Connection.Port );
			} else {
				Proxy = null;
			}
		}

		private string OAuth;
		private WebProxy Proxy;


		/// <summary>
		/// read the after-session, determinate whether it will send to kancolle-db.net
		/// </summary>
		public void ExecuteSession( Session oSession, bool sendWithProxy ) {

			if ( string.IsNullOrEmpty( OAuth ) ) {
				return;
			}

			// find the url in dict.
			string url = oSession.PathAndQuery;

			if ( apis.Contains( url ) ) {
				PostToServer( oSession, sendWithProxy );
			}

		}

		private static Regex RequestRegex = new Regex( @"&api(_|%5F)token=[0-9a-f]+|api(_|%5F)token=[0-9a-f]+&?", RegexOptions.Compiled );

		private void PostToServer( Session oSession, bool sendWithProxy ) {

			string oauth = OAuth;
			string url = oSession.fullUrl;
			string request = oSession.GetRequestBodyAsString();
			string response = oSession.GetResponseBodyAsString();

			request = RequestRegex.Replace( request, "" );

			try {

				//*
				using ( System.Net.WebClient wc = new System.Net.WebClient() ) {
					wc.Headers["User-Agent"] = "ElectronicObserver/v" + SoftwareInformation.VersionEnglish;

					if ( sendWithProxy && Proxy != null ) {
						wc.Proxy = Proxy;
					}

					System.Collections.Specialized.NameValueCollection post = new System.Collections.Specialized.NameValueCollection();
					post.Add( "token", oauth );
					// agent key for 'ElectronicObserver'
					// https://github.com/about518/kanColleDbPost/issues/3#issuecomment-105534030
					post.Add( "agent", "L57Mi4hJeCYinbbBSH5K" );
					post.Add( "url", url );
					post.Add( "requestbody", request );
					post.Add( "responsebody", response );

					wc.UploadValuesCompleted += ( sender, e ) => {
						if ( e.Error != null ) {

							// 結構頻繁に出るのでレポートは残さない方針で　申し訳ないです
							//Utility.ErrorReporter.SendErrorReport( e.Error, string.Format( "艦これ統計データベースへの {0} の送信に失敗しました。", url.Substring( url.IndexOf( "/api" ) + 1 ) ) );

							Utility.Logger.Add( 1, string.Format( "艦これ統計データベースへの {0} の送信に失敗しました。{1}", url.Substring( url.IndexOf( "/api" ) + 1 ), e.Error.Message ) );

						} else {
							Utility.Logger.Add( 0, string.Format( "艦これ統計データベースへ {0} を送信しました。", url.Substring( url.IndexOf( "/api" ) + 1 ) ) );
						}
					};

					wc.UploadValuesAsync( new Uri( "http://api.kancolle-db.net/2/" ), post );
				}
				//*/

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "艦これ統計データベースへの送信中にエラーが発生しました。" );
			}

		}

	}
}
