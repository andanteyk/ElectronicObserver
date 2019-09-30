using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou
{

	public class createitem : APIBase
	{
		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			KCDatabase.Instance.Development.LoadFromRequest(APIName, data);

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{

			var db = KCDatabase.Instance;
			var dev = db.Development;

			dev.LoadFromResponse(APIName, data);


			//logging
			if (Utility.Configuration.Config.Log.ShowSpoiler)
			{
				//Utility.Logger.Add(2, $"開発結果: {string.Join(", ", dev.Results)} ({dev.Fuel}/{dev.Ammo}/{dev.Steel}/{dev.Bauxite} 秘書艦: {db.Fleet[1].MembersInstance[0].NameWithLevel})");

				foreach (var result in dev.Results)
				{
					if (result.IsSucceeded)
					{
						Utility.Logger.Add(2, string.Format("{0}「{1}」の開発に成功しました。({2}/{3}/{4}/{5} 秘書艦: {6})",
							result.MasterEquipment.CategoryTypeInstance.Name,
							result.MasterEquipment.Name,
							dev.Fuel, dev.Ammo, dev.Steel, dev.Bauxite,
							db.Fleet[1].MembersInstance[0].NameWithLevel));
					}
					else
					{
						Utility.Logger.Add(2, string.Format("開発に失敗しました。({0}/{1}/{2}/{3} 秘書艦: {4})",
							dev.Fuel, dev.Ammo, dev.Steel, dev.Bauxite,
							db.Fleet[1].MembersInstance[0].NameWithLevel));
					}
				}
			}

			base.OnResponseReceived((object)data);
		}

		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => true;

		public override string APIName => "api_req_kousyou/createitem";
	}


}
