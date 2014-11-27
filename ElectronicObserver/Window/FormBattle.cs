using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
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

	public partial class FormBattle : DockContent {

		private List<ShipStatusHP> HPBars;


		public FormBattle( FormMain parent ) {
			InitializeComponent();

			HPBars = new List<ShipStatusHP>( 18 );


			for ( int i = 0; i < 18; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = new Size( 80, 20 );
				HPBars[i].Margin = new Padding( 2, 0, 2, 0 );
				HPBars[i].Anchor = AnchorStyles.None;
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

			TableMain.Visible = false;
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

			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			
		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager battle = db.Battle;


			switch ( apiname ) {

				case "api_req_map/start":
				case "api_req_map/next":
					TableMain.Visible = false;
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle":
					UpdateNormalDayBattle( battle );
					break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle":
					UpdateNormalNightBattle( battle );
					break;

				case "api_req_battle_midnight/sp_midnight":
					UpdateNightOnlyBattle( battle );
					break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water":
					UpdateCombinedDayBattle( battle );
					break;

				case "api_req_combined_battle/airbattle":
					UpdateCombinedAirBattle( battle );
					break;
				
				case "api_req_combined_battle/midnight_battle":
					UpdateCombinedNightBattle( battle );
					break;

				case "api_req_combined_battle/sp_midnight":
					UpdateCombinedNightOnlyBattle( battle );
					break;

				

				case "api_port/port":
					TableMain.Visible = false;
					break;

			}

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

				ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].FleetMember[i]];

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

					ShipData ship = db.Ships[db.Fleet[2].FleetMember[i]];

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



		private void UpdateNormalDayBattle( BattleManager bm ) {

			int[] hp = bm.BattleDay.EmulateBattle();


			TableMain.SuspendLayout();


			SetFormation( bm.BattleDay );
			SetSearchingResult( bm.BattleDay );
			SetAerialWarfare( bm.BattleDay ); 
			SetHPNormal( hp, bm.BattleDay );
			SetDamageRateNormal( hp, bm.BattleDay );


			TableMain.ResumeLayout();
			TableMain.Visible = true;
		}


		private void UpdateNormalNightBattle( BattleManager bm ) {

			int[] hp = bm.BattleNight.EmulateBattle();


			TableMain.SuspendLayout();


			SetHPNormal( hp, bm.BattleNight );
			SetDamageRateNormal( hp, bm.BattleDay );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}

		private void UpdateNightOnlyBattle( BattleManager bm ) {

			int[] hp = bm.BattleNight.EmulateBattle();


			TableMain.SuspendLayout();


			SetFormation( bm.BattleNight );
			ClearAerialWarfare();
			ClearSearchingResult();
			SetHPNormal( hp, bm.BattleNight );
			SetDamageRateNormal( hp, bm.BattleNight );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}

		private void UpdateCombinedDayBattle( BattleManager bm ) {

			int[] hp = bm.BattleDay.EmulateBattle();


			TableMain.SuspendLayout();


			SetFormation( bm.BattleDay );
			SetSearchingResult( bm.BattleDay );
			SetAerialWarfare( bm.BattleDay );
			SetHPCombined( hp, bm.BattleDay );
			SetDamageRateNormal( hp, bm.BattleDay );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}

		private void UpdateCombinedNightBattle( BattleManager bm ) {

			int[] hp = bm.BattleNight.EmulateBattle();


			TableMain.SuspendLayout();


			SetHPCombined( hp, bm.BattleNight );
			SetDamageRateCombined( hp, bm.BattleDay );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}

		private void UpdateCombinedNightOnlyBattle( BattleManager bm ) {

			int[] hp = bm.BattleNight.EmulateBattle();


			TableMain.SuspendLayout();


			SetFormation( bm.BattleNight );
			ClearAerialWarfare();
			ClearSearchingResult();
			SetHPCombined( hp, bm.BattleNight );
			SetDamageRateCombined( hp, bm.BattleNight );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}

		private void UpdateCombinedAirBattle( BattleManager bm ) {

			int[] hp = bm.BattleDay.EmulateBattle();


			TableMain.SuspendLayout();


			SetFormation( bm.BattleDay );
			SetSearchingResult( bm.BattleDay );
			SetAerialWarfareAirBattle( bm.BattleDay );
			SetHPCombined( hp, bm.BattleDay );
			SetDamageRateCombined( hp, bm.BattleDay );


			TableMain.ResumeLayout();
			TableMain.Visible = true;

		}


		private void SetFormation( BattleData bd ) {

			FormationFriend.Text = Constants.GetFormationShort( bd.Data.api_formation[0] is string ? int.Parse( bd.Data.api_formation[0] ) : (int)bd.Data.api_formation[0] );
			FormationEnemy.Text = Constants.GetFormationShort( bd.Data.api_formation[1] is string ? int.Parse( bd.Data.api_formation[1] ) : (int)bd.Data.api_formation[1] );
			Formation.Text = Constants.GetEngagementForm( (int)bd.Data.api_formation[2] );

		}

		private void SetSearchingResult( BattleData bd ) {

			SearchingFriend.Text = Constants.GetSearchingResult( (int)bd.Data.api_search[0] );
			SearchingEnemy.Text = Constants.GetSearchingResult( (int)bd.Data.api_search[1] );

		}

		private void ClearSearchingResult() {

			SearchingFriend.Text = "-";
			SearchingEnemy.Text = "-";

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
			} else {
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				AirStage1Friend.Text = "-";
				AirStage1Enemy.Text = "-";
			}

			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {
				AirStage2Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_f_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_f_count );
				AirStage2Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_e_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_e_count );
			} else {
				AirStage2Friend.Text = "-";
				AirStage2Enemy.Text = "-";
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
			} else {
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				AirStage1Friend.Text = "-";
				AirStage1Enemy.Text = "-";
			}

			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {
				AirStage2Friend.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_f_lostcount + ( (int)bd.Data.api_stage_flag2[1] != 0 ? (int)bd.Data.api_kouku2.api_stage2.api_f_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage2.api_f_count );
				AirStage2Enemy.Text = string.Format( "-{0}/{1}",
					(int)bd.Data.api_kouku.api_stage2.api_e_lostcount + ( (int)bd.Data.api_stage_flag2[1] != 0 ? (int)bd.Data.api_kouku2.api_stage2.api_e_lostcount : 0 ),
					(int)bd.Data.api_kouku.api_stage2.api_e_count );
			} else {
				AirStage2Friend.Text = "-";
				AirStage2Enemy.Text = "-";
			}

		}

		private void ClearAerialWarfare() {
			AirSuperiority.Text = "-";
			AirStage1Friend.Text = "-";
			AirStage1Enemy.Text = "-";
			AirStage2Friend.Text = "-";
			AirStage2Enemy.Text = "-";
		}

		private void SetHPNormal( int[] hp, BattleData bd ) {

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

			FleetCombined.Visible = false;
			for ( int i = 12; i < 18; i++ ) {
				HPBars[i].Visible = false;
			}

		}

		private void SetHPCombined( int[] hp, BattleData bd ) {

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


			//undone: 戦績判定

		}

		private void SetDamageRateCombined( int[] hp, BattleData bd ) {

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
				friendbefore += Math.Max( (int)bd.Data.api_nowhps_combined[i + 1], 0 );
				friendafter += Math.Max( hp[i + 12], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:0.0}%", friendrate * 100.0 );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:0.0}%", enemyrate * 100.0 );


			//undone: 戦績判定
		}

		
		protected override string GetPersistString() {
			return "Battle";
		}

	}

}
