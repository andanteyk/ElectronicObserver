using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 装備を破棄した回数(≠個数)をカウントします。
	/// </summary>
	public class DestroyEquipment : QuestCounter {

		
		public override void Register() {
			APIObserver ao = APIObserver.Instance;

			ao.APIList["api_req_kousyou/destroyitem2"].RequestReceived += Received;

		}

		public override void Unregister() {
			APIObserver ao = APIObserver.Instance;

			ao.APIList["api_req_kousyou/destroyitem2"].RequestReceived -= Received;
		}
	}

}
