using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 航空戦フェーズの処理を行います。
	/// </summary>
	public class PhaseAirBattle : PhaseAirBattleBase {

		public PhaseAirBattle( BattleData data, string title, string suffix = "" )
			: base( data, title ) {

			AirBattleData = RawData.IsDefined( "api_kouku" + suffix ) ? RawData["api_kouku" + suffix] : null;
			StageFlag = RawData.IsDefined( "api_stage_flag" + suffix ) ? (int[])RawData["api_stage_flag" + suffix] : null;

			LaunchedShipIndexFriend = GetLaunchedShipIndex( 0 );
			LaunchedShipIndexEnemy = GetLaunchedShipIndex( 1 );

			TorpedoFlags = ConcatStage3Array<int>( "api_frai_flag", "api_erai_flag" );
			BomberFlags = ConcatStage3Array<int>( "api_fbak_flag", "api_ebak_flag" );
			Criticals = ConcatStage3Array<int>( "api_fcl_flag", "api_ecl_flag" );
			Damages = ConcatStage3Array<double>( "api_fdam", "api_edam" );
		}


		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;

			CalculateAttack( 0, hps );
			CalculateAttackDamage( damages );
		}



		/// <summary>
		/// 航空戦での与ダメージを推測します。
		/// </summary>
		/// <param name="damages">与ダメージリスト。</param>
		private void CalculateAttackDamage( int[] damages ) {
			// 敵はめんどくさすぎるので省略
			// 仮想火力を求め、それに従って合計ダメージを分配

			var firepower = new int[12];
			var launchedIndex = LaunchedShipIndexFriend;
			var members = _battleData.Initial.FriendFleet.MembersWithoutEscaped;

			foreach ( int i in launchedIndex ) {

				ShipData ship;
				if ( i < 6 )
					ship = _battleData.Initial.FriendFleet.MembersWithoutEscaped[i];
				else
					ship = _battleData.Initial.FriendFleetEscort.MembersWithoutEscaped[i - 6];

				if ( ship == null )
					continue;

				var slots = ship.SlotInstanceMaster;
				var aircrafts = ship.Aircraft;
				for ( int s = 0; s < slots.Count; s++ ) {

					if ( slots[s] == null )
						continue;

					switch ( slots[s].CategoryType ) {
						case 7:		// 艦上爆撃機
						case 11:	// 水上爆撃機
						case 57:	// 噴式戦闘爆撃機
							firepower[i] += (int)( 1.0 * ( slots[s].Bomber * Math.Sqrt( aircrafts[s] ) + 25 ) );
							break;

						case 8:		// 艦上攻撃機 (80%と150%はランダムのため係数は平均値)
						case 58:	// 噴式攻撃機
							firepower[i] += (int)( 1.15 * ( slots[s].Torpedo * Math.Sqrt( aircrafts[s] ) + 25 ) );
							break;
					}
				}
			}

			int totalFirepower = firepower.Sum();
			int totalDamage = Damages.Select( dmg => (int)dmg ).Skip( 6 ).Take( 6 ).Sum() + damages.Skip( 18 ).Take( 6 ).Sum();

			for ( int i = 0; i < firepower.Length; i++ ) {
				damages[( i < 6 ? i : ( i + 6 ) )] += (int)Math.Round( (double)totalDamage * firepower[i] / Math.Max( totalFirepower, 1 ) );
			}
		}

		protected override IEnumerable<BattleDetail> SearchBattleDetails( int index ) {
			return BattleDetails.Where( d => d.DefenderIndex == index );
		}

	}
}
