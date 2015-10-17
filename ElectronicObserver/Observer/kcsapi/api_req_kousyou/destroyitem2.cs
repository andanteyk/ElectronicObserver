using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class destroyitem2 : APIBase {

		Dictionary<string, string> request;

		public override void OnRequestReceived( Dictionary<string, string> data ) {
			request = data;
			
			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			if ( request != null )
			{
				KCDatabase db = KCDatabase.Instance;

				foreach ( string id in request["api_slotitem_ids"].Split( ",".ToCharArray() ) )
				{

					db.Equipments.Remove( int.Parse( id ) );
				}
			}

			KCDatabase.Instance.Material.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override string APIName {
			get { return "api_req_kousyou/destroyitem2"; }
		}
	}
}
