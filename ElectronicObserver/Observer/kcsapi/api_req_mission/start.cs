using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission {

	public class start : APIBase {

		private int FleetID;


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			/*	//checkme: どちらにせよあとで deck が呼ばれるので不要？
			FleetID = int.Parse( data["api_deck_id"] );
			KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromRequest( APIName, data );
			*/

			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			/*
			KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromResponse( APIName, data );
			*/

			base.OnResponseReceived( (object)data );

		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override string APIName {
			get { return "api_req_mission/start"; }
		}

	}

}
