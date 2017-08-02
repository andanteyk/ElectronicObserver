using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 噴式強襲航空攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseJetAirBattle : PhaseAirBattleBase {

		public PhaseJetAirBattle( BattleData data, string title )
			: base( data, title ) {

			AirBattleData = RawData.api_injection_kouku() ? RawData.api_injection_kouku : null;
			if ( AirBattleData != null ) {
				StageFlag = new int[] { 
					AirBattleData.api_stage1() ? 1 : 0,
					AirBattleData.api_stage2() ? 1 : 0,
					AirBattleData.api_stage3() ? 1 : 0,
				};
			}

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
						case 57:	// 噴式戦闘爆撃機
							firepower[i] += (int)( 1.0 * ( slots[s].Bomber * Math.Sqrt( aircrafts[s] ) + 25 ) );
							break;

						case 58:	// 噴式攻撃機 (80%と150%はランダムのため係数は平均値)
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
