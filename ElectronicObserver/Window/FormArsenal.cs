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

	public partial class FormArsenal : DockContent {

		private class TableArsenalControl {

			public Label ShipName;
			public Label CompletionTime;
			private ToolTip tooltip;

			public TableArsenalControl( FormArsenal parent ) {

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

				CompletionTime = new Label();
				CompletionTime.Text = "";
				CompletionTime.Anchor = AnchorStyles.Left;
				CompletionTime.Font = parent.Font;
				CompletionTime.ForeColor = parent.ForeColor;
				CompletionTime.Tag = null;
				CompletionTime.TextAlign = ContentAlignment.MiddleLeft;
				CompletionTime.Padding = new Padding( 0, 1, 0, 1 );
				CompletionTime.Margin = new Padding( 2, 0, 2, 0 );
				CompletionTime.MinimumSize = new Size( 60, 10 );
				CompletionTime.AutoSize = true;
				CompletionTime.Visible = true;


				tooltip = parent.ToolTipInfo;
				#endregion

			}


			public TableArsenalControl( FormArsenal parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( CompletionTime, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion

			}


			public void Update( int arsenalID ) {

				KCDatabase db = KCDatabase.Instance;
				ArsenalData arsenal = db.Arsenals[arsenalID];
				bool showShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;


				tooltip.SetToolTip( ShipName, null );
				tooltip.SetToolTip( CompletionTime, null );

				if ( arsenal == null || arsenal.State == -1 ) {
					//locked
					ShipName.Text = "";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;
					
				} else if ( arsenal.State == 0 ) {
					//empty
					ShipName.Text = "----";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;
					
				} else if ( arsenal.State == 2 ) {
					//building
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip( ShipName, name );
					CompletionTime.Text = DateTimeHelper.ToTimeRemainString( arsenal.CompletionTime );
					CompletionTime.Tag = arsenal.CompletionTime;
					tooltip.SetToolTip( CompletionTime, "完了日時 : " + arsenal.CompletionTime.ToString() );

				} else if ( arsenal.State == 3 ) {
					//complete!
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip( ShipName, name );
					CompletionTime.Text = "完成！";
					CompletionTime.Tag = null;
					
				}

			}


			public void Refresh( int arsenalID ) {

				if ( CompletionTime.Tag != null ) {
					CompletionTime.Text = DateTimeHelper.ToTimeRemainString( (DateTime)CompletionTime.Tag );
				}
			}

		}


		private TableArsenalControl[] ControlArsenal;


		public FormArsenal( FormMain parent ) {
			InitializeComponent();

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ControlHelper.SetDoubleBuffered( TableArsenal );

			ConfigurationChanged();

			TableArsenal.SuspendLayout();
			ControlArsenal = new TableArsenalControl[4];
			for ( int i = 0; i < ControlArsenal.Length; i++ ) {
				ControlArsenal[i] = new TableArsenalControl( this, TableArsenal, i );
			}
			TableArsenal.ResumeLayout();

			

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal] );
		}


		
		private void FormArsenal_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_kousyou/createship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;

			o.APIList["api_get_member/kdock"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		void Updated( string apiname, dynamic data ) {

			UpdateUI();
		}

		void UpdateUI() {

			if ( ControlArsenal == null ) return;

			TableArsenal.SuspendLayout();
			for ( int i = 0; i < ControlArsenal.Length; i++ )
				ControlArsenal[i].Update( i + 1 );
			TableArsenal.ResumeLayout();

		}

		void UpdateTimerTick() {

			TableArsenal.SuspendLayout();
			for ( int i = 0; i < ControlArsenal.Length; i++ )
				ControlArsenal[i].Refresh( i + 1 );
			TableArsenal.ResumeLayout();

		}


		void ConfigurationChanged() {

			Font = Utility.Configuration.Config.UI.MainFont;
			MenuMain_ShowShipName.Checked = Utility.Configuration.Config.FormArsenal.ShowShipName;

		}


		private void MenuMain_ShowShipName_CheckedChanged( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormArsenal.ShowShipName = MenuMain_ShowShipName.Checked;

			UpdateUI();
		}


		private void TableArsenal_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		protected override string GetPersistString() {
			return "Arsenal";
		}


		
	}

}
