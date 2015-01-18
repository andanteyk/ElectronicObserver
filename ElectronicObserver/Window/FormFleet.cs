using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private bool IsRemodeling = false;


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
				State = FleetData.FleetStates.NoShip;
				Timer = DateTime.Now;

				#endregion

			}

			public TableFleetControl( FormFleet parent, TableLayoutPanel table ) 
				: this( parent ) {
				AddToTable( table );
			}

			public void AddToTable( TableLayoutPanel table ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, 0 );
				table.Controls.Add( StateMain, 1, 0 );
				table.Controls.Add( AirSuperiority, 2, 0 );
				table.Controls.Add( SearchingAbility, 3, 0 );
				table.ResumeLayout();

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
				{
					int sum = fleet.MembersInstance.Sum( s => s != null ? s.Level : 0 );
					ToolTipInfo.SetToolTip( Name, string.Format( "合計レベル：{0}\r\n平均レベル：{1:0.00}", sum, (double)sum / Math.Max( fleet.Members.Count( id => id != -1 ), 1 ) ) );
				}
				

				State = FleetData.UpdateFleetState( fleet, StateMain, ToolTipInfo, State, ref Timer ); 
				

				//制空戦力計算	
				AirSuperiority.Text = fleet.GetAirSuperiority().ToString();
				
				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbility().ToString();		


			}


			public void ResetState() {
				State = FleetData.FleetStates.NoShip;
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
			private FormFleet Parent;


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
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
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
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.Font = parent.MainFont;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
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
				Equipments.Font = parent.SubFont;
				Equipments.Size = new Size( 40, 20 );
				Equipments.AutoSize = true;
				Equipments.Visible = false;
				Equipments.ResumeLayout();


				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion

			}

			
			public TableMemberControl( FormFleet parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, row );
				table.Controls.Add( Level, 1, row );
				table.Controls.Add( HP, 2, row );
				table.Controls.Add( Condition, 3, row );
				table.Controls.Add( ShipResource, 4, row );
				table.Controls.Add( Equipments, 5, row );
				table.ResumeLayout();

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
					

					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					ToolTipInfo.SetToolTip( Name,
						string.Format( 
							"{0} {1}\n火力: {2}/{3}\n雷装: {4}/{5}\n対空: {6}/{7}\n装甲: {8}/{9}\n対潜: {10}/{11}\n回避: {12}/{13}\n索敵: {14}/{15}\n運: {16}\n(右クリックで図鑑)\n",
							ship.MasterShip.ShipTypeName, ship.NameWithLevel,
							ship.FirepowerBase, ship.FirepowerTotal,
							ship.TorpedoBase, ship.TorpedoTotal,
							ship.AABase, ship.AATotal,
							ship.ArmorBase, ship.ArmorTotal,
							ship.ASWBase, ship.ASWTotal,
							ship.EvasionBase, ship.EvasionTotal,
							ship.LOSBase, ship.LOSTotal,
							ship.LuckTotal
							) );


					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;

					if ( ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel ) {
						ToolTipInfo.SetToolTip( Level, string.Format( "改装まで: {0}", ship.ExpNextRemodel ) );
					} else if ( ship.Level <= 99 ) {
						ToolTipInfo.SetToolTip( Level, string.Format( "Lv99まで: {0}", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 99 ), 0 ) ) );
					} else {
						ToolTipInfo.SetToolTip( Level, string.Format( "Lv150まで: {0}", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 150 ), 0 ) ) );
					}

					
					HP.Value = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					{
						int dockID = ship.RepairingDockID;

						HP.RepairTime = null;
						if ( dockID != -1 ) {
							HP.RepairTime = db.Docks[dockID].CompletionTime;
						}
					}
					if ( KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains( shipMasterID ) ) {
						HP.BackColor = Color.Silver;
					} else {
						HP.BackColor = SystemColors.Control;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat( "HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState( hprate ) );
						if ( hprate > 0.50 ) {
							sb.AppendFormat( "中破まで: {0} / 大破まで: {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4 );
						} else if ( hprate > 0.25 ) {
							sb.AppendFormat( "大破まで: {0}\n", ship.HPCurrent - ship.HPMax / 4 );
						} else {
							sb.AppendLine( "大破しています！" );
						}

						if ( ship.RepairTime > 0 ) {
							sb.AppendFormat( "入渠時間: {0}\n", DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( ship.RepairTime ) ) );
						}

						ToolTipInfo.SetToolTip( HP, sb.ToString() );
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
					if ( ship.Condition < 49 ) {
						ToolTipInfo.SetToolTip( Condition, string.Format( "完全回復まで 約 {0}", DateTimeHelper.ToTimeRemainString( new TimeSpan( 0, (int)Math.Ceiling( ( 49 - ship.Condition ) / 3.0 ) * 3, 0 ) ) ) );
					} else {
						ToolTipInfo.SetToolTip( Condition, string.Format( "あと {0} 回遠征可能", (int)Math.Ceiling( ( ship.Condition - 49 ) / 3.0 ) ) );
					}

					ShipResource.SetResources( ship.Fuel, ship.MasterShip.Fuel, ship.Ammo, ship.MasterShip.Ammo );


					Equipments.SetSlotList( ship );
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( ship ) );

				} else {
					Name.Tag = -1;
				}


				Name.Visible =
				Level.Visible =
				HP.Visible = 
				Condition.Visible = 
				ShipResource.Visible = 
				Equipments.Visible = shipMasterID != -1;

			}

			void Name_MouseDown( object sender, MouseEventArgs e ) {
				int id = (int)Name.Tag;

				if ( id != -1 && ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					new DialogAlbumMasterShip( id ).Show();
				}

			}


			private string GetEquipmentString( ShipData ship ) {
				StringBuilder sb = new StringBuilder();
				
				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( ship.SlotInstance[i] != null )
						sb.AppendFormat( "[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], KCDatabase.Instance.Equipments[ship.Slot[i]].NameWithLevel );
				}

				sb.AppendFormat( "\r\n昼戦: {0}\r\n夜戦: {1}\r\n", 
					Constants.GetDayAttackKind( Calculator.GetDayAttackKind( ship.SlotMaster.ToArray(), ship.ShipID, -1 ) ), 
					Constants.GetNightAttackKind( Calculator.GetNightAttackKind( ship.SlotMaster.ToArray(), ship.ShipID, -1 ) ) );

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
			MainFont = Font = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;
			MainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			SubFontColor = Color.FromArgb( 0x88, 0x88, 0x88 );


			//ui init

			ControlHelper.SetDoubleBuffered( TableFleet );
			ControlHelper.SetDoubleBuffered( TableMember );


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


			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

		}

		

		private void FormFleet_Load( object sender, EventArgs e ) {

			Text = string.Format( "#{0}", FleetID );

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );
			APIReceivedEventHandler r_org = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( ChangeOrganization ), apiname, data );

			o.APIList["api_req_hensei/change"].RequestReceived += r_org;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += r_org;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += r_org;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += r_org;
		
			o.APIList["api_req_nyukyo/start"].RequestReceived += rec;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += rec;
			o.APIList["api_req_hensei/change"].RequestReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += rec;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += rec;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += rec;
			o.APIList["api_req_map/start"].RequestReceived += rec;

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_get_member/ship2"].ResponseReceived += rec;
			o.APIList["api_get_member/ndock"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += rec;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += rec;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += rec;
			o.APIList["api_get_member/ship3"].ResponseReceived += rec;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += rec;		//requestのほうは面倒なのでこちらでまとめてやる
			o.APIList["api_get_member/deck"].ResponseReceived += rec;
			o.APIList["api_get_member/slot_item"].ResponseReceived += rec;
			
			//追加するときは FormFleetOverview にも同様に追加してください
		}


		void Updated( string apiname, dynamic data ) {

			if ( IsRemodeling ) {
				if ( apiname == "api_get_member/slot_item" )
					IsRemodeling = false;
				else
					return;
			}
			if ( apiname == "api_req_kaisou/remodeling" ) {
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if ( db.Ships.Count == 0 || db.Equipments.Count == 0 ) return;
			
			FleetData fleet = db.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			ControlFleet.Update( fleet );
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( fleet.Members[i] );
			}
			TableMember.ResumeLayout();


			if ( Icon != null )	ResourceManager.DestroyIcon( Icon );
			Icon = ResourceManager.ImageToIcon( ControlFleet.StateMain.Image );
			if ( Parent != null ) Parent.Refresh();		//アイコンを更新するため
			
		}

		void ChangeOrganization( string apiname, dynamic data ) {

			ControlFleet.ResetState();
			
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
			for ( int i = 0; i < fleet.Members.Count; i++ ) {
				if ( fleet[i] == -1 )
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat( "{0}/{1}\t", ship.MasterShip.Name, ship.Level );

				var eq = ship.SlotInstance;


				if ( eq != null ) {
					for ( int j = 0; j < eq.Count; j++ ) {

						if ( eq[j] == null ) continue;

						int count = 1;
						for ( int k = j + 1; k < eq.Count; k++ ) {
							if ( eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level ) {
								count++;
							} else {
								break;
							}
						}

						if ( count == 1 ) {
							sb.AppendFormat( "{0}{1}", j == 0 ? "" : "/", eq[j].NameWithLevel );
						} else {
							sb.AppendFormat( "{0}{1}x{2}", j == 0 ? "" : "/", eq[j].NameWithLevel, count );
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}




		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		protected override string GetPersistString() {
			return "Fleet #" + FleetID.ToString();
		}



	
	}

}
