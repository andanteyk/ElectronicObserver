using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_hensei
{
	public class preset_register : APIBase
	{
		public override void OnResponseReceived(dynamic data)
		{
			KCDatabase.Instance.FleetPreset.LoadFromResponse(APIName, data);

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_hensei/preset_register";
	}
}
