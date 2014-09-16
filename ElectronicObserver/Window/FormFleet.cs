using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormFleet : DockContent {

		public int FleetID { get; private set; }


		public FormFleet()
			: this( 0 ) {
		}

		public FormFleet( int fleetID ) {
			InitializeComponent();

			FleetID = fleetID;
		}



		private void FormFleet_Load( object sender, EventArgs e ) {

			KCDatabase Database = KCDatabase.Instance;

			Text = string.Format( "[{0}]", FleetID );
			TextDebug.Text = "*not loaded*";

			Database.FleetUpdated += Database_FleetUpdated;
		}


		void Database_FleetUpdated( DatabaseUpdatedEventArgs e ) {

			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet.Fleets[FleetID];
		
			//debug

			StringBuilder sb = new StringBuilder();

			Text = string.Format( "[{0}]", FleetID );
			
			sb.AppendLine( "[" + FleetID.ToString() + "]:" + fleet.Name );
			
			for ( int i = 0; i < fleet.FleetMember.Count; i++ ) {

				int id = fleet.FleetMember[i];

				if ( id == -1 ) {

					sb.AppendLine( string.Format( "{0} : <empty>", i + 1 ) );

				} else {
					ShipData ship = db.Ships[id];
					ShipDataMaster mship = db.MasterShips[ship.ShipID];

					sb.AppendLine( string.Format( "{0} : {1} Lv.{2} HP:{3}/{4}", i + 1, mship.Name, ship.Level, ship.HPCurrent, ship.HPMax ) );
				} 
			}

			

			TextDebug.Text = sb.ToString();
		}


	}

}
