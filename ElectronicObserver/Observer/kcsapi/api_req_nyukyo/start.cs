using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_nyukyo
{

	public class start : APIBase
	{

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			KCDatabase db = KCDatabase.Instance;

			DockData dock = db.Docks[int.Parse(data["api_ndock_id"])];
			bool bucketUsed = data["api_highspeed"] == "1";

			int shipID = int.Parse(data["api_ship_id"]);
			ShipData ship = db.Ships[shipID];


			Utility.Logger.Add(2, string.Format("入渠ドック #{0}で {1} ({2}/{3}) の修復を開始しました。(燃料x{4}, 鋼材x{5}, {6})",
				dock.DockID, ship.NameWithLevel,
				ship.HPCurrent, ship.HPMax,
				ship.RepairFuel, ship.RepairSteel,
				bucketUsed ? "高速修復材x1" : ("修理完了予定: " + DateTimeHelper.TimeToCSVString(DateTime.Now + TimeSpan.FromMilliseconds(ship.RepairTime)))
				));


			db.Material.Fuel -= ship.RepairFuel;
			db.Material.Steel -= ship.RepairSteel;


			if (bucketUsed)
			{
				ship.Repair();
				db.Material.InstantRepair--;
			}
			else if (ship.RepairTime <= 60000)
			{
				ship.Repair();
			}
			else
			{
				//この場合は直後に ndock が呼ばれるので自力で更新しなくてもよい
				/*
				dock.State = 1;
				dock.ShipID = shipID;
				dock.CompletionTime = DateTime.Now.AddMilliseconds( ship.RepairTime );
				*/
			}


			db.Fleet.LoadFromRequest(APIName, data);

			base.OnRequestReceived(data);
		}


		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => false;


		public override string APIName => "api_req_nyukyo/start";
	}


}
