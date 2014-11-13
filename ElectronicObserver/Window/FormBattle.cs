using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
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

		public FormBattle( FormMain parent ) {
			InitializeComponent();

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
			o.APIList["api_req_combined_battle/battleresult"].ResponseReceived += rec;
			o.APIList["api_req_practice/battle"].ResponseReceived += rec;
			o.APIList["api_req_practice/midnight_battle"].ResponseReceived += rec;
			o.APIList["api_req_practice/battle_result"].ResponseReceived += rec;

			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			TextDebug.Font = Font;

		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager battle = db.Battle;


			switch ( apiname ) {

				case "api_req_map/start":
				case "api_req_map/next":
					TextDebug.Text = "";
					break;


				case "api_req_sortie/battle":
					TextDebug.Text = GetBattleString( battle.BattleDay );
					break;

				case "api_req_battle_midnight/battle":
					TextDebug.Text = GetBattleString( battle.BattleNight );
					break;

				case "api_req_battle_midnight/sp_midnight":
					TextDebug.Text = GetBattleString( battle.BattleNight );
					break;

				case "api_req_practice/battle":
					TextDebug.Text = GetBattleString( battle.BattleDay );
					break;

				case "api_req_practice/midnight_battle":
					TextDebug.Text = GetBattleString( battle.BattleNight );
					break;


				case "api_port/port":
					TextDebug.Text = "";
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
			int enemyoffset = !isCombined ? 6 : 12;


			sb.AppendLine( "---- 自軍艦隊 ----" );
			if ( isCombined )
				sb.AppendLine( "[機動部隊本隊]" );

			for ( int i = 0; i < 6; i++ ) {

				ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].FleetMember[i]];

				if ( ship != null ) {

					sb.Append( ship.MasterShip.Name );
					sb.Append( " Lv. " );
					sb.Append( ship.Level );
					sb.Append( " HP: " );
					sb.Append( bd.InitialHP[i + 1] );
					sb.Append( " -> " );
					sb.Append( hp[i] );
					sb.Append( " (" );
					sb.Append( hp[i] - bd.InitialHP[i + 1] );
					sb.Append( ") " );
					if ( hp[i] == 0 ) {
						if ( isPractice )
							sb.Append( "[離脱]" );
						else
							sb.Append( "[撃沈]" );
					} else if ( (double)hp[i] / bd.MaxHP[i + 1] <= 0.25 ) {
						sb.Append( "[大破]" );
					} else if ( (double)hp[i] / bd.MaxHP[i + 1] <= 0.50 ) {
						sb.Append( "[中破]" );
					} else if ( (double)hp[i] / bd.MaxHP[i + 1] <= 0.75 ) {
						sb.Append( "[小破]" );
					} else if ( (double)hp[i] < bd.MaxHP[i + 1] ) {
						sb.Append( "[健在]" );
					} else {
						sb.Append( "[無傷]" );
					}
					sb.AppendLine();

				} else {
					sb.AppendLine( "-" );
				}

			}

			if ( isCombined ) {

				BattleDataCombined bdc = (BattleDataCombined)bd;

				sb.AppendLine();
				sb.AppendLine( "[随伴護衛艦隊]" );


				for ( int i = 0; i < 6; i++ ) {

					ShipData ship = db.Ships[db.Fleet[2].FleetMember[i]];

					if ( ship != null ) {

						sb.Append( ship.MasterShip.Name );
						sb.Append( " Lv. " );
						sb.Append( ship.Level );
						sb.Append( " HP: " );
						sb.Append( bdc.InitialHPCombined[i + 1] );
						sb.Append( " -> " );
						sb.Append( hp[i + 6] );
						sb.Append( " (" );
						sb.Append( hp[i + 6] - bdc.InitialHPCombined[i + 1] );
						sb.Append( ") " );
						if ( hp[i + 6] == 0 ) {
							if ( isPractice )
								sb.Append( "[離脱]" );
							else
								sb.Append( "[撃沈]" );
						} else if ( (double)hp[i + 6] / bd.MaxHP[i + 1] <= 0.25 ) {
							sb.Append( "[大破]" );
						} else if ( (double)hp[i + 6] / bd.MaxHP[i + 1] <= 0.50 ) {
							sb.Append( "[中破]" );
						} else if ( (double)hp[i + 6] / bd.MaxHP[i + 1] <= 0.75 ) {
							sb.Append( "[小破]" );
						} else if ( (double)hp[i + 6] < bd.MaxHP[i + 1] ) {
							sb.Append( "[健在]" );
						} else {
							sb.Append( "[無傷]" );
						}
						sb.AppendLine();

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

					sb.Append( db.MasterShips[eid].Name );
					if ( !isPractice &&
						 db.MasterShips[eid].NameReading != null &&
						 db.MasterShips[eid].NameReading != "" &&
						 db.MasterShips[eid].NameReading != "-" ) {
						sb.Append( " " );
						sb.Append( db.MasterShips[eid].NameReading );
					}
					sb.Append( " Lv. " );
					sb.Append( bd.EnemyLevels[i + 1] );
					sb.Append( " HP: " );
					sb.Append( bd.InitialHP[i + 7] );
					sb.Append( " -> " );
					sb.Append( hp[i + enemyoffset] );
					sb.Append( " (" );
					sb.Append( hp[i + enemyoffset] - bd.InitialHP[i + 7] );
					sb.Append( ") " );
					if ( hp[i + enemyoffset] == 0 ) {
						if ( isPractice )
							sb.Append( "[離脱]" );
						else
							sb.Append( "[撃沈]" );
					} else if ( (double)hp[i + enemyoffset] / bd.MaxHP[i + 7] <= 0.25 ) {
						sb.Append( "[大破]" );
					} else if ( (double)hp[i + enemyoffset] / bd.MaxHP[i + 7] <= 0.50 ) {
						sb.Append( "[中破]" );
					} else if ( (double)hp[i + enemyoffset] / bd.MaxHP[i + 7] <= 0.75 ) {
						sb.Append( "[小破]" );
					} else if ( (double)hp[i + enemyoffset] < bd.MaxHP[i + 7] ) {
						sb.Append( "[健在]" );
					} else {
						sb.Append( "[無傷]" );
					}
					sb.AppendLine();

				} else {
					sb.AppendLine( "-" );
				}
			}

			return sb.ToString();
		}


		protected override string GetPersistString() {
			return "Battle";
		}

	}

}
