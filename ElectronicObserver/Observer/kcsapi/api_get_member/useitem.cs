using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	public class useitem : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			db.UseItems.Clear();

			if (data != null)
			{
				foreach (var elem in data)
				{

					var item = new UseItem();
					item.LoadFromResponse(APIName, elem);
					db.UseItems.Add(item);

				}
			}

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_get_member/useitem";
	}


}
