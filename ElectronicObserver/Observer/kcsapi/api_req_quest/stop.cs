using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_quest {
	public class stop : APIBase {

		public override bool IsRequestSupported { get { return true; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase.Instance.Quest.LoadFromRequest( APIName, data );

			base.OnRequestReceived( data );
		}


		public override string APIName {
			get { return "api_req_quest/stop"; }
		}
	}
}
