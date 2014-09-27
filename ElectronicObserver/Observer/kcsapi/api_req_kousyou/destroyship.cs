using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public static class destroyship {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;


			//todo: ここに処理を書くのはみょんな感があるので、可能なら移動する

			int shipID = int.Parse( data["api_ship_id"] );

			db.Fleet.LoadFromRequest( apiname, data );


			ShipData ship = db.Ships[shipID];
			for ( int i = 0; i < ship.Slot.Count; i++ ) {
				if ( ship.Slot[i] != -1 )
					db.Equipments.Remove( ship.Slot[i] );
			}

			db.Ships.Remove( shipID );


			db.OnShipsUpdated();
			db.OnEquipmentsUpdated();
			db.OnFleetUpdated();

		}


		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			db.Material.LoadFromResponse( apiname, data.api_material );

			db.OnMaterialUpdated();

		}



	}

}
