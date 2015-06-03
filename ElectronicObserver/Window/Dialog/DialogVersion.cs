using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogVersion : Form {
		public DialogVersion() {

			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			string ver;
			try {
				var assembly = Assembly.GetExecutingAssembly();
				ver = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
			} catch {
				ver = SoftwareInformation.VersionEnglish;
			}

			TextVersion.Text = string.Format( "{0} (ver. {1} - {2} Release)", SoftwareInformation.VersionJapanese, ver, SoftwareInformation.UpdateTime.ToString( "d" ) ); 

			this.ResumeLayoutForDpiScale();
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
