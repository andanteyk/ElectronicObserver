using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {
	
	public class marriage : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			Utility.Logger.Add( 2, string.Format( "祝贺提督与 {0} 喜结连理。愿百年好合！", KCDatabase.Instance.Ships[(int)data.api_id].Name ) );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_kaisou/marriage"; }
		}
	}
}
