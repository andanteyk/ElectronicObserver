using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {
	
	public class createship : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			//undone: このAPIが呼ばれた後 api_get_member/kdock が呼ばれ情報自体は更新されるので、建造ログのために使用？

			KCDatabase.Instance.Material.LoadFromRequest( APIName, data );
			
			base.OnRequestReceived( data );
		}

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }


		public override string APIName {
			get { return "api_req_kousyou/createship"; }
		}
	}


}
