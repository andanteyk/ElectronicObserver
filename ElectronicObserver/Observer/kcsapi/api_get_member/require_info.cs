using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public class require_info : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			// Admiral - 各所でバグるので封印
			//db.Admiral.LoadFromResponse( APIName, data.api_basic );


			// Equipments
			db.Equipments.Clear();
			foreach ( var elem in data.api_slot_item ) {

				var eq = new EquipmentData();
				eq.LoadFromResponse( APIName, elem );
				db.Equipments.Add( eq );

			}


			// Arsenal
			foreach ( var ars in data.api_kdock ) {

				int id = (int)ars.api_id;

				if ( !db.Arsenals.ContainsKey( id ) ) {
					var a = new ArsenalData();
					a.LoadFromResponse( APIName, ars );
					db.Arsenals.Add( a );

				} else {
					db.Arsenals[id].LoadFromResponse( APIName, ars );
				}
			}


			// UseItem
			db.UseItems.Clear();
			foreach ( var elem in data.api_useitem ) {

				var item = new UseItem();
				item.LoadFromResponse( APIName, elem );
				db.UseItems.Add( item );

			}


			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/require_info"; }
		}
	}
}
