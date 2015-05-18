using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;
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

	public partial class FormFleetOverview : DockContent {

		private class TableFleetControl {

			public ImageLabel Number;
			public ImageLabel State;
			public ToolTip ToolTipInfo;
			private int fleetID;

			public TableFleetControl( FormFleetOverview parent, int fleetID ) {

				#region Initialize

				Number = new ImageLabel();
				Number.Anchor = AnchorStyles.Left;
				Number.ImageAlign = ContentAlignment.MiddleCenter;
				Number.Font = parent.Font;
				Number.Margin = new Padding( 3, 2, 3, 2 );
				Number.Text = string.Format( "#{0}:", fleetID );
				Number.Tag = null;

				State = new ImageLabel();
				State.Anchor = AnchorStyles.Left;
				State.Font = parent.Font;
				State.Margin = new Padding( 3, 2, 3, 2 );
				State.ImageList = ResourceManager.Instance.Icons;
				State.Text = "-";
				State.Tag = FleetData.FleetStates.NoShip;

				this.fleetID = fleetID;
				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl( FormFleetOverview parent, int fleetID, TableLayoutPanel table )
				: this( parent, fleetID ) {

				AddToTable( table, fleetID - 1 );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( Number, 0, row );
				table.Controls.Add( State, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.AutoSize, 0 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion

			}


			public void Update() {

				FleetData fleet =  KCDatabase.Instance.Fleet[fleetID];
				if ( fleet == null ) return;

				DateTime dt = (DateTime?)Number.Tag ?? DateTime.Now;
				State.Tag = FleetData.UpdateFleetState( fleet, State, ToolTipInfo, (FleetData.FleetStates)State.Tag, ref dt );
				Number.Tag = dt;

				ToolTipInfo.SetToolTip( Number, fleet.Name );
			}

			public void ResetState() {
				State.Tag = FleetData.FleetStates.NoShip;
			}

			public void Refresh() {

				FleetData.RefreshFleetState( State, (FleetData.FleetStates)State.Tag, (DateTime?)Number.Tag ?? DateTime.Now );
			}
		}


		private List<TableFleetControl> ControlFleet;
		private ImageLabel CombinedTag;


		public FormFleetOverview( FormMain parent ) {
			InitializeComponent();


			ConfigurationChanged();

			ControlHelper.SetDoubleBuffered( TableFleet );


			ControlFleet = new List<TableFleetControl>( 4 );
			for ( int i = 0; i < 4; i++ ) {
				ControlFleet.Add( new TableFleetControl( this, i + 1, TableFleet ) );
			}


			#region CombinedTag
			{
				CombinedTag = new ImageLabel();
				CombinedTag.Anchor = AnchorStyles.Left;
				CombinedTag.Font = Utility.Configuration.Config.UI.MainFont;
				CombinedTag.Margin = new Padding( 3, 2, 3, 2 );
				CombinedTag.ImageList = ResourceManager.Instance.Icons;
				CombinedTag.ImageIndex = (int)ResourceManager.IconContent.FleetCombined;
				CombinedTag.Text = "-";
				CombinedTag.Visible = false;


				TableFleet.Controls.Add( CombinedTag, 1, 4 );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.AutoSize, 0 );

				if ( TableFleet.RowStyles.Count > 4 )
					TableFleet.RowStyles[4] = rs;
				else
					while ( TableFleet.RowStyles.Count <= 4 )
						TableFleet.RowStyles.Add( rs );
				#endregion

			}
			#endregion


			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;
		}



		private void FormFleetOverview_Load( object sender, EventArgs e ) {



			//api register
			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_hensei/change"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += ChangeOrganization;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_hensei/change"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += Updated;
			o.APIList["api_req_map/start"].RequestReceived += Updated;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_get_member/ndock"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship3"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;		//requestのほうは面倒なのでこちらでまとめてやる
			o.APIList["api_get_member/deck"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void ConfigurationChanged() {
			Font = Utility.Configuration.Config.UI.MainFont;
		}


		private void Updated( string apiname, dynamic data ) {

			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Update();
			}

			if ( KCDatabase.Instance.Fleet.CombinedFlag > 0 ) {
				CombinedTag.Text = Constants.GetCombinedFleet( KCDatabase.Instance.Fleet.CombinedFlag );
				CombinedTag.Visible = true;
			} else {
				CombinedTag.Visible = false;
			}
		}

		void ChangeOrganization( string apiname, dynamic data ) {

			for ( int i = 0; i < ControlFleet.Count; i++ )
				ControlFleet[i].ResetState();

		}


		void UpdateTimerTick() {
			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Refresh();
			}
		}



		private void TableFleet_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );

		}



		protected override string GetPersistString() {
			return "FleetOverview";
		}

	}

}
