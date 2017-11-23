using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
	/// <summary>
	/// 通常/連合艦隊 vs 通常艦隊　夜昼戦
	/// </summary>
	public class BattleDayFromNight : BattleDay
	{

		public PhaseSupport NightSupport { get; protected set; }
		public PhaseNightBattle NightBattle1 { get; protected set; }
		public PhaseNightBattle NightBattle2 { get; protected set; }

		public bool NextToDay => (int)RawData.api_day_flag != 0;


		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightSupport = new PhaseSupport(this, "夜間支援", true);
			NightBattle1 = new PhaseNightBattle(this, "第一次夜戦", 1, false);
			NightBattle2 = new PhaseNightBattle(this, "第二次夜戦", 2, false);


			if (NextToDay)
			{
				JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "噴式基地航空隊攻撃");
				JetAirBattle = new PhaseJetAirBattle(this, "噴式航空戦");
				BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空隊攻撃");
				Support = new PhaseSupport(this, "支援攻撃");
				AirBattle = new PhaseAirBattle(this, "航空戦");
				OpeningASW = new PhaseOpeningASW(this, "先制対潜");
				OpeningTorpedo = new PhaseTorpedo(this, "先制雷撃", 0);
				Shelling1 = new PhaseShelling(this, "砲撃戦", 1, "1");
				// 砲撃戦2?
				Torpedo = new PhaseTorpedo(this, "雷撃戦", 2);
			}

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}

		public override string APIName => "api_req_sortie/night_to_day";

		public override string BattleName => "対通常艦隊　夜昼戦";



		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightSupport;
			yield return NightBattle1;
			yield return NightBattle2;

			if (NextToDay)
			{
				yield return JetBaseAirAttack;
				yield return JetAirBattle;
				yield return BaseAirAttack;
				yield return AirBattle;
				yield return Support;
				yield return OpeningASW;
				yield return OpeningTorpedo;
				yield return Shelling1;
				yield return Torpedo;
			}
		}
	}
}
