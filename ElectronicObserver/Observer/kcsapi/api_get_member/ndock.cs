using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public static class ndock {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			foreach ( var dock in data ) {

				int id = (int)dock.api_id;

				if ( !db.Docks.ContainsKey( id ) ) {
					var d = new DockData();
					d.LoadFromResponse( apiname, dock );
					db.Docks.Add( d );

				} else {
					db.Docks[id].LoadFromResponse( apiname, dock );
				}
			}

			db.OnShipsUpdated();
			db.OnDocksUpdated();
			
		}

	}


}
