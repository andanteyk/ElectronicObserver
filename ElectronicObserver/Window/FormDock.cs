using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormDock : DockContent {

		private class TableDockControl {

			public Label Name;
			public Label RepairTime;


			public TableDockControl( FormDock parent ) {

				#region Initialize

				Name = new Label();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.Font = parent.Font;
				Name.ForeColor = parent.ForeColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.MaximumSize = new Size( 60, 20 );
				Name.AutoEllipsis = true;
				Name.AutoSize = true;
				Name.Visible = false;

				RepairTime = new Label();
				RepairTime.Text = "*nothing*";
				RepairTime.Anchor = AnchorStyles.Left;
				RepairTime.Font = parent.Font;
				RepairTime.ForeColor = parent.ForeColor;
				RepairTime.Padding = new Padding( 0, 1, 0, 1 );
				RepairTime.Margin = new Padding( 2, 0, 2, 0 );
				RepairTime.AutoSize = true;
				RepairTime.Visible = false;
				
				
				#endregion

			}


			public TableDockControl( FormDock parent, TableLayoutPanel table, int row )
				: this( parent ) {


			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( Name, 0, row );
				table.Controls.Add( RepairTime, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion

			}


			public void Update( int dockID ) {

				KCDatabase db = KCDatabase.Instance;

				DockData dock = db.Docks[dockID];

				if ( dock == null || dock.State == -1 ) {
					//locked
					Name.Text = "";
					RepairTime.Text = "";

				} else if ( dock.State == 0 ) {
					//empty
					Name.Text = "----";
					RepairTime.Text = "";

				} else {
					//repairing
					Name.Text = db.MasterShips[db.Ships[dock.ShipID].ShipID].Name;
					RepairTime.Text = DateConverter.ToTimeRemainString( dock.CompletionTime );

				}

			}

		}



		private TableDockControl[] ControlDock;




		public FormDock( FormMain parent ) {
			InitializeComponent();

			TableDock.SuspendLayout();
			ControlDock = new TableDockControl[4];
			for ( int i = 0; i < ControlDock.Length; i++ ) {
				ControlDock[i] = new TableDockControl( this );
			}
			TableDock.ResumeLayout();

		}


		private void FormDock_Load( object sender, EventArgs e ) {

			KCDatabase Database = KCDatabase.Instance;

			Database.DocksUpdated += ( DatabaseUpdatedEventArgs e1 ) => Invoke( new KCDatabase.DatabaseUpdatedEventHandler( Database_DocksUpdated ), e1 );

		}

		void Database_DocksUpdated( DatabaseUpdatedEventArgs e ) {

			TableDock.SuspendLayout();
			for ( int i = 0; i < ControlDock.Length; i++ )
				ControlDock[i].Update( i + 1 );
			TableDock.ResumeLayout();

		}


		//undone:timerupdate, cellpaint
	}

}
