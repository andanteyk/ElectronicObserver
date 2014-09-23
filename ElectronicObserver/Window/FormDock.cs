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
				Name.MaximumSize = new Size( 80, 20 );
				Name.AutoEllipsis = true;
				Name.AutoSize = true;
				Name.Visible = true;

				RepairTime = new Label();
				RepairTime.Text = "";
				RepairTime.Anchor = AnchorStyles.Left;
				RepairTime.Font = parent.Font;
				RepairTime.ForeColor = parent.ForeColor;
				RepairTime.Tag = null;
				RepairTime.Padding = new Padding( 0, 1, 0, 1 );
				RepairTime.Margin = new Padding( 2, 0, 2, 0 );
				RepairTime.MinimumSize = new Size( 60, 20 );
				RepairTime.AutoSize = true;
				RepairTime.Visible = true;
				
				
				#endregion

			}


			public TableDockControl( FormDock parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
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


			//データ更新時
			public void Update( int dockID ) {

				KCDatabase db = KCDatabase.Instance;

				DockData dock = db.Docks[dockID];

				if ( dock == null || dock.State == -1 ) {
					//locked
					Name.Text = "";
					RepairTime.Text = "";
					RepairTime.Tag = null;

				} else if ( dock.State == 0 ) {
					//empty
					Name.Text = "----";
					RepairTime.Text = "";
					RepairTime.Tag = null;

				} else {
					//repairing
					Name.Text = db.MasterShips[db.Ships[dock.ShipID].ShipID].Name;
					RepairTime.Text = DateConverter.ToTimeRemainString( dock.CompletionTime );
					RepairTime.Tag = dock.CompletionTime;

				}

			}

			//タイマー更新時
			public void Refresh( int dockID ) {

				if ( RepairTime.Tag != null )
					RepairTime.Text = DateConverter.ToTimeRemainString( (DateTime)RepairTime.Tag );

			}

		}



		private TableDockControl[] ControlDock;




		public FormDock( FormMain parent ) {
			InitializeComponent();

			parent.UpdateTimerTick += parent_UpdateTimerTick;

			TableDock.SuspendLayout();
			ControlDock = new TableDockControl[4];
			for ( int i = 0; i < ControlDock.Length; i++ ) {
				ControlDock[i] = new TableDockControl( this, TableDock, i );
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


		void parent_UpdateTimerTick( object sender, EventArgs e ) {

			TableDock.SuspendLayout();
			for ( int i = 0; i < ControlDock.Length; i++ )
				ControlDock[i].Refresh( i + 1 );
			TableDock.ResumeLayout();

		}



		private void TableDock_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



	}

}
