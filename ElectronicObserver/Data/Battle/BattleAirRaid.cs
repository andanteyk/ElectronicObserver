using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 通常艦隊長距離空襲戦
	/// </summary>
	public class BattleAirRaid : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			AirBattle = new PhaseAirBattle( this );
			// 支援は出ないものとする

			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
		}

		public override string APIName {
			get { return "api_req_sortie/ld_airbattle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day; }
		}
	}
}
