using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {
	
	public static class ship2 {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			
			//api_data
			foreach ( var elem in data.api_data ) {

				int id = (int)elem.api_id;

				if ( !db.Ships.ContainsKey( id ) ) {
					var a = new ShipData();
					a.LoadFromResponse( apiname, elem );
					db.Ships.Add( a );

				} else {
					db.Ships[id].LoadFromResponse( apiname, elem );
				}
			}


			//api_data_deck
			/*
			foreach ( var elem in data.api_data_deck ) {

				int id = (int)elem.api_id;

				if ( !db.Fleets.ContainsKey( id ) ) {
					var a = new FleetData();
					a.LoadFromResponse( apiname, elem );
					db.Fleets.Add( a );

				} else {
					db.Fleets[id].LoadFromResponse( apiname, elem );
				}
			}
			*/

			db.Fleet.LoadFromResponse( apiname, data.api_data_deck );

		}

	}

}
