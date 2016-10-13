using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 敵連合艦隊昼戦
	/// </summary>
	public class BattleEnemyCombinedDay : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			BaseAirAttack = new PhaseBaseAirAttack( this );
			AirBattle = new PhaseAirBattle( this );
			Support = new PhaseSupport( this );
			OpeningASW = new PhaseOpeningASW( this, false );
			OpeningTorpedo = new PhaseTorpedo( this, 0 );
			Shelling1 = new PhaseShelling( this, 1, "1", false, true );
			Torpedo = new PhaseTorpedo( this, 2 );
			Shelling2 = new PhaseShelling( this, 3, "2", false, false );
			Shelling3 = new PhaseShelling( this, 4, "3", false, false );


			BaseAirAttack.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
			Support.EmulateBattle( _resultHPs, _attackDamages );
			OpeningASW.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );
			
		}


		public override string APIName {
			get { return "api_req_combined_battle/ec_battle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.EnemyCombined; }
		}

		public override string GetBattleDetail( int index ) {
			var sb = new StringBuilder();

			string baseair = BaseAirAttack.GetBattleDetail( index );
			string airbattle = AirBattle.GetBattleDetail( index );
			string support = Support.GetBattleDetail( index );
			string asw = OpeningASW.GetBattleDetail( index );
			string openingTorpedo = OpeningTorpedo.GetBattleDetail( index );
			string shelling1 = Shelling1.GetBattleDetail( index );
			string torpedo = Torpedo.GetBattleDetail( index );
			string shelling2 = Shelling2.GetBattleDetail( index );
			string shelling3 = Shelling3.GetBattleDetail( index );

			if ( baseair != null )
				sb.AppendLine( "《基地航空隊攻撃》" ).Append( baseair );
			if ( airbattle != null )
				sb.AppendLine( "《航空戦》" ).Append( airbattle );
			if ( support != null )
				sb.AppendLine( "《支援攻撃》" ).Append( support );
			if ( asw != null )
				sb.AppendLine( "《開幕対潜》" ).Append( asw );
			if ( openingTorpedo != null )
				sb.AppendLine( "《開幕雷撃》" ).Append( openingTorpedo );
			if ( shelling1 != null )
				sb.AppendLine( "《第一次砲撃戦》" ).Append( shelling1 );
			if ( torpedo != null )
				sb.AppendLine( "《雷撃戦》" ).Append( torpedo );
			if ( shelling2 != null )
				sb.AppendLine( "《第二次砲撃戦》" ).Append( shelling2 );
			if ( shelling3 != null )
				sb.AppendLine( "《第三次砲撃戦》" ).Append( shelling3 );

			return sb.ToString();
		}
	}
}
