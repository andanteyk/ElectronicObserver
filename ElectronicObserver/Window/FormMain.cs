using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window {
	public partial class FormMain : Form {

		#region Forms

		public FormFleet[] fFleet;

		#endregion




		public FormMain() {
			InitializeComponent();
		}



		private void FormMain_Load( object sender, EventArgs e ) {

			ResourceManager.Instance.Load();


			APIObserver.Instance.Start( 40620 );


			MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();

			//form init
			fFleet = new FormFleet[4];
			for ( int i = 0; i < fFleet.Length; i++ ) {
				fFleet[i] = new FormFleet( i + 1 );
				fFleet[i].Show( MainDockPanel );
			}
			
		}




		private void StripMenu_Debug_LoadAPIFromFile_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogLocalAPILoader() ) {
				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					if ( dialog.IsResponse ) {
						APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
					}
					if ( dialog.IsRequest ) {
						APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
					}

				}

			}

		}


	}
}
