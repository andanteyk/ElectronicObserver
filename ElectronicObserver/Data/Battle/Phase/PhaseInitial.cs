using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 戦闘開始フェーズの処理を行います。
	/// </summary>
	public class PhaseInitial : PhaseBase
	{

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID { get; private set; }

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet => KCDatabase.Instance.Fleet[FriendFleetID];

		/// <summary>
		/// 自軍随伴艦隊
		/// </summary>
		public FleetData FriendFleetEscort => IsFriendCombined ? KCDatabase.Instance.Fleet[2] : null;


		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public int[] EnemyMembers { get; private set; }

		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance { get; private set; }


		/// <summary>
		/// 敵艦隊メンバ(随伴艦隊)
		/// </summary>
		public int[] EnemyMembersEscort { get; private set; }

		/// <summary>
		/// 敵艦隊メンバ(随伴艦隊)
		/// </summary>
		public ShipDataMaster[] EnemyMembersEscortInstance { get; private set; }


		/// <summary>
		/// 敵艦のレベル
		/// </summary>
		public int[] EnemyLevels { get; private set; }

		/// <summary>
		/// 敵艦のレベル(随伴艦隊)
		/// </summary>
		public int[] EnemyLevelsEscort { get; private set; }


		public int[] FriendInitialHPs { get; private set; }
		public int[] FriendInitialHPsEscort { get; private set; }
		public int[] EnemyInitialHPs { get; private set; }
		public int[] EnemyInitialHPsEscort { get; private set; }

		public int[] FriendMaxHPs { get; private set; }
		public int[] FriendMaxHPsEscort { get; private set; }
		public int[] EnemyMaxHPs { get; private set; }
		public int[] EnemyMaxHPsEscort { get; private set; }



		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public int[][] EnemySlots { get; private set; }

		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public EquipmentDataMaster[][] EnemySlotsInstance { get; private set; }


		/// <summary>
		/// 敵艦のスロット(随伴艦隊)
		/// </summary>
		public int[][] EnemySlotsEscort { get; private set; }

		/// <summary>
		/// 敵艦のスロット(随伴艦隊)
		/// </summary>
		public EquipmentDataMaster[][] EnemySlotsEscortInstance { get; private set; }


		/// <summary>
		/// 敵艦のパラメータ
		/// </summary>
		public int[][] EnemyParameters { get; private set; }

		/// <summary>
		/// 敵艦のパラメータ(随伴艦隊)
		/// </summary>
		public int[][] EnemyParametersEscort { get; private set; }


		/// <summary>
		/// 装甲破壊されているか
		/// </summary>
		public bool IsBossDamaged => RawData.api_xal01() && (int)RawData.api_xal01 > 0;


		/// <summary>
		/// 戦闘糧食を食べた艦娘のインデックス [0-11]
		/// </summary>
		public int[] RationIndexes { get; private set; }


		public bool IsFriendCombined => FriendInitialHPsEscort != null;
		public bool IsEnemyCombined => EnemyInitialHPsEscort != null;



		public PhaseInitial(BattleData data, string title)
			: base(data, title)
		{
			{
				dynamic id = RawData.api_dock_id() ? RawData.api_dock_id :
					RawData.api_deck_id() ? RawData.api_deck_id : 1;
				FriendFleetID = id is string ? int.Parse((string)id) : (int)id;
			}
			if (FriendFleetID <= 0)
				FriendFleetID = 1;


			int[] GetArrayOrDefault(string objectName, int length) => !RawData.IsDefined(objectName) ? null : FixedArray((int[])RawData[objectName], length);
			int[][] GetArraysOrDefault(string objectName, int topLength, int bottomLength)
			{
				if (!RawData.IsDefined(objectName))
					return null;

				int[][] ret = new int[topLength][];
				dynamic[] raw = (dynamic[])RawData[objectName];
				for (int i = 0; i < ret.Length; i++)
				{
					if (i < raw.Length)
						ret[i] = FixedArray((int[])raw[i], bottomLength);
					else
						ret[i] = Enumerable.Repeat(-1, bottomLength).ToArray();
				}
				return ret;
			}

			int mainMemberCount = 7;
			int escortMemberCount = 6;

			EnemyMembers = GetArrayOrDefault("api_ship_ke", mainMemberCount);
			EnemyMembersInstance = EnemyMembers.Select(id => KCDatabase.Instance.MasterShips[id]).ToArray();

			EnemyMembersEscort = GetArrayOrDefault("api_ship_ke_combined", escortMemberCount);
			EnemyMembersEscortInstance = EnemyMembersEscort?.Select(id => KCDatabase.Instance.MasterShips[id]).ToArray();

			EnemyLevels = GetArrayOrDefault("api_ship_lv", mainMemberCount);
			EnemyLevelsEscort = GetArrayOrDefault("api_ship_lv_combined", escortMemberCount);

			FriendInitialHPs = GetArrayOrDefault("api_f_nowhps", mainMemberCount);
			FriendInitialHPsEscort = GetArrayOrDefault("api_f_nowhps_combined", escortMemberCount);
			EnemyInitialHPs = GetArrayOrDefault("api_e_nowhps", mainMemberCount);
			EnemyInitialHPsEscort = GetArrayOrDefault("api_e_nowhps_combined", escortMemberCount);

			FriendMaxHPs = GetArrayOrDefault("api_f_maxhps", mainMemberCount);
			FriendMaxHPsEscort = GetArrayOrDefault("api_f_maxhps_combined", escortMemberCount);
			EnemyMaxHPs = GetArrayOrDefault("api_e_maxhps", mainMemberCount);
			EnemyMaxHPsEscort = GetArrayOrDefault("api_e_maxhps_combined",escortMemberCount);


			EnemySlots = GetArraysOrDefault("api_eSlot", mainMemberCount, 5);
			EnemySlotsInstance = EnemySlots.Select(part => part.Select(id => KCDatabase.Instance.MasterEquipments[id]).ToArray()).ToArray();

			EnemySlotsEscort = GetArraysOrDefault("api_eSlot_combined", escortMemberCount, 5);
			EnemySlotsEscortInstance = EnemySlotsEscort?.Select(part => part.Select(id => KCDatabase.Instance.MasterEquipments[id]).ToArray()).ToArray();

			EnemyParameters = GetArraysOrDefault("api_eParam", mainMemberCount, 4);
			EnemyParametersEscort = GetArraysOrDefault("api_eParam_combined", escortMemberCount, 4);

			{
				var rations = new List<int>();
				if (RawData.api_combat_ration())
				{
					rations.AddRange(((int[])RawData.api_combat_ration).Select(i => FriendFleet.Members.IndexOf(i)));
				}
				if (RawData.api_combat_ration_combined())
				{
					rations.AddRange(((int[])RawData.api_combat_ration_combined).Select(i => FriendFleetEscort.Members.IndexOf(i) + 6));
				}
				RationIndexes = rations.ToArray();
			}
		}



		// note: 直ったのでは？
		/// <summary>
		/// 2016/11/19 現在、連合艦隊夜戦において味方随伴艦隊が 最大HP = 現在HP となる不具合が存在するため、
		/// 昼戦データから最大HPを取得する
		/// </summary>
		public void TakeOverMaxHPs(BattleData bd)
		{
			Array.Copy(bd.Initial.FriendMaxHPsEscort, FriendMaxHPsEscort, bd.Initial.FriendMaxHPsEscort.Length);
		}


		public ShipData GetFriendShip(int index)
		{
			if (index < 0 || index >= 12)
				return null;

			if (index < FriendFleet.Members.Count)
				return FriendFleet.MembersInstance[index];
			else if (index >= 6 && FriendFleetEscort != null)
				return FriendFleetEscort.MembersInstance[index - 6];
			else
				return null;
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



		public override bool IsAvailable => RawData != null;

		public override void EmulateBattle(int[] hps, int[] damages)
		{
		}

	}
}
