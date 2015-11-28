using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission {

	public class result : APIBase {

		private int _fleetID;


		public override bool IsRequestSupported { get { return true; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			_fleetID = int.Parse( data["api_deck_id"] );

			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			var fleet = KCDatabase.Instance.Fleet[_fleetID];


			Utility.Logger.Add( 2, string.Format( "#{0}「{1}」が遠征「{2}: {3}」から帰投しました。", fleet.FleetID, fleet.Name, fleet.ExpeditionDestination, data.api_quest_name ) );


			// 獲得資源表示
			if ( Utility.Configuration.Config.Log.ShowSpoiler ) {

				var sb = new LinkedList<string>();

				//materials
				if ( !( data.api_get_material is double ) ) {		//(強制帰還などで)ないときに -1 になる
					int[] materials = (int[])data.api_get_material;
					for ( int i = 0; i < 4; i++ ) {
						if ( materials[i] > 0 ) {
							sb.AddLast( Constants.GetMaterialName( i + 1 ) + "x" + materials[i] );
						}
					}

				}

				//items
				{
					for ( int i = 0; i < 2; i++ ) {

						int kind = (int)data.api_useitem_flag[i];

						if ( kind > 0 ) {

							int id = (int)data["api_get_item" + ( i + 1 )].api_useitem_id;
							int count = (int)data["api_get_item" + ( i + 1 )].api_useitem_count;

							switch ( kind ) {
								case 1:
									sb.AddLast( "高速修復材x" + count );
									break;
								case 2:
									sb.AddLast( "高速建造材x" + count );
									break;
								case 3:
									sb.AddLast( "開発資材x" + count );
									break;
								case 4:
									sb.AddLast( KCDatabase.Instance.MasterUseItems[id].Name + "x" + count );
									break;
								case 5:
									sb.AddLast( "家具コインx" + count );
									break;
							}

						}
					}
				}

				//exp
				{
					int admiralExp = (int)data.api_get_exp;
					if ( admiralExp > 0 ) {
						sb.AddLast( "提督Exp+" + admiralExp );
					}

					int shipExp = ( (int[])data.api_get_ship_exp ).Min();
					if ( shipExp > 0 ) {
						sb.AddLast( "艦娘Exp+" + shipExp );
					}
				}

				Utility.Logger.Add( 2, "遠征結果 - " + Constants.GetExpeditionResult( (int)data.api_clear_result ) + ": " + ( sb.Count == 0 ? "獲得資源なし" : string.Join( ", ", sb ) ) );
			}


			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_mission/result"; }
		}
	}
}
