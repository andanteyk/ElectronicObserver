using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	public class BattleAirBattle : BattleData {

		public override int FleetIDFriend {
			get { return (int)RawData.api_dock_id; }
		}

		public override int[] EmulateBattle() {

			int[] hp = new int[12];

			KCDatabase db = KCDatabase.Instance;

			Action<int, int> DealDamageFriend = ( int index, int damage ) => {
				//if ( hp[index] == -1 ) return;
				hp[index] -= Math.Max( damage, 0 );
				if ( hp[index] <= 0 ) {
					ShipData ship = db.Ships[db.Fleet[FleetIDFriend].Members[index]];
					if ( ship == null ) return;

					foreach ( int id in ship.SlotMaster ) {

						if ( id == 42 ) {			//応急修理要員
							hp[index] = (int)( ship.HPMax * 0.2 );
							break;
						} else if ( id == 43 ) {	//応急修理女神
							hp[index] = ship.HPMax;
							break;
						}
					}
				}
			};

			Action<int, int> DealDamageEnemy = ( int index, int damage ) => {
				//if ( hp[index + 6] == -1 ) return;
				hp[index + 6] -= Math.Max( damage, 0 );
			};


			for ( int i = 0; i < 12; i++ ) {
				hp[i] = (int)RawData.api_nowhps[i + 1];
			}


			//第一次航空戦
			if ( (int)RawData.api_stage_flag[2] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_kouku.api_stage3.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_kouku.api_stage3.api_edam[i + 1] );
				}
			}


			//*/	//今のところ未実装だけど念のため
			if ( RawData.api_support_flag() ) {
				//支援艦隊(空撃)
				if ( (int)RawData.api_support_flag == 1 ) {
					for ( int i = 0; i < 6; i++ ) {
						DealDamageEnemy( i, (int)RawData.api_support_info.api_support_airatack.api_stage3.api_edam[i + 1] );
					}
				}

				//支援艦隊(砲雷撃)
				if ( (int)RawData.api_support_flag == 2 ||
					 (int)RawData.api_support_flag == 3 ) {
					for ( int i = 0; i < 6; i++ ) {
						DealDamageEnemy( i, (int)RawData.api_support_info.api_support_hourai.api_damage[i + 1] );
					}
				}
			}
			//*/

			//第二次航空戦
			if ( (int)RawData.api_stage_flag2[2] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_kouku2.api_stage3.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_kouku2.api_stage3.api_edam[i + 1] );
				}
			}

			return hp;

		}

		public override string APIName {
			get { return "api_req_sortie/airbattle"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day; }
		}
	}
}
