using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {


	public class slot_item : APIBase {


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			db.Equipments.Clear();
			foreach ( var elem in data ) {

				var eq = new EquipmentData();
				eq.LoadFromResponse( APIName, elem );
				db.Equipments.Add( eq );

			}

			db.Battle.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/slot_item"; }
		}
	}

}
