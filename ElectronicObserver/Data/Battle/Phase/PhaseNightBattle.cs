using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 夜戦フェーズの処理を行います。
	/// </summary>
	public class PhaseNightBattle : PhaseBase {

		private readonly bool isEscort;

		public PhaseNightBattle( BattleData data, bool isEscort )
			: base( data ) {

			this.isEscort = isEscort;
		}

		public override bool IsAvailable {
			get { return true; }
		}

		public dynamic ShellingData { get { return RawData.api_hougeki; } }

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;

			int[] attackers = (int[])ShellingData.api_at_list;

			for ( int i = 1; i < attackers.Length; i++ ) {		//skip header(-1)

				int[] tempDamages = Enumerable.Repeat( 0, hps.Length ).ToArray();

				int[] defenders = (int[])( ShellingData.api_df_list[i] );
				int[] unitDamages = (int[])( ShellingData.api_damage[i] );

				for ( int j = 0; j < defenders.Length; j++ ) {
					if ( defenders[j] != -1 ) {
						tempDamages[GetIndex( defenders[j] )] += Math.Max( unitDamages[j], 0 );
					}
				}

				for ( int j = 0; j < tempDamages.Length; j++ )
					AddDamage( hps, j, tempDamages[j] );


				BattleDetails.Add( new BattleNightDetail( _battleData, attackers[i] + ( isEscort && attackers[i] <= 6 ? 12 : 0 ), defenders.LastOrDefault( x => x != -1 ) + ( isEscort && defenders.LastOrDefault( x => x != -1 ) <= 6 ? 12 : 0 ), unitDamages, (int[])ShellingData.api_cl_list[i], (int)ShellingData.api_sp_list[i] ) );

				damages[GetIndex( attackers[i] )] += tempDamages.Sum();
			}

		}


		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet { get { return KCDatabase.Instance.Fleet[isEscort ? 2 : _battleData.Initial.FriendFleetID]; } }


		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend { get { return (int)RawData.api_touch_plane[0]; } }

		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy { get { return (int)RawData.api_touch_plane[1]; } }

		/// <summary>
		/// 自軍照明弾投射艦番号
		/// </summary>
		public int FlareIndexFriend {
			get {
				int index = (int)RawData.api_flare_pos[0];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexEnemy {
			get {
				int index = (int)RawData.api_flare_pos[1];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦
		/// </summary>
		public ShipDataMaster FlareEnemyInstance {
			get {
				int index = FlareIndexEnemy;
				return index == -1 ? null : _battleData.Initial.EnemyMembersInstance[index];
			}
		}

		/// <summary>
		/// 自軍探照灯照射艦番号
		/// </summary>
		public int SearchlightIndexFriend {
			get {
				var ships = KCDatabase.Instance.Fleet[isEscort ? 2 : _battleData.Initial.FriendFleetID].MembersWithoutEscaped;
				int index = -1;

				for ( int i = 0; i < ships.Count; i++ ) {

					var ship = ships[i];
					if ( ship != null && _battleData.Initial.InitialHPs[( isEscort ? 12 : 0 ) + i] > 1 ) {

						if ( ship.SlotInstanceMaster.Count( e => e != null && e.CategoryType == 42 ) > 0 )		//大型探照灯
							return i;
						else if ( ship.SlotInstanceMaster.Count( e => e != null && e.CategoryType == 29 ) > 0 && index == -1 )		//探照灯
							index = i;
					}
				}

				return index;
			}
		}

		/// <summary>
		/// 敵軍探照灯照射艦番号(0-5)
		/// </summary>
		public int SearchlightIndexEnemy {
			get {
				var ships = _battleData.Initial.EnemyMembersInstance;
				var eqs = _battleData.Initial.EnemySlotsInstance;
				int index = -1;

				for ( int i = 0; i < ships.Length; i++ ) {

					if ( ships[i] != null && _battleData.Initial.InitialHPs[6 + i] > 1 ) {

						if ( eqs[i].Count( e => e != null && e.CategoryType == 42 ) > 0 )		//大型探照灯
							return i;
						else if ( eqs[i].Count( e => e != null && e.CategoryType == 29 ) > 0 && index == -1 )		//探照灯
							index = i;

					}
				}

				return index;
			}
		}

		public ShipDataMaster SearchlightEnemyInstance {
			get {
				int index = SearchlightIndexEnemy;
				return index == -1 ? null : _battleData.Initial.EnemyMembersInstance[index];
			}
		}


		private int GetIndex( int index ) {
			if ( isEscort && index <= 6 )
				return 12 + index - 1;
			return index - 1;
		}
	}
}
