using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_map
{
	public class anchorage_repair : APIBase
	{
		public override void OnResponseReceived(dynamic data)
		{

			var db = KCDatabase.Instance;

			foreach (var elem in data.api_ship_data)
			{
				int id = (int)elem.api_id;

				var ship = db.Ships[id];
				if (ship != null)
					ship.LoadFromResponse(APIName, elem);
				else
				{
					ship = new ShipData();
					ship.LoadFromResponse(APIName, elem);
					db.Ships.Add(ship);
				}
			}

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_map/anchorage_repair";
	}
}
