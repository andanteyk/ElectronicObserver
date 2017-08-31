using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member {

	public class sortie_conditions : APIBase {

		public override void OnResponseReceived( dynamic data ) {
			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_get_member/sortie_conditions"; }
		}
	}
}
