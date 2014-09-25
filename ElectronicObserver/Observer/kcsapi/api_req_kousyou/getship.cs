using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public static class getship {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			//api_kdock
			foreach ( var ars in data.api_kdock ) {

				int id = (int)ars.api_id;

				if ( !db.Arsenals.ContainsKey( id ) ) {
					var a = new ArsenalData();
					a.LoadFromResponse( apiname, ars );
					db.Arsenals.Add( a );

				} else {
					db.Arsenals[id].LoadFromResponse( apiname, ars );
				}
			}

			//api_slotitem
			foreach ( var elem in data.api_slotitem ) {

				var eq = new EquipmentData();
				eq.LoadFromResponse( apiname, elem );
				db.Equipments.Add( eq );

			}

			//api_ship
			{
				ShipData ship = new ShipData();
				ship.LoadFromResponse( apiname, data.api_ship );
				db.Ships.Add( ship );
			}


			db.OnArsenalsUpdated();
			db.OnEquipmentsUpdated();
			db.OnShipsUpdated();

		}


	}

}
