using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou
{

	public class createship_speedchange : APIBase
	{


		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			KCDatabase db = KCDatabase.Instance;

			ArsenalData arsenal = db.Arsenals[int.Parse(data["api_kdock_id"])];

			arsenal.State = 3;
			db.Material.InstantConstruction -= arsenal.Fuel >= 1000 ? 10 : 1;


			base.OnRequestReceived(data);
		}


		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => false;


		public override string APIName => "api_req_kousyou/createship_speedchange";
	}


}
