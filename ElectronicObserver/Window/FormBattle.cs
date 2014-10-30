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

			o.ResponseList["api_port/port"].ResponseReceived += rec;
			o.ResponseList["api_req_sortie/battle"].ResponseReceived += rec;
			o.ResponseList["api_req_sortie/battleresult"].ResponseReceived += rec;
			o.ResponseList["api_req_battle_midnight/battle"].ResponseReceived += rec;
			o.ResponseList["api_req_battle_midnight/sp_midnight"].ResponseReceived += rec;
			o.ResponseList["api_req_combined_battle/battle"].ResponseReceived += rec;
			o.ResponseList["api_req_combined_battle/midnight_battle"].ResponseReceived += rec;
			o.ResponseList["api_req_combined_battle/sp_midnight"].ResponseReceived += rec;
			o.ResponseList["api_req_combined_battle/airbattle"].ResponseReceived += rec;
			o.ResponseList["api_req_combined_battle/battleresult"].ResponseReceived += rec;
			o.ResponseList["api_req_practice/battle"].ResponseReceived += rec;
			o.ResponseList["api_req_practice/midnight_battle"].ResponseReceived += rec;
			o.ResponseList["api_req_practice/battle_result"].ResponseReceived += rec;

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

				case "api_req_sortie/battle": {

						StringBuilder sb = new StringBuilder();
						int[] hp = battle.BattleDay.EmulateBattle();

						sb.AppendLine( "---- 味方艦隊 ----" );

						for ( int i = 0; i < 6; i++ ) {

							ShipData ship = db.Ships[db.Fleet[battle.BattleDay.FleetIDFriend].FleetMember[i]];

							if ( ship != null ) {

								sb.Append( ship.MasterShip.Name );
								sb.Append( " Lv. " );
								sb.Append( ship.Level );
								sb.Append( " HP: " );
								sb.Append( battle.BattleDay.InitialHP[i + 1] );
								sb.Append( " -> " );
								sb.Append( hp[i] );
								sb.AppendLine();

							} else {
								sb.AppendLine( "-" );
							}

						}

						sb.AppendLine();
						sb.AppendLine( "---- 敵艦隊 ----" );

						for ( int i  = 0; i < 6; i++ ) {

							int eid = battle.BattleDay.EnemyFleetMembers[i + 1];
							if ( eid != -1 ) {

								sb.Append( db.MasterShips[eid].Name );
								sb.Append( " Lv. " );
								sb.Append( battle.BattleDay.EnemyLevels[i + 1] );
								sb.Append( " HP: " );
								sb.Append( battle.BattleDay.InitialHP[i + 7] );
								sb.Append( " -> " );
								sb.Append( hp[i + 6] );
								sb.AppendLine();

							} else {
								sb.AppendLine( "-" );
							}
						}

						TextDebug.Text = sb.ToString();

					} break;


				case "api_req_battle_midnight/battle": {

						StringBuilder sb = new StringBuilder();
						int[] hp = battle.BattleNight.EmulateBattle();

						sb.AppendLine( "---- 味方艦隊 ----" );

						for ( int i = 0; i < 6; i++ ) {

							ShipData ship = db.Ships[db.Fleet[battle.BattleNight.FleetIDFriend].FleetMember[i]];

							if ( ship != null ) {

								sb.Append( ship.MasterShip.Name );
								sb.Append( " Lv. " );
								sb.Append( ship.Level );
								sb.Append( " HP: " );
								sb.Append( battle.BattleNight.InitialHP[i + 1] );
								sb.Append( " -> " );
								sb.Append( hp[i] );
								sb.AppendLine();

							} else {
								sb.AppendLine( "-" );
							}

						}

						sb.AppendLine();
						sb.AppendLine( "---- 敵艦隊 ----" );

						for ( int i  = 0; i < 6; i++ ) {

							int eid = battle.BattleNight.EnemyFleetMembers[i + 1];
							if ( eid != -1 ) {

								sb.Append( db.MasterShips[eid].Name );
								sb.Append( " Lv. " );
								sb.Append( battle.BattleNight.EnemyLevels[i + 1] );
								sb.Append( " HP: " );
								sb.Append( battle.BattleNight.InitialHP[i + 7] );
								sb.Append( " -> " );
								sb.Append( hp[i + 6] );
								sb.AppendLine();

							} else {
								sb.AppendLine( "-" );
							}
						}

						TextDebug.Text = sb.ToString();

					} break;


				case "api_port/port":
					TextDebug.Text = "";
					break;

			}

		}

		protected override string GetPersistString() {
			return "Battle";
		}

	}

}
