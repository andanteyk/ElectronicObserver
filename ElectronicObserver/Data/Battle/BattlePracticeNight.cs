using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public class BattlePracticeNight : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			NightBattle = new PhaseNightBattle( data, true );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_practice/midnight_battle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night | BattleTypeFlag.Practice; }
		}
	}
}
