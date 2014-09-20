using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public static class kdock {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			foreach ( var ars in data ) {

				int id = (int)ars.api_id;

				if ( !db.Arsenals.ContainsKey( id ) ) {
					var a = new ArsenalData();
					a.LoadFromResponse( apiname, ars );
					db.Arsenals.Add( a );

				} else {
					db.Arsenals[id].LoadFromResponse( apiname, ars );
				}
			}


			db.OnArsenalsUpdated();
		}

	}
}

