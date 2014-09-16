using Codeplex.Data;
using ElectronicObserver.Observer.kcsapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {
	

	public sealed class APIObserver {


		#region Singleton

		private static readonly APIObserver instance = new APIObserver();

		public static APIObserver Instance {
			get { return instance; }
		}

		#endregion




		private APIObserver() {

			//TODO: その他のイニシャライズも行ってくだち
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

			if ( oSession.fullUrl.Contains( "kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {	//このあたりの条件も後々変わる可能性があるので注意


				System.Diagnostics.Debug.WriteLine( "Response Received : " + oSession.fullUrl );


				LoadResponse( oSession.fullUrl, oSession.GetResponseBodyAsString() );

			}
		}



		private void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "kcsapi/" ) ) {

				System.Diagnostics.Debug.WriteLine( "Request Received : " + oSession.fullUrl );


				/*
				//ゆるして
				switch ( oSession.fullUrl.Substring( oSession.fullUrl.LastIndexOf( "kcsapi/" + 7 ) ) ) {

					case "api_req_koushou/destroyship":
						break;

				}
				*/

				
			}
		}



		public void LoadRequest( string path, string data ) {

			try {

				string shortpath = path.Substring( path.LastIndexOf( "kcsapi/" ) + 7 );
				switch ( shortpath ) {

					case "api_req_quest/clearitemget":
						kcsapi.api_req_quest.clearitemget.LoadFromRequest( shortpath, data ); break;

				}

			} catch ( Exception e ) {
				
				System.Diagnostics.Debug.WriteLine( e.Message );
			}

		}


		public void LoadResponse( string path, string data ) {

			try {


				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				if ( json.api_result != 1 ) {
					//TODO: cats came here
				}

				

				string shortpath = path.Substring( path.LastIndexOf( "kcsapi/" ) + 7 );
				switch ( shortpath ) {

					case "api_start2":
						kcsapi.api_start2.LoadFromResponse( shortpath, json.api_data ); break;		//糞設計っぽい？改修求む

					case "api_get_member/basic":
						kcsapi.api_get_member.basic.LoadFromResponse( shortpath, json.api_data ); break;

					case "api_get_member/slot_item":
						kcsapi.api_get_member.slot_item.LoadFromResponse( shortpath, json.api_data ); break;

					case "api_get_member/useitem":
						kcsapi.api_get_member.useitem.LoadFromResponse( shortpath, json.api_data ); break;

					case "api_get_member/kdock":
						kcsapi.api_get_member.kdock.LoadFromResponse( shortpath, json.api_data ); break;

					case "api_port/port":
						kcsapi.api_port.port.LoadFromResponse( shortpath, json.api_data ); break;

					case "api_get_member/ship2":
						kcsapi.api_get_member.ship2.LoadFromResponse( shortpath, json ); break;

					case "api_get_member/questlist":
						kcsapi.api_get_member.questlist.LoadFromResponse( shortpath, json.api_data ); break;

				}


			} catch ( Exception e ) {

				System.Diagnostics.Debug.WriteLine( e.Message );
			}

		}

	}



}
