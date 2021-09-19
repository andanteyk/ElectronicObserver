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
	/// 連合艦隊 vs 通常艦隊 夜戦
	/// </summary>
	public class BattleCombinedNormalNight : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightInitial = new PhaseNightInitial(this, "夜戦開始", true);
			FriendlySupportInfo = new PhaseFriendlySupportInfo(this, "友軍艦隊");
			FriendlyShelling = new PhaseFriendlyShelling(this, "友軍艦隊援護");
			// 支援なし?
			NightBattle = new PhaseNightBattle(this, "夜戦", 0);


			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}


		public override string APIName => "api_req_combined_battle/midnight_battle";

		public override string BattleName => "連合艦隊 夜戦";


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightInitial;
			yield return FriendlySupportInfo;
			yield return FriendlyShelling;
			yield return NightBattle;
		}
	}

}
