using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	
	/// <summary>
	/// 基地空襲戦
	/// </summary>
	public class BattleBaseAirRaid : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			AirBattle = new PhaseBaseAirRaid( this, "空襲戦" );

			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
		}


		public override string APIName {
			get { return "api_req_map/next"; }
		}

		public override string BattleName {
			get { return "基地空襲戦"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.BaseAirRaid; }
		}

		public override IEnumerable<Phase.PhaseBase> GetPhases() {
			yield return Initial;
			yield return Searching;
			yield return AirBattle;
		}
	}
}
