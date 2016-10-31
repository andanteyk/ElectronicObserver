﻿using ElectronicObserver.Data.Battle.Phase;
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

			BaseAirAttack = new PhaseBaseAirAttack( this );
			AirBattle = new PhaseAirBattle( this );
			// 支援は出ないものとする

			BaseAirAttack.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
		}

		public override string APIName {
			get { return "api_req_sortie/ld_airbattle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day; }
		}

		public override string GetBattleDetail( int index ) {
			var sb = new StringBuilder();

			string baseair = BaseAirAttack.GetBattleDetail( index );
			string airbattle = AirBattle.GetBattleDetail( index );
			
			if ( baseair != null )
				sb.AppendLine( "《基地航空隊攻撃》" ).Append( baseair );
			if ( airbattle != null )
				sb.AppendLine( "《航空戦》" ).Append( airbattle );
			
			return sb.ToString();
		}

        public override string GetBattleDetail()
        {
            var sb = new StringBuilder();

            string baseair = BaseAirAttack.GetBattleDetail();
            string airbattle = AirBattle.GetBattleDetail();

            if (baseair != null)
                sb.AppendLine("《基地航空隊攻撃》").Append(baseair);
            if (airbattle != null)
                sb.AppendLine("《航空戦》").Append(airbattle);

            return sb.ToString();
        }
	}
}
