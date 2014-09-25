using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {
	
	public static class createship_speedchange {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			int arsenalID = int.Parse( data["api_kdock_id"] );
			ArsenalData arsenal = db.Arsenals[arsenalID];

			//checkme: このAPI値が高速建造材消費量であると仮定; 間違っていたら修正すること
			//資源がないので今すぐはテストできない;;
			int instantConstruction = int.Parse( data["api_highspeed"] );

			arsenal.State = 3;
			db.Material.InstantConstruction -= instantConstruction;


			db.OnMaterialUpdated();
			db.OnArsenalsUpdated();

		}

	}

}
