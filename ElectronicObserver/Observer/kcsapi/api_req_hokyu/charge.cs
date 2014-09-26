using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_hokyu {

	public static class charge {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_ship
			foreach ( var elem in data.api_ship ) {

				int shipID = (int)elem.api_id;
				ShipData ship = db.Ships[shipID];

				ship.LoadFromResponse( apiname, elem );
			}


			//api_material
			db.Material.LoadFromResponse( apiname, data.api_material );


			db.OnMaterialUpdated();
			db.OnShipsUpdated();

		}

	}

}
