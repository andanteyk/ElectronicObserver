using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// 夜戦における友軍艦隊砲撃フェーズの処理を行います。
	/// </summary>
	public class PhaseFriendlyShelling : PhaseBase
	{
		public PhaseFriendlyShelling(BattleData battle, string title)
			: base(battle, title)
		{
			if (!IsAvailable)
				return;

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
					Attacker = new BattleIndex(attackers[i] + (fleetflag[i] == 0 ? 0 : 12), false, Battle.IsEnemyCombined),
					NightAirAttackFlag = nightAirAttackFlags[i] == -1,
					AttackType = attackTypes[i],
					EquipmentIDs = attackEquipments[i],
				};
				for (int k = 0; k < defenders[i].Length; k++)
				{
					var defender = new PhaseFriendlySupportDefender
					{
						Defender = new BattleIndex(defenders[i][k] + (fleetflag[i] == 0 ? 12 : 0), false, Battle.IsEnemyCombined),
						CriticalFlag = criticals[i][k],
						RawDamage = rawDamages[i][k]
					};
					attack.Defenders.Add(defender);
				}

				Attacks.Add(attack);
			}
		}

		public override bool IsAvailable => RawData.api_friendly_battle();


		public override void EmulateBattle(int[] hps, int[] damages)
		{
			if (!IsAvailable)
				return;

			int[] friendhps = Battle.FriendlySupportInfo.FriendlyInitialHPs;

			foreach (var attack in Attacks)
			{
				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleFriendlyShellingDetail(
						(BattleNight)Battle,
						attack.Attacker,
						defs.Key,
						defs.Select(d => d.RawDamage).ToArray(),
						defs.Select(d => d.CriticalFlag).ToArray(),
						attack.AttackType,
						attack.EquipmentIDs,
						attack.NightAirAttackFlag,
						defs.Key.IsFriend ? friendhps[defs.Key] : hps[defs.Key]));

					if (defs.Key.IsFriend)
						friendhps[defs.Key] -= Math.Max(defs.Sum(d => d.Damage), 0);
					else
						AddDamage(hps, defs.Key, defs.Sum(d => d.Damage));
				}
			}

		}



		/// <summary>
		/// 戦闘データ
		/// </summary>
		public dynamic BattleData => RawData.api_friendly_battle;

		/// <summary>
		/// 砲撃戦データ
		/// </summary>
		public dynamic ShellingData => RawData.api_friendly_battle.api_hougeki;



		/// <summary>
		/// 自軍照明弾投射艦インデックス
		/// </summary>
		public int FlareIndexFriend => (int)BattleData.api_flare_pos[0];

		/// <summary>
		/// 敵軍照明弾投射艦インデックス
		/// </summary>
		public int FlareIndexEnemy => (int)BattleData.api_flare_pos[1];


		/// <summary>
		/// 自軍照明弾投射艦
		/// </summary>
		public ShipDataMaster FlareFriendInstance
		{
			get
			{
				int index = FlareIndexFriend;
				if (0 <= index && index < Battle.FriendlySupportInfo.FriendlyMembersInstance.Length)
					return Battle.FriendlySupportInfo.FriendlyMembersInstance[index];
				return null;
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
				var nightinitial = (Battle as BattleNight)?.NightInitial;

				if (nightinitial != null &&
					0 <= index && index < nightinitial.EnemyMembersInstance.Length)
					return nightinitial.EnemyMembersInstance[index];
				return null;
			}
		}


		/// <summary>
		/// 自軍探照灯照射艦番号
		/// </summary>
		public int SearchlightIndexFriend
		{
			get
			{
				int index = -1;
				var eqmaster = KCDatabase.Instance.MasterEquipments;

				for (int i = 0; i < Battle.FriendlySupportInfo.FriendlyMembersInstance.Length; i++)
				{
					if (Battle.FriendlySupportInfo.FriendlyMembers[i] != -1 && Battle.FriendlySupportInfo.FriendlyInitialHPs[i] > 1)
					{
						if (Battle.FriendlySupportInfo.FriendlySlots[i].Any(id => eqmaster[id]?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (Battle.FriendlySupportInfo.FriendlySlots[i].Any(id => eqmaster[id]?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
							index = i;
					}
				}

				return index;
			}
		}


		/// <summary>
		/// 敵軍探照灯照射艦番号
		/// 厳密には異なるが(友軍の攻撃で探照灯所持艦の HP が 1 になった場合 -1 になる)、めったに起こるものでもないので気にしないことにする
		/// </summary>
		public int SearchlightIndexEnemy => (Battle as BattleNight)?.NightInitial?.SearchlightIndexEnemy ?? -1;


		/// <summary>
		/// 自軍探照灯照射艦
		/// </summary>
		public ShipDataMaster SearchlightFriendInstance
		{
			get
			{
				int index = SearchlightIndexFriend;
				if (0 <= index && index < Battle.FriendlySupportInfo.FriendlyMembersInstance.Length)
					return Battle.FriendlySupportInfo.FriendlyMembersInstance[index];
				return null;
			}
		}

		/// <summary>
		/// 敵軍探照灯投射艦
		/// </summary>
		public ShipDataMaster SearchlightEnemyInstance
		{
			get
			{
				int index = SearchlightIndexEnemy;
				var nightinitial = (Battle as BattleNight)?.NightInitial;

				if (nightinitial != null &&
					0 <= index && index < nightinitial.EnemyMembersInstance.Length)
					return nightinitial.EnemyMembersInstance[index];
				return null;
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

	}
}
