using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	public class ndock : APIBase
	{


		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			foreach (var dock in data)
			{

				int id = (int)dock.api_id;

				if (!db.Docks.ContainsKey(id))
				{
					var d = new DockData();
					d.LoadFromResponse(APIName, dock);
					db.Docks.Add(d);

				}
				else
				{
					db.Docks[id].LoadFromResponse(APIName, dock);
				}
			}


			db.Fleet.LoadFromResponse(APIName, data);

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_get_member/ndock";
	}



}
