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
	
	public class APIKancolleDB {

		public enum APIType : int {
			PORT = 0,
			SHIP2,
			SHIP3,
			KDOCK,
			CHANGE,
			CREATESHIP,
			GETSHIP,
			CREATEITEM,
			START,
			NEXT,
			BATTLE,
			BATTLE_MIDNIGHT,
			BATTLE_SP_MIDNIGHT,
			BATTLE_NIGHT_TO_DAY,
			BATTLERESULT,
			PRACTICE_BATTLE,
			PRACTICE_BATTLERESULT,
			COMBINED_BATTLE,
			COMBINED_BATTLE_AIR,
			COMBINED_BATTLE_MIDNIGHT,
			COMBINED_BATTLE_RESULT
		}

		/// <summary>
		/// all apis
		/// </summary>
		private static readonly Dictionary<APIType, string> apis = new Dictionary<APIType, string> {
			{ APIType.PORT,                     "/kcsapi/api_port/port"                          },
            { APIType.SHIP2,                    "/kcsapi/api_get_member/ship2"                   },
            { APIType.SHIP3,                    "/kcsapi/api_get_member/ship3"                   },
            { APIType.KDOCK,                    "/kcsapi/api_get_member/kdock"                   },
            { APIType.CHANGE,                   "/kcsapi/api_req_hensei/change"                  },
            { APIType.CREATESHIP,               "/kcsapi/api_req_kousyou/createship"             },
            { APIType.GETSHIP,                  "/kcsapi/api_req_kousyou/getship"                },
            { APIType.CREATEITEM,               "/kcsapi/api_req_kousyou/createitem"             },
            { APIType.START,                    "/kcsapi/api_req_map/start"                      },
            { APIType.NEXT,                     "/kcsapi/api_req_map/next"                       },
            { APIType.BATTLE,                   "/kcsapi/api_req_sortie/battle"                  },
            { APIType.BATTLE_MIDNIGHT,          "/kcsapi/api_req_battle_midnight/battle"         },
            { APIType.BATTLE_SP_MIDNIGHT,       "/kcsapi/api_req_battle_midnight/sp_midnight"    },
            { APIType.BATTLE_NIGHT_TO_DAY,      "/kcsapi/api_req_sortie/night_to_day"            },
            { APIType.BATTLERESULT,             "/kcsapi/api_req_sortie/battleresult"            },
            { APIType.PRACTICE_BATTLE,          "/kcsapi/api_req_practice/battle"                },
            { APIType.PRACTICE_BATTLERESULT,    "/kcsapi/api_req_practice/battle_result"         },
            { APIType.COMBINED_BATTLE,          "/kcsapi/api_req_combined_battle/battle"         },
            { APIType.COMBINED_BATTLE_AIR,      "/kcsapi/api_req_combined_battle/airbattle"      },
            { APIType.COMBINED_BATTLE_MIDNIGHT, "/kcsapi/api_req_combined_battle/midnight_battle"},
            { APIType.COMBINED_BATTLE_RESULT,   "/kcsapi/api_req_combined_battle/battleresult"   }
		};

		/// <summary>
		/// read the after-session, determinate whether it will send to kancolle-db.net
		/// </summary>
		/// <param name="oSession"></param>
		public static void ExecuteSession( Session oSession ) {

			if ( !Utility.Configuration.Config.Connection.SendDataToKancolleDB ||
				Utility.Configuration.Config.Connection.SendKancolleDBApis == 0 ||
				string.IsNullOrEmpty( Utility.Configuration.Config.Connection.SendKancolleOAuth ) ) {

				return;
			}

			// find the url in dict.
			string url = oSession.PathAndQuery;
			uint apiMask = Utility.Configuration.Config.Connection.SendKancolleDBApis;

			foreach ( var kv in apis ) {
				if ( url == kv.Value ) {

					// if we allow to post this api.
					if ( ( ( 1 << (int)kv.Key ) & apiMask ) > 0 ) {

						PostToServer( oSession );
						return;

					}
				}
			}

		}

		private static Regex RequestRegex = new Regex( @"&api(_|%5F)token=[0-9a-f]+|api(_|%5F)token=[0-9a-f]+&?", RegexOptions.Compiled );

		private static void PostToServer( Session oSession ) {

			string oauth = Utility.Configuration.Config.Connection.SendKancolleOAuth;
			string url = oSession.fullUrl;
			string request = oSession.GetRequestBodyAsString();
			string response = oSession.GetResponseBodyAsString().Replace( "svdata=", "" );

			request = RequestRegex.Replace( request, "" );

			try {

				var req = WebRequest.Create( "http://api.kancolle-db.net/2/" );
				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";

				string body =
					"token=" + HttpUtility.UrlEncode( oauth ) + "&" +
					"agent=&" +
					"url=" + HttpUtility.UrlEncode( url ) + "&" +
					"requestbody=" + HttpUtility.UrlEncode( request ) + "&" +
					"responsebody=" + HttpUtility.UrlEncode( response );
				byte[] data = Encoding.ASCII.GetBytes( body );
				req.ContentLength = data.Length;

				using ( var reqStream = req.GetRequestStream() ) {
					reqStream.Write( data, 0, data.Length );
					reqStream.Flush();
				}

				using ( var resp = (HttpWebResponse)req.GetResponse() ) {
					using ( var respReader = new StreamReader(resp.GetResponseStream()) )
					using ( var output = new StreamWriter( @"kancolle-db.log", true, Encoding.UTF8 ) ) {

						output.WriteLine( "[{0}] - {1}: {2}", DateTime.Now, resp.StatusCode, respReader.ReadToEnd() );

					}
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "kancolle-db.netの送信中にエラーが発生しました。" );
			}

		}

	}
}
