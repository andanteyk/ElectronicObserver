using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 基地空襲戦フェーズ
	/// </summary>
	public class PhaseBaseAirRaid : PhaseAirBattle {

		public PhaseBaseAirRaid( BattleData data, string title )
			: base( data, title, "_it_will_never_be_processed_" ) {

			AirBattleData = data.RawData.api_air_base_attack;
			StageFlag = AirBattleData.api_stage_flag() ? (int[])AirBattleData.api_stage_flag : null;

			TorpedoFlags = ConcatStage3Array( "api_frai_flag", "api_erai_flag" );
			BomberFlags = ConcatStage3Array( "api_fbak_flag", "api_ebak_flag" );
			Criticals = ConcatStage3Array( "api_fcl_flag", "api_ecl_flag" );
			Damages = ConcatStage3Array( "api_fdam", "api_edam" );
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;

			CalculateAttack( -1, hps, damages );
		}

	}
}
