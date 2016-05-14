using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_hokyu {

	public class charge : APIBase {


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			//api_ship
			foreach ( var elem in data.api_ship ) {

				int shipID = (int)elem.api_id;
				ShipData ship = db.Ships[shipID];

				ship.LoadFromResponse( APIName, elem );
			}


			int[] material = new int[]{
				db.Material.Fuel,
				db.Material.Ammo,
				db.Material.Steel,
				db.Material.Bauxite,
			};

			//api_material
			db.Material.LoadFromResponse( APIName, data.api_material );

			material[0] -= db.Material.Fuel;
			material[1] -= db.Material.Ammo;
			material[2] -= db.Material.Steel;
			material[3] -= db.Material.Bauxite;

			{
				var sb = new StringBuilder( "補給を行いました。消費: " );

				for ( int i = 0; i < 4; i++ ) {
					if ( material[i] > 0 ) {
						sb.Append( Constants.GetMaterialName( i + 1 ) ).Append( "x" ).Append( material[i] ).Append( ", " );
					}
				}


				sb.Remove( sb.Length - 2, 2 );
				Utility.Logger.Add( 2, sb.ToString() );
			}


			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_hokyu/charge"; }
		}
	}

}
