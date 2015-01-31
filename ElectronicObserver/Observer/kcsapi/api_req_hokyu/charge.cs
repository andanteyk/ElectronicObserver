using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_hokyu {

	public class charge : APIBase {


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_ship
			foreach ( var elem in data.api_ship ) {

				int shipID = (int)elem.api_id;
				ShipData ship = db.Ships[shipID];

				ship.LoadFromResponse( APIName, elem );
			}


			//api_material
			db.Material.LoadFromResponse( APIName, data.api_material );


			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_hokyu/charge"; }
		}
	}

}
