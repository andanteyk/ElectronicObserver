using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_nyukyo {

	public class start : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			DockData dock = db.Docks[int.Parse( data["api_ndock_id"] )];

			int shipID = int.Parse( data["api_ship_id"] );
			ShipData ship = db.Ships[shipID];


			db.Material.Fuel -= ship.RepairFuel;
			db.Material.Steel -= ship.RepairSteel;


			if ( data["api_highspeed"] == "1" ) {

				ship.Repair();
				db.Material.InstantRepair--;

			} else if ( ship.RepairTime <= 60000 ) {

				ship.Repair();

			} else {

				//この場合は直後に ndock が呼ばれるので自力で更新しなくてもよい
				/*
				dock.State = 1;
				dock.ShipID = shipID;
				dock.CompletionTime = DateTime.Now.AddMilliseconds( ship.RepairTime );
				*/

			}


			db.Fleet.LoadFromRequest( APIName, data );

			base.OnRequestReceived( data );
		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }


		public override string APIName {
			get { return "api_req_nyukyo/start"; }
		}
	}

}
