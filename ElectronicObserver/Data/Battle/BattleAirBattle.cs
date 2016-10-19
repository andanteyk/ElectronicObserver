using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 通常艦隊航空戦
	/// </summary>
	public class BattleAirBattle : BattleDay {

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
			AirBattle2.EmulateBattle( _resultHPs, _attackDamages );

		}

		public override string APIName {
			get { return "api_req_sortie/airbattle"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day; }
		}


		public override string GetBattleDetail( int index ) {
			var sb = new StringBuilder();

			string baseair = BaseAirAttack.GetBattleDetail( index );
			string airbattle1 = AirBattle.GetBattleDetail( index );
			string support = Support.GetBattleDetail( index );
			string airbattle2 = AirBattle2.GetBattleDetail( index );

			if ( baseair != null )
				sb.AppendLine( "《基地航空隊攻撃》" ).Append( baseair );
			if ( airbattle1 != null )
				sb.AppendLine( "《第一次航空戦》" ).Append( airbattle1 );
			if ( support != null )
				sb.AppendLine( "《支援攻撃》" ).Append( support );
			if ( airbattle2 != null )
				sb.AppendLine( "《第二次航空戦》" ).Append( airbattle2 );

			return sb.ToString();
		}
	}
}
