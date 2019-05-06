using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 夜戦フェーズの処理を行います。
	/// </summary>
	public class PhaseNightBattle : PhaseBase
	{

		private readonly int PhaseID;


		public PhaseNightBattle(BattleData data, string title, int phaseID)
			: base(data, title)
		{

			PhaseID = phaseID;


			if (!IsAvailable)
				return;


			int[] fleetflag = (int[])ShellingData.api_at_eflag;
			int[] attackers = (int[])ShellingData.api_at_list;
			int[] nightAirAttackFlags = (int[])ShellingData.api_n_mother_list;
			int[] attackTypes = (int[])ShellingData.api_sp_list;
			int[][] defenders = ((dynamic[])ShellingData.api_df_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			int[][] attackEquipments = ((dynamic[])ShellingData.api_si_list).Select(elem => ((dynamic[])elem).Select<dynamic, int>(ch => ch is string ? int.Parse(ch) : (int)ch).ToArray()).ToArray();
			int[][] criticals = ((dynamic[])ShellingData.api_cl_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			double[][] rawDamages = ((dynamic[])ShellingData.api_damage).Select(elem => ((double[])elem).Where(e => e != -1).ToArray()).ToArray();

			Attacks = new List<PhaseNightBattleAttack>();



			for (int i = 0; i < attackers.Length; i++)
			{
				var attack = new PhaseNightBattleAttack
				{
					Attacker = new BattleIndex(attackers[i] + (fleetflag[i] == 0 ? 0 : 12), Battle.IsFriendCombined, Battle.IsEnemyCombined),
					NightAirAttackFlag = nightAirAttackFlags[i] == -1,
					AttackType = attackTypes[i],
					EquipmentIDs = attackEquipments[i],
				};
				for (int k = 0; k < defenders[i].Length; k++)
				{
					var defender = new PhaseNightBattleDefender
					{
						Defender = new BattleIndex(defenders[i][k] + (fleetflag[i] == 0 ? 12 : 0), Battle.IsFriendCombined, Battle.IsEnemyCombined),
						CriticalFlag = criticals[i][k],
						RawDamage = rawDamages[i][k]
					};
					attack.Defenders.Add(defender);
				}

				Attacks.Add(attack);
			}

		}

		public override bool IsAvailable =>
			RawData.IsDefined(ShellingDataName) &&
			RawData[ShellingDataName].api_at_list() &&
			RawData[ShellingDataName].api_at_list != null;


		public dynamic ShellingData => RawData[ShellingDataName];

		private string ShellingDataName => PhaseID == 0 ? "api_hougeki" : ("api_n_hougeki" + PhaseID);


		public override void EmulateBattle(int[] hps, int[] damages)
		{

			if (!IsAvailable) return;


			foreach (var atk in Attacks)
			{
				switch (atk.AttackType)
				{
					case 100:
						// nelson touch
						for (int i = 0; i < atk.Defenders.Count; i++)
						{
							var comboatk = new BattleIndex(atk.Attacker.Side, i * 2);       // #1, #3, #5
							BattleDetails.Add(new BattleNightDetail(Battle, comboatk, atk.Defenders[i].Defender, new[] { atk.Defenders[i].RawDamage }, new[] { atk.Defenders[i].CriticalFlag }, atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[atk.Defenders[i].Defender]));
							AddDamage(hps, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
							damages[comboatk] += atk.Defenders[i].Damage;
						}
						break;

					case 101:
                    case 102:
						// nagato/mutsu touch
						for (int i = 0; i < atk.Defenders.Count; i++)
						{
							var comboatk = new BattleIndex(atk.Attacker.Side, i / 2);       // #1, #1, #2
							BattleDetails.Add(new BattleNightDetail(Battle, comboatk, atk.Defenders[i].Defender, new[] { atk.Defenders[i].RawDamage }, new[] { atk.Defenders[i].CriticalFlag }, atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[atk.Defenders[i].Defender]));
							AddDamage(hps, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
							damages[comboatk] += atk.Defenders[i].Damage;
						}
						break;

					default:
						foreach (var defs in atk.Defenders.GroupBy(d => d.Defender))
						{
							BattleDetails.Add(new BattleNightDetail(Battle, atk.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[defs.Key]));
							AddDamage(hps, defs.Key, defs.Sum(d => d.Damage));
						}
						damages[atk.Attacker] += atk.Defenders.Sum(d => d.Damage);
						break;
				}
			}

		}


		public List<PhaseNightBattleAttack> Attacks { get; private set; }
		public class PhaseNightBattleAttack
		{
			public BattleIndex Attacker;
			public int AttackType;
			public bool NightAirAttackFlag;
			public List<PhaseNightBattleDefender> Defenders;
			public int[] EquipmentIDs;

			public PhaseNightBattleAttack()
			{
				Defenders = new List<PhaseNightBattleDefender>();
			}

			public override string ToString() => $"{Attacker}[{AttackType}] -> [{string.Join(", ", Defenders)}]";

		}
		public class PhaseNightBattleDefender
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

	}
}
