using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Battle.Phase;

namespace ElectronicObserver.Data.Battle
{
	/// <summary>
	/// 連合艦隊 vs 通常艦隊 レーダー射撃
	/// </summary>
	public class BattleCombinedRadar : BattleDay
	{
		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			Shelling1 = new PhaseRadar(this, "レーダー射撃");

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}

		public override string APIName => "api_req_combined_battle/ld_shooting";

		public override string BattleName => "連合艦隊 レーダー射撃";

		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;     // ?
			yield return Shelling1;
		}
	}
}
