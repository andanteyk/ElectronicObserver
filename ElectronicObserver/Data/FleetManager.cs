using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 艦隊情報を統括して扱います。
	/// </summary>
	public class FleetManager : APIWrapper {

		public IDDictionary<FleetData> Fleets { get; private set; }


		/// <summary>
		/// 連合艦隊フラグ
		/// </summary>
		public int CombinedFlag { get; internal set; }

		/// <summary>
		/// 泊地修理タイマ
		/// </summary>
		public DateTime AnchorageRepairingTimer { get; private set; }


		public FleetManager() {
			Fleets = new IDDictionary<FleetData>();
			AnchorageRepairingTimer = DateTime.Now;
		}


		public FleetData this[int fleetID] {
			get {
				return Fleets[fleetID];
			}
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_req_combined_battle/goback_port":
					foreach ( int index in KCDatabase.Instance.Battle.Result.EscapingShipIndex ) {
						Fleets[( index - 1 ) < 6 ? 1 : 2].Escape( ( index - 1 ) % 6 );
					}
					break;

				case "api_get_member/ndock":
					foreach ( var fleet in Fleets.Values ) {
						fleet.LoadFromResponse( apiname, data );
					}
					break;

				case "api_req_hensei/preset_select": {
						int id = (int)data.api_id;

						if ( !Fleets.ContainsKey( id ) ) {
							var a = new FleetData();
							a.LoadFromResponse( apiname, data );
							Fleets.Add( a );

						} else {
							Fleets[id].LoadFromResponse( apiname, data );
						}

					} break;

				default:
					base.LoadFromResponse( apiname, (object)data );

					//api_port/port, api_get_member/deck
					foreach ( var elem in data ) {

						int id = (int)elem.api_id;

						if ( !Fleets.ContainsKey( id ) ) {
							var a = new FleetData();
							a.LoadFromResponse( apiname, elem );
							Fleets.Add( a );

						} else {
							Fleets[id].LoadFromResponse( apiname, elem );
						}
					}
					break;
			}


			// 泊地修理の処理
			if ( apiname == "api_port/port" ) {

				if ( ( DateTime.Now - AnchorageRepairingTimer ).TotalMinutes >= 20 )
					StartAnchorageRepairingTimer();

			}
		}



		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			switch ( apiname ) {
				case "api_req_hensei/change": {
						int memberID = int.Parse( data["api_ship_idx"] );		//変更スロット
						if ( memberID != -1 )
							data.Add( "replaced_id", Fleets[int.Parse( data["api_id"] )].Members[memberID].ToString() );

						foreach ( int i in Fleets.Keys )
							Fleets[i].LoadFromRequest( apiname, data );

					} break;

				case "api_req_map/start": {
						int fleetID = int.Parse( data["api_deck_id"] );
						if ( CombinedFlag != 0 && fleetID == 1 ) {
							Fleets[2].IsInSortie = true;
						}
						Fleets[fleetID].IsInSortie = true;
					} goto default;

				case "api_req_hensei/combined":
					CombinedFlag = int.Parse( data["api_combined_type"] );
					break;

				default:
					foreach ( int i in Fleets.Keys )
						Fleets[i].LoadFromRequest( apiname, data );
					break;

			}

		}


		/// <summary>
		/// 泊地修理タイマを現在時刻にセットします。
		/// </summary>
		public void StartAnchorageRepairingTimer() {
			AnchorageRepairingTimer = DateTime.Now;
		}

	}

}
