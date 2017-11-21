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



		protected static int[] FixedArray(int[] array) => FixedArray(array, MemberCount);

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


			int[] GetArrayOrDefault(string objectName) => !RawData.IsDefined(objectName) ? null : FixedArray((int[])RawData[objectName]);
			int[][] GetArraysOrDefault(string objectName, int bottomLength)
			{
				if (!RawData.IsDefined(objectName))
					return null;

				int[][] ret = new int[MemberCount][];
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

			EnemyMembers = GetArrayOrDefault("api_ship_ke");
			EnemyMembersInstance = EnemyMembers.Select(id => KCDatabase.Instance.MasterShips[id]).ToArray();

			EnemyMembersEscort = GetArrayOrDefault("api_ship_ke_combined");
			EnemyMembersEscortInstance = EnemyMembersEscort?.Select(id => KCDatabase.Instance.MasterShips[id]).ToArray();

			EnemyLevels = GetArrayOrDefault("api_ship_lv");
			EnemyLevelsEscort = GetArrayOrDefault("api_ship_lv_combined");

			FriendInitialHPs = GetArrayOrDefault("api_f_nowhps");
			FriendInitialHPsEscort = GetArrayOrDefault("api_f_nowhps_combined");
			EnemyInitialHPs = GetArrayOrDefault("api_e_nowhps");
			EnemyInitialHPsEscort = GetArrayOrDefault("api_e_nowhps_combined");

			FriendMaxHPs = GetArrayOrDefault("api_f_maxhps");
			FriendMaxHPsEscort = GetArrayOrDefault("api_f_maxhps_combined");
			EnemyMaxHPs = GetArrayOrDefault("api_e_maxhps");
			EnemyMaxHPsEscort = GetArrayOrDefault("api_e_maxhps_combined");


			EnemySlots = GetArraysOrDefault("api_eSlot", 5);
			EnemySlotsInstance = EnemySlots.Select(part => part.Select(id => KCDatabase.Instance.MasterEquipments[id]).ToArray()).ToArray();

			EnemySlotsEscort = GetArraysOrDefault("api_eSlot_combined", 5);
			EnemySlotsEscortInstance = EnemySlotsEscort?.Select(part => part.Select(id => KCDatabase.Instance.MasterEquipments[id]).ToArray()).ToArray();

			EnemyParameters = GetArraysOrDefault("api_eParam", 4);
			EnemyParametersEscort = GetArraysOrDefault("api_eParam_combined", 4);

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



		/// <summary>
		/// 2016/11/19 現在、連合艦隊夜戦において味方随伴艦隊が 最大HP = 現在HP となる不具合が存在するため、
		/// 昼戦データから最大HPを取得する
		/// </summary>
		public void TakeOverMaxHPs(BattleData bd)
		{
			Array.Copy(bd.Initial.FriendMaxHPsEscort, FriendMaxHPsEscort, MemberCount);
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

		public override bool IsAvailable => RawData != null;

		public override void EmulateBattle(int[] hps, int[] damages)
		{
		}

	}
}
