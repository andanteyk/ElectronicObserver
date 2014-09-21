using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Control {
	public partial class ShipStatusResource : UserControl {

		private ToolTip ResourceTip;


		#region Properties

		public int FuelCurrent {
			get { return BarFuel.Value; }
			set { 
				BarFuel.Value = value;
				PropertyChanged();
			}
		}

		public int FuelMax {
			get { return BarFuel.MaximumValue; }
			set { 
				BarFuel.MaximumValue = value;
				PropertyChanged();
			}
		}

		public int AmmoCurrent {
			get { return BarAmmo.Value; }
			set { 
				BarAmmo.Value = value;
				PropertyChanged();
			}
		}

		public int AmmoMax {
			get { return BarAmmo.MaximumValue; }
			set { 
				BarAmmo.MaximumValue = value;
				PropertyChanged();
			}
		}



		
		#endregion


		public ShipStatusResource( ToolTip resourceTip ) {
			InitializeComponent();

			ResourceTip = resourceTip;			
		}


		
		private void PropertyChanged() {
			
			string tiptext = string.Format( "燃: {0}/{1} ({2}%)\r\n弾: {3}/{4} ({5}%)",
				FuelCurrent, FuelMax, (int)Math.Ceiling( 100.0 * FuelCurrent / FuelMax ), 
				AmmoCurrent, AmmoMax, (int)Math.Ceiling( 100.0 * AmmoCurrent / AmmoMax ) );

			ResourceTip.SetToolTip( this, tiptext );
			ResourceTip.SetToolTip( BarFuel, tiptext );
			ResourceTip.SetToolTip( BarAmmo, tiptext );
			
		}
		
		
	}
}
