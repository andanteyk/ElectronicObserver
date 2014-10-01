using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {
	
	public class ship2 : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_data
			foreach ( var elem in data.api_data ) {

				int id = (int)elem.api_id;

				if ( !db.Ships.ContainsKey( id ) ) {
					var a = new ShipData();
					a.LoadFromResponse( APIName, elem );
					db.Ships.Add( a );

				} else {
					db.Ships[id].LoadFromResponse( APIName, elem );
				}
			}


			//api_data_deck
			db.Fleet.LoadFromResponse( APIName, data.api_data_deck );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/ship2"; }
		}
	}

}
