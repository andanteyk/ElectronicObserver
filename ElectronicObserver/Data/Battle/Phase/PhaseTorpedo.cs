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
			{
				int[] dmg = Damages;
				for ( int i = 0; i < hps.Length; i++ ) {
					AddDamage( hps, i, dmg[i] );
				}
			}
			{
				int[] dmg = AttackDamages;
				for ( int i = 0; i < damages.Length; i++ ) {
					damages[i] += dmg[i];
				}
			}
			{
				int[] friendTargets = (int[])TorpedoData.api_frai;
				int[] enemyTargets = (int[])TorpedoData.api_erai;
				int[] friendTargetDamages = (int[])TorpedoData.api_fydam;
				int[] enemyTargetDamages = (int[])TorpedoData.api_eydam;
				int[] friendCritical = (int[])TorpedoData.api_fcl;
				int[] enemyCritical = (int[])TorpedoData.api_ecl;

				bool isEscort = ( _battleData.BattleType & BattleData.BattleTypeFlag.Combined ) > 0;

				for ( int i = 1; i < friendTargets.Length; i++ ) {		//skip header(-1)
					if ( friendTargets[i] > 0 ) {
						BattleDetails.Add( new BattleDayDetail( _battleData, i + ( isEscort ? 12 : 0 ), friendTargets[i] + 6, new int[] { friendTargetDamages[i] }, new int[] { friendCritical[i] }, -1 ) );
					}
				}
				for ( int i = 1; i < enemyTargets.Length; i++ ) {
					if ( enemyTargets[i] > 0 ) {
						BattleDetails.Add( new BattleDayDetail( _battleData, i + 6, enemyTargets[i] + ( isEscort ? 12 : 0 ), new int[] { enemyTargetDamages[i] }, new int[] { enemyCritical[i] }, -1 ) );
					}
				}
			}
		}


		public dynamic TorpedoData {
			get { return phaseID == 0 ? RawData.api_opening_atack : RawData.api_raigeki; }
		}

		/// <summary>
		/// 各艦の被ダメージ
		/// </summary>
		public int[] Damages {
			get {
				if ( IsCombined ) {
					return Enumerable.Repeat( 0, 6 )
						.Concat( ( (int[])TorpedoData.api_edam ).Skip( 1 ) )
						.Concat( ( (int[])TorpedoData.api_fdam ).Skip( 1 ) ).ToArray();
				} else {
					return ( (int[])TorpedoData.api_fdam ).Skip( 1 )
						.Concat( ( (int[])TorpedoData.api_edam ).Skip( 1 ) ).ToArray();
				}
			}
		}

		/// <summary>
		/// 各艦の与ダメージ
		/// </summary>
		public int[] AttackDamages {
			get {
				if ( IsCombined ) {
					return Enumerable.Repeat( 0, 6 )
						.Concat( ( (int[])TorpedoData.api_eydam ).Skip( 1 ) )
						.Concat( ( (int[])TorpedoData.api_fydam ).Skip( 1 ) ).ToArray();
				} else {
					return ( (int[])TorpedoData.api_fydam ).Skip( 1 )
						.Concat( ( (int[])TorpedoData.api_eydam ).Skip( 1 ) ).ToArray();
				}
			}
		}
	}
}
