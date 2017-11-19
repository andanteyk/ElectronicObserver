using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	public class ship3 : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			//api_ship_data
			foreach (var elem in data.api_ship_data)
			{

				int id = (int)elem.api_id;

				ShipData ship = db.Ships[id];
				ship.LoadFromResponse(APIName, elem);

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					if (ship.Slot[i] == -1) continue;
					if (!db.Equipments.ContainsKey(ship.Slot[i]))
					{       //改装時に新装備を入手するが、追加される前にIDを与えられてしまうため
						EquipmentData eq = new EquipmentData();
						eq.LoadFromResponse(APIName, ship.Slot[i]);
						db.Equipments.Add(eq);
					}
				}

			}

			//api_deck_data
			db.Fleet.LoadFromResponse(APIName, data.api_deck_data);



			base.OnResponseReceived((object)data);
		}


		public override string APIName => "api_get_member/ship3";
	}


}
