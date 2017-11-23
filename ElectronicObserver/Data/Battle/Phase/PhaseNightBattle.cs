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
		private readonly bool IsEscort;

		public PhaseNightBattle(BattleData data, string title, int phaseID, bool isEscort)
			: base(data, title)
		{

			PhaseID = phaseID;
			IsEscort = isEscort;

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
					Attacker = new BattleIndex(attackers[i] + (fleetflag[i] == 0 ? 0 : 12),Battle.IsFriendCombined, Battle.IsEnemyCombined),
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


			foreach (var attack in Attacks)
			{

				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleNightDetail(Battle, attack.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), attack.AttackType, attack.EquipmentIDs, attack.NightAirAttackFlag, hps[defs.Key]));
					AddDamage(hps, defs.Key, defs.Sum(d => d.Damage));
				}

				damages[attack.Attacker] += attack.Defenders.Sum(d => d.Damage);
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



		/// <summary>
		/// 戦闘する自軍艦隊
		/// 1=主力艦隊, 2=随伴艦隊
		/// </summary>
		public int ActiveFriendFleet => !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[0];

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID
		{
			get
			{
				if (IsFriendEscort)
					return 2;
				else
					return Battle.Initial.FriendFleetID;
			}
		}

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet => KCDatabase.Instance.Fleet[FriendFleetID];

		/// <summary>
		/// 自軍が随伴艦隊かどうか
		/// </summary>
		public bool IsFriendEscort => IsEscort || ActiveFriendFleet != 1;


		/// <summary>
		/// 敵軍艦隊ID
		/// </summary>
		public int EnemyFleetID => !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[1];

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public int[] EnemyMembers => !IsEnemyEscort ? Battle.Initial.EnemyMembers : Battle.Initial.EnemyMembersEscort;

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance => !IsEnemyEscort ? Battle.Initial.EnemyMembersInstance : Battle.Initial.EnemyMembersEscortInstance;

		/// <summary>
		/// 敵軍が随伴艦隊かどうか
		/// </summary>
		public bool IsEnemyEscort => EnemyFleetID != 1;


		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend => (RawData.api_touch_plane[0] is string) ? int.Parse(RawData.api_touch_plane[0]) : (int)RawData.api_touch_plane[0];


		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy => (RawData.api_touch_plane[1] is string) ? int.Parse(RawData.api_touch_plane[1]) : (int)RawData.api_touch_plane[1];


		/// <summary>
		/// 自軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexFriend
		{
			get
			{
				int index = (int)RawData.api_flare_pos[0];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexEnemy
		{
			get
			{
				int index = (int)RawData.api_flare_pos[1];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦
		/// </summary>
		public ShipDataMaster FlareEnemyInstance
		{
			get
			{
				int index = FlareIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}

		/// <summary>
		/// 自軍探照灯照射艦番号
		/// </summary>
		public int SearchlightIndexFriend
		{
			get
			{
				var ships = FriendFleet.MembersWithoutEscaped;
				var hps = IsEscort ? Battle.Initial.FriendInitialHPsEscort : Battle.Initial.FriendInitialHPs;
				int index = -1;

				for (int i = 0; i < ships.Count; i++)
				{
					var ship = ships[i];
					if (ship != null && hps[i] > 1)
					{

						if (ship.SlotInstanceMaster.Any(e => e?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (ship.SlotInstanceMaster.Any(e => e?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
							index = i;
					}
				}

				return index;
			}
		}

		/// <summary>
		/// 敵軍探照灯照射艦番号(0-5)
		/// </summary>
		public int SearchlightIndexEnemy
		{
			get
			{
				var ships = EnemyMembersInstance;
				var eqs = Battle.Initial.EnemySlotsInstance;
				var hps = IsEnemyEscort ? Battle.Initial.EnemyInitialHPsEscort : Battle.Initial.EnemyInitialHPs;
				int index = -1;

				for (int i = 0; i < ships.Length; i++)
				{
					if (ships[i] != null && hps[i] > 1)
					{

						if (eqs[i].Any(e => e?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (eqs[i].Any(e => e?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
							index = i;

					}
				}

				return index;
			}
		}

		public ShipDataMaster SearchlightEnemyInstance
		{
			get
			{
				int index = SearchlightIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}

	}
}
