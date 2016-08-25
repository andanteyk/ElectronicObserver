using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 砲撃戦フェーズの処理を行います。
	/// </summary>
	public class PhaseShelling : PhaseBase {

		protected readonly int phaseID;
		protected readonly string suffix;
		protected readonly bool isEscort;

		public PhaseShelling( BattleData data, int phaseID, string suffix, bool isEscort )
			: base( data ) {

			this.phaseID = phaseID;
			this.suffix = suffix;
			this.isEscort = isEscort;
		}


		public override bool IsAvailable {
			get { return (int)RawData.api_hourai_flag[phaseID - 1] != 0; }
		}


		public virtual dynamic ShellingData {
			get { return RawData["api_hougeki" + suffix]; }
		}


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


				BattleDetails.Add( new BattleDayDetail( _battleData, attackers[i] + ( isEscort && attackers[i] <= 6 ? 12 : 0 ), defenders.LastOrDefault( x => x != -1 ) + ( isEscort && defenders.LastOrDefault( x => x != -1 ) <= 6 ? 12 : 0 ), unitDamages, (int[])ShellingData.api_cl_list[i], (int)ShellingData.api_at_type[i] ) );

				damages[GetIndex( attackers[i] )] += tempDamages.Sum();
			}

		}

		private int GetIndex( int index ) {
			if ( isEscort && index <= 6 )
				return 12 + index - 1;
			return index - 1;
		}
	}
}
