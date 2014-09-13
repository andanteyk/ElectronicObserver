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



		public delegate void ResponseReceivedEventHandler( ResponseReceivedEventArgs e );



		public event ResponseReceivedEventHandler RaiseResponseReceivedEvent = delegate { };		//null避け


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
		}

		public void LoadResponse( string path, string data ) {

			try {


				var json = DynamicJson.Parse( data.Substring( 7 ) );		//remove "svdata="

				if ( json.api_result != 1 ) {
					//TODO: cats came here
				}

				json = json.api_data;


				//ゆるして
				switch ( path.Substring( path.LastIndexOf( "kcsapi/" ) + 7 ) ) {

					case "api_start2":
						api_start2.LoadFromResponse( "api_start2", json ); break;		//糞設計っぽい？改修求む

				}


			} catch ( Exception e ) {

				System.Diagnostics.Debug.WriteLine( e.Message );
			}

		}

	}



}
