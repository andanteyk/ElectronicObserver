using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_member
{
	public class itemuse : APIBase
	{
		public override void OnResponseReceived(dynamic data)
		{
			if (data != null && data.api_getitem())
			{
				foreach (var elem in data.api_getitem)
				{
					if (elem != null && elem.api_slotitem())
					{
						var eq = new EquipmentData();
						eq.LoadFromResponse(APIName, elem.api_slotitem);
						KCDatabase.Instance.Equipments.Add(eq);
					}
				}
			}

			base.OnResponseReceived((object)data);
		}

		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => true;

		public override string APIName => "api_req_member/itemuse";
	}
}
