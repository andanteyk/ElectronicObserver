using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 砲撃戦フェーズの処理を行います。
	/// </summary>
	public class PhaseShelling : PhaseBase
	{

		protected readonly int PhaseID;
		protected readonly string Suffix;


		public List<PhaseShellingAttack> Attacks { get; private set; }


		public class PhaseShellingAttack
		{
			public BattleIndex Attacker;
			public int AttackType;
			public List<PhaseShellingDefender> Defenders;
			public int[] EquipmentIDs;

			public PhaseShellingAttack()
			{
				Defenders = new List<PhaseShellingDefender>();
			}

			public override string ToString() => $"{Attacker}[{AttackType}] -> [{string.Join(", ", Defenders)}]";

		}
		public class PhaseShellingDefender
		{
			public BattleIndex Defender;
			public int CriticalFlag;
			public double RawDamage;
			public bool GuardsFlagship => RawDamage != Math.Floor(RawDamage);
			public int Damage => (int)RawDamage;

			public override string ToString()
			{
				return string.Format("{0};{1}-{2}{3}", Defender, Damage,
					CriticalFlag == 0 ? "miss" : CriticalFlag == 1 ? "dmg" : CriticalFlag == 2 ? "crit" : "INVALID",
					GuardsFlagship ? " (guard)" : "");
			}
		}



		public PhaseShelling(BattleData data, string title, int phaseID, string suffix)
			: base(data, title)
		{

			PhaseID = phaseID;
			Suffix = suffix;

			if (!IsAvailable)
				return;

			// "translate"

			int[] fleetflag = (int[])ShellingData.api_at_eflag;
			int[] attackers = (int[])ShellingData.api_at_list;
			int[] attackTypes = (int[])ShellingData.api_at_type;
			int[][] defenders = ((dynamic[])ShellingData.api_df_list).Select(elem => (int[])elem).ToArray();
			int[][] attackEquipments = ((dynamic[])ShellingData.api_si_list).Select(elem => ((dynamic[])elem).Select<dynamic, int>(ch => ch is string ? int.Parse(ch) : (int)ch).ToArray()).ToArray();
			int[][] criticalFlags = ((dynamic[])ShellingData.api_cl_list).Select(elem => (int[])elem).ToArray();
			double[][] rawDamages = ((dynamic[])ShellingData.api_damage).Select(elem => ((double[])elem).Select(p => Math.Max(p, 0)).ToArray()).ToArray();

			Attacks = new List<PhaseShellingAttack>();

			for (int i = 0; i < attackers.Length; i++)
			{
				var attack = new PhaseShellingAttack()
				{
					Attacker = new BattleIndex(attackers[i] + (fleetflag[i] == 0 ? 0 : 12), Battle.IsFriendCombined, Battle.IsEnemyCombined),
				};


				for (int k = 0; k < defenders[i].Length; k++)
				{
					var defender = new PhaseShellingDefender
					{
						Defender = new BattleIndex(defenders[i][k] + (fleetflag[i] == 0 ? 12 : 0), Battle.IsFriendCombined, Battle.IsEnemyCombined),
						CriticalFlag = criticalFlags[i][k],
						RawDamage = rawDamages[i][k],
					};

					attack.Defenders.Add(defender);
				}

				attack.AttackType = attackTypes[i];
				attack.EquipmentIDs = attackEquipments[i];

				Attacks.Add(attack);
			}

		}


		public override bool IsAvailable => (int)RawData.api_hourai_flag[PhaseID - 1] != 0;


		public virtual dynamic ShellingData => RawData["api_hougeki" + Suffix];


		public override void EmulateBattle(int[] hps, int[] damages)
		{

			if (!IsAvailable)
				return;


			foreach (var attack in Attacks)
			{

				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleDayDetail(Battle, attack.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), attack.AttackType, attack.EquipmentIDs, hps[defs.Key]));
					AddDamage(hps, defs.Key, defs.Sum(d => d.Damage));
				}

				damages[attack.Attacker] += attack.Defenders.Sum(d => d.Damage);
			}

		}

	}
}
