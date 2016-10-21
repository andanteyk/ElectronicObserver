using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou {

	public class remodel_slot : APIBase {

		private int _equipmentID;

		public override bool IsRequestSupported { get { return true; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			_equipmentID = int.Parse( data["api_slot_id"] );

			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			db.Material.LoadFromResponse( APIName, data.api_after_material );


			if ( data.api_after_slot() ) {	//改修成功時のみ存在
				EquipmentData eq = db.Equipments[(int)data.api_after_slot.api_id];
				if ( eq != null ) {
					eq.LoadFromResponse( APIName, data.api_after_slot );

					if ( Utility.Configuration.Config.Log.ShowSpoiler )
						Utility.Logger.Add( 2, string.Format( "{0} への改修に成功しました。", eq.NameWithLevel ) );
				}

			} else if ( Utility.Configuration.Config.Log.ShowSpoiler ) {
				Utility.Logger.Add( 2, string.Format( "{0} の改修に失敗しました。", db.Equipments[_equipmentID].NameWithLevel ) );
			}


			if ( data.api_use_slot_id() ) {
				foreach ( int id in data.api_use_slot_id ) {
					db.Equipments.Remove( id );
				}
			}

			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_req_kousyou/remodel_slot"; }
		}
	}

}
