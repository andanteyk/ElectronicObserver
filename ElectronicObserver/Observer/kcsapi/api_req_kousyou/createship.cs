using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {
	
	public static class createship {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			int arsenalID = int.Parse( data["api_kdock_id"] );
			ArsenalData arsenal = db.Arsenals[arsenalID];

			//undone: このAPIが呼ばれた後 api_get_member/kdock が呼ばれ情報自体は更新されるので、建造ログのために使用？
			//APIObserver その他にも登録していないので、実装の際はそちらも更新すること

		}

	}


}
