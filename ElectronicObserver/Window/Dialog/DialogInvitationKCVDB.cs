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
	public partial class DialogInvitationKCVDB : Form {
		public DialogInvitationKCVDB() {
			InitializeComponent();
		}

		private void ButtonYes_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.Connection.SendDataToKCVDB = true;
			Close();
		}

		private void ButtonNo_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.Connection.SendDataToKCVDB = false;
			Close();
		}

		private void LinkGuideline_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
			System.Diagnostics.Process.Start( "http://kcvdb.jp/guidelines" );
		}
	}
}
