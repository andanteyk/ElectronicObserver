using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public class require_info : APIBase {


		public override void OnResponseReceived( dynamic dat ) {

			KCDatabase db = KCDatabase.Instance;

			// origin: basic
			var data = dat.api_basic;
			db.Admiral.LoadFromResponse(APIName, data);

			// origin: slot_item
			data = dat.api_slot_item;
			db.Equipments.Clear();
			foreach (var elem in data)
			{

				var eq = new EquipmentData();
				eq.LoadFromResponse(APIName, elem);
				db.Equipments.Add(eq);

			}

			db.Battle.LoadFromResponse(APIName, data);

			// origin: useitem
			data = dat.api_useitem;
			db.UseItems.Clear();
			foreach (var elem in data)
			{

				var item = new UseItem();
				item.LoadFromResponse(APIName, elem);
				db.UseItems.Add(item);

			}

			// origin: kdock
			data = dat.api_kdock;
			foreach (var ars in data)
			{

				int id = (int)ars.api_id;

				if (!db.Arsenals.ContainsKey(id))
				{
					var a = new ArsenalData();
					a.LoadFromResponse(APIName, ars);
					db.Arsenals.Add(a);

				}
				else {
					db.Arsenals[id].LoadFromResponse(APIName, ars);
				}
			}

			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_get_member/require_info"; }
		}
	}
}

