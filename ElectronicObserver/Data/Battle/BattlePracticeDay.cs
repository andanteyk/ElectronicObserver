using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public class BattlePracticeDay : BattleData {

		public double[] friendDamages;
		public double airDamage;

		public override int[] EmulateBattle() {


			friendDamages = new double[6];
			airDamage = 0;
			int[] hp = new int[12];

			KCDatabase db = KCDatabase.Instance;
			Action<int, int> DealDamageFriend = ( int index, int damage ) => {
				//if ( hp[index] == -1 ) return;
				hp[index] -= Math.Max( damage, 0 );
				
				//ダメコンは発動しない。
			};

			Action<int, int> DealDamageEnemy = ( int index, int damage ) => {
				//if ( hp[index + 6] == -1 ) return;
				hp[index + 6] -= Math.Max( damage, 0 );
			};


			for ( int i = 0; i < 12; i++ ) {
				hp[i] = (int)RawData.api_nowhps[i + 1];
			}


			//航空戦
			if ( (int)RawData.api_stage_flag[2] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_kouku.api_stage3.api_fdam[i + 1] );
					double damage = RawData.api_kouku.api_stage3.api_edam[i + 1];
					DealDamageEnemy( i, (int)damage );
					airDamage += damage;
				}
			}


			//支援艦隊は発動しない。

			
			//開幕雷撃
			if ( (int)RawData.api_opening_flag != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_opening_atack.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_opening_atack.api_edam[i + 1] );
					friendDamages[i] += (int)RawData.api_opening_atack.api_fydam[i + 1];
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

					int source = (int)hougeki.api_at_list[i];
					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						int target = (int)hougeki.api_df_list[i][j];
						if ( target != -1 ) {
							double dmg = hougeki.api_damage[i][j];
							damageList[target - 1] += (int)dmg;
							if ( source <= 6 ) {
								friendDamages[source - 1] += dmg;
							}
						}
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

					int source = (int)hougeki.api_at_list[i];
					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						int target = (int)hougeki.api_df_list[i][j];
						if ( target != -1 ) {
							double dmg = hougeki.api_damage[i][j];
							damageList[target - 1] += (int)dmg;
							if ( source <= 6 ) {
								friendDamages[source - 1] += dmg;
							}
						}
					}

					for ( int j = 0; j < 6; j++ ) {
						DealDamageFriend( j, damageList[j] );
						DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}


			//雷撃戦
			if ( (int)RawData.api_hourai_flag[3] != 0 ) {
				for ( int i = 0; i < 6; i++ ) {
					DealDamageFriend( i, (int)RawData.api_raigeki.api_fdam[i + 1] );
					DealDamageEnemy( i, (int)RawData.api_raigeki.api_edam[i + 1] );
					friendDamages[i] += (int)RawData.api_raigeki.api_fydam[i + 1];
				}
			}


			return hp;

		}


		public override string APIName {
			get { return "api_req_practice/battle"; }
		}

		public override int FleetIDFriend {
			get { return (int)RawData.api_dock_id; }
		}


		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Practice; }
		}
	}

}
