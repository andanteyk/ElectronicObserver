using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {
	
	public static class useitem {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			db.UseItems.Clear();
			foreach ( var elem in data ) {

				var item = new UseItem();
				item.LoadFromResponse( apiname, elem );
				db.UseItems.Add( item );

			}

			db.OnUseItemsUpdated();
		}
	
	}

}
