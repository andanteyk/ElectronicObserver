using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission {

	public class start : APIBase {

		private static int FleetID;		//todo: せっかく個別化したんだからなるべく static は使わないよう…


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			FleetID = int.Parse( data["api_deck_id"] );
			KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromRequest( APIName, data );
			
			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );

		}

		public override string APIName {
			get { return "api_req_mission/start"; }
		}

	}

}
