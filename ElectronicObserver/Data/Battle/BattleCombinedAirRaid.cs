using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊長距離空襲戦
	/// </summary>
	public class BattleCombinedAirRaid : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			BaseAirAttack = new PhaseBaseAirAttack( this );
			AirBattle = new PhaseAirBattle( this );
			// 支援はないものとする

			BaseAirAttack.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/ld_airbattle"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}
	}
}
