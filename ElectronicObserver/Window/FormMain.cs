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

		#region Properties
		#endregion


		#region Events

		public event EventHandler UpdateTimerTick = delegate { };

		#endregion


		#region Forms

		public FormFleet[] fFleet;
		public FormDock fDock;

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
				fFleet[i] = new FormFleet( this, i + 1 );
				//fFleet[i].Show( MainDockPanel );
			}
			fDock = new FormDock( this );

			UIUpdateTimer.Start();

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



		private void UIUpdateTimer_Tick( object sender, EventArgs e ) {

			UpdateTimerTick( this, new EventArgs() );


			{
				DateTime now = DateTime.Now;
				StripStatus_Clock.Text = string.Format( "{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second );
			}
		}





		#region フォーム表示
		
		private void StripMenu_View_Fleet_1_Click( object sender, EventArgs e ) {
			fFleet[0].Show( MainDockPanel );
		}


		private void StripMenu_View_Fleet_2_Click( object sender, EventArgs e ) {
			fFleet[1].Show( MainDockPanel );
		}

		private void StripMenu_View_Fleet_3_Click( object sender, EventArgs e ) {
			fFleet[2].Show( MainDockPanel ); 
		}

		private void StripMenu_View_Fleet_4_Click( object sender, EventArgs e ) {
			fFleet[3].Show( MainDockPanel );
		}

		private void StripMenu_View_Dock_Click( object sender, EventArgs e ) {
			fDock.Show( MainDockPanel );
		}

		#endregion

		
		

		

	}
}
