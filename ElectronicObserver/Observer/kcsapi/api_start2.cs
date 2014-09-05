using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi {
	
	public class api_start2 {


		public static void LoadFromResponse( string apiName, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			
			//under construction
			//api_mst_ship
			foreach ( var ship in data.api_mst_ship ) {
				if ( db.MasterShips[ship.api_id] == null )
					db.MasterShips.Add( new ShipDataMaster( ship.api_id ) );
				
				db.MasterShips[ship.api_id].LoadFromResponse( apiName, ship );
			}

			//api_mst_slotitem_equiptype
			foreach ( var eqtype in data.api_mst_slotitem_equiptype ) {
				if ( db.EquipmentTypes[eqtype.api_id] == null )
					db.EquipmentTypes.Add( new EquipmentType( eqtype.api_id ) );

				db.EquipmentTypes[eqtype.api_id].LoadFromResponse( apiName, eqtype );
			}

			//api_mst_stype
			foreach ( var stype in data.api_mst_stype ) {
				if ( db.ShipTypes[stype.api_id] == null )
					db.ShipTypes.Add( new ShipType( stype.api_id ) );

				db.ShipTypes[stype.api_id].LoadFromResponse( apiName, stype );
			}

		}
	}

}
