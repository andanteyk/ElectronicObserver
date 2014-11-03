using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class remodeling : APIBase {

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }

		public override string APIName {
			get { return "api_req_kaisou/remodeling"; }
		}
	}
}
