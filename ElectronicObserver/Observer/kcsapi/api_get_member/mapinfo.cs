using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public class mapinfo : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			// 旧データとの互換性確保
			var list = data.api_map_info() ? data.api_map_info : data;

			foreach ( var elem in list ) {

				int id = (int)elem.api_id;

				if ( db.MapInfo[id] != null ) {
					db.MapInfo[id].LoadFromResponse( APIName, elem );
				}
			}

			if ( data.api_air_base() ) {
				db.BaseAirCorps.Clear();
				foreach ( var elem in data.api_air_base ) {
					int id = BaseAirCorpsData.GetID( elem );

					if ( db.BaseAirCorps[id] == null ) {
						var inst = new BaseAirCorpsData();
						inst.LoadFromResponse( APIName, elem );
						db.BaseAirCorps.Add( inst );

					} else {
						db.BaseAirCorps[id].LoadFromResponse( APIName, elem );
					}
				}
			}

			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_get_member/mapinfo"; }
		}
	}
}
