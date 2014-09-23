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

		//todo: 練り直すべき
		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			int shipID = int.Parse( data["api_ship_id"] );


			if ( data["api_highspeed"] == "1" ) {
				
				db.Ships[shipID].Heal();
				db.Material.Decrement( 0, db.Ships[shipID].RepairFuel );
				db.Material.Decrement( 2, db.Ships[shipID].RepairSteel );		//fixme: いろいろ矛盾しそう
				db.Material.Decrement( 5, 1 );

			} else {

				dynamic newdata = new DynamicJson();

				newdata.api_id = int.Parse( data["api_ndock_id"] );
				newdata.api_state = 1;
				newdata.api_ship_id = shipID;
				newdata.api_complete_time = DateConverter.ToAPITime( DateTime.Now.AddSeconds( db.Ships[shipID].RepairTime ) );

				db.Docks[(int)newdata.api_id].LoadFromResponse( apiname, newdata );		//fixme: どう考えてもスマートじゃない
				//あと、うっかりdockが初期化されてないと落ちる

			}



			db.OnMaterialUpdated();
			db.OnDocksUpdated();
		}

	}

}
