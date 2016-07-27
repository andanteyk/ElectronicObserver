using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class destroyitem2 : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {
			
			KCDatabase db = KCDatabase.Instance;

			// 削除処理が終わってからだと装備データが取れないため
			db.QuestProgress.EquipmentDiscarded( APIName, data );


			foreach ( string sid in data["api_slotitem_ids"].Split( ",".ToCharArray() ) ) {

				int id = int.Parse( sid );
				Utility.Logger.Add( 2, KCDatabase.Instance.Equipments[id].NameWithLevel + " を廃棄しました。" );
				db.Equipments.Remove( id );
			}
			
			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Material.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }

		public override string APIName {
			get { return "api_req_kousyou/destroyitem2"; }
		}
	}
}