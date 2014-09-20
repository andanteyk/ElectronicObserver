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
			set { BarFuel.Value = value; }
		}

		public int FuelMax {
			get { return BarFuel.MaximumValue; }
			set { BarFuel.MaximumValue = value; }
		}

		public int AmmoCurrent {
			get { return BarAmmo.Value; }
			set { BarAmmo.Value = value; }
		}

		public int AmmoMax {
			get { return BarAmmo.MaximumValue; }
			set { BarAmmo.MaximumValue = value; }
		}

		#endregion


		public ShipStatusResource() {
			InitializeComponent();


		}


	}
}
