using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi {
	
	public class api_start2 {


		public static void LoadFromResponse( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			
			//under construction
			//api_mst_ship
			foreach ( var elem in data.api_mst_ship ) {
				if ( db.MasterShips[elem.api_id] == null ) {
					var ship = new ShipDataMaster();
					ship.LoadFromResponse( apiname, elem );
					db.MasterShips.Add( ship );
				} else {
					db.MasterShips[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}

			//api_mst_slotitem_equiptype
			foreach ( var elem in data.api_mst_slotitem_equiptype ) {
				if ( db.EquipmentTypes[elem.api_id] == null ) {
					var eqt = new EquipmentType();
					eqt.LoadFromResponse( apiname, elem );
					db.EquipmentTypes.Add( eqt );
				} else {
					db.EquipmentTypes[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}

			//api_mst_stype
			foreach ( var elem in data.api_mst_stype ) {
				if ( db.ShipTypes[elem.api_id] == null ) {
					var spt = new ShipType();
					spt.LoadFromResponse( apiname, elem );
					db.ShipTypes.Add( spt );
				} else {
					db.ShipTypes[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}

			//api_mst_slotitem
			foreach ( var elem in data.api_mst_slotitem ) {
				if ( db.MasterEquipments[elem.api_id] == null ) {
					var eq = new EquipmentDataMaster();
					eq.LoadFromResponse( apiname, elem );
					db.MasterEquipments.Add( eq );
				} else {
					db.MasterEquipments[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}

			//api_mst_useitem
			foreach ( var elem in data.api_mst_useitem ) {
				if ( db.MasterUseItems[elem.api_id] == null ) {
					var item = new UseItemMaster();
					item.LoadFromResponse( apiname, elem );
					db.MasterUseItems.Add( item );
				} else {
					db.MasterUseItems[elem.api_id].LoadFromResponse( apiname, elem );
				}
			}
		}
	}

}
