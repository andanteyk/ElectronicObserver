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
		public StatusBarModule BarFuel { get; private set; }
		public StatusBarModule BarAmmo { get; private set; }


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

			BarFuel = new StatusBarModule();
			BarAmmo = new StatusBarModule();

			BarFuel.UsePrevValue = BarAmmo.UsePrevValue = false;

			ResourceTip = resourceTip;
		}



		private void PropertyChanged() {

			//FIXME: サブウィンドウ状態のときToolTipが出現しない不具合を確認。

			string tiptext = string.Format( "燃 : {0}/{1} ({2}%)\r\n弾 : {3}/{4} ({5}%)",
				FuelCurrent, FuelMax, (int)Math.Ceiling( 100.0 * FuelCurrent / FuelMax ),
				AmmoCurrent, AmmoMax, (int)Math.Ceiling( 100.0 * AmmoCurrent / AmmoMax ) );

			ResourceTip.SetToolTip( this, tiptext );

			Invalidate();
		}


		/// <summary>
		/// 資源を一度に設定します。
		/// </summary>
		/// <param name="fuelCurrent">燃料の現在値。</param>
		/// <param name="fuelMax">燃料の最大値。</param>
		/// <param name="ammoCurrent">弾薬の現在値。</param>
		/// <param name="ammoMax">燃料の最大値。</param>
		public void SetResources( int fuelCurrent, int fuelMax, int ammoCurrent, int ammoMax ) {

			BarFuel.Value = fuelCurrent;
			BarFuel.MaximumValue = fuelMax;
			BarAmmo.Value = ammoCurrent;
			BarAmmo.MaximumValue = ammoMax;

			PropertyChanged();
		}



		private void ShipStatusResource_Paint( object sender, PaintEventArgs e ) {

			const int margin = 3;

			BarFuel.Paint( e.Graphics, new Rectangle( 0, margin, this.Width, BarFuel.GetPreferredSize().Height ) );
			BarAmmo.Paint( e.Graphics, new Rectangle( 0, this.Height - margin - BarFuel.GetPreferredSize().Height, this.Width, BarFuel.GetPreferredSize().Height ) );

		}

	}
}
