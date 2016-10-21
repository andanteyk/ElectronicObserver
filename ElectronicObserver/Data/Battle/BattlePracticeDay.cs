using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 演習昼戦
	/// </summary>
	public class BattlePracticeDay : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			AirBattle = new PhaseAirBattle( this );
			OpeningASW = new PhaseOpeningASW( this, false );
			OpeningTorpedo = new PhaseTorpedo( this, 0 );
			Shelling1 = new PhaseShelling( this, 1, "1", false );
			Shelling2 = new PhaseShelling( this, 2, "2", false );
			Shelling3 = new PhaseShelling( this, 3, "3", false );
			Torpedo = new PhaseTorpedo( this, 4 );


			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
			OpeningASW.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_practice/battle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Practice; }
		}

		public override string GetBattleDetail( int index ) {
			var sb = new StringBuilder();

			string airbattle = AirBattle.GetBattleDetail( index );
			string asw = OpeningASW.GetBattleDetail( index );
			string openingTorpedo = OpeningTorpedo.GetBattleDetail( index );
			string shelling1 = Shelling1.GetBattleDetail( index );
			string shelling2 = Shelling2.GetBattleDetail( index );
			string shelling3 = Shelling3.GetBattleDetail( index );
			string torpedo = Torpedo.GetBattleDetail( index );

			if ( airbattle != null )
				sb.AppendLine( "《航空戦》" ).Append( airbattle );
			if ( asw != null )
				sb.AppendLine( "《開幕対潜》" ).Append( asw );
			if ( openingTorpedo != null )
				sb.AppendLine( "《開幕雷撃》" ).Append( openingTorpedo );
			if ( shelling1 != null )
				sb.AppendLine( "《第一次砲撃戦》" ).Append( shelling1 );
			if ( shelling2 != null )
				sb.AppendLine( "《第二次砲撃戦》" ).Append( shelling2 );
			if ( shelling3 != null )
				sb.AppendLine( "《第三次砲撃戦》" ).Append( shelling3 );
			if ( torpedo != null )
				sb.AppendLine( "《雷撃戦》" ).Append( torpedo );

			return sb.ToString();
		}
	}

}
