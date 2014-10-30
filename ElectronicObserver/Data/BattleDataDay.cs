using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	//undone
	/// <summary>
	/// 昼戦の戦闘詳報を保持します。
	/// </summary>
	[Obsolete]
	public class BattleDataDay : APIWrapper {

		/// <summary>
		/// 交戦中の艦隊ID
		/// </summary>
		public int FleetID {
			get { return (int)RawData.api_dock_id; }
		}

		/// <summary>
		/// 敵艦隊所属艦ID
		/// todo: 先頭のダミー(-1)を除外できるようにする
		/// </summary>
		public ReadOnlyCollection<int> EnemyMemberID {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_ship_ke ); }
		}

		/// <summary>
		/// 敵艦隊所属艦のレベル
		/// </summary>
		public ReadOnlyCollection<int> EnemyMemberLevel {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_ship_lv ); }
		}


		public ReadOnlyCollection<int> ShipHPCurrent {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_nowhps ); }
		}

		public ReadOnlyCollection<int> ShipHPMax {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_nowhps ); }
		}

		//api_midnight_flag

		//api_eSlot

		//api_eKyouka

		//api_fParam

		//api_eParam

		/// <summary>
		/// 索敵成否(味方)
		/// </summary>
		public int SearchingResultFriend {
			get { return (int)RawData.api_search[0]; }
		}

		/// <summary>
		/// 索敵成否(敵)
		/// </summary>
		public int SearchingResultEnemy {
			get { return (int)RawData.api_search[1]; }
		}

		/// <summary>
		/// 陣形(味方)
		/// </summary>
		public int FormationFriend {
			get { return (int)RawData.api_formation[0]; }
		}

		/// <summary>
		/// 陣形(敵)
		/// </summary>
		public int FormationEnemy {
			get { return (int)RawData.api_formation[1]; }
		}

		/// <summary>
		/// 交戦形態
		/// </summary>
		public int EngagementForm {
			get { return (int)RawData.api_formation[2]; }
		}


		//これ以外の戦闘データはプロパティ化が困難なうえメリットがなさそうなので逃げる

		
		//カオス極まりないのでちゃんと考えて
		public List<int> EmulateBattle() {

			var HPFriend = new List<int>();
			var HPEnemy = new List<int>();

			
			Func<int, int, int> DealDamageFriend = ( int index, int damage ) => {
				int hp = HPFriend[index];
				hp = Math.Min( hp - damage, 0 );

				//undone; ダメコンが未考慮
				//ダメコン処理：
				//要員/女神にかかわらず、スロットの上にあるほうが優先して使われる
				//要員の回復量はint(最大HP*20%)
				//戦闘終了を待たず、使用直後に消滅する
				//演習時は処理しないよう注意すること

				return hp;
			};

			Func<int, int, int> DealDamageEnemy = ( int index, int damage ) => {
				return Math.Min( HPEnemy[index] - damage, 0 );
			};



			//init phase

			for ( int i = 0; i < 6; i++ )
				HPFriend.Add( (int)RawData.api_nowhps[i + 1] );
			for ( int i = 0; i < 6; i++ )
				HPEnemy.Add( (int)RawData.api_nowhps[i + 7] );


			//aerial combat phase
			if ( (int)RawData.api_stage_flag[2] != 0 ) {
				dynamic kouku = RawData.api_kouku;

				for ( int i = 0; i < 6; i++ )
					HPFriend[i] = DealDamageFriend( i, (int)kouku.api_stage3.api_fdam[i + 1] );
				for ( int i = 0; i < 6; i++ )
					HPEnemy[i] = DealDamageEnemy( i, (int)kouku.api_stage3.api_edam[i + 1] );
			}

			
			//support attack phase - air support
			if ( (int)RawData.api_support_flag == 1 ) {
				dynamic kouku = RawData.api_support_info.api_support_airattack;

				for ( int i = 0; i < 6; i++ )
					HPEnemy[i] = DealDamageEnemy( i, (int)kouku.api_stage3.api_edam[i + 1] );
			}
			//support attack phase - shelling | torpedo support
			if ( (int)RawData.api_support_flag == 2 || (int)RawData.api_support_flag == 3 ) {
				for ( int i = 0; i < 6; i++ )
					HPEnemy[i] = DealDamageEnemy( i, (int)RawData.api_support_info.api_support_hourai.api_damage[i + 1] );
			}


			//opening torpedo phase
			if ( (int)RawData.api_opening_flag != 0 ) {
				for ( int i = 0; i < 6; i++ )
					HPFriend[i] = DealDamageFriend( i, (int)RawData.api_opening_atack.api_fdam[i + 1] );
				for ( int i = 0; i < 6; i++ )
					HPEnemy[i] = DealDamageEnemy( i, (int)RawData.api_opening_atack.api_edam[i + 1] );
			}


			//shelling phase #1
			if ( (int)RawData.api_hourai_flag[0] != 0 ) {

				dynamic hougeki = RawData.api_hougeki1;
				var damageList = new List<int>();			//連撃で撃沈されたときにきょどらないようにするため、1行動におけるダメージ判定は最後に行う

				for ( int i = 0; i < 12; i++ ) {
					damageList.Add( 0 );
				}


				for ( int i = 1; i < ( (int[])hougeki.api_at_list ).Length; i++ ) {

					for ( int j = 0; j < 12; j++ )
						damageList[j] = 0;


					for ( int j = 1; j < ( (int[])hougeki.api_df_list ).Length; j++ ) {
						int defender = (int)hougeki.api_df_list[i][j] - 1;

						damageList[defender] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						HPFriend[j] = DealDamageFriend( j, damageList[j] );
						HPEnemy[j] = DealDamageEnemy( j, damageList[j + 6] );
					}
				}

			}


			//shelling phase #2 - copipe
			if ( (int)RawData.api_hourai_flag[1] != 0 ) {

				dynamic hougeki = RawData.api_hougeki2;
				var damageList = new List<int>();			//連撃で撃沈されたときにきょどらないようにするため

				for ( int i = 0; i < 12; i++ ) {
					damageList.Add( 0 );
				}


				for ( int i = 1; i < ( (int[])hougeki.api_at_list ).Length; i++ ) {

					for ( int j = 0; j < 12; j++ )
						damageList[j] = 0;


					for ( int j = 1; j < ( (int[])hougeki.api_df_list ).Length; j++ ) {
						int defender = (int)hougeki.api_df_list[i][j] - 1;

						damageList[defender] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						HPFriend[j] = DealDamageFriend( j, damageList[j] );
						HPEnemy[j] = DealDamageEnemy( j, damageList[j + 6] );
					}
				}

			}


			//shelling phase #3 - copipe
			if ( (int)RawData.api_hourai_flag[2] != 0 ) {

				dynamic hougeki = RawData.api_hougeki3;
				var damageList = new List<int>();			//連撃で撃沈されたときにきょどらないようにするため

				for ( int i = 0; i < 12; i++ ) {
					damageList.Add( 0 );
				}


				for ( int i = 1; i < ( (int[])hougeki.api_at_list ).Length; i++ ) {

					for ( int j = 0; j < 12; j++ )
						damageList[j] = 0;


					for ( int j = 1; j < ( (int[])hougeki.api_df_list ).Length; j++ ) {
						int defender = (int)hougeki.api_df_list[i][j] - 1;

						damageList[defender] += (int)hougeki.api_damage[i][j];
					}

					for ( int j = 0; j < 6; j++ ) {
						HPFriend[j] = DealDamageFriend( j, damageList[j] );
						HPEnemy[j] = DealDamageEnemy( j, damageList[j + 6] );
					}
				}
			}


			//torpedo phase
			if ( (int)RawData.api_hourai_flag[3] != 0 ) {
				for ( int i = 0; i < 6; i++ )
					HPFriend[i] = DealDamageFriend( i, (int)RawData.api_raigeki.api_fdam[i + 1] );
				for ( int i = 0; i < 6; i++ )
					HPEnemy[i] = DealDamageEnemy( i, (int)RawData.api_raigeki.api_edam[i + 1] );
			}


			return null;
		}
	}

}
