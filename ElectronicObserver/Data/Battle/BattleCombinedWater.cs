using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public class BattleCombinedWater : BattleDataCombined {

		
		public override int[] EmulateBattle() {


			int[] hp = new int[18];

			KCDatabase db = KCDatabase.Instance;

			Action<int, int> DealDamageFriend = ( int index, int damage ) => {
				//if ( hp[index] == -1 ) return;
				hp[index] -= Math.Max( damage, 0 );
				if ( hp[index] <= 0 ) {
					ShipData ship = db.Ships[db.Fleet[index < 6 ? 1 : 2].Members[index]];
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

			Action<int, int> DealDamageFriendEscort = ( int index, int damage ) => DealDamageFriend( index + 12, damage );


			for ( int i = 0; i < 12; i++ ) {
				hp[i] = (int)RawData.api_nowhps[i + 1];
			}
			for ( int i = 0; i < 6; i++ ) {
				hp[i + 12] = (int)RawData.api_nowhps_combined[i + 1];
			}


			//航空戦
			if ( (int)RawData.api_stage_flag[2] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_kouku.api_stage3.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_kouku.api_stage3.api_edam[i + 1] );
					DealDamageFriendEscort( i, (int)RawData.api_kouku.api_stage3_combined.api_fdam[i + 1] );
				}
			}


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


			//開幕雷撃
			if ( (int)RawData.api_opening_flag != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriendEscort( i, (int)RawData.api_opening_atack.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_opening_atack.api_edam[i + 1] );
				}
			}


			

			//砲撃戦(1巡目)
			if ( (int)RawData.api_hourai_flag[0] != 0 ) {
				dynamic hougeki = RawData.api_hougeki1;

				int[] damageList = new int[12];
				int leni = ( (int[])hougeki.api_at_list ).Length;

				for ( int i = 1; i < leni; i++ ) {

					for ( int j = 0; j < damageList.Length; j++ ) {
						damageList[j] = 0;
					}

					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						int target = (int)hougeki.api_df_list[i][j];
						if ( target != -1 )
							damageList[target - 1] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						DealDamageFriend( j, damageList[j] );
						DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}


			//砲撃戦(2巡目)
			if ( (int)RawData.api_hourai_flag[1] != 0 ) {
				dynamic hougeki = RawData.api_hougeki2;

				int[] damageList = new int[12];
				int leni = ( (int[])hougeki.api_at_list ).Length;

				for ( int i = 1; i < leni; i++ ) {

					for ( int j = 0; j < damageList.Length; j++ ) {
						damageList[j] = 0;
					}

					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						int target = (int)hougeki.api_df_list[i][j];
						if ( target != -1 )
							damageList[target - 1] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						DealDamageFriend( j, damageList[j] );
						DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}


			//砲撃戦(3巡目)
			if ( (int)RawData.api_hourai_flag[2] != 0 ) {
				dynamic hougeki = RawData.api_hougeki3;

				int[] damageList = new int[12];
				int leni = ( (int[])hougeki.api_at_list ).Length;

				for ( int i = 1; i < leni; i++ ) {

					for ( int j = 0; j < damageList.Length; j++ ) {
						damageList[j] = 0;
					}

					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						int target = (int)hougeki.api_df_list[i][j];
						if ( target != -1 )
							damageList[target - 1] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						DealDamageFriendEscort( j, damageList[j] );
						DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}

			//雷撃戦
			if ( (int)RawData.api_hourai_flag[3] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriendEscort( i, (int)RawData.api_raigeki.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_raigeki.api_edam[i + 1] );
				}
			}



			return hp;

		}




		public override int FleetIDFriend {
			get { return int.Parse( RawData.api_deck_id ); }
		}

		public override int FleetIDFriendCombined {
			get { return 2; }
		}


		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}

		public override string APIName {
			get { return "api_req_combined_battle/battle_water"; }
		}
	}

}
