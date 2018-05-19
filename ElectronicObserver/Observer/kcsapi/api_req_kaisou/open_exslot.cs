using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class open_exslot : APIBase
	{

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			var ship = KCDatabase.Instance.Ships[Convert.ToInt32(data["api_id"])];
			if (ship != null)
			{
				ship.LoadFromRequest(APIName, data);

				Utility.Logger.Add(2, $"{ship.NameWithLevel} の補強装備増設改修が完了しました。");
			}

			base.OnRequestReceived(data);
		}


		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => false;

		public override string APIName => "api_req_kaisou/open_exslot";
	}

}
