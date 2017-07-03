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

			Dictionary<string, int> itemsDestroyed = new Dictionary<string, int>();

			foreach ( int id in data["api_slotitem_ids"].Split( ",".ToCharArray() ).Select( str => int.Parse( str ) ) ) {
				string name = KCDatabase.Instance.Equipments[id].NameWithLevel;
				int amount;
				itemsDestroyed.TryGetValue( name, out amount );
				itemsDestroyed[name] = amount + 1;

				db.Equipments.Remove( id );
			}

			foreach ( var item in itemsDestroyed ) {
				Utility.Logger.Add( 2, String.Format( "{0}{1} を廃棄しました。", item.Key, item.Value > 1 ? " x " + item.Value : "" ) );
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
