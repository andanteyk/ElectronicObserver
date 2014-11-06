using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogVersion : Form {
		public DialogVersion() {
			InitializeComponent();

			this.StartPosition = FormStartPosition.CenterParent;
		}

		private void TextAuthor_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {

			System.Diagnostics.Process.Start( "https://twitter.com/andanteyk" );

		}

		private void ButtonClose_Click( object sender, EventArgs e ) {

			this.Close();

		}
	}
}
