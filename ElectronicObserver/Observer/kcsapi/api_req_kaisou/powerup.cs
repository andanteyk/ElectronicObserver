using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class powerup : APIBase {

		Dictionary<string, string> request;

		public override void OnRequestReceived( Dictionary<string, string> data ) {
			request = data;

			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			if ( request != null )
			{
				db.Fleet.LoadFromRequest( APIName, request );


				foreach ( string id in request["api_id_items"].Split( ",".ToCharArray() ) ) {

					int shipID = int.Parse( id );


					ShipData ship = db.Ships[shipID];
					for ( int i = 0; i < ship.Slot.Count; i++ ) {
						if ( ship.Slot[i] != -1 )
							db.Equipments.Remove( ship.Slot[i] );
					}

					Utility.Logger.Add( 2, ship.NameWithLevel + " を除籍しました。" );
					db.Ships.Remove( shipID );

				}
			}
			{
				var ship = db.Ships[(int)data.api_ship.api_id];
				if ( ship != null )
					ship.LoadFromResponse( APIName, data.api_ship );

				db.Fleet.LoadFromResponse( APIName, data.api_deck );


				if ( Utility.Configuration.Config.Log.ShowSpoiler )
					Utility.Logger.Add( 2, string.Format( "{0} の近代化改修に{1}しました。", ship.NameWithLevel, ( (int)data.api_powerup_flag ) != 0 ? "成功" : "失敗" ) );
			}

			base.OnResponseReceived( (object)data );
		}



		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }


		public override string APIName {
			get { return "api_req_kaisou/powerup"; }
		}

	}


}