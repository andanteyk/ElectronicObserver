using Codeplex.Data;
using ElectronicObserver.Observer.kcsapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicObserver.Observer {
	

	public sealed class APIObserver {


		#region Singleton

		private static readonly APIObserver instance = new APIObserver();

		public static APIObserver Instance {
			get { return instance; }
		}

		#endregion


		public APIDictionary RequestList;
		public APIDictionary ResponseList;


		private APIObserver() {

			RequestList = new APIDictionary();
			RequestList.Add( new kcsapi.api_req_quest.clearitemget() );
			RequestList.Add( new kcsapi.api_req_nyukyo.start() );
			RequestList.Add( new kcsapi.api_req_kousyou.createship() );
			RequestList.Add( new kcsapi.api_req_kousyou.createship_speedchange() );
			RequestList.Add( new kcsapi.api_req_hensei.change() );
			RequestList.Add( new kcsapi.api_req_kousyou.destroyship() );
			RequestList.Add( new kcsapi.api_req_kousyou.destroyitem2() );
			RequestList.Add( new kcsapi.api_req_mission.start() );
			RequestList.Add( new kcsapi.api_req_member.updatedeckname() );
			RequestList.Add( new kcsapi.api_req_kaisou.powerup() );

			ResponseList = new APIDictionary();
			ResponseList.Add( new kcsapi.api_start2() );
			ResponseList.Add( new kcsapi.api_get_member.basic() );
			ResponseList.Add( new kcsapi.api_get_member.slot_item() );
			ResponseList.Add( new kcsapi.api_get_member.useitem() );
			ResponseList.Add( new kcsapi.api_get_member.kdock() );
			ResponseList.Add( new kcsapi.api_port.port() );
			ResponseList.Add( new kcsapi.api_get_member.ship2() );
			ResponseList.Add( new kcsapi.api_get_member.questlist() );
			ResponseList.Add( new kcsapi.api_get_member.ndock() );
			ResponseList.Add( new kcsapi.api_req_kousyou.getship() );
			ResponseList.Add( new kcsapi.api_req_hokyu.charge() );
			ResponseList.Add( new kcsapi.api_req_kousyou.destroyship() );
			ResponseList.Add( new kcsapi.api_req_kousyou.destroyitem2() );
			ResponseList.Add( new kcsapi.api_req_member.get_practice_enemyinfo() );
			ResponseList.Add( new kcsapi.api_get_member.picture_book() );
			ResponseList.Add( new kcsapi.api_req_mission.start() );
			ResponseList.Add( new kcsapi.api_get_member.ship3() );
			ResponseList.Add( new kcsapi.api_req_kaisou.powerup() );

			Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
			Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
			
		}




		public int Start( int portID ) {

			Fiddler.FiddlerApplication.Startup( portID, Fiddler.FiddlerCoreStartupFlags.ChainToUpstreamGateway | Fiddler.FiddlerCoreStartupFlags.RegisterAsSystemProxy );

			Fiddler.URLMonInterop.SetProxyInProcess( string.Format( "127.0.0.1:{0}",
						Fiddler.FiddlerApplication.oProxy.ListenPort ), "<local>" );

			return Fiddler.FiddlerApplication.oProxy.ListenPort;
		}


		public void Stop() {
			
			Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
			Fiddler.FiddlerApplication.Shutdown();

		}




		private void FiddlerApplication_AfterSessionComplete( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {	//checkme: このあたりの条件も後々変わる可能性があるので注意

				LoadResponse( oSession.fullUrl, oSession.GetResponseBodyAsString() );

			}
		}



		private void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "kcsapi/" ) ) {

				LoadRequest( oSession.fullUrl, oSession.GetRequestBodyAsString() );

			}
		}



		public void LoadRequest( string path, string data ) {

			Utility.Logger.Add( 1, "Request を受信しました : " + path );

			try {

				string shortpath = path.Substring( path.LastIndexOf( "kcsapi/" ) + 7 );

				var parsedData = new Dictionary<string,string>();
				data = HttpUtility.UrlDecode( data );

				foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
					string[] pair = unit.Split( "=".ToCharArray() );
					parsedData.Add( pair[0], pair[1] );
				}


				RequestList.OnRequestReceived( shortpath, parsedData );


			} catch ( Exception e ) {

				Utility.Logger.Add( 3, "Request の受信中にエラーが発生しました。\r\n" + e.Message );
				System.Diagnostics.Debug.WriteLine( e.Message );
			}

		}


		public void LoadResponse( string path, string data ) {

			Utility.Logger.Add( 1, "Responseを受信しました : " + path );

				
			try {


				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				if ( (int)json.api_result != 1 ) {
					Utility.Logger.Add( 3, "エラーコードを含むメッセージを受信しました。" );
					throw new ArgumentException( "エラーコードを含むメッセージを受信しました。" );
				}

				

				string shortpath = path.Substring( path.LastIndexOf( "kcsapi/" ) + 7 );

				if ( shortpath == "api_get_member/ship2" )
					ResponseList.OnResponseReceived( shortpath, json );
				else if ( json.IsDefined( "api_data" ) )
					ResponseList.OnResponseReceived( shortpath, json.api_data );

			} catch ( Exception e ) {

				Utility.Logger.Add( 3, "Responseの受信中にエラーが発生しました。\r\n" + e.Message );
				System.Diagnostics.Debug.WriteLine( e.Message );
			}

		}

	}



}
