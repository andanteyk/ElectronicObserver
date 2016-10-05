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

			foreach ( var elem in data.api_map_info ) {

				int id = (int)elem.api_id;

				if ( db.MapInfo[id] != null ) {
					db.MapInfo[id].LoadFromResponse( APIName, elem );
				}
			}


			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_get_member/mapinfo"; }
		}
	}
}
