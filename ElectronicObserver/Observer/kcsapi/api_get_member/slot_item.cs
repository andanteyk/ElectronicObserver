using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {


	public static class slot_item {

		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			db.Equipments.Clear();
			foreach ( var elem in data ) {

				var eq = new EquipmentData();
				eq.LoadFromResponse( apiname, elem );
				db.Equipments.Add( eq );

			}

		}

	}

}
