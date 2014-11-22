using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
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

	public partial class FormFleet : DockContent {

		private class TableFleetControl {
			public Label Name;
			public ImageLabel StateMain;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ToolTip ToolTipInfo;
			public ElectronicObserver.Data.FleetData.FleetStates State;
			public DateTime Timer;
		
			public TableFleetControl( FormFleet parent ) {

				#region Initialize

				Name = new Label();
				Name.Text = "[" + parent.FleetID.ToString() + "]";
				Name.Anchor = AnchorStyles.Left;
				Name.Font = parent.MainFont;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				//Name.Visible = false;

				StateMain = new ImageLabel();
				StateMain.Anchor = AnchorStyles.Left;
				StateMain.Font = parent.MainFont;
				StateMain.ForeColor = parent.MainFontColor;
				StateMain.ImageList = ResourceManager.Instance.Icons;
				StateMain.Padding = new Padding( 2, 2, 2, 2 );
				StateMain.Margin = new Padding( 2, 0, 2, 0 );
				StateMain.AutoSize = true;

				AirSuperiority = new ImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Left;
				AirSuperiority.Font = parent.MainFont;
				AirSuperiority.ForeColor = parent.MainFontColor;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
				AirSuperiority.Padding = new Padding( 2, 2, 2, 2 );
				AirSuperiority.Margin = new Padding( 2, 0, 2, 0 );
				AirSuperiority.AutoSize = true;

				SearchingAbility = new ImageLabel();
				SearchingAbility.Anchor = AnchorStyles.Left;
				SearchingAbility.Font = parent.MainFont;
				SearchingAbility.ForeColor = parent.MainFontColor;
				SearchingAbility.ImageList = ResourceManager.Instance.Equipments;
				SearchingAbility.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon;
				SearchingAbility.Padding = new Padding( 2, 2, 2, 2 );
				SearchingAbility.Margin = new Padding( 2, 0, 2, 0 );
				SearchingAbility.AutoSize = true;

				ToolTipInfo = parent.ToolTipInfo;
				State = 0;
				Timer = DateTime.Now;

				#endregion

			}

			public TableFleetControl( FormFleet parent, TableLayoutPanel table ) 
				: this( parent ) {
				AddToTable( table );
			}

			public void AddToTable( TableLayoutPanel table ) {

				table.Controls.Add( Name, 0, 0 );
				table.Controls.Add( StateMain, 1, 0 );
				table.Controls.Add( AirSuperiority, 2, 0 );
				table.Controls.Add( SearchingAbility, 3, 0 );

				int row = 0;
				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}

			public void Update( FleetData fleet ) {

				KCDatabase db = KCDatabase.Instance;

				Name.Text = fleet.Name;



				#region set StateMain

				

				State = FleetData.UpdateFleetState( fleet, StateMain, ToolTipInfo, ref Timer ); 
				
				#endregion


				//制空戦力計算	
				AirSuperiority.Text = fleet.GetAirSuperiority().ToString();
				
				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbility().ToString();		


			}


			public void Refresh() {

				FleetData.RefreshFleetState( StateMain, State, Timer );
		
			}


		}


		private class TableMemberControl {
			public Label Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;


			public TableMemberControl( FormFleet parent ) {

				#region Initialize

				Name = new Label();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.Font = parent.MainFont;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				Name.Visible = false;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left;
				Level.Value = 0;
				Level.MaximumValue = 150;
				Level.ValueNext = 0;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding( 0, 0, 0, 0 );
				Level.Margin = new Padding( 2, 0, 2, 0 );
				Level.AutoSize = true;
				Level.Visible = false;
				Name.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendLayout();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.Padding = new Padding( 0, 0, 0, 0 );
				HP.Margin = new Padding( 2, 1, 2, 2 );
				HP.AutoSize = true;
				HP.Visible = false;
				HP.ResumeLayout();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Right;
				Condition.Font = parent.MainFont;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding( 2, 2, 2, 2 );
				Condition.Margin = new Padding( 2, 0, 2, 0 );
				Condition.Size = new Size( 40, 20 );
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource( parent.ToolTipInfo );
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding( 0, 2, 0, 1 );
				ShipResource.Margin = new Padding( 2, 0, 2, 0 );
				ShipResource.Size = new Size( 40, 20 );
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding( 0, 2, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.Size = new Size( 40, 20 );
				Equipments.AutoSize = true;
				Equipments.Visible = false;
				Equipments.ResumeLayout();


				ToolTipInfo = parent.ToolTipInfo;
				#endregion

			}

			public TableMemberControl( FormFleet parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( Name, 0, row );
				table.Controls.Add( Level, 1, row );
				table.Controls.Add( HP, 2, row );
				table.Controls.Add( Condition, 3, row );
				table.Controls.Add( ShipResource, 4, row );
				table.Controls.Add( Equipments, 5, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}

			public void Update( int shipMasterID ) {

				KCDatabase db = KCDatabase.Instance;

				if ( shipMasterID != -1 ) {

					ShipData ship = db.Ships[shipMasterID];
					ShipDataMaster shipmaster = db.MasterShips[ship.ShipID];


					Name.Text = shipmaster.Name;
					
					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;
					
					HP.Value = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					HP.RepairTime = null;
					foreach ( var dock in db.Docks ) {
						if ( dock.Value.ShipID == shipMasterID ) {
							HP.RepairTime = dock.Value.CompletionTime;
							break;
						}
					}
					
					Condition.Text = ship.Condition.ToString();
					if ( ship.Condition < 20 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					} else if ( ship.Condition < 30 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					} else if ( ship.Condition < 40 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;
					} else if ( ship.Condition < 50 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					} else {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
					}

					ShipResource.SetResources( ship.Fuel, shipmaster.Fuel, ship.Ammo, shipmaster.Ammo );


					Equipments.SetSlotList( ship );
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( ship ) );

				}


				Name.Visible =
				Level.Visible =
				HP.Visible = 
				Condition.Visible = 
				ShipResource.Visible = 
				Equipments.Visible = shipMasterID != -1;

			}


			private string GetEquipmentString( ShipData ship ) {
				StringBuilder sb = new StringBuilder();
				
				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( ship.Slot[i] != -1 )
						sb.AppendFormat( "[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], KCDatabase.Instance.Equipments[ship.Slot[i]].MasterEquipment.Name );
				}

				return sb.ToString();
			}
		}




		public int FleetID { get; private set; }

		
		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;
		

		
		public FormFleet( FormMain parent, int fleetID ) {
			InitializeComponent();

			FleetID = fleetID;
			parent.UpdateTimerTick += parent_UpdateTimerTick;

			//todo: 後々外部から設定できるように
			MainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			SubFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			MainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			SubFontColor = Color.FromArgb( 0x88, 0x88, 0x88 );


			//ui init

			//doublebuffered
			System.Reflection.PropertyInfo prop = typeof( TableLayoutPanel ).GetProperty( "DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic );
			prop.SetValue( TableFleet, true, null );
			prop.SetValue( TableMember, true, null );

			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			TableFleet.BorderStyle = BorderStyle.FixedSingle;
			ControlFleet = new TableFleetControl( this, TableFleet );
			TableFleet.ResumeLayout();


			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[6];
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableMemberControl( this, TableMember, i );
			}
			TableMember.ResumeLayout();

		}

		

		private void FormFleet_Load( object sender, EventArgs e ) {

			Text = string.Format( "#{0}", FleetID );

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_req_nyukyo/start"].RequestReceived += rec;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += rec;
			o.APIList["api_req_hensei/change"].RequestReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += rec;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += rec;

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_get_member/ship2"].ResponseReceived += rec;
			o.APIList["api_get_member/ndock"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += rec;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += rec;
			o.APIList["api_get_member/ship3"].ResponseReceived += rec;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += rec;		//requestのほうは面倒なのでこちらでまとめてやる
			o.APIList["api_get_member/deck"].ResponseReceived += rec;

		}


		void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			ControlFleet.Update( fleet );
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( fleet.FleetMember[i] );
			}
			TableMember.ResumeLayout();

		}


		void parent_UpdateTimerTick( object sender, EventArgs e ) {

			TableFleet.SuspendLayout();
			{
				FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];
				if ( fleet != null )
					ControlFleet.Refresh();		//タイマだけ保持する実装にしないとDB更新中に参照して死ぬ

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();

		}


		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];

			sb.AppendFormat( "{0}\t制空戦力{1}/索敵能力{2}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbility() );
			for ( int i = 0; i < fleet.FleetMember.Count; i++ ) {
				if ( fleet[i] == -1 )
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat( "{0}/{1}\t", ship.MasterShip.Name, ship.Level );

				int[] slot = new int[ship.Slot.Count];
				for ( int j = 0; j < slot.Length; j++ ) {
					if ( ship.Slot[j] != -1 )
						slot[j] = db.Equipments[ship.Slot[j]].EquipmentID;
					else
						slot[j] = -1;
				}


				for ( int j = 0; j < slot.Length; j++ ) {

					if ( slot[j] == -1 ) continue;

					int count = 1;
					for ( int k = j + 1; k < ship.Slot.Count; k++ ) {
						if ( slot[k] == slot[j] ) {
							count++;
						} else {
							break;
						}
					}

					if ( count == 1 ) {
						sb.AppendFormat( "{0}{1}", j == 0 ? "" : "/", db.MasterEquipments[slot[j]].Name );
					} else {
						sb.AppendFormat( "{0}{1}x{2}", j == 0 ? "" : "/", db.MasterEquipments[slot[j]].Name, count );
					}

					j += count - 1;
				}

				sb.AppendLine();
			}


			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}




		//checkme:別クラスへの移動も考える

		

		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		protected override string GetPersistString() {
			return "Fleet #" + FleetID.ToString();
		}



	
	}

}
