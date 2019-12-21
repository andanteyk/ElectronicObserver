using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_air_corps
{

	public class set_plane : APIBase
	{

		private int _aircorpsID;


		public override bool IsRequestSupported => true;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			_aircorpsID = BaseAirCorpsData.GetID(data);

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{

			var corps = KCDatabase.Instance.BaseAirCorps;

			if (corps.ContainsKey(_aircorpsID))
				corps[_aircorpsID].LoadFromResponse(APIName, data);

			if (data.api_after_bauxite())
			{
				var db = KCDatabase.Instance;
				int consumed = db.Material.Bauxite - (int)data.api_after_bauxite;
				Utility.Logger.Add(1, $"基地航空隊の編成により、ボーキ {consumed} が消費されました。");
			}

			KCDatabase.Instance.Material.LoadFromResponse(APIName, data);

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_air_corps/set_plane";
	}

}
