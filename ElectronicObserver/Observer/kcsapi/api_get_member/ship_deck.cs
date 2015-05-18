using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public class ship_deck : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_ship_data
			db.Ships.Clear();
			foreach ( var elem in data.api_ship_data ) {

				var a = new ShipData();
				a.LoadFromResponse( APIName, elem );
				db.Ships.Add( a );

			}


			//api_deck_data
			db.Fleet.LoadFromResponse( APIName, data.api_deck_data );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/ship_deck"; }
		}
	}

}
