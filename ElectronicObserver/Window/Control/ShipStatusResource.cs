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


		public ShipStatusResource() {
			InitializeComponent();

		}


		
		private void PropertyChanged() {
			/*
			string tiptext = string.Format( "燃: {0}/{1}\n弾: {2}/{3}", FuelCurrent, FuelMax, AmmoCurrent, AmmoMax );
			ResourceTip.SetToolTip( this, tiptext );
			ResourceTip.SetToolTip( BarFuel, tiptext );
			ResourceTip.SetToolTip( BarAmmo, tiptext );
			*/
		}
		
		
	}
}
