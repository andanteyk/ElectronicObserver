using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// 夜戦における友軍艦隊攻撃フェーズの処理を行います。
	/// </summary>
	public class PhaseFriendlySupport : PhaseBase
	{
		public PhaseFriendlySupport(BattleData battle, string title)
			: base(battle, title)
		{
			if (!IsAvailable)
				return;

			// info translation

			int[] GetArrayOrDefault(string objectName, int length) => !InfoData.IsDefined(objectName) ? null : FixedArray((int[])InfoData[objectName], length);
			int[][] GetArraysOrDefault(string objectName, int topLength, int bottomLength)
			{
				if (!InfoData.IsDefined(objectName))
					return null;

				int[][] ret = new int[topLength][];
				dynamic[] raw = (dynamic[])InfoData[objectName];
				for (int i = 0; i < ret.Length; i++)
				{
					if (i < raw.Length)
						ret[i] = FixedArray((int[])raw[i], bottomLength);
					else
						ret[i] = Enumerable.Repeat(-1, bottomLength).ToArray();
				}
				return ret;
			}

			FriendlyMembers = GetArrayOrDefault("api_ship_id", 7);
			FriendlyMembersInstance = FriendlyMembers.Select(id => KCDatabase.Instance.MasterShips[id]).ToArray();
			FriendlyLevels = GetArrayOrDefault("api_ship_lv", 7);
			FriendlyInitialHPs = GetArrayOrDefault("api_nowhps", 7);
			FriendlyMaxHPs = GetArrayOrDefault("api_maxhps", 7);

			FriendlySlots = GetArraysOrDefault("api_Slot", 7, 5);
			FriendlyParameters = GetArraysOrDefault("api_Param", 7, 4);


			// battle translation

			int[] fleetflag = (int[])ShellingData.api_at_eflag;
			int[] attackers = (int[])ShellingData.api_at_list;
			int[] nightAirAttackFlags = (int[])ShellingData.api_n_mother_list;
			int[] attackTypes = (int[])ShellingData.api_sp_list;
			int[][] defenders = ((dynamic[])ShellingData.api_df_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			int[][] attackEquipments = ((dynamic[])ShellingData.api_si_list).Select(elem => ((dynamic[])elem).Select<dynamic, int>(ch => ch is string ? int.Parse(ch) : (int)ch).ToArray()).ToArray();
			int[][] criticals = ((dynamic[])ShellingData.api_cl_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			double[][] rawDamages = ((dynamic[])ShellingData.api_damage).Select(elem => ((double[])elem).Where(e => e != -1).ToArray()).ToArray();

			Attacks = new List<PhaseFriendlySupportAttack>();



			for (int i = 0; i < attackers.Length; i++)
			{
				var attack = new PhaseFriendlySupportAttack
				{
					Attacker = new BattleIndex(attackers[i] + (fleetflag[i] == 0 ? 0 : 12), Battle.IsFriendCombined, Battle.IsEnemyCombined),
					NightAirAttackFlag = nightAirAttackFlags[i] == -1,
					AttackType = attackTypes[i],
					EquipmentIDs = attackEquipments[i],
				};
				for (int k = 0; k < defenders[i].Length; k++)
				{
					var defender = new PhaseFriendlySupportDefender
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

		public override bool IsAvailable => RawData.api_friendly_info();


		public override void EmulateBattle(int[] hps, int[] damages)
		{
			if (!IsAvailable)
				return;

			// note: HP計算が正しくできない - 送られてくるHPにはすでに友軍支援のダメージが適用済みであるため、昼戦終了時のHPを参照しなければならない

			foreach (var attack in Attacks)
			{
				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleFriendlySupportDetail((BattleNight)Battle, attack.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), attack.AttackType, attack.EquipmentIDs, attack.NightAirAttackFlag, hps[defs.Key]));
				}
			}

		}



		public dynamic InfoData => RawData.api_friendly_info;
		public dynamic BattleData => RawData.api_friendly_battle;
		public dynamic ShellingData => RawData.api_friendly_battle.api_hougeki;

		public int Type => (int)InfoData.api_production_type;

		public int[] FriendlyMembers { get; private set; }
		public ShipDataMaster[] FriendlyMembersInstance { get; private set; }
		public int[] FriendlyLevels { get; private set; }
		public int[] FriendlyInitialHPs { get; private set; }
		public int[] FriendlyMaxHPs { get; private set; }

		public int[][] FriendlySlots { get; private set; }
		public int[][] FriendlyParameters { get; private set; }

		// api_voice_id
		// api_voice_p_no



		public int FlareIndexFriend
		{
			get
			{
				int index = (int)BattleData.api_flare_pos[0];
				return index != -1 ? index - 1 : -1;
			}
		}

		public int FlareIndexEnemy
		{
			get
			{
				int index = (int)BattleData.api_flare_pos[1];
				return index != -1 ? index - 1 : -1;
			}
		}


		public List<PhaseFriendlySupportAttack> Attacks { get; private set; }


		public class PhaseFriendlySupportAttack
		{
			public BattleIndex Attacker;
			public int AttackType;
			public bool NightAirAttackFlag;
			public List<PhaseFriendlySupportDefender> Defenders;
			public int[] EquipmentIDs;

			public PhaseFriendlySupportAttack()
			{
				Defenders = new List<PhaseFriendlySupportDefender>();
			}
		}

		public class PhaseFriendlySupportDefender
		{
			public BattleIndex Defender;
			public int CriticalFlag;
			public double RawDamage;
			public bool GuardsFlagship => RawDamage != Math.Floor(RawDamage);
			public int Damage => (int)RawDamage;
		}




		protected static int[] FixedArray(int[] array, int length, int defaultValue = -1)
		{
			var ret = new int[length];
			int l = Math.Min(length, array.Length);
			Array.Copy(array, ret, l);
			if (l < length)
			{
				for (int i = l; i < length; i++)
					ret[i] = defaultValue;
			}

			return ret;
		}

	}
}
