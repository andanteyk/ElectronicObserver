using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_sortie
{
	public class night_to_day : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);


			base.OnResponseReceived((object)data);
		}


		public override string APIName => "api_req_sortie/night_to_day";
	}
}
