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

				if ( fleet == null ) return;



				Name.Text = fleet.Name;
				{
					int levelSum = fleet.MembersInstance.Sum( s => s != null ? s.Level : 0 );
					int fueltotal = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)( s.MasterShip.Fuel * ( s.IsMarried ? 0.85 : 1.00 ) ) );
					int ammototal = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)( s.MasterShip.Ammo * ( s.IsMarried ? 0.85 : 1.00 ) ) );
					int speed = fleet.MembersWithoutEscaped.Min( s => s == null ? 10 : s.MasterShip.Speed );
					ToolTipInfo.SetToolTip( Name, string.Format(
						"Lv合計: {0} / 平均: {1:0.00}\r\n{2}艦隊\r\nドラム缶搭載: {3}個 ({4}艦)\r\n大発動艇搭載: {5}個\r\n総積載: 燃 {6} / 弾 {7}\r\n(1戦当たり 燃 {8} / 弾 {9})",
						levelSum,
						(double)levelSum / Math.Max( fleet.Members.Count( id => id != -1 ), 1 ),
						Constants.GetSpeed( speed ),
						fleet.MembersInstance.Sum( s => s == null ? 0 : s.SlotInstanceMaster.Count( q => q == null ? false : q.CategoryType == 30 ) ),
						fleet.MembersInstance.Count( s => s == null ? false : s.SlotInstanceMaster.Count( q => q == null ? false : q.CategoryType == 30 ) > 0 ),
						fleet.MembersInstance.Sum( s => s == null ? 0 : s.SlotInstanceMaster.Count( q => q == null ? false : q.CategoryType == 24 ) ),
						fueltotal,
						ammototal,
						(int)( fueltotal * 0.2 ),
						(int)( ammototal * 0.2 )
						) );

				}


				State = FleetData.UpdateFleetState( fleet, StateMain, ToolTipInfo, State, ref Timer );


				//制空戦力計算	
				{
					int airSuperiority = fleet.GetAirSuperiority();
					AirSuperiority.Text = airSuperiority.ToString();
					ToolTipInfo.SetToolTip( AirSuperiority,
						string.Format( "確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
						(int)( airSuperiority / 3.0 ),
						(int)( airSuperiority / 1.5 ),
						(int)( airSuperiority * 1.5 ),
						(int)( airSuperiority * 3.0 ) ) );
				}


				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbilityString();
				ToolTipInfo.SetToolTip( SearchingAbility,
					string.Format( "(旧)2-5式: {0}\r\n2-5式(秋): {1}\r\n2-5新秋簡易式: {2}\r\n",
					fleet.GetSearchingAbilityString( 0 ),
					fleet.GetSearchingAbilityString( 1 ),
					fleet.GetSearchingAbilityString( 2 ) ) );

			}


			public void ResetState() {
				State = FleetData.FleetStates.NoShip;
			}

			public void Refresh() {

				FleetData.RefreshFleetState( StateMain, State, Timer );

			}


		}


		private class TableMemberControl {
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;


			public TableMemberControl( FormFleet parent ) {

				#region Initialize

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.Font = parent.MainFont;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				//Name.AutoEllipsis = true;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				Level.Value = 0;
				Level.MaximumValue = 150;
				Level.ValueNext = 0;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding( 0, 0, 0, 0 );
				Level.Margin = new Padding( 2, 0, 2, 1 );
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
				HP.RepairFontColor = parent.SubFontColor;
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
				ShipResource.Size = new Size( 30, 20 );
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
				Equipments.ShowAircraft = Utility.Configuration.Config.FormFleet.ShowAircraft;
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

			private double CalculateFire( ShipData ship ) {
				return Math.Floor( ( ship.FirepowerTotal + ship.TorpedoTotal ) * 1.5 + ship.BombTotal * 2 + 50 );
			}

			public void Update( int shipMasterID ) {

				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if ( ship != null ) {

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains( shipMasterID );


					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					ToolTipInfo.SetToolTip( Name,
						string.Format(
							"{0} {1}\n火力: {2}/{3}\n雷装: {4}/{5}\n対空: {6}/{7}\n装甲: {8}/{9}\n対潜: {10}/{11}\n回避: {12}/{13}\n索敵: {14}/{15}\n運: {16}\n射程: {17} / 速力: {18}\n(右クリックで図鑑)\n",
							ship.MasterShip.ShipTypeName, ship.NameWithLevel,
							ship.FirepowerBase,
							(ship.MasterShip.ShipType == 7 ||	// 轻空母
							ship.MasterShip.ShipType == 11 ||	// 正规空母
							ship.MasterShip.ShipType == 18) ?	// 装甲空母
							string.Format( "{0}（空母火力：{1:F0}）", ship.FirepowerTotal, CalculateFire( ship ) ) :
							ship.FirepowerTotal.ToString(),
							ship.TorpedoBase, ship.TorpedoTotal,
							ship.AABase, ship.AATotal,
							ship.ArmorBase, ship.ArmorTotal,
							ship.ASWBase, ship.ASWTotal,
							ship.EvasionBase, ship.EvasionTotal,
							ship.LOSBase, ship.LOSTotal,
							ship.LuckTotal,
							Constants.GetRange( ship.Range ),
							Constants.GetSpeed( ship.MasterShip.Speed )
							) );


					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;

					{
						StringBuilder tip = new StringBuilder();
						if ( !Utility.Configuration.Config.FormFleet.ShowNextExp )
							tip.AppendFormat( "次のレベルまで: {0}\n", ship.ExpNext );

						if ( ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel ) {
							tip.AppendFormat( "改装まで: {0}", ship.ExpNextRemodel );

						} else if ( ship.Level <= 99 ) {
							tip.AppendFormat( "Lv99まで: {0}", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 99 ), 0 ) );

						} else {
							tip.AppendFormat( "Lv150まで: {0}", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 150 ), 0 ) );

						}

						ToolTipInfo.SetToolTip( Level, tip.ToString() );
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
					if ( isEscaped ) {
						HP.BackColor = Color.Silver;
					} else {
						HP.BackColor = Utility.Configuration.Config.UI.BackColor;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat( "HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState( hprate ) );
						if ( isEscaped ) {
							sb.AppendLine( "退避中" );
						} else if ( hprate > 0.50 ) {
							sb.AppendFormat( "中破まで: {0} / 大破まで: {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4 );
						} else if ( hprate > 0.25 ) {
							sb.AppendFormat( "大破まで: {0}\n", ship.HPCurrent - ship.HPMax / 4 );
						} else {
							sb.AppendLine( "大破しています！" );
						}

						if ( ship.RepairTime > 0 ) {
							var span = DateTimeHelper.FromAPITimeSpan( ship.RepairTime );
							sb.AppendFormat( "入渠時間: {0}\n",
								DateTimeHelper.ToTimeRemainString( span ) );
							/*/
							sb.AppendFormat( "( @ 1HP: {0} )\n",
								DateTimeHelper.ToTimeRemainString( new TimeSpan( span.Ticks / ( ship.HPMax - ship.HPCurrent ) ) ) );
							//*/
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
						TimeSpan ts = new TimeSpan( 0, (int)Math.Ceiling( ( 49 - ship.Condition ) / 3.0 ) * 3, 0 );
						ToolTipInfo.SetToolTip( Condition, string.Format( "完全回復まで 約 {0:D2}:{1:D2}", (int)ts.TotalMinutes, (int)ts.Seconds ) );
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
				int? id = Name.Tag as int?;

				if ( id != null && id != -1 && ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					new DialogAlbumMasterShip( (int)id ).Show( Parent );
				}

			}


			private string GetEquipmentString( ShipData ship ) {
				StringBuilder sb = new StringBuilder();

				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( ship.SlotInstance[i] != null )
						sb.AppendFormat( "[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], KCDatabase.Instance.Equipments[ship.Slot[i]].NameWithLevel );
				}


				int[] slotmaster = ship.SlotMaster.ToArray();

				sb.AppendFormat( "\r\n昼戦: {0}\r\n夜戦: {1}\r\n",
					Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slotmaster, ship.ShipID, -1 ) ),
					Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slotmaster, ship.ShipID, -1 ) ) );
				{
					int aacutin = Calculator.GetAACutinKind( ship.ShipID, slotmaster );
					if ( aacutin != 0 ) {
						sb.AppendFormat( "対空: {0}\r\n", Constants.GetAACutinKind( aacutin ) );
					}
				}
				{
					int airsup = Calculator.GetAirSuperiority( ship );
					if ( airsup > 0 ) {
						sb.AppendFormat( "制空戦力: {0}\r\n", airsup );
					}
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


		private Pen LinePen = Pens.Silver;


		public FormFleet( FormMain parent, int fleetID ) {
            SuspendLayout();
			InitializeComponent();

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			SubFontColor = Utility.Configuration.Config.UI.SubForeColor;


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


			ConfigurationChanged();		//fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
        }



		private void FormFleet_Load( object sender, EventArgs e ) {

			Text = string.Format( "#{0}", FleetID );

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
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += Updated;
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
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;


			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
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

			if ( db.Ships.Count == 0 ) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if ( fleet == null ) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update( fleet );
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( fleet.Members[i] );
			}
			TableMember.ResumeLayout();


			if ( Icon != null ) ResourceManager.DestroyIcon( Icon );
			Icon = ResourceManager.ImageToIcon( ControlFleet.StateMain.Image );
			if ( Parent != null ) Parent.Refresh();		//アイコンを更新するため

		}

		void ChangeOrganization( string apiname, dynamic data ) {

			ControlFleet.ResetState();

		}


		void UpdateTimerTick() {

			TableFleet.SuspendLayout();
			{
				FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];
				if ( fleet != null )
					ControlFleet.Refresh();

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
			if ( fleet == null ) return;

			sb.AppendFormat( "{0}\t制空戦力{1}/索敵能力{2}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString() );
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


		private void ContextMenuFleet_Opening( object sender, CancelEventArgs e ) {

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}

		private void ContextMenuFleet_Capture_Click( object sender, EventArgs e ) {

			using ( Bitmap bitmap = new Bitmap( this.ClientSize.Width, this.ClientSize.Height ) ) {
				this.DrawToBitmap( bitmap, this.ClientRectangle );

				Clipboard.SetData( DataFormats.Bitmap, bitmap );
			}
		}




		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			MainFont = Font = c.UI.MainFont;
			SubFont = c.UI.SubFont;

			LinePen = new Pen( c.UI.LineColor.ColorData );

			AutoScroll = ContextMenuFleet_IsScrollable.Checked = c.FormFleet.IsScrollable;
			ContextMenuFleet_FixShipNameWidth.Checked = c.FormFleet.FixShipNameWidth;

			if ( ControlFleet != null && KCDatabase.Instance.Fleet[FleetID] != null ) {
				ControlFleet.Update( KCDatabase.Instance.Fleet[FleetID] );
			}

			if ( ControlMember != null ) {
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool showNext = c.FormFleet.ShowNextExp;

				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Equipments.ShowAircraft = showAircraft;
					if ( fixShipNameWidth ) {
						ControlMember[i].Name.AutoSize = false;
						ControlMember[i].Name.Size = new Size( 40, 20 );
					} else {
						ControlMember[i].Name.AutoSize = true;
					}

					ControlMember[i].HP.Text = shortHPBar ? "" : "HP:";
					ControlMember[i].Level.TextNext = showNext ? "next:" : null;
				}
			}
			TableMember.PerformLayout();		//fixme:サイズ変更に親パネルが追随しない

		}


		//よく考えたら別の艦隊タブと同期しないといけないので封印
		private void ContextMenuFleet_IsScrollable_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormFleet.IsScrollable = ContextMenuFleet_IsScrollable.Checked;
			ConfigurationChanged();
		}

		private void ContextMenuFleet_FixShipNameWidth_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormFleet.FixShipNameWidth = ContextMenuFleet_FixShipNameWidth.Checked;
			ConfigurationChanged();
		}


		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		protected override string GetPersistString() {
			return "Fleet #" + FleetID.ToString();
		}



	}

}
