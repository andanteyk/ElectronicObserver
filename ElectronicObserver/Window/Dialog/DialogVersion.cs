using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
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
            SuspendLayout();
			InitializeComponent();

			this.Text = string.Format( "魔改版本 - {0:F4}", SoftwareInformation.MakaiVersion );
			TextVersion.Text = string.Format( "{0} (ver. {1} - {2} Release)", SoftwareInformation.VersionJapanese, SoftwareInformation.VersionEnglish, SoftwareInformation.UpdateTime.ToString( "d" ) );
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
        }

		private void Text__LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {

			System.Diagnostics.Process.Start( "https://github.com/tsanie/ElectronicObserver/tree/development" );

		}

		private void TextAuthor_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {

			System.Diagnostics.Process.Start( "https://twitter.com/andanteyk" );

		}

		private void ButtonClose_Click( object sender, EventArgs e ) {

			this.Close();

		}

		private void TextInformation_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {

			System.Diagnostics.Process.Start( "http://electronicobserver.blog.fc2.com/" );

		}

		private void DialogVersion_Load( object sender, EventArgs e ) {

			this.Icon = ResourceManager.Instance.AppIcon;
		}
	}
}
