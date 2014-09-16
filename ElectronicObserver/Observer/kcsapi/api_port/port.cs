using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_port {
	
	public static class port {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			//api_material
			db.Material.LoadFromResponse( apiname, data.api_material );

			//api_basic
			db.Admiral.LoadFromResponse( apiname, data.api_basic );

			//api_ship
			foreach ( var elem in data.api_ship ) {

				int id = (int)elem.api_id;

				if ( !db.Ships.ContainsKey( id ) ) {
					var a = new ShipData();
					a.LoadFromResponse( apiname, elem );
					db.Ships.Add( a );

				} else {
					db.Ships[id].LoadFromResponse( apiname, elem );
				}
			}

			//api_ndock
			foreach ( var elem in data.api_ndock ) {

				int id = (int)elem.api_id;

				if ( !db.Docks.ContainsKey( id ) ) {
					var a = new DockData();
					a.LoadFromResponse( apiname, elem );
					db.Docks.Add( a );

				} else {
					db.Docks[id].LoadFromResponse( apiname, elem );
				}
			}

			//api_deck_port
			db.Fleet.LoadFromResponse( apiname, data.api_deck_port );
			db.Fleet.CombinedFlag = (int)data.api_combined_flag;			//fixme:きたない



			db.OnMaterialUpdated();
			db.OnAdmiralUpdated();
			db.OnShipsUpdated();
			db.OnDocksUpdated();
			db.OnFleetUpdated();
			
		}

	}

}
