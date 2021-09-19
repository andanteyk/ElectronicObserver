using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常/連合艦隊 vs 通常艦隊 開幕夜戦
	/// </summary>
	public class BattleNightOnly : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightInitial = new PhaseNightInitial(this, "夜戦開始", false);
			FriendlySupportInfo = new PhaseFriendlySupportInfo(this, "友軍艦隊");
			FriendlyShelling = new PhaseFriendlyShelling(this, "友軍艦隊援護");
			Support = new PhaseSupport(this, "夜間支援攻撃", true);
			NightBattle = new PhaseNightBattle(this, "夜戦", 0);
			

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}


		public override string APIName => "api_req_battle_midnight/sp_midnight";

		public override string BattleName => "通常艦隊 開幕夜戦";

	

		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return NightInitial;
			yield return FriendlySupportInfo;
			yield return FriendlyShelling;
			yield return Support;
			yield return NightBattle;
		}
	}

}
