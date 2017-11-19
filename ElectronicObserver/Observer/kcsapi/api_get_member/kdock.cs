using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	public class kdock : APIBase
	{


		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			foreach (var ars in data)
			{

				int id = (int)ars.api_id;

				if (!db.Arsenals.ContainsKey(id))
				{
					var a = new ArsenalData();
					a.LoadFromResponse(APIName, ars);
					db.Arsenals.Add(a);

				}
				else
				{
					db.Arsenals[id].LoadFromResponse(APIName, ars);
				}
			}


			base.OnResponseReceived((object)data);
		}


		public override string APIName => "api_get_member/kdock";
	}

}

