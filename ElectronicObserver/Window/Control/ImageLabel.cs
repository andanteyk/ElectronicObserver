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

	//こんなコントロールで大変申し訳ない


	public partial class ImageLabel : UserControl {

		public ImageLabel() {
			InitializeComponent();
		}



		private void Label_SizeChanged( object sender, EventArgs e ) {

			Label.Location = new Point( Label.Location.X, Height / 2 - Label.Height / 2 );

		}

		private void Image_SizeChanged( object sender, EventArgs e ) {

			Label.Location = new Point( Image.Right + Image.Margin.Right + Label.Margin.Left, Image.Top + Image.Height / 2 - Label.Height / 2 );

		}
	}
}
