using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊(水上部隊)抽選
	/// </summary>
	public class BattleCombinedWater : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

            AirBaseAttack = new PhaseAirBaseAttack(this);
            AirBattle = new PhaseAirBattle( this );
			Support = new PhaseSupport( this );
			OpeningTorpedo = new PhaseTorpedo( this, 0 );
			Shelling1 = new PhaseShelling( this, 1, "1", false );
			Shelling2 = new PhaseShelling( this, 2, "2", false );
			Shelling3 = new PhaseShelling( this, 3, "3", true );
			Torpedo = new PhaseTorpedo( this, 4 );

            AirBaseAttack.EmulateBattle(_resultHPs, _attackAirBaseDamages);
            AirBattle.EmulateBattle( _resultHPs, _attackAirDamages );
			Support.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );
			
		}


		public override string APIName {
			get { return "api_req_combined_battle/battle_water"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}

	}

}
