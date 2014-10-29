using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	public class DestroyShip : QuestCounter {

		/// <summary>
		/// 艦船を解体した回数をカウントします。
		/// </summary>
		public override void Register() {
			APIObserver ao = APIObserver.Instance;

			ao.RequestList["api_req_kousyou/destroyship"].RequestReceived += Received;

		}

		public override void Unregister() {
			APIObserver ao = APIObserver.Instance;

			ao.RequestList["api_req_kousyou/destroyship"].RequestReceived -= Received;
		}

	}
}
