using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class createitem : APIBase {

		//materialデータはresponseで返ってくるので、requestで読む必要があるのはログをとるときぐらい

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			//装備の追加　データが不十分のため、自力で構築しなければならない
			if ( (int)data.api_create_flag != 0 ) {
				var eq = new EquipmentData();
				eq.LoadFromResponse( APIName, data.api_slot_item );
				db.Equipments.Add( eq );
			}

			db.Material.LoadFromResponse( APIName, data.api_material );
			
			base.OnResponseReceived( (object) data );
		}

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override string APIName {
			get { return "api_req_kousyou/createitem"; }
		}
	}

}
