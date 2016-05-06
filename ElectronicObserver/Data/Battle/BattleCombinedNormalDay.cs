using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊(機動部隊)昼戦
	/// </summary>
	public class BattleCombinedNormalDay : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

            AirBaseAttack = new PhaseAirBaseAttack(this);
            AirBattle = new PhaseAirBattle( this );
			Support = new PhaseSupport( this );
			OpeningTorpedo = new PhaseTorpedo( this, 0 );
			Shelling1 = new PhaseShelling( this, 1, "1", true );
			Torpedo = new PhaseTorpedo( this, 2 );
			Shelling2 = new PhaseShelling( this, 3, "2", false );
			Shelling3 = new PhaseShelling( this, 4, "3", false );

            AirBaseAttack.EmulateBattle(_resultHPs, _attackAirBaseDamages);
            AirBattle.EmulateBattle( _resultHPs, _attackAirDamages );
			Support.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/battle"; }
		}


		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}

	}

}
