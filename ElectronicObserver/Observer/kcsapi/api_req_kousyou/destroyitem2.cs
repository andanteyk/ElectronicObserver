using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public static class destroyitem2 {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			foreach ( string id in data["api_slotitem_ids"].Split( ",".ToCharArray() ) ) {
				
				db.Equipments.Remove( int.Parse( id ) );
			}


			db.OnEquipmentsUpdated();
		}


		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			db.Material.LoadFromResponse( apiname, data );

			db.OnMaterialUpdated();
			
		}

	}
}
