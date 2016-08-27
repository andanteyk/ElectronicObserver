using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_port {

	public class port : APIBase {


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_material
			db.Material.LoadFromResponse( APIName, data.api_material );

			//api_basic
			db.Admiral.LoadFromResponse( APIName, data.api_basic );

			//api_ship
			db.Ships.Clear();
			foreach ( var elem in data.api_ship ) {

				var a = new ShipData();
				a.LoadFromResponse( APIName, elem );
				db.Ships.Add( a );

			}


			//api_ndock
			foreach ( var elem in data.api_ndock ) {

				int id = (int)elem.api_id;

				if ( !db.Docks.ContainsKey( id ) ) {
					var a = new DockData();
					a.LoadFromResponse( APIName, elem );
					db.Docks.Add( a );

				} else {
					db.Docks[id].LoadFromResponse( APIName, elem );
				}
			}

			//api_deck_port
			db.Fleet.LoadFromResponse( APIName, data.api_deck_port );
			db.Fleet.CombinedFlag = data.api_combined_flag() ? (int)data.api_combined_flag : 0;


			// 基地航空隊　配置転換系の処理
			if ( data.api_event_object() && data.api_event_object.api_base_convert_slot() ) {

				var prev = new HashSet<int>( BaseAirCorpsData.RelocatedEquipments );
				BaseAirCorpsData.SetRelocatedEquipments( (int[])data.api_event_object.api_base_convert_slot );

				foreach ( int deleted in prev.Except( BaseAirCorpsData.RelocatedEquipments ) ) {
					db.Equipments[deleted].RelocatedTime = DateTime.MinValue;
				}

				foreach ( int added in BaseAirCorpsData.RelocatedEquipments.Except( prev ) ) {
					db.Equipments[added].RelocatedTime = DateTime.Now;
				}

			} else {

				foreach ( int deleted in BaseAirCorpsData.RelocatedEquipments ) {
					db.Equipments[deleted].RelocatedTime = DateTime.MinValue;
				}

				BaseAirCorpsData.RelocatedEquipments.Clear();
			}

			db.Battle.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_port/port"; }
		}
	}

}
