using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {
	public class base_air_corps : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			var db = KCDatabase.Instance;

			foreach ( var elem in data ) {

				int id = (int)elem.api_rid;

				if ( !db.BaseAirCorps.ContainsKey( id ) ) {
					var a = new BaseAirCorpsData();
					a.LoadFromResponse( APIName, elem );
					db.BaseAirCorps.Add( a );

				} else {
					db.BaseAirCorps[id].LoadFromResponse( APIName, elem );
				}
			}
			
			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/base_air_corps"; }
		}
	}
}
