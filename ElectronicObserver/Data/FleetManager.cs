using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	public class FleetManager : ResponseWrapper {
	
		public IDDictionary<FleetData> Fleets { get; private set; }


		public int CombinedFlag { get; internal set; }


		public FleetManager() {
			Fleets = new IDDictionary<FleetData>();
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );


			foreach ( var elem in data ) {

				int id = (int)elem.api_id;

				if ( !Fleets.ContainsKey( id ) ) {
					var a = new FleetData();
					a.LoadFromResponse( apiname, elem );
					Fleets.Add( a );

				} else {
					Fleets[id].LoadFromResponse( apiname, elem );
				}
			}

		}
	}

}
