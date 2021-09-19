using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{
	/// <summary>
	/// 友軍艦隊の情報を処理します。
	/// </summary>
	public class PhaseFriendlySupportInfo : PhaseBase
	{
		public PhaseFriendlySupportInfo(BattleData battle, string title)
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
			FriendlyExpansionSlots = GetArrayOrDefault("api_slot_ex", 7);
			FriendlyParameters = GetArraysOrDefault("api_Param", 7, 4);
		}

		public override bool IsAvailable => RawData.api_friendly_info();

		public override void EmulateBattle(int[] hps, int[] damages)
		{
			// nop
		}


		/// <summary>
		/// 友軍情報
		/// </summary>
		public dynamic InfoData => RawData.api_friendly_info;

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
		/// 友軍艦隊装備 (拡張スロット)
		/// </summary>
		public int[] FriendlyExpansionSlots { get; private set; }

		/// <summary>
		/// 友軍艦隊パラメータ
		/// </summary>
		public int[][] FriendlyParameters { get; private set; }

		// api_voice_id
		// api_voice_p_no



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
