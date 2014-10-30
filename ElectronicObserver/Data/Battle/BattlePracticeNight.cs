using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	
	public class BattlePracticeNight : BattleData {

		public override int[] EmulateBattle() {


			int[] hp = new int[12];

			KCDatabase db = KCDatabase.Instance;
			Action<int, int> DealDamageFriend = ( int index, int damage ) => {
				if ( hp[index] == -1 ) return;
				hp[index] = Math.Max( hp[index] - Math.Max( damage, 0 ), 0 );
				
				//ダメコンは発動しない。
			};

			Action<int, int> DealDamageEnemy = ( int index, int damage ) => {
				if ( hp[index + 6] == -1 ) return;
				hp[index + 6] = Math.Max( hp[index + 6] - Math.Max( damage, 0 ), 0 );
			};


			for ( int i = 0; i < 12; i++ ) {
				hp[i] = (int)RawData.api_nowhps[i + 1];
			}


			//夜間砲撃戦
			{
				dynamic hougeki = RawData.api_hougeki;

				int[] damageList = new int[12];
				int leni = ( (int[])hougeki.api_at_list ).Length;

				for ( int i = 1; i < leni; i++ ) {

					for ( int j = 0; j < damageList.Length; j++ ) {
						damageList[j] = 0;
					}

					int lenj = ( (int[])hougeki.api_df_list[i] ).Length;
					for ( int j = 0; j < lenj; j++ ) {
						damageList[(int)hougeki.api_df_list[i][j] - 1] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						DealDamageFriend( j, damageList[j] );
						DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}

			return hp;

		}

		public override string APIName {
			get { return "api_req_practice/midnight_battle"; }
		}


		public override int FleetIDFriend {
			get { return (int)RawData.api_deck_id; }
		}
	}
}
