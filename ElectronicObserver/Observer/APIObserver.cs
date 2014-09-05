using Codeplex.Data;
using ElectronicObserver.Observer.kcsapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {
	



	//シングルトン化？
	public class APIObserver {

		
		public delegate void ResponseReceivedEventHandler( ResponseReceivedEventArgs e );



		public event ResponseReceivedEventHandler RaiseResponseReceivedEvent = delegate { };		//null避け


		public APIObserver() {

			//TODO: その他のイニシャライズも行ってくだち
			Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
			Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
		}



		void FiddlerApplication_AfterSessionComplete( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "kcsapi/" ) && oSession.oResponse.MIMEType == "text/plain" ) {

				var json = DynamicJson.Parse( oSession.GetResponseBodyAsString().Substring( 7 ) );		//remove "svdata="

				if ( json.api_result != 1 ) {
					//TODO: cats came here
				}

				json = json.api_data;

				
				//ゆるして
				switch( oSession.fullUrl.Substring( oSession.fullUrl.LastIndexOf( "kcsapi/" + 7 ) ) ){

				case "api_start2":
					api_start2.LoadFromResponse( "api_start2", json ); break;

				}

			}
		}



		void FiddlerApplication_BeforeRequest( Fiddler.Session oSession ) {

			if ( oSession.fullUrl.Contains( "kcsapi/" ) ) {

				//ゆるして
				switch ( oSession.fullUrl.Substring( oSession.fullUrl.LastIndexOf( "kcsapi/" + 7 ) ) ) {

					case "api_req_koushou/destroyship":
						break;

				}

			}
		}


	}



}
