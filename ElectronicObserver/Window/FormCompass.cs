﻿using ElectronicObserver.Data;
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

			public FormCompass Parent;
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
				//ShipName.MaximumSize = new Size( 60, 20 );
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

				Parent = parent;
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
				RowStyle rs = new RowStyle( SizeType.AutoSize );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}



			public void Update( int shipID ) {
				var slot = shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
				Update( shipID, slot != null ? slot.ToArray() : null );
			}


			public void Update( int shipID, int[] slot ) {

				ShipName.Tag = shipID;

				if ( shipID == -1 ) {
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip( ShipName, null );
					ToolTipInfo.SetToolTip( Equipments, null );

				} else {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					switch ( ship.AbyssalShipClass ) {
						case 0:
						case 1:		//normal
						default:
							ShipName.ForeColor = Utility.Configuration.Config.UI.ForeColor; break;
						case 2:		//elite
							ShipName.ForeColor = Utility.Configuration.Config.UI.EliteColor; break;
						case 3:		//flagship
							ShipName.ForeColor = Utility.Configuration.Config.UI.FlagshipColor; break;
						case 4:		//latemodel / flagship kai
							ShipName.ForeColor = Utility.Configuration.Config.UI.LateModelColor; break;
						case 5:		//latemodel elite
							ShipName.ForeColor = Color.FromArgb( 0x88, 0x00, 0x00 ); break;
						case 6:		//latemodel flagship
							ShipName.ForeColor = Color.FromArgb( 0x88, 0x44, 0x00 ); break;
					}
					ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot ) );

					Equipments.SetSlotList( shipID, slot );
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( shipID, slot ) );
				}

			}


			public void UpdateEquipmentToolTip( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {

				ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor ) );
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

			private string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
				if ( ship == null ) return null;

				return GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor,
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
				int asw_c = asw;
				int evasion_c = evasion;
				int los_c = los;
				int luck_c = luck;
				int range = ship.Range;

				asw = Math.Max( asw, 0 );
				evasion = Math.Max( evasion, 0 );
				los = Math.Max( los, 0 );

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
						range = Math.Max( range, eq.Range );
					}
				}

				return string.Format(
							"{0} {1}{2}\n耐久: {3}\n火力: {4}/{5}\n雷装: {6}/{7}\n対空: {8}/{9}\n加权对空: {22:0.##}\n装甲: {10}/{11}\n対潜: {12}/{13}\n回避: {14}/{15}\n索敵: {16}/{17}\n運: {18}/{19}\n射程: {20} / 速力: {21}\n(右クリックで図鑑)\n",
							ship.ShipTypeName, ship.NameWithClass, level < 1 ? "" : string.Format( " Lv. {0}", level ),
							hp,
							firepower_c,

							( ship.ShipType == 7 ||	// 轻空母
							ship.ShipType == 11 ||	// 正规空母
							ship.IsLandBase ) ?		// 陆基
							string.Format( "{0}（空母火力：{1:F0}）", firepower, CalculatorEx.CalculateFireEnemy( shipID, slot, firepower_c, torpedo_c ) ) :
							firepower.ToString(),

							torpedo_c, torpedo,
							aa_c, aa,
							armor_c, armor,
							asw_c == -1 ? "???" : asw_c.ToString(), asw,
							evasion_c == -1 ? "???" : evasion_c.ToString(), evasion,
							los_c == -1 ? "???" : los_c.ToString(), los,
							luck_c, luck,
							Constants.GetRange( range ),
							Constants.GetSpeed( ship.Speed ),
							CalculatorEx.CalculateWeightingAAEnemy( shipID, slot, aa_c )
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
				{
					int airsup = Calculator.GetAirSuperiority( slot, ship.Aircraft.ToArray() );
					if ( airsup > 0 ) {
						sb.AppendFormat( "制空戦力: {0}\r\n", airsup );
					}
				}

				return sb.ToString();
			}


			void ShipName_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int? shipID = ShipName.Tag as int?;

					if ( shipID != null && shipID != -1 )
						new DialogAlbumMasterShip( (int)ShipName.Tag ).Show( Parent );
				}

			}


		}



		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }

		private Pen LinePen = Pens.Silver;


		private TableEnemyMemberControl[] ControlMember;


		/// <summary>
		/// 次に遭遇する敵艦隊候補
		/// </summary>
		private List<EnemyFleetRecord.EnemyFleetElement> _enemyFleetCandidate = null;

		/// <summary>
		/// 表示中の敵艦隊候補のインデックス
		/// </summary>
		private int _enemyFleetCandidateIndex = 0;




		public FormCompass( FormMain parent ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();


			ConfigurationChanged();


			ControlHelper.SetDoubleBuffered( BasePanel );
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
			BasePanel.SetFlowBreak( TextAA, true );

			TextDestination.ImageList = ResourceManager.Instance.Equipments;
			TextEventDetail.ImageList = ResourceManager.Instance.Equipments;
			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass] );

			this.ResumeLayoutForDpiScale();
		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;
			TextAA.ImageList = TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
			TextAA.ImageIndex = (int)ResourceManager.EquipmentContent.AADirector;


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

			Func<int, Color> getColorFromEventKind = ( int kind ) => {
				switch ( kind ) {
					case 0:
					case 1:
					default:	//昼夜戦・その他
						return Utility.Configuration.Config.UI.ForeColor;
					case 2:
					case 3:		//夜戦・夜昼戦
						return Color.Navy;
					case 4:		//航空戦
						return Color.DarkGreen;
				}
			};

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else if ( apiname == "api_req_member/get_practice_enemyinfo" ) {

				TextMapArea.Text = "演習";
				TextDestination.Text = string.Format( "{0} {1}", data.api_nickname, Constants.GetAdmiralRank( (int)data.api_rank ) );
				TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
				TextDestination.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextDestination, null );
				TextEventKind.Text = data.api_cmt;
				TextEventKind.ForeColor = getColorFromEventKind( 0 );
				TextEventDetail.Text = string.Format( "Lv. {0} / {1} exp.", data.api_level, data.api_experience[0] );
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				TextEnemyFleetName.Text = data.api_deckname;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;
				TextEnemyFleetName.Visible =
				TextFormation.Visible =
				TextAirSuperiority.Visible = false;
				TextAA.Visible = false;

				_enemyFleetCandidate = null;
				_enemyFleetCandidateIndex = -1;


				TextMapArea.Text = string.Format( "出撃海域 : {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
					compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty( compass.MapInfo.EventDifficulty ) + "]" : "" );

				TextDestination.Text = string.Format( "次のセル : {0}{1}", compass.Destination, ( compass.IsEndPoint ? " (終点)" : "" ) );
				if ( compass.LaunchedRecon != 0 ) {
					TextDestination.ImageAlign = ContentAlignment.MiddleRight;
					TextDestination.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					string tiptext;
					switch ( compass.CommentID ) {
						case 1:
							tiptext = "敵艦隊発見！";
							break;
						case 2:
							tiptext = "攻撃目標発見！";
							break;
						case 3:
							tiptext = "針路哨戒！";
							break;
						default:
							tiptext = "索敵機発艦！";
							break;
					}
					ToolTipInfo.SetToolTip( TextDestination, tiptext );

				} else {
					TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
					TextDestination.ImageIndex = -1;
					ToolTipInfo.SetToolTip( TextDestination, null );
				}

				//とりあえずリセット
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;


				TextEventKind.ForeColor = getColorFromEventKind( 0 );

				{
					string eventkind = Constants.GetMapEventID( compass.EventID );

					switch ( compass.EventID ) {

						case 0:		//初期位置
						case 1:		//不明
							TextEventDetail.Text = "どうしてこうなった";
							break;

						case 2:		//資源
						case 8:		//船団護衛成功
							TextEventDetail.Text = GetMaterialName( compass ) + " x " + compass.GetItemAmount;
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
							if ( compass.EventKind >= 2 ) {
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );

								TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );
							}
							UpdateEnemyFleet();
							break;

						case 5:		//ボス戦闘
							TextEventKind.ForeColor = Color.Red;
							goto case 4;

						case 6:		//気のせいだった
							switch ( compass.EventKind ) {

								case 0:		//気のせいだった
								default:
									TextEventDetail.Text = "";
									break;
								case 1:		//敵影を見ず
									eventkind = "敵影を見ず";
									TextEventDetail.Text = "";
									break;
								case 2:		//能動分岐
									eventkind = "能動分岐";
									TextEventDetail.Text = string.Join( "/", compass.RouteChoices );
									break;
							}
							break;

						case 7:		//航空戦or航空偵察
							TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );

							switch ( compass.EventKind ) {
								case 0:		//航空偵察
									eventkind = "航空偵察";

									switch ( compass.AirReconnaissanceResult ) {
										case 0:
										default:
											TextEventDetail.Text = "失敗";
											break;
										case 1:
											TextEventDetail.Text = "成功";
											break;
										case 2:
											TextEventDetail.Text = "大成功";
											break;
									}

									switch ( compass.AirReconnaissancePlane ) {
										case 0:
										default:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
											TextEventDetail.ImageIndex = -1;
											break;
										case 1:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.FlyingBoat;
											break;
										case 2:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
											break;
									}

									if ( compass.GetItemID != -1 ) {
										TextEventDetail.Text += string.Format( "　{0} x {1}", GetMaterialName( compass ), compass.GetItemAmount );
									}

									break;

								case 4:		//航空戦
								default:
									UpdateEnemyFleet();
									break;
							}
							break;


						default:
							TextEventDetail.Text = "";
							break;

					}
					TextEventKind.Text = eventkind;
				}

				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		private string GetMaterialName( CompassData compass ) {

			if ( compass.GetItemID == 4 ) {		//"※"　大方資源専用ID

				return Constants.GetMaterialName( compass.GetItemIDMetadata );

			} else {
				UseItemMaster item =  KCDatabase.Instance.MasterUseItems[compass.GetItemIDMetadata];
				if ( item != null )
					return item.Name;
				else
					return "謎のアイテム";
			}
		}



		private void BattleStarted( string apiname, dynamic data ) {
			UpdateEnemyFleetInstant();
		}





		private void UpdateEnemyFleet() {

			CompassData compass = KCDatabase.Instance.Battle.Compass;

			_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values.Where(
				r =>
					r.MapAreaID == compass.MapAreaID &&
					r.MapInfoID == compass.MapInfoID &&
					r.CellID == compass.Destination &&
					r.Difficulty == compass.MapInfo.EventDifficulty
				).ToList();
			_enemyFleetCandidateIndex = -1;


			if ( _enemyFleetCandidate.Count == 0 ) {
				TextEventDetail.Text = "(敵艦隊候補なし)";
				TextEnemyFleetName.Text = string.Empty;	//			"(敵艦隊情報不明)";

				TextFormation.Visible = false;
				TextAirSuperiority.Visible = false;
				TextAA.Visible = false;
				TableEnemyMember.Visible = false;

			} else {
				NextEnemyFleetCandidate();
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

			int[] enemies = bd.Initial.EnemyMembers;
			int[][] slots = bd.Initial.EnemySlots;
			int[] levels = bd.Initial.EnemyLevels;
			int[][] parameters = bd.Initial.EnemyParameters;
			int[] hps = bd.Initial.MaxHPs;


			_enemyFleetCandidate = null;
			_enemyFleetCandidateIndex = -1;



			if ( ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) != BattleManager.BattleModes.Practice ) {
				var efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
				var efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
				if ( efrecord != null ) {
					TextEnemyFleetName.Text = efrecord.FleetName;
				}
				TextEventDetail.Text = "敵艦隊ID: " + efcurrent.FleetID.ToString( "x8" );
			}

			TextFormation.Text = Constants.GetFormationShort( (int)bd.Searching.FormationEnemy );
			TextFormation.Visible = true;
			int airSuperiority = Calculator.GetAirSuperiority( enemies, slots );
			TextAirSuperiority.Text = airSuperiority.ToString();
			//string.Format( "{0}，优势 {1:F0}，确保 {2:F0}", airSuperiority, airSuperiority * 1.5, airSuperiority * 3 );
			ToolTipInfo.SetToolTip( TextAirSuperiority, string.Format( "优势 {0:F0}，确保 {1:F0}", airSuperiority * 1.5, airSuperiority * 3 ) );
			TextAA.Text = CalculatorEx.GetEnemyFleetAAValue( enemies, bd.Searching.FormationEnemy ).ToString();

			TableEnemyMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				int shipID = enemies[i];
				ControlMember[i].Update( shipID, shipID != -1 ? slots[i] : null );

				if ( shipID != -1 )
					ControlMember[i].UpdateEquipmentToolTip( shipID, slots[i], levels[i], hps[i + 6], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;
			TextEnemyFleetName.Visible =
			TextFormation.Visible =
			TextAirSuperiority.Visible = true;
			TextAA.Visible = true;
			BasePanel.Visible = true;			//checkme

		}



		private void TextEnemyFleetName_MouseDown( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Left )
				NextEnemyFleetCandidate();
			else if ( e.Button == System.Windows.Forms.MouseButtons.Right )
				NextEnemyFleetCandidate( -1 );
		}


		private void NextEnemyFleetCandidate( int offset = 1 ) {

			if ( _enemyFleetCandidate != null && _enemyFleetCandidate.Count != 0 ) {

				_enemyFleetCandidateIndex = ( _enemyFleetCandidateIndex + offset ) % _enemyFleetCandidate.Count;
				if ( _enemyFleetCandidateIndex < 0 )
					_enemyFleetCandidateIndex += _enemyFleetCandidate.Count;


				var candidate = _enemyFleetCandidate[_enemyFleetCandidateIndex];


				TextEventDetail.Text = string.Format( "敵艦隊候補 : {0} / {1}", _enemyFleetCandidateIndex + 1, _enemyFleetCandidate.Count );

				TextEnemyFleetName.Text = candidate.FleetName;
				TextFormation.Text = Constants.GetFormationShort( candidate.Formation );
				TextAirSuperiority.Text = Calculator.GetAirSuperiority( candidate.FleetMember ).ToString();
				TextAA.Text = CalculatorEx.GetEnemyFleetAAValue( candidate.FleetMember, candidate.Formation ).ToString();

				TableEnemyMember.SuspendLayout();
				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Update( candidate.FleetMember[i] );
				}
				TableEnemyMember.ResumeLayout();

				TextFormation.Visible = true;
				TextAirSuperiority.Visible = true;
				TextAA.Visible = true;
				TableEnemyMember.Visible = true;


				PanelEnemyFleet.Visible = true;

			}
		}


		void ConfigurationChanged() {

			Font = PanelEnemyFleet.Font = MainFont = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

			LinePen = new Pen( Utility.Configuration.Config.UI.LineColor.ColorData );

			if ( ControlMember != null ) {
				bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Equipments.ShowAircraft = flag;
				}
			}
		}



		public override string GetPersistString()
		{
			return "Compass";
		}

		private void TableEnemyMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}





	}

}
