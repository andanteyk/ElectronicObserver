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


		public FleetManager() {
			Fleets = new IDDictionary<FleetData>();
		}


		public FleetData this[int fleetID] {
			get {
				return Fleets[fleetID];
			}
		}

		
		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			//api_port/port
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

		}



		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			switch ( apiname ) {
				case "api_req_hensei/change": {
						int memberID = int.Parse( data["api_ship_idx"] );
						if ( memberID != -1 )
							data.Add( "replaced_id", Fleets[int.Parse( data["api_id"] )].FleetMember[memberID].ToString() );

						foreach ( int i in Fleets.Keys )
							Fleets[i].LoadFromRequest( apiname, data );

					} break;

				case "api_req_kousyou/destroyship":
					foreach ( int i in Fleets.Keys )
						Fleets[i].LoadFromRequest( apiname, data );
					break;

			}

		}
	}

}
