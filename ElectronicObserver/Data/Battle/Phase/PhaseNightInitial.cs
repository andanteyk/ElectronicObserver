using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// 夜戦開始フェーズの処理を行います。
	/// </summary>
	public class PhaseNightInitial : PhaseBase
	{

		private readonly bool IsEscort;

		public PhaseNightInitial(BattleData battle, string title, bool isEscort)
			: base(battle, title)
		{
			IsEscort = isEscort;
		}

		public override bool IsAvailable => RawData != null;

		public override void EmulateBattle(int[] hps, int[] damages)
		{
			// nop
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
		/// 自軍照明弾投射艦インデックス(0-11, -1=発動せず)
		/// </summary>
		public int FlareIndexFriend => (int)RawData.api_flare_pos[0];

		/// <summary>
		/// 敵軍照明弾投射艦インデックス(0-11, -1=発動せず)
		/// </summary>
		public int FlareIndexEnemy => (int)RawData.api_flare_pos[1];


		/// <summary>
		/// 自軍照明弾投射艦
		/// </summary>
		public ShipData FlareFriendInstance
		{
			get
			{
				int index = FlareIndexFriend;

				if (index < 0)
					return null;

				if (IsFriendEscort)
					return FriendFleet.MembersInstance[index - 6];
				else
					return FriendFleet.MembersInstance[index];

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

				if (index < 0)
					return null;

				if (IsEnemyEscort)
					return EnemyMembersInstance[index - 6];
				else
					return EnemyMembersInstance[index];
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
				var hps = IsFriendEscort ? Battle.Initial.FriendInitialHPsEscort : Battle.Initial.FriendInitialHPs;
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

		/// <summary>
		/// 敵軍探照灯照射艦
		/// </summary>
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
