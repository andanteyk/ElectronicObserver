using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public class BattleNightOnly : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			Searching = new PhaseSearching( this );
			NightBattle = new PhaseNightBattle( data, true );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_battle_midnight/sp_midnight"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night; }
		}

	}

}
