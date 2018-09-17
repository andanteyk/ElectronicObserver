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

		public override bool IsAvailable => RawData.api_friendly_info();


		public override void EmulateBattle(int[] hps, int[] damages)
		{
			if (!IsAvailable)
				return;

			int[] friendhps = FriendlyInitialHPs;

			foreach (var attack in Attacks)
			{
				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleFriendlySupportDetail(
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
		/// 戦闘情報データ
		/// </summary>
		public dynamic InfoData => RawData.api_friendly_info;

		/// <summary>
		/// 戦闘データ
		/// </summary>
		public dynamic BattleData => RawData.api_friendly_battle;

		/// <summary>
		/// 砲撃戦データ
		/// </summary>
		public dynamic ShellingData => RawData.api_friendly_battle.api_hougeki;


		/// <summary>
		/// 種別？
		/// </summary>
		public int Type => (int)InfoData.api_production_type;


		/// <summary>
		/// 友軍艦隊ID
		/// </summary>
		public int[] FriendlyMembers { get; private set; }

		/// <summary>
		/// 友軍艦隊
		/// </summary>
		public ShipDataMaster[] FriendlyMembersInstance { get; private set; }


		/// <summary>
		/// 友軍艦隊レベル
		/// </summary>
		public int[] FriendlyLevels { get; private set; }

		/// <summary>
		/// 友軍艦隊初期HP
		/// </summary>
		public int[] FriendlyInitialHPs { get; private set; }

		/// <summary>
		/// 友軍艦隊最大HP
		/// </summary>
		public int[] FriendlyMaxHPs { get; private set; }


		/// <summary>
		/// 友軍艦隊装備
		/// </summary>
		public int[][] FriendlySlots { get; private set; }

		/// <summary>
		/// 友軍艦隊パラメータ
		/// </summary>
		public int[][] FriendlyParameters { get; private set; }

		// api_voice_id
		// api_voice_p_no


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
				if (0 <= index && index < FriendlyMembersInstance.Length)
					return FriendlyMembersInstance[index];
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

				for (int i = 0; i < FriendlyMembersInstance.Length; i++)
				{
					if (FriendlyMembers[i] != -1 && FriendlyInitialHPs[i] > 1)
					{
						if (FriendlySlots[i].Any(id => eqmaster[id]?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (FriendlySlots[i].Any(id => eqmaster[id]?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
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
				if (0 <= index && index < FriendlyMembersInstance.Length)
					return FriendlyMembersInstance[index];
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
