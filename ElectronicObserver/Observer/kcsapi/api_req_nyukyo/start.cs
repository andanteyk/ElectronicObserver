using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_nyukyo {

	public static class start {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			int dockID = int.Parse( data["api_ndock_id"] );
			DockData dock = db.Docks[dockID];
	
			int shipID = int.Parse( data["api_ship_id"] );
			ShipData ship = db.Ships[shipID];

			if ( data["api_highspeed"] == "1" ) {

				ship.Repair();
				db.Material.InstantRepair--;

			} else {

				dock.State = 1;
				dock.ShipID = shipID;
				dock.CompletionTime = DateTime.Now.AddSeconds( ship.RepairTime );

			}

			db.Material.Fuel -= ship.RepairFuel;
			db.Material.Steel -= ship.RepairSteel;
				

			db.OnMaterialUpdated();
			db.OnShipsUpdated();
			db.OnDocksUpdated();

		}

	}

}
