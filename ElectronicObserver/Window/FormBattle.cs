using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
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

	public partial class FormBattle : DockContent {

		private List<ShipStatusHP> HPBars;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }



		public FormBattle( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableMain );


			ConfigurationChanged();

			HPBars = new List<ShipStatusHP>( 18 );


			TableMain.SuspendLayout();
			for ( int i = 0; i < 18; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = new Size( 80, 20 );
				HPBars[i].Margin = new Padding( 2, 0, 2, 0 );
				HPBars[i].Anchor = AnchorStyles.None;
				HPBars[i].MainFont = MainFont;
				HPBars[i].SubFont = SubFont;
				HPBars[i].UsePrevValue = true;
				HPBars[i].ShowDifference = true;
				HPBars[i].MaximumDigit = 9999;

				if ( i < 6 ) {
					TableMain.Controls.Add( HPBars[i], 0, i + 6 );
				} else if ( i < 12 ) {
					TableMain.Controls.Add( HPBars[i], 2, i );
				} else {
					TableMain.Controls.Add( HPBars[i], 1, i - 6 );
				}
			}
			TableMain.ResumeLayout();


			SearchingFriend.ImageList = ResourceManager.Instance.Equipments;
			SearchingEnemy.ImageList = ResourceManager.Instance.Equipments;

			TableMain.Visible = false;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle] );

		}




		private void FormBattle_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_req_map/start"].ResponseReceived += rec;
			o.APIList["api_req_map/next"].ResponseReceived += rec;
			o.APIList["api_req_sortie/battle"].ResponseReceived += rec;
			o.APIList["api_req_sortie/battleresult"].ResponseReceived += rec;
			o.APIList["api_req_battle_midnight/battle"].ResponseReceived += rec;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += rec;
			o.APIList["api_req_combined_battle/battleresult"].ResponseReceived += rec;
			o.APIList["api_req_practice/battle"].ResponseReceived += rec;
			o.APIList["api_req_practice/midnight_battle"].ResponseReceived += rec;
			o.APIList["api_req_practice/battle_result"].ResponseReceived += rec;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager bm = db.Battle;


			TableMain.SuspendLayout();
			switch ( apiname ) {

				case "api_req_map/start":
				case "api_req_map/next":
					TableMain.Visible = false;
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay );
						SetHPNormal( hp, bm.BattleDay );
						SetDamageRateNormal( hp, bm.BattleDay );

						TableMain.Visible = true;
					} break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetNightBattleEvent( hp, false, bm.BattleNight );
						SetHPNormal( hp, bm.BattleNight );
						SetDamageRateNormal( hp, bm.BattleDay );
						
						TableMain.Visible = true;
					} break;

				case "api_req_battle_midnight/sp_midnight": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetNightBattleEvent( hp, false, bm.BattleNight );
						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetHPNormal( hp, bm.BattleNight );
						SetDamageRateNormal( hp, bm.BattleNight );

						TableMain.Visible = true;
					} break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay );
						SetHPCombined( hp, bm.BattleDay );
						SetDamageRateCombined( hp, bm.BattleDay );

						TableMain.Visible = true;
					} break;

				case "api_req_combined_battle/airbattle": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay );
						SetHPCombined( hp, bm.BattleDay );
						SetDamageRateCombined( hp, bm.BattleDay );

						TableMain.Visible = true;
					} break;

				case "api_req_combined_battle/midnight_battle": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetNightBattleEvent( hp, true, bm.BattleNight );
						SetHPCombined( hp, bm.BattleNight );
						SetDamageRateCombined( hp, bm.BattleDay );

						TableMain.Visible = true;
					} break;

				case "api_req_combined_battle/sp_midnight": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( hp, true, bm.BattleNight );
						SetHPCombined( hp, bm.BattleNight );
						SetDamageRateCombined( hp, bm.BattleNight );

						TableMain.Visible = true;
					} break;



				case "api_port/port":
					TableMain.Visible = false;
					ToolTipInfo.RemoveAll();
					break;

			}
			TableMain.ResumeLayout();

		}



		/// <summary>
		/// 戦況を表す文字列を取得します。＊デバッグ用です＊
		/// </summary>
		/// <param name="bd">戦闘データ</param>
		/// <returns>戦況</returns>
		private string GetBattleString( BattleData bd ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			int[] hp = bd.EmulateBattle();
			bool isPractice = bd.APIName.Contains( "practice" );	//仕方ないね
			bool isCombined = bd.APIName.Contains( "combined" );


			Func<int, double, string> GetState = 
			( int shipID, double percentage ) => {

				bool isLandBase = KCDatabase.Instance.MasterShips[shipID].IsLandBase;

				if ( percentage <= 0.0 )
					if ( isPractice ) return "[離脱]";
					else if ( isLandBase ) return "[破壊]";
					else return "[撃沈]";
				else if ( percentage <= 0.25 )
					return isLandBase ? "[損壊]" : "[大破]";
				else if ( percentage <= 0.5 )
					return isLandBase ? "[損害]" : "[中破]";
				else if ( percentage <= 0.75 )
					return isLandBase ? "[混乱]" : "[小破]";
				else if ( percentage < 1.0 )
					return "[健在]";
				else
					return "[無傷]";

			};



			sb.AppendLine( "---- 自軍艦隊 ----" );
			if ( isCombined )
				sb.AppendLine( "[部隊本隊]" );

			for ( int i = 0; i < 6; i++ ) {

				ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].Members[i]];

				if ( ship != null ) {

					sb.AppendFormat( "{0} Lv. {1} HP: {2} -> {3} ({4}) {5}\r\n",
						ship.MasterShip.Name,
						ship.Level,
						bd.InitialHP[i + 1],
						hp[i],
						hp[i] - bd.InitialHP[i + 1],
						GetState( ship.ShipID, (double)hp[i] / bd.MaxHP[i + 1] ) );

				} else {
					sb.AppendLine( "-" );
				}

			}

			if ( isCombined ) {

				BattleDataCombined bdc = (BattleDataCombined)bd;

				sb.AppendLine();
				sb.AppendLine( "[随伴艦隊]" );


				for ( int i = 0; i < 6; i++ ) {

					ShipData ship = db.Ships[db.Fleet[2].Members[i]];

					if ( ship != null ) {

						sb.AppendFormat( "{0} Lv. {1} HP: {2} -> {3} ({4}) {5}\r\n",
							ship.MasterShip.Name,
							ship.Level,
							bdc.InitialHPCombined[i + 1],
							hp[i + 12],
							hp[i + 12] - bdc.InitialHPCombined[i + 1],
							GetState( ship.ShipID, (double)hp[i + 12] / bdc.MaxHPCombined[i + 1] ) );


					} else {
						sb.AppendLine( "-" );
					}

				}
			}



			sb.AppendLine();
			sb.AppendLine( "---- 敵軍艦隊 ----" );

			for ( int i  = 0; i < 6; i++ ) {

				int eid = bd.EnemyFleetMembers[i + 1];
				if ( eid != -1 ) {

					sb.AppendFormat( "{0} Lv. {1} HP: {2} -> {3} ({4}) {5}\r\n",
						db.MasterShips[eid].NameWithClass,
						bd.EnemyLevels[i + 1],
						bd.InitialHP[i + 7],
						hp[i + 6],
						hp[i + 6] - bd.InitialHP[i + 7],
						GetState( eid, (double)hp[i + 6] / bd.MaxHP[i + 7] ) );

				} else {
					sb.AppendLine( "-" );
				}
			}

			return sb.ToString();
		}





		private void SetFormation( BattleData bd ) {

			FormationFriend.Text = Constants.GetFormationShort( bd.Data.api_formation[0] is string ? int.Parse( bd.Data.api_formation[0] ) : (int)bd.Data.api_formation[0] );
			FormationEnemy.Text = Constants.GetFormationShort( bd.Data.api_formation[1] is string ? int.Parse( bd.Data.api_formation[1] ) : (int)bd.Data.api_formation[1] );
			Formation.Text = Constants.GetEngagementForm( (int)bd.Data.api_formation[2] );

		}

		private void SetSearchingResult( BattleData bd ) {

			SearchingFriend.Text = Constants.GetSearchingResultShort( (int)bd.Data.api_search[0] );
			SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingFriend.ImageIndex = (int)( (int)bd.Data.api_search[0] < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
			SearchingEnemy.Text = Constants.GetSearchingResultShort( (int)bd.Data.api_search[1] );
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingEnemy.ImageIndex = (int)( (int)bd.Data.api_search[1] < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );

		}

		private void ClearSearchingResult() {

			SearchingFriend.Text = "-";
			SearchingFriend.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingFriend.ImageIndex = -1;
			SearchingEnemy.Text = "-";
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingEnemy.ImageIndex = -1;

		}

		private void SetAerialWarfare( BattleData bd ) {

			if ( (int)bd.Data.api_stage_flag[0] != 0 ) {
				AirSuperiority.Text = Constants.GetAirSuperiority( (int)bd.Data.api_kouku.api_stage1.api_disp_seiku );
				AirStage1Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage1.api_f_lostcount,
					(int)bd.Data.api_kouku.api_stage1.api_f_count );
				AirStage1Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage1.api_e_lostcount,
					(int)bd.Data.api_kouku.api_stage1.api_e_count );

				if ( (int)bd.Data.api_kouku.api_stage1.api_touch_plane[0] != -1 )
					ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_kouku.api_stage1.api_touch_plane[0]].Name ) );
				else
					ToolTipInfo.SetToolTip( AirStage1Friend, null );

				if ( (int)bd.Data.api_kouku.api_stage1.api_touch_plane[1] != -1 )
					ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_kouku.api_stage1.api_touch_plane[1]].Name ) );
				else
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );

			} else {
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				AirStage1Friend.Text = "-";
				AirStage1Enemy.Text = "-";
				ToolTipInfo.SetToolTip( AirStage1Friend, null );
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {
				AirStage2Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_f_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_f_count );
				AirStage2Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_e_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_e_count );

				if ( bd.Data.api_kouku.api_stage2.api_air_fire() ) {	//対空カットイン
					int cutinID = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_kind;
					int cutinIndex = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_idx;

					ToolTipInfo.SetToolTip( AirStage2Friend, string.Format(
						"対空カットイン: {0}\r\nカットイン種別: {1} ({2})", 
						KCDatabase.Instance.Fleet[cutinIndex >= 6 ? 2 : bd.FleetIDFriend].MembersInstance[cutinIndex % 6].NameWithLevel, 
						cutinID,
						Constants.GetAACutinKind( cutinID ) ) );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Friend, null );
				}

			} else {
				AirStage2Friend.Text = "-";
				AirStage2Enemy.Text = "-";
				ToolTipInfo.SetToolTip( AirStage2Friend, null );
			}

		}

		private void SetAerialWarfareAirBattle( BattleData bd ) {

			if ( (int)bd.Data.api_stage_flag[0] != 0 ) {
				AirSuperiority.Text = Constants.GetAirSuperiority( (int)bd.Data.api_kouku.api_stage1.api_disp_seiku );
				AirStage1Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage1.api_f_lostcount + ( (int)bd.Data.api_stage_flag2[0] != 0 ? (int)bd.Data.api_kouku2.api_stage1.api_f_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage1.api_f_count );
				AirStage1Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage1.api_e_lostcount + ( (int)bd.Data.api_stage_flag2[0] != 0 ? (int)bd.Data.api_kouku2.api_stage1.api_e_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage1.api_e_count );

				if ( (int)bd.Data.api_kouku.api_stage1.api_touch_plane[0] != -1 )
					ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_kouku.api_stage1.api_touch_plane[0]].Name ) );
				else
					ToolTipInfo.SetToolTip( AirStage1Friend, null );

				if ( (int)bd.Data.api_kouku.api_stage1.api_touch_plane[1] != -1 )
					ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_kouku.api_stage1.api_touch_plane[1]].Name ) );
				else
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );

			} else {
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				AirStage1Friend.Text = "-";
				AirStage1Enemy.Text = "-";
				ToolTipInfo.SetToolTip( AirStage1Friend, null );
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {
				AirStage2Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_f_lostcount + ( (int)bd.Data.api_stage_flag2[1] != 0 ? (int)bd.Data.api_kouku2.api_stage2.api_f_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage2.api_f_count );
				AirStage2Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_e_lostcount + ( (int)bd.Data.api_stage_flag2[1] != 0 ? (int)bd.Data.api_kouku2.api_stage2.api_e_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage2.api_e_count );


				if ( bd.Data.api_kouku.api_stage2.api_air_fire() ) {	//対空カットイン
					int cutinID = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_kind;
					int cutinIndex = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_idx;

					ToolTipInfo.SetToolTip( AirStage2Friend, string.Format(
						"対空カットイン: {0}\r\nカットイン種別: {1} ({2})",
						KCDatabase.Instance.Fleet[cutinIndex >= 6 ? 2 : bd.FleetIDFriend].MembersInstance[cutinIndex % 6].NameWithLevel,
						cutinID,
						Constants.GetAACutinKind( cutinID ) ) );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Friend, null );
				}
				
			} else {
				AirStage2Friend.Text = "-";
				AirStage2Enemy.Text = "-";

				ToolTipInfo.SetToolTip( AirStage2Friend, null );
			}

		}

		private void ClearAerialWarfare() {
			AirSuperiority.Text = "-";
			AirStage1Friend.Text = "-";
			AirStage1Enemy.Text = "-";
			AirStage2Friend.Text = "-";
			AirStage2Enemy.Text = "-";
			ToolTipInfo.SetToolTip( AirStage2Friend, null );
		}

		private void SetHPNormal( int[] hp, BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0; 

			for ( int i = 0; i < 12; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					HPBars[i].Value = hp[i];
					HPBars[i].PrevValue = (int)bd.Data.api_nowhps[i + 1];
					HPBars[i].MaximumValue = (int)bd.Data.api_maxhps[i + 1];
					HPBars[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].Members[i]];

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase )
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 7] != -1 ) {
					ShipDataMaster ship = db.MasterShips[bd.EnemyFleetMembers[i + 1]];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.EnemyLevels[i + 1],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}


			FleetCombined.Visible = false;
			for ( int i = 12; i < 18; i++ ) {
				HPBars[i].Visible = false;
			}

		}

		private void SetHPCombined( int[] hp, BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			for ( int i = 0; i < 12; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					HPBars[i].Value = hp[i];
					HPBars[i].PrevValue = (int)bd.Data.api_nowhps[i + 1];
					HPBars[i].MaximumValue = (int)bd.Data.api_maxhps[i + 1];
					HPBars[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
				}
			}

			FleetCombined.Visible = true;
			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps_combined[i + 1] != -1 ) {
					HPBars[i + 12].Value = hp[i + 12];
					HPBars[i + 12].PrevValue = (int)bd.Data.api_nowhps_combined[i + 1];
					HPBars[i + 12].MaximumValue = (int)bd.Data.api_maxhps_combined[i + 1];
					HPBars[i + 12].Visible = true;
				} else {
					HPBars[i + 12].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].Members[i]];
					bool isEscaped = db.Fleet[bd.FleetIDFriend].EscapedShipList.Contains( ship.MasterID );

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped )
							)
						);

					if ( isEscaped ) HPBars[i].BackColor = Color.Silver;
					else HPBars[i].BackColor = SystemColors.Control;
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 7] != -1 ) {
					ShipDataMaster ship = db.MasterShips[bd.EnemyFleetMembers[i + 1]];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.EnemyLevels[i + 1],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps_combined[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[2].Members[i]];
					bool isEscaped = db.Fleet[2].EscapedShipList.Contains( ship.MasterID );

					ToolTipInfo.SetToolTip( HPBars[i + 12],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i + 12].PrevValue, 0 ),
							Math.Max( HPBars[i + 12].Value, 0 ),
							HPBars[i + 12].MaximumValue,
							HPBars[i + 12].Value - HPBars[i + 12].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 12].Value / HPBars[i + 12].MaximumValue, ship.MasterShip.IsLandBase, isEscaped )
							)
						);

					if ( isEscaped ) HPBars[i + 12].BackColor = Color.Silver;
					else HPBars[i + 12].BackColor = SystemColors.Control;
				}
			}
		}

		private void SetDamageRateNormal( int[] hp, BattleData bd ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( (int)bd.Data.api_nowhps[i + 1], 0 );
				friendafter += Math.Max( hp[i], 0 );
				enemybefore += Math.Max( (int)bd.Data.api_nowhps[i + 7], 0 );
				enemyafter += Math.Max( hp[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:0.0}%", friendrate * 100.0 );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:0.0}%", enemyrate * 100.0 );


			//戦績判定
			{
				int countFriend = KCDatabase.Instance.Fleet[(int)bd.FleetIDFriend].Members.Count( v => v != -1 );
				int countEnemy = ( bd.EnemyFleetMembers.Skip( 1 ).Count( v => v != -1 ) );
				int sunkFriend = hp.Take( countFriend ).Count( v => v <= 0 );
				int sunkEnemy = hp.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );
				int rank;
				Color colorWin = SystemColors.WindowText;
				Color colorLose = Color.Red;

				if ( enemyrate >= 1.0 ) {
					if ( friendrate <= 0.0 ) {
						rank = 7;
					} else {
						rank = 6;
					}

				} else if ( sunkEnemy >= (int)Math.Round( countEnemy * 0.6 ) ) {
					rank = 5;

				} else if ( hp[6] <= 0 ||
					(int)( enemyrate * 100 ) > (int)( friendrate * 100 ) * 2.5 ) {
					rank = 4;

				} else if ( (int)( enemyrate * 100 ) > (int)( friendrate * 100 ) ) {
					rank = 3;
				} else {
					rank = 2;
				}

				if ( sunkFriend > 0 )
					rank = Math.Min( rank, 5 ) - 1;


				switch ( rank ) {
					case 2:
						DamageRate.Text = "D";
						DamageRate.ForeColor = colorLose;
						break;
					case 3:
						DamageRate.Text = "C";
						DamageRate.ForeColor = colorLose;
						break;
					case 4:
						DamageRate.Text = "B";
						DamageRate.ForeColor = colorWin;
						break;
					case 5:
						DamageRate.Text = "A";
						DamageRate.ForeColor = colorWin;
						break;
					case 6:
						DamageRate.Text = "S";
						DamageRate.ForeColor = colorWin;
						break;
					case 7:
						DamageRate.Text = "SS";
						DamageRate.ForeColor = colorWin;
						break;
					default:
						DamageRate.Text = "E";
						DamageRate.ForeColor = colorLose;
						break;
				}


			}
		}

		//fixme
		private void SetDamageRateCombined( int[] hp, BattleData bd ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			BattleDataCombined bdc = bd as BattleDataCombined;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( (int)bdc.Data.api_nowhps[i + 1], 0 );
				friendafter += Math.Max( hp[i], 0 );
				friendbefore += Math.Max( (int)bdc.Data.api_nowhps_combined[i + 1], 0 );
				friendafter += Math.Max( hp[i + 12], 0 );
				enemybefore += Math.Max( (int)bdc.Data.api_nowhps[i + 7], 0 );
				enemyafter += Math.Max( hp[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:0.0}%", friendrate * 100.0 );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:0.0}%", enemyrate * 100.0 );


			//戦績判定
			{
				int countFriend = KCDatabase.Instance.Fleet[bdc.FleetIDFriend].Members.Count( v => v != -1 );
				int countFriendCombined = KCDatabase.Instance.Fleet[bdc.FleetIDFriendCombined].Members.Count( v => v != -1 );
				int countEnemy = ( bdc.EnemyFleetMembers.Skip( 1 ).Count( v => v != -1 ) );
				int sunkFriend = hp.Take( countFriend ).Count( v => v <= 0 ) + hp.Skip( 12 ).Take( countFriendCombined ).Count( v => v <= 0 );
				int sunkEnemy = hp.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );
				int rank;
				Color colorWin = SystemColors.WindowText;
				Color colorLose = Color.Red;

				if ( enemyrate >= 1.0 ) {
					if ( friendrate <= 0.0 ) {
						rank = 7;
					} else {
						rank = 6;
					}

				} else if ( sunkEnemy >= (int)Math.Round( countEnemy * 0.6 ) ) {
					rank = 5;

				} else if ( hp[6] <= 0 ||
					(int)( enemyrate * 100 ) > (int)( friendrate * 100 ) * 2.5 ) {
					rank = 4;

				} else if ( (int)( enemyrate * 100 ) > (int)( friendrate * 100 ) ) {
					rank = 3;
				} else {
					rank = 2;
				}

				if ( sunkFriend > 0 )
					rank = Math.Min( rank, 5 ) - 1;


				switch ( rank ) {
					case 2:
						DamageRate.Text = "D";
						DamageRate.ForeColor = colorLose;
						break;
					case 3:
						DamageRate.Text = "C";
						DamageRate.ForeColor = colorLose;
						break;
					case 4:
						DamageRate.Text = "B";
						DamageRate.ForeColor = colorWin;
						break;
					case 5:
						DamageRate.Text = "A";
						DamageRate.ForeColor = colorWin;
						break;
					case 6:
						DamageRate.Text = "S";
						DamageRate.ForeColor = colorWin;
						break;
					case 7:
						DamageRate.Text = "SS";
						DamageRate.ForeColor = colorWin;
						break;
					default:
						DamageRate.Text = "E";
						DamageRate.ForeColor = colorLose;
						break;
				}


			}
		}



		private void SetNightBattleEvent( int[] hp, bool isCombined, BattleData bd ) {

			FleetData fleet = KCDatabase.Instance.Fleet[isCombined ? 2 : bd.FleetIDFriend];

			//味方探照灯判定
			{
				ShipData ship = null;
				for ( int i = 0; i < 6; i++ ) {
					ShipData s = fleet.MembersInstance[i];
					if ( s != null &&
						s.SlotInstanceMaster.Count( e => e != null && e.EquipmentType[2] == 29 ) > 0 &&
						hp[isCombined ? 12 + i : i] > 1 ) {
						ship = s;
						break;
					}
				}

				if ( ship != null ) {
					ToolTipInfo.SetToolTip( FleetFriend, string.Format( "探照灯照射: {0}", ship.MasterShip.Name ) );
				} else {
					ToolTipInfo.SetToolTip( FleetFriend, null );
				}
			}

			//敵探照灯判定
			{
				int idx = -1;
				for ( int i = 1; i < bd.EnemyFleetMembers.Count; i++ ) {
					if ( bd.EnemyFleetMembers[i] == -1 ) continue;
					if ( hp[i + 6 - 1] <= 1 ) continue;

					if ( ( (int[])bd.Data.api_eSlot[i - 1] ).Count( 
						id => KCDatabase.Instance.MasterEquipments.ContainsKey( id ) && 
							KCDatabase.Instance.MasterEquipments[id].EquipmentType[2] == 29
							) > 0 ) {
						idx = i - 1;
						break;
					}
				}

				if ( idx != -1 ) {
					ToolTipInfo.SetToolTip( FleetEnemy, string.Format( "探照灯照射: {0}", KCDatabase.Instance.MasterShips[bd.EnemyFleetMembers[idx]].NameWithClass ) );
				} else {
					ToolTipInfo.SetToolTip( FleetEnemy, null );
				}
			}


			//夜間触接判定
			if ( (int)bd.Data.api_touch_plane[0] != -1 )
				ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "夜間触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_touch_plane[0]].Name ) );
			else
				ToolTipInfo.SetToolTip( AirStage1Friend, null );

			if ( (int)bd.Data.api_touch_plane[1] != -1 )
				ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "夜間触接中: {0}", KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_touch_plane[1]].Name ) );
			else
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );


			//照明弾投射判定(仮)
			if ( (int)bd.Data.api_flare_pos[0] != -1 )
				ToolTipInfo.SetToolTip( AirStage2Friend, string.Format(
						"照明弾投射: {0}", fleet.MembersInstance[(int)bd.Data.api_flare_pos[0] - 1].MasterShip.Name ) );
			else
				ToolTipInfo.SetToolTip( AirStage2Friend, null );

			if ( (int)bd.Data.api_flare_pos[1] != -1 )
				ToolTipInfo.SetToolTip( AirStage2Enemy, string.Format(
						"照明弾投射: {0}", KCDatabase.Instance.MasterShips[bd.EnemyFleetMembers[(int)bd.Data.api_flare_pos[1] - 1]].NameWithClass ) );
			else
				ToolTipInfo.SetToolTip( AirStage2Enemy, null );


		}



		void ConfigurationChanged() {

			MainFont = TableMain.Font = Font = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

		}



		protected override string GetPersistString() {
			return "Battle";
		}


		private void TableMain_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 1 || e.Row == 4 || e.Row == 12 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

	}

}
