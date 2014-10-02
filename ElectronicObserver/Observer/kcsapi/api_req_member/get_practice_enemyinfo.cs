using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_member {

	public class get_practice_enemyinfo : APIBase {

		public override void OnResponseReceived( dynamic data ) {
			
			base.OnResponseReceived( (object)data );
		}



		public override string APIName {
			get { return "api_req_member/get_practice_enemyinfo"; }
		}
	}

}
