using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi {
	
	public static class api_start2 {


		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_mst_ship
			foreach ( var elem in data.api_mst_ship ) {

				int id = elem.api_id;
				if ( db.MasterShips[id] == null ) {
					var ship = new ShipDataMaster();
					ship.LoadFromResponse( apiname, elem );
					db.MasterShips.Add( ship );
				} else {
					db.MasterShips[id].LoadFromResponse( apiname, elem );
				}
			}


			//改装関連のデータ設定
			foreach ( var ship in db.MasterShips ) {
				int remodelID = ship.Value.RemodelAfterShipID;
				if ( remodelID != 0 ) {
					db.MasterShips[remodelID].RemodelBeforeShipID = ship.Key;
				}
			}


			//api_mst_slotitem_equiptype
			foreach ( var elem in data.api_mst_slotitem_equiptype ) {

				int id = elem.api_id;
				if ( db.EquipmentTypes[id] == null ) {
					var eqt = new EquipmentType();
					eqt.LoadFromResponse( apiname, elem );
					db.EquipmentTypes.Add( eqt );
				} else {
					db.EquipmentTypes[id].LoadFromResponse( apiname, elem );
				}
			}


			//api_mst_stype
			foreach ( var elem in data.api_mst_stype ) {

				int id = elem.api_id;
				if ( db.ShipTypes[id] == null ) {
					var spt = new ShipType();
					spt.LoadFromResponse( apiname, elem );
					db.ShipTypes.Add( spt );
				} else {
					db.ShipTypes[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}


			//api_mst_slotitem
			foreach ( var elem in data.api_mst_slotitem ) {

				int id = elem.api_id;
				if ( db.MasterEquipments[id] == null ) {
					var eq = new EquipmentDataMaster();
					eq.LoadFromResponse( apiname, elem );
					db.MasterEquipments.Add( eq );
				} else {
					db.MasterEquipments[id].LoadFromResponse( apiname, elem );
				}
			}


			//api_mst_useitem
			foreach ( var elem in data.api_mst_useitem ) {

				int id = elem.api_id;
				if ( db.MasterUseItems[id] == null ) {
					var item = new UseItemMaster();
					item.LoadFromResponse( apiname, elem );
					db.MasterUseItems.Add( item );
				} else {
					db.MasterUseItems[id].LoadFromResponse( apiname, elem );
				}
			}


			//api_mst_shipupgrade
			foreach ( var elem in data.api_mst_shipupgrade ) {

				if ( elem.api_drawing_count > 0 ) {
					int id = db.MasterShips[(int)elem.api_id].RemodelBeforeShipID;
					if ( id != 0 ) {
						db.MasterShips[id].NeedBlueprint = elem.api_drawing_count;
					}
				}
			}

		}
	}

}
