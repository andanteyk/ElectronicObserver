using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 演習夜戦
	/// </summary>
	public class BattlePracticeNight : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightBattle = new PhaseNightBattle(this, "夜戦", false);

			NightBattle.EmulateBattle(_resultHPs, _attackDamages);

		}


		public override string APIName => "api_req_practice/midnight_battle";

		public override string BattleName => "演習 夜戦";

		public override BattleTypeFlag BattleType => BattleTypeFlag.Night | BattleTypeFlag.Practice;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightBattle;
		}
	}
}
