using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Control;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormCompass : DockContent {


		private class TableEnemyMemberControl {

			public ImageLabel ShipName;
			public ShipStatusEquipment Equipments;

			public ToolTip ToolTipInfo;


			public TableEnemyMemberControl( FormCompass parent ) {

				#region Initialize

				ShipName = new ImageLabel();
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.Font = parent.MainFont;
				ShipName.ForeColor = parent.MainFontColor;
				ShipName.ImageAlign = ContentAlignment.MiddleCenter;
				ShipName.Padding = new Padding( 0, 1, 0, 1 );
				ShipName.Margin = new Padding( 2, 0, 2, 0 );
				ShipName.MaximumSize = new Size( 60, 20 );
				ShipName.AutoEllipsis = true;
				ShipName.AutoSize = true;
				ShipName.Cursor = Cursors.Help;
				ShipName.MouseClick += ShipName_MouseClick;

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Font = parent.SubFont;
				Equipments.Padding = new Padding( 0, 2, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.Size = new Size( 40, 20 );	//checkme: 要る？
				Equipments.AutoSize = true;
				Equipments.ResumeLayout();

				ToolTipInfo = parent.ToolTipInfo;
				#endregion

			}


			public TableEnemyMemberControl( FormCompass parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( Equipments, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}



			public void Update( int shipID ) {
				Update( shipID, shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot.ToArray() : null );
			}

			//fixme: slotがnullだと間違いなく死ぬ
			public void Update( int shipID, int[] slot ) {

				ShipName.Tag = shipID;

				if ( shipID == -1 ) {
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = Color.FromArgb( 0x00, 0x00, 0x00 );
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip( ShipName, null );
					ToolTipInfo.SetToolTip( Equipments, null );

				} else {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					switch ( ship.AbyssalShipClass ) {
						case 0:
						case 1:		//normal
							ShipName.ForeColor = Color.FromArgb( 0x00, 0x00, 0x00 ); break;
						case 2:		//elite
							ShipName.ForeColor = Color.FromArgb( 0xFF, 0x00, 0x00 ); break;
						case 3:		//flagship
							ShipName.ForeColor = Color.FromArgb( 0xFF, 0x88, 0x00 ); break;
						case 4:		//latemodel
							ShipName.ForeColor = Color.FromArgb( 0x00, 0x88, 0xFF ); break;
					}
					ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot ) );

					Equipments.SetSlotList( shipID, slot );
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( shipID, slot ) );
				}

			}


			public void UpdateEquipmentToolTip( int shipID, int[] slot, int level, int firepower, int torpedo, int aa, int armor ) {

				ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot, level, firepower, torpedo, aa, armor ) );
			}


			private string GetShipString( int shipID, int[] slot ) {

				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
				if ( ship == null ) return null;

				return GetShipString( shipID, slot, -1, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
					 ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
					 ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
					 ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
					 ship.LuckMin );
			}

			private string GetShipString( int shipID, int[] slot, int level, int firepower, int torpedo, int aa, int armor ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
				if ( ship == null ) return null;

				return GetShipString( shipID, slot, level, level > 99 ? ship.HPMaxMarried : ship.HPMin, firepower, torpedo, aa, armor,
					ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter( level ) : -1,
					ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter( level ) : -1,
					ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter( level ) : -1,
					level > 99 ? Math.Min( ship.LuckMin + 3, ship.LuckMax ) : ship.LuckMin );
			}

			private string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck ) {

				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
				if ( ship == null ) return null;

				int firepower_c = firepower;
				int torpedo_c = torpedo;
				int aa_c = aa;
				int armor_c = armor;
				int asw_c = Math.Max( asw, 0 );
				int evasion_c = Math.Max( evasion, 0 );
				int los_c = Math.Max( los, 0 );
				int luck_c = luck;

				if ( slot != null ) {
					int count = slot.Length;
					for ( int i = 0; i < count; i++ ) {
						EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
						if ( eq == null ) continue;

						firepower += eq.Firepower;
						torpedo += eq.Torpedo;
						aa += eq.AA;
						armor += eq.Armor;
						asw += eq.ASW;
						evasion += eq.Evasion;
						los += eq.LOS;
						luck += eq.Luck;
					}
				}

				return string.Format(
							"{0} {1}{2}\n耐久: {3}\n火力: {4}/{5}\n雷装: {6}/{7}\n対空: {8}/{9}\n装甲: {10}/{11}\n対潜: {12}/{13}\n回避: {14}/{15}\n索敵: {16}/{17}\n運: {18}/{19}\n(右クリックで図鑑)\n",
							ship.ShipTypeName, ship.NameWithClass, level < 1 ? "" : string.Format( " Lv. {0}", level ),
							hp,
							firepower_c, firepower,
							torpedo_c, torpedo,
							aa_c, aa,
							armor_c, armor,
							asw_c == -1 ? "???" : asw_c.ToString(), asw,
							evasion_c == -1 ? "???" : evasion_c.ToString(), evasion,
							los_c == -1 ? "???" : los_c.ToString(), los,
							luck_c, luck
							);
			}

			private string GetEquipmentString( int shipID, int[] slot ) {
				StringBuilder sb = new StringBuilder();
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

				if ( ship == null || slot == null ) return null;

				for ( int i = 0; i < slot.Length; i++ ) {
					if ( slot[i] != -1 )
						sb.AppendFormat( "[{0}] {1}\r\n", ship.Aircraft[i], KCDatabase.Instance.MasterEquipments[slot[i]].Name );
				}

				sb.AppendFormat( "\r\n昼戦: {0}\r\n夜戦: {1}\r\n",
					Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slot, ship.ShipID, -1 ) ),
					Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slot, ship.ShipID, -1 ) ) );

				{
					int aacutin = Calculator.GetAACutinKind( shipID, slot );
					if ( aacutin != 0 ) {
						sb.AppendFormat( "対空: {0}\r\n", Constants.GetAACutinKind( aacutin ) );
					}
				}

				return sb.ToString();
			}


			void ShipName_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int? shipID = ShipName.Tag as int?;

					if ( shipID != null && shipID != -1 )
						new DialogAlbumMasterShip( (int)ShipName.Tag ).Show();
				}

			}


		}



		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableEnemyMemberControl[] ControlMember;



		public FormCompass( FormMain parent ) {
			InitializeComponent();


			ConfigurationChanged();

			MainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			SubFontColor = Color.FromArgb( 0x88, 0x88, 0x88 );


			ControlHelper.SetDoubleBuffered( BasePanel );
			ControlHelper.SetDoubleBuffered( TableEnemyFleet );
			ControlHelper.SetDoubleBuffered( TableEnemyMember );


			TableEnemyMember.SuspendLayout();
			ControlMember = new TableEnemyMemberControl[6];
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableEnemyMemberControl( this, TableEnemyMember, i );
			}
			TableEnemyMember.ResumeLayout();


			//BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak( TextDestination, true );
			//BasePanel.SetFlowBreak( TextEventKind, true );
			BasePanel.SetFlowBreak( TextEventDetail, true );

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass] );

		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;
			TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;


			Font = MainFont;
			TextMapArea.Font = MainFont;
			TextDestination.Font = MainFont;
			TextEventKind.Font = MainFont;
			TextEventDetail.Font = MainFont;


			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;

			o.APIList["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o.APIList["api_req_practice/battle"].ResponseReceived += BattleStarted;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		private void Updated( string apiname, dynamic data ) {

			Color colorNormal = SystemColors.ControlText;
			Color colorNight = Color.Navy;


			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else if ( apiname == "api_req_member/get_practice_enemyinfo" ) {

				TextMapArea.Text = "演習";
				TextDestination.Text = string.Format( "{0} {1}", data.api_nickname, Constants.GetAdmiralRank( (int)data.api_rank ) );
				TextEventKind.Text = data.api_cmt;
				TextEventKind.ForeColor = colorNormal;
				TextEventDetail.Text = string.Format( "Lv. {0} / {1} exp.", data.api_level, data.api_experience[0] );
				TextEnemyFleetName.Text = data.api_deckname;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;

				TextMapArea.Text = "出撃海域 : " + compass.MapAreaID + "-" + compass.MapInfoID;
				TextDestination.Text = "次のセル : " + compass.Destination + ( compass.IsEndPoint ? " (終点)" : "" );
				TextEventKind.ForeColor = colorNormal;

				{
					string eventkind = Constants.GetMapEventID( compass.EventID );

					switch ( compass.EventID ) {

						case 0:		//初期位置
						case 1:		//不明
							TextEventDetail.Text = "どうしてこうなった";
							break;

						case 2:		//資源
						case 8:		//船団護衛成功
							{
								string materialname;
								if ( compass.GetItemID == 4 ) {		//"※"　大方資源専用ID

									materialname = Constants.GetMaterialName( compass.GetItemIDMetadata );

								} else {
									UseItemMaster item =  KCDatabase.Instance.MasterUseItems[compass.GetItemIDMetadata];
									if ( item != null )
										materialname = item.Name;
									else
										materialname = "謎のアイテム";
								}

								TextEventDetail.Text = materialname + " x " + compass.GetItemAmount;
							}

							break;

						case 3:		//渦潮
							{
								int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
									.Where( f => f != null && f.IsInSortie )
									.SelectMany( f => f.MembersWithoutEscaped )
									.Max( s => {
										if ( s == null ) return 0;
										switch ( compass.WhirlpoolItemID ) {
											case 1:
												return s.Fuel;
											case 2:
												return s.Ammo;
											default:
												return 0;
										}
									} );

								TextEventDetail.Text = string.Format( "{0} x {1} ({2:p0})",
									Constants.GetMaterialName( compass.WhirlpoolItemID ),
									compass.WhirlpoolItemAmount,
									(double)compass.WhirlpoolItemAmount / Math.Max( materialmax, 1 ) );

							}
							break;

						case 4:		//通常戦闘
						case 5:		//ボス戦闘
							if ( compass.EventKind >= 2 ) {
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );

								if ( compass.EventKind == 2 || compass.EventKind == 3 ) {
									TextEventKind.ForeColor = colorNight;
								}
							}
							UpdateEnemyFleet( compass.EnemyFleetID );
							break;

						case 6:		//気のせいだった
							TextEventDetail.Text = "";
							break;

						case 7:		//航空戦(連合艦隊)
							if ( compass.EventKind >= 2 && compass.EventKind != 4 )		//必ず"航空戦"のはずなので除外
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );
							UpdateEnemyFleet( compass.EnemyFleetID );
							break;


						default:
							eventkind += "不明";
							TextEventDetail.Text = "";
							break;

					}
					TextEventKind.Text = eventkind;
				}

				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}



		private void BattleStarted( string apiname, dynamic data ) {
			UpdateEnemyFleetInstant();
		}



		private void UpdateEnemyFleet( int fleetID ) {

			TextEventDetail.Text = string.Format( "敵艦隊ID : {0}", fleetID );


			var efleet = RecordManager.Instance.EnemyFleet;

			if ( !efleet.Record.ContainsKey( fleetID ) ) {

				//unknown
				TextEnemyFleetName.Text = "(敵艦隊情報不明)";
				TextFormation.Visible = false;
				TextAirSuperiority.Visible = false;
				TableEnemyMember.Visible = false;

			} else {

				var fdata = efleet[fleetID];

				TextEnemyFleetName.Text = fdata.FleetName;
				TextFormation.Text = Constants.GetFormationShort( fdata.Formation );
				TextFormation.Visible = true;
				TextAirSuperiority.Text = Calculator.GetAirSuperiority( fdata.FleetMember ).ToString();
				TextAirSuperiority.Visible = true;

				TableEnemyMember.SuspendLayout();
				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Update( fdata.FleetMember[i] );
				}
				TableEnemyMember.ResumeLayout();
				TableEnemyMember.Visible = true;

			}

			PanelEnemyFleet.Visible = true;

		}


		private void UpdateEnemyFleetInstant() {

			BattleManager bm = KCDatabase.Instance.Battle;
			BattleData bd;

			switch ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
				case BattleManager.BattleModes.NightOnly:
				case BattleManager.BattleModes.NightDay:
					bd = bm.BattleNight;
					break;
				default:
					bd = bm.BattleDay;
					break;
			}

			TextFormation.Text = Constants.GetFormationShort( (int)bd.Data.api_formation[1] );
			TextFormation.Visible = true;
			TextAirSuperiority.Text = Calculator.GetAirSuperiority( ( (int[])bd.Data.api_ship_ke ).Skip( 1 ).ToArray(), (int[][])bd.Data.api_eSlot ).ToString();
			TextAirSuperiority.Visible = true;

			TableEnemyMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				int shipID = (int)bd.Data.api_ship_ke[i + 1];
				ControlMember[i].Update( shipID, shipID != -1 ? (int[])bd.Data.api_eSlot[i] : null );

				if ( shipID != -1 )
					ControlMember[i].UpdateEquipmentToolTip( shipID, (int[])bd.Data.api_eSlot[i], (int)bd.Data.api_ship_lv[i + 1],
						(int)bd.Data.api_eParam[i][0], (int)bd.Data.api_eParam[i][1], (int)bd.Data.api_eParam[i][2], (int)bd.Data.api_eParam[i][3] );
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;
			BasePanel.Visible = true;			//checkme

		}



		void ConfigurationChanged() {

			Font = PanelEnemyFleet.Font = MainFont = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;


			if ( ControlMember != null ) {
				bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Equipments.ShowAircraft = flag;
				}
			}
		}



		protected override string GetPersistString() {
			return "Compass";
		}

		private void TableEnemyMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

	}

}
