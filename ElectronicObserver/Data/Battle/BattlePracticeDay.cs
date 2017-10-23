using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 演習昼戦
	/// </summary>
	public class BattlePracticeDay : BattleDay
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			JetAirBattle = new PhaseJetAirBattle(this, "噴式航空戦");
			AirBattle = new PhaseAirBattle(this, "航空戦");
			OpeningASW = new PhaseOpeningASW(this, "先制対潜", false);
			OpeningTorpedo = new PhaseTorpedo(this, "先制雷撃", 0);
			Shelling1 = new PhaseShelling(this, "第一次砲撃戦", 1, "1", false);
			Shelling2 = new PhaseShelling(this, "第二次砲撃戦", 2, "2", false);
			Shelling3 = new PhaseShelling(this, "第三次砲撃戦", 3, "3", false);
			Torpedo = new PhaseTorpedo(this, "雷撃戦", 4);


			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);

		}


		public override string APIName
		{
			get { return "api_req_practice/battle"; }
		}

		public override string BattleName
		{
			get { return "演習 昼戦"; }
		}

		public override BattleTypeFlag BattleType
		{
			get { return BattleTypeFlag.Day | BattleTypeFlag.Practice; }
		}


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return JetAirBattle;
			yield return AirBattle;
			yield return OpeningASW;
			yield return OpeningTorpedo;
			yield return Shelling1;
			yield return Shelling2;
			yield return Shelling3;
			yield return Torpedo;
		}
	}

}
