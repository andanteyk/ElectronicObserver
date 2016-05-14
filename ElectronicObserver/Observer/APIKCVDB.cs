using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCVDB.Client;
using ElectronicObserver.Utility;
using ElectronicObserver.Data;
using System.Text.RegularExpressions;

namespace ElectronicObserver.Observer {

	/// <summary>
	/// 艦これ検証データベースへのデータ送信を行います。
	/// </summary>
	/// <remarks>http://kcvdb.jp/</remarks>
	public class APIKCVDB {


		/// <summary>
		/// 送信のキューイングを行います。
		/// </summary>
		public class APIQueue {
			public Uri RequestUri;
			public int StatusCode;
			public string RequestBody;
			public string ResponseBody;
			public string DateHeaderValue;

			public APIQueue( Uri requestUri, int statusCode, string requestBody, string responseBody, string dateHeaderValue ) {
				RequestUri = requestUri;
				StatusCode = statusCode;
				RequestBody = requestBody;
				ResponseBody = responseBody;
				DateHeaderValue = dateHeaderValue;
			}
		}


		private IKCVDBClient _client;
		private Queue<APIQueue> _apiQueue = new Queue<APIQueue>();



		/// <summary>
		/// 送信クライアントを初期化します。
		/// </summary>
		public void Init() {
			string agentID = string.Format( "{0}{1}_{2}",
				SoftwareInformation.SoftwareNameEnglish, SoftwareInformation.VersionEnglish,
				KCDatabase.Instance.Admiral.AdmiralID );
			var sessionID = Guid.NewGuid();

			_client = KCVDBClientService.Instance.CreateClient( agentID, sessionID.ToString() );

			Logger.Add( 2, "KCVDB sender: 送信準備を開始します。セッションID: " + sessionID.ToString() );

			_client.ApiDataSent += ( _, e ) => {
				foreach ( var api in e.ApiData )
					Logger.Add( 0, "KCVDB sender: " + api.RequestUri + " を送信しました。" );
			};

			_client.SendingError += ( _, e ) => {
				foreach ( var api in e.ApiData )
					Logger.Add( 1, "KCVDB sender: " + api.RequestUri + " の送信に失敗しました。 : " + e.Message );
			};

			_client.InternalError += ( _, e ) => {
				foreach ( var api in e.ApiData )
					Logger.Add( 1, "KCVDB sender: " + api.RequestUri + " の送信中に内部エラーが発生しました。 : " + e.Message );
			};

			_client.FatalError += ( _, e ) => {
				Logger.Add( 1, "KCVDB sender: 送信中にエラーが発生しました。 : " + e.Message );
			};
		}



		/// <summary>
		/// サーバへデータを送信します。
		/// </summary>
		public void PostToServer( Nekoxy.Session session ) {
			try {

				if ( session.Response.Headers == null )
					Logger.Add( 1, "KCVDB sender: response header is null" );

				var postUri = Uri.IsWellFormedUriString( session.Request.RequestLine.URI, UriKind.Absolute )
					? new Uri( session.Request.RequestLine.URI, UriKind.Absolute )
					: new Uri( "http://" + session.Request.Headers.Host + session.Request.PathAndQuery, UriKind.Absolute );
				string requestBody = System.Web.HttpUtility.HtmlDecode( session.Request.BodyAsString );
				string postRequestBody = Regex.Replace( requestBody, @"&api(_|%5F)token=[0-9a-f]+|api(_|%5F)token=[0-9a-f]+&?", "" );
				string postResponseBody = session.Response.BodyAsString;
				int statusCode = session.Response.StatusLine.StatusCode;

				string dateHeaderValue = "";
				session.Response.Headers.Headers.TryGetValue( "date", out dateHeaderValue );


				// 提督IDが分かるようになるまではキューに溜めておく
				if ( !KCDatabase.Instance.Admiral.IsAvailable ) {
					
					var queue = new APIQueue( postUri, statusCode, postRequestBody, postResponseBody, dateHeaderValue );
					_apiQueue.Enqueue( queue );
					//Logger.Add( 1, "KCVDB sender: enqueue " + queue.RequestUri + " (" + queue.StatusCode + ") " + queue.DateHeaderValue );

				} else {
					if ( _client == null )
						Init();

					while ( _apiQueue.Count > 0 ) {
						var queue = _apiQueue.Dequeue();
						_client.SendRequestDataAsync( queue.RequestUri, queue.StatusCode, queue.RequestBody, queue.ResponseBody, queue.DateHeaderValue );
						//Logger.Add( 1, "KCVDB sender: dequeue " + queue.RequestUri + " (" + queue.StatusCode + ") " + queue.DateHeaderValue );
					}

					_client.SendRequestDataAsync( postUri, statusCode, postRequestBody, postResponseBody, dateHeaderValue );
					//Logger.Add( 1, "KCVDB sender: sending " + postUri + " (" + statusCode +") " + dateHeaderValue );

				}

			} catch ( Exception ex ) {

				Logger.Add( 1, "KCVDB sender: 送信処理中にエラーが発生しました。 : " + ex.Message );

				// 鯖落ち時等にエラーの頻発が予想されるため
				//ErrorReporter.SendErrorReport( ex, "KCVDB sender: 送信処理中にエラーが発生しました。 : " + ex.Message );

			}
		}

	}
}
