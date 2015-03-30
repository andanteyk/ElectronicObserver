﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormDock : DockContent {

		private class TableDockControl {

			public Label ShipName;
			public Label RepairTime;
			public ToolTip ToolTipInfo;

			public TableDockControl( FormDock parent ) {

				#region Initialize

				ShipName = new Label();
				ShipName.Text = "???";
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.Font = parent.Font;
				ShipName.ForeColor = parent.ForeColor;
				ShipName.TextAlign = ContentAlignment.MiddleLeft;
				ShipName.Padding = new Padding( 0, 1, 0, 1 );
				ShipName.Margin = new Padding( 2, 0, 2, 0 );
				ShipName.MaximumSize = new Size( 60, 20 );
				ShipName.AutoEllipsis = true;
				ShipName.AutoSize = true;
				ShipName.Visible = true;

				RepairTime = new Label();
				RepairTime.Text = "";
				RepairTime.Anchor = AnchorStyles.Left;
				RepairTime.Font = parent.Font;
				RepairTime.ForeColor = parent.ForeColor;
				RepairTime.Tag = null;
				RepairTime.TextAlign = ContentAlignment.MiddleLeft;
				RepairTime.Padding = new Padding( 0, 1, 0, 1 );
				RepairTime.Margin = new Padding( 2, 0, 2, 0 );
				RepairTime.MinimumSize = new Size( 60, 10 );
				RepairTime.AutoSize = true;
				RepairTime.Visible = true;

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}


			public TableDockControl( FormDock parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
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

				ToolTipInfo.SetToolTip( ShipName, null );
				ToolTipInfo.SetToolTip( RepairTime, null );

				if ( dock == null || dock.State == -1 ) {
					//locked
					ShipName.Text = "";
					RepairTime.Text = "";
					RepairTime.Tag = null;

				} else if ( dock.State == 0 ) {
					//empty
					ShipName.Text = "----";
					RepairTime.Text = "";
					RepairTime.Tag = null;

				} else {
					//repairing
					ShipName.Text = db.Ships[dock.ShipID].Name;
					ToolTipInfo.SetToolTip( ShipName, db.Ships[dock.ShipID].NameWithLevel );
					RepairTime.Text = DateTimeHelper.ToTimeRemainString( dock.CompletionTime );
					RepairTime.Tag = dock.CompletionTime;
					ToolTipInfo.SetToolTip( RepairTime, "完了日時 : " + dock.CompletionTime );

				}

			}

			//タイマー更新時
			public void Refresh( int dockID ) {

				if ( RepairTime.Tag != null )
					RepairTime.Text = DateTimeHelper.ToTimeRemainString( (DateTime)RepairTime.Tag );

			}

		}



		private TableDockControl[] ControlDock;




		public FormDock( FormMain parent ) {
			InitializeComponent();

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;


			ControlHelper.SetDoubleBuffered( TableDock );

			ConfigurationChanged();

			
			TableDock.SuspendLayout();
			ControlDock = new TableDockControl[4];
			for ( int i = 0; i < ControlDock.Length; i++ ) {
				ControlDock[i] = new TableDockControl( this, TableDock, i );
			}
			TableDock.ResumeLayout();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDock] );

		}

		
		private void FormDock_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ndock"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		

		void Updated( string apiname, dynamic data ) {

			TableDock.SuspendLayout();
			for ( int i = 0; i < ControlDock.Length; i++ )
				ControlDock[i].Update( i + 1 );
			TableDock.ResumeLayout();

		}


		void UpdateTimerTick() {

			TableDock.SuspendLayout();
			for ( int i = 0; i < ControlDock.Length; i++ )
				ControlDock[i].Refresh( i + 1 );
			TableDock.ResumeLayout();

		}



		private void TableDock_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		void ConfigurationChanged() {

			Font = Utility.Configuration.Config.UI.MainFont;
		}


		protected override string GetPersistString() {
			return "Dock";
		}

	}

}
