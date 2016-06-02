using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class slot_deprive : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			int setid = (int)data.api_ship_data.api_set_ship.api_id;
			int unsetid = (int)data.api_ship_data.api_unset_ship.api_id;

			// 念のため
			if ( !db.Ships.ContainsKey( setid ) ) {
				var a = new ShipData();
				a.LoadFromResponse( APIName, data.api_ship_data.api_set_ship );
				db.Ships.Add( a );

			} else {
				db.Ships[setid].LoadFromResponse( APIName, data.api_ship_data.api_set_ship );
			}


			if ( !db.Ships.ContainsKey( unsetid ) ) {
				var a = new ShipData();
				a.LoadFromResponse( APIName, data.api_ship_data.api_unset_ship );
				db.Ships.Add( a );

			} else {
				db.Ships[unsetid].LoadFromResponse( APIName, data.api_ship_data.api_unset_ship );
			}

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_kaisou/slot_deprive"; }
		}
	}
}
