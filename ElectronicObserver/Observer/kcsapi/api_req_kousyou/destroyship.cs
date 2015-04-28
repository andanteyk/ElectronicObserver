using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class destroyship : APIBase {

	
		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;


			//todo: ここに処理を書くのはみょんな感があるので、可能なら移動する

			int shipID = int.Parse( data["api_ship_id"] );

			db.Fleet.LoadFromRequest( APIName, data );

			ShipData ship = db.Ships[shipID];
			
			Utility.Logger.Add( 2, ship.NameWithLevel + " を解体しました。" );

			for ( int i = 0; i < ship.Slot.Count; i++ ) {
				if ( ship.Slot[i] != -1 )
					db.Equipments.Remove( ship.Slot[i] );
			}

			db.Ships.Remove( shipID );

			
			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Material.LoadFromResponse( APIName, data.api_material );

			base.OnResponseReceived( (object)data );
		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override string APIName {
			get { return "api_req_kousyou/destroyship"; }
		}
	}

}
