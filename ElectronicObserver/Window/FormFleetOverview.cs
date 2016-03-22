using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
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
				Number.Margin = new Padding( 3, 2, 3, 2 );
				Number.Text = string.Format( "#{0}:", fleetID );
				Number.Tag = null;

				State = new ImageLabel();
				State.Anchor = AnchorStyles.Left;
				State.Margin = new Padding( 3, 2, 3, 2 );
				State.ImageList = ResourceManager.Instance.Icons;
				State.Text = "-";
				State.Tag = FleetData.FleetStates.NoShip;

				ConfigurationChanged( parent );

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


			public void ConfigurationChanged( FormFleetOverview parent ) {
				Number.Font = parent.Font;
				State.Font = parent.Font;
				State.BackColor = Color.Transparent;
			}
		}


		private List<TableFleetControl> ControlFleet;
		private ImageLabel CombinedTag;
		private ImageLabel AnchorageRepairingTimer;

		private Pen LinePen = Pens.Silver;

		private Brush fleetReady;
		private Brush fleetExpedition;
		private Brush fleetSortie;
		private Brush fleetNotReady;
		private Brush fleetDamage;


		public FormFleetOverview( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableFleet );


			ControlFleet = new List<TableFleetControl>( 4 );
			for ( int i = 0; i < 4; i++ ) {
				ControlFleet.Add( new TableFleetControl( this, i + 1, TableFleet ) );
			}

			{
				AnchorageRepairingTimer = new ImageLabel();
				AnchorageRepairingTimer.Anchor = AnchorStyles.Left;
				AnchorageRepairingTimer.Margin = new Padding( 3, 2, 3, 2 );
				AnchorageRepairingTimer.ImageList = ResourceManager.Instance.Icons;
				AnchorageRepairingTimer.ImageIndex = (int)ResourceManager.IconContent.FleetDocking;
				AnchorageRepairingTimer.Text = "-";
				//AnchorageRepairingTimer.Visible = false;

				TableFleet.Controls.Add( AnchorageRepairingTimer, 1, 4 );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.AutoSize, 0 );

				if ( TableFleet.RowStyles.Count > 4 )
					TableFleet.RowStyles[4] = rs;
				else
					while ( TableFleet.RowStyles.Count <= 4 )
						TableFleet.RowStyles.Add( rs );
				#endregion
			}

			#region CombinedTag
			{
				CombinedTag = new ImageLabel();
				CombinedTag.Anchor = AnchorStyles.Left;
				CombinedTag.Margin = new Padding( 3, 2, 3, 2 );
				CombinedTag.ImageList = ResourceManager.Instance.Icons;
				CombinedTag.ImageIndex = (int)ResourceManager.IconContent.FleetCombined;
				CombinedTag.Text = "-";
				CombinedTag.Visible = false;

				TableFleet.Controls.Add( CombinedTag, 1, 5 );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.AutoSize, 0 );

				if ( TableFleet.RowStyles.Count > 5 )
					TableFleet.RowStyles[5] = rs;
				else
					while ( TableFleet.RowStyles.Count <= 5 )
						TableFleet.RowStyles.Add( rs );
				#endregion

			}
			#endregion



			ConfigurationChanged();

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
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += ChangeOrganization;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_hensei/change"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += Updated;
			o.APIList["api_req_map/start"].RequestReceived += Updated;
			o.APIList["api_req_hensei/combined"].RequestReceived += Updated;

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
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void ConfigurationChanged() {
			Font = Utility.Configuration.Config.UI.MainFont;
			LinePen = new Pen( Utility.Configuration.Config.UI.LineColor.ColorData );

			fleetReady = new SolidBrush( Utility.Configuration.Config.UI.FleetReadyColor );
			fleetExpedition = new SolidBrush( Utility.Configuration.Config.UI.FleetExpeditionColor );
			fleetSortie = new SolidBrush( Utility.Configuration.Config.UI.FleetSortieColor );
			fleetNotReady = new SolidBrush( Utility.Configuration.Config.UI.FleetNotReadyColor );
			fleetDamage = new SolidBrush( Utility.Configuration.Config.UI.FleetDamageColor );

			AutoScroll = Utility.Configuration.Config.FormFleet.IsScrollable;

			foreach ( var c in ControlFleet )
				c.ConfigurationChanged( this );

			CombinedTag.Font = Font;
			AnchorageRepairingTimer.Font = Font;
			AnchorageRepairingTimer.Visible = Utility.Configuration.Config.FormFleet.ShowAnchorageRepairingTimer;
		}


		private void Updated( string apiname, dynamic data ) {

			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Update();
			}

			if ( KCDatabase.Instance.Fleet.CombinedFlag > 0 ) {
				CombinedTag.Text = Constants.GetCombinedFleet( KCDatabase.Instance.Fleet.CombinedFlag );

				if ( KCDatabase.Instance.Fleet.CombinedFlag == 3 ) {
					ToolTipInfo.SetToolTip( CombinedTag, string.Format( "ドラム缶搭載: {0}個\r\n大発動艇搭載: {1}個\r\n輸送量(TP): {2}\r\n",
						KCDatabase.Instance.Fleet[1].MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 30 ) ) +
						KCDatabase.Instance.Fleet[2].MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 30 ) ),
						KCDatabase.Instance.Fleet[1].MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 24 ) ) +
						KCDatabase.Instance.Fleet[2].MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 24 ) ),
						Calculator.GetTPDamage( KCDatabase.Instance.Fleet[1] ) + Calculator.GetTPDamage( KCDatabase.Instance.Fleet[2] )
						) );
				} else {
					ToolTipInfo.SetToolTip( CombinedTag, null );
				}

				CombinedTag.Visible = true;
			} else {
				CombinedTag.Visible = false;
			}

			AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer );
			AnchorageRepairingTimer.Tag = KCDatabase.Instance.Fleet.AnchorageRepairingTimer;
			ToolTipInfo.SetToolTip( AnchorageRepairingTimer, "泊地修理タイマ\r\n開始: " + DateTimeHelper.TimeToCSVString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer ) + "\r\n回復: " + DateTimeHelper.TimeToCSVString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer.AddMinutes( 20 ) ) );

			TableFleet.Invalidate();
		}

		void ChangeOrganization( string apiname, dynamic data ) {

			for ( int i = 0; i < ControlFleet.Count; i++ )
				ControlFleet[i].ResetState();

		}


		void UpdateTimerTick() {
			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Refresh();

				if ( i > 0 && Utility.Configuration.Config.UI.NotExpeditionBlink ) {
					FleetData.FleetStates state = (FleetData.FleetStates)ControlFleet[i].State.Tag;

					if ( state == FleetData.FleetStates.Ready
						|| state == FleetData.FleetStates.NotReplenished
						|| state == FleetData.FleetStates.Sparkled ) {

						TableFleet.Invalidate( false );
					}
				}
			}

			if ( AnchorageRepairingTimer.Visible && AnchorageRepairingTimer.Tag != null )
				AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString( (DateTime)AnchorageRepairingTimer.Tag );
		}

		private void TableFleet_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {

			Rectangle bounds = e.CellBounds;

			if ( e.Row < ControlFleet.Count ) {

				var state = (FleetData.FleetStates)ControlFleet[e.Row].State.Tag;
				Brush brush = null;

				switch ( state ) {
					case FleetData.FleetStates.Ready:
					case FleetData.FleetStates.Sparkled:
						if ( e.Row > 0 ) {
							brush = DateTime.Now.Second % 2 == 0 ? fleetReady : null;
						} else {
							brush = fleetReady;
						}
						break;

					case FleetData.FleetStates.Expedition:
						brush = fleetExpedition;
						break;

					case FleetData.FleetStates.Sortie:
						brush = fleetSortie;
						break;

					case FleetData.FleetStates.Damaged:
					case FleetData.FleetStates.SortieDamaged:
						brush = DateTime.Now.Second % 2 == 0 ? fleetDamage : null;
						break;

					case FleetData.FleetStates.NotReplenished:
					case FleetData.FleetStates.Tired:
						if ( e.Row > 0 ) {
							brush = DateTime.Now.Second % 2 == 0 ? fleetNotReady : null;
						} else {
							brush = fleetNotReady;
						}
						break;
				}

				if ( brush != null ) {
					e.Graphics.FillRectangle( brush, bounds.X, bounds.Top, bounds.Width, bounds.Height - 1 );
				}
			}

			e.Graphics.DrawLine( LinePen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1 );

		}



		public override string GetPersistString()
		{
			return "FleetOverview";
		}

	}

}
