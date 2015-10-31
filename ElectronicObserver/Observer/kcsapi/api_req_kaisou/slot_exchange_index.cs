using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class slot_exchange_index : APIBase {

		private int shipID = -1;

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			shipID = int.Parse( data["api_id"] );

			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			var ship = KCDatabase.Instance.Ships[shipID];
			if ( ship != null )
				ship.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_kaisou/slot_exchange_index"; }
		}
	}
}
