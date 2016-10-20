using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 雷撃戦フェーズの処理を行います。
	/// </summary>
	public class PhaseTorpedo : PhaseBase {

		/// <summary>
		/// フェーズID 0=開幕雷撃, 1-4=雷撃戦
		/// </summary>
		private readonly int phaseID;

		public PhaseTorpedo( BattleData data, int phaseID )
			: base( data ) {

			this.phaseID = phaseID;

			if ( !IsAvailable )
				return;


			Damages = GetConcatArray( "api_fdam", "api_edam" );
			AttackDamages = GetConcatArray( "api_fydam", "api_eydam" );
			Targets = GetConcatArray( "api_frai", "api_erai" );
			CriticalFlags = GetConcatArray( "api_fcl", "api_ecl" );

		}


		public override bool IsAvailable {
			get {

				if ( phaseID == 0 ) {
					return RawData.api_opening_flag() ? (int)RawData.api_opening_flag != 0 : false;

				} else {
					return (int)RawData.api_hourai_flag[phaseID - 1] != 0;
				}
			}
		}


		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;

			for ( int i = 0; i < hps.Length; i++ ) {
				AddDamage( hps, i, Damages[i] );
			}

			for ( int i = 0; i < damages.Length; i++ ) {
				damages[i] += AttackDamages[i];
			}

			for ( int i = 0; i < Targets.Length; i++ ) {
				if ( Targets[i] > 0 ) {
					int target = Targets[i] - 1;
					if ( target >= 6 )
						target += 6;
					if ( PhaseBase.IsIndexFriend( i ) )
						target += 6;

					BattleDetails.Add( new BattleDayDetail( _battleData, i, target, new int[] { AttackDamages[i] }, new int[] { CriticalFlags[i] }, -1 ) );
				}
			}
		}


		public dynamic TorpedoData {
			get { return phaseID == 0 ? RawData.api_opening_atack : RawData.api_raigeki; }
		}


		/// <summary>
		/// 各艦の被ダメージ
		/// </summary>
		public int[] Damages { get; private set; }

		/// <summary>
		/// 各艦の与ダメージ
		/// </summary>
		public int[] AttackDamages { get; private set; }

		/// <summary>
		/// 各艦のターゲットインデックス
		/// </summary>
		public int[] Targets { get; private set; }

		/// <summary>
		/// クリティカルフラグ(攻撃側)
		/// </summary>
		public int[] CriticalFlags { get; private set; }


		private int[] GetConcatArray( string friendName, string enemyName ) {
			var friend = ( (int[])TorpedoData[friendName] ).Skip( 1 );
			var enemy = ( (int[])TorpedoData[enemyName] ).Skip( 1 );

			// 敵連合艦隊
			if ( friend.Count() == 12 && enemy.Count() == 12 ) {
				return friend.Take( 6 )
					.Concat( enemy.Take( 6 ) )
					.Concat( friend.Skip( 6 ) )
					.Concat( enemy.Skip( 6 ) ).ToArray();

			} else {
				if ( IsCombined ) {
					return Enumerable.Repeat( 0, 6 )
						.Concat( enemy )
						.Concat( friend )
						.Concat( Enumerable.Repeat( 0, 6 ) )
						.ToArray();

				} else {
					return friend
						.Concat( enemy )
						.Concat( Enumerable.Repeat( 0, 12 ) )
						.ToArray();
				}
			}
		}
	}
}
