﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 通常艦隊夜戦
	/// </summary>
	public class BattleNormalNight : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			NightBattle = new PhaseNightBattle( this, false );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_battle_midnight/battle"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night; }
		}

		public override string GetBattleDetail( int index ) {
			var sb = new StringBuilder();

			string night = NightBattle.GetBattleDetail( index );

			if ( night != null )
				sb.AppendLine( "《夜戦》" ).Append( night );

			return sb.ToString();
		}

        public override string GetBattleDetail()
        {
            var sb = new StringBuilder();

            string night = NightBattle.GetBattleDetail();

            if (night != null)
                sb.AppendLine("《夜戦》").Append(night);

            return sb.ToString();
        }
	}
}
