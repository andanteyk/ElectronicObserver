using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 連合艦隊 vs 通常艦隊 開幕夜戦
	/// </summary>
	public class BattleCombinedNightOnly : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			Support = new PhaseSupport(this, "支援攻撃", true);
			NightBattle = new PhaseNightBattle(this, "夜戦", 0, true);


			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}



		public override string APIName => "api_req_combined_battle/sp_midnight";

		public override string BattleName => "連合艦隊 開幕夜戦";


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return Support;
			yield return NightBattle;
		}
	}

}
