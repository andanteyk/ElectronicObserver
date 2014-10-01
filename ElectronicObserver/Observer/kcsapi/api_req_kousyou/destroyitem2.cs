using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class destroyitem2 : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			foreach ( string id in data["api_slotitem_ids"].Split( ",".ToCharArray() ) ) {

				db.Equipments.Remove( int.Parse( id ) );
			}
			
			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Material.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_req_kousyou/destroyitem2"; }
		}
	}
}
