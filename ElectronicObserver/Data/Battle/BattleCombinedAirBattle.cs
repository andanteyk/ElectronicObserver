using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊航空戦
	/// </summary>
	public class BattleCombinedAirBattle : BattleDay {

		public PhaseAirBattle AirBattle2 { get; protected set; }

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			BaseAirAttack = new PhaseBaseAirAttack( this );
			AirBattle = new PhaseAirBattle( this );
			Support = new PhaseSupport( this );
			AirBattle2 = new PhaseAirBattle( this, "2" );

			BaseAirAttack.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
			Support.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle2.EmulateBattle( _resultHPs, _attackAirDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/airbattle"; }
		}


		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}

	}

}
