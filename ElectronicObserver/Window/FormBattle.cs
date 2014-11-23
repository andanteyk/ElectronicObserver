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
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += rec;
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
				case "api_req_battle_midnight/sp_midnight":
					TextDebug.Text = GetBattleString( battle.BattleNight );
					break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/airbattle":
				case "api_req_combined_battle/battle_water":
					TextDebug.Text = GetBattleString( battle.BattleDay );
					break;

				case "api_req_combined_battle/midnight_battle":
				case "api_req_combined_battle/sp_midnight":
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


		protected override string GetPersistString() {
			return "Battle";
		}

	}

}
