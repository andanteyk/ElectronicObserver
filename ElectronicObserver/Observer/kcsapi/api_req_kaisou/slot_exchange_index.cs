using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class slot_exchange_index : APIBase
	{
		public override bool IsResponseSupported => true;

		
		public override void OnResponseReceived(dynamic data)
		{
			int shipID = (int)data.api_ship_data.api_id;

			var ship = KCDatabase.Instance.Ships[shipID];
			if (ship != null)
				ship.LoadFromResponse(APIName, data.api_ship_data);

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_kaisou/slot_exchange_index";
	}

}
