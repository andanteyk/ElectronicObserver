using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_air_corps {
	public class set_action : APIBase {

		public override bool IsRequestSupported { get { return true; } }


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			int areaID = int.Parse( data["api_area_id"] );
			foreach ( var c in KCDatabase.Instance.BaseAirCorps.Values.Where( b => b.MapAreaID == areaID ) ) {
				c.LoadFromRequest( APIName, data );
			}

			base.OnRequestReceived( data );
		}


		public override string APIName {
			get { return "api_req_air_corps/set_action"; }
		}
	}
}
