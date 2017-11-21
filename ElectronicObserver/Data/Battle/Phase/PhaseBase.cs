using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 戦闘フェーズの基底クラスです。
	/// </summary>
	public abstract class PhaseBase
	{
		protected static readonly int MemberCount = 7;


		protected BattleData Battle;
		public List<BattleDetail> BattleDetails { get; protected set; }
		public readonly string Title;


		protected PhaseBase(BattleData battle, string title)
		{
			Battle = battle;
			BattleDetails = new List<BattleDetail>();
			Title = title;
		}


		protected dynamic RawData => Battle.RawData;

		protected bool IsPractice => (Battle.BattleType & BattleData.BattleTypeFlag.Practice) != 0;
		protected bool IsFriendCombined => (Battle.BattleType & BattleData.BattleTypeFlag.Combined) != 0;
		protected bool IsEnemyCombined => (Battle.BattleType & BattleData.BattleTypeFlag.EnemyCombined) != 0;

		protected static bool IsIndexFriend(int index) => 0 <= index && index < 12;
		protected static bool IsIndexEnemy(int index) => 12 <= index && index < 24;


		/// <summary>
		/// 被ダメージ処理を行います。
		/// </summary>
		/// <param name="hps">各艦のHPリスト。</param>
		/// <param name="index">ダメージを受ける艦のインデックス。</param>
		/// <param name="damage">ダメージ。</param>
		protected void AddDamage(int[] hps, int index, int damage)
		{

			hps[index] -= Math.Max(damage, 0);

			// 自軍艦の撃沈が発生した場合(ダメコン処理)
			if (hps[index] <= 0 && IsIndexFriend(index) && !IsPractice)
			{
				var ship = Battle.Initial.GetFriendShip(index);
				if (ship == null)
					return;


				//補強スロットが最優先
				if (ship.ExpansionSlotMaster == 42)
				{
					//応急修理要員
					hps[index] = (int)(ship.HPMax * 0.2);
					return;
				}
				else if (ship.ExpansionSlotMaster == 43)
				{
					//応急修理女神
					hps[index] = ship.HPMax;
					return;
				}

				foreach (var eid in ship.SlotMaster)
				{
					if (eid == 42)
					{
						//応急修理要員
						hps[index] = (int)(ship.HPMax * 0.2);
						break;
					}
					else if (eid == 43)
					{
						//応急修理女神
						hps[index] = ship.HPMax;
						break;
					}
				}
			}
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


		protected virtual IEnumerable<BattleDetail> SearchBattleDetails(int index)
		{
			return BattleDetails.Where(d => d.AttackerIndex == index || d.DefenderIndex == index);
		}
		public virtual string GetBattleDetail(int index)
		{
			IEnumerable<BattleDetail> list;
			if (index == -1)
				list = BattleDetails;
			else
				list = SearchBattleDetails(index);

			if (list.Any())
			{
				return string.Join("\r\n", list) + "\r\n";
			}
			else return null;
		}
		public virtual string GetBattleDetail() { return GetBattleDetail(-1); }


		public override string ToString() => string.Join(" / \r\n", BattleDetails);



		/// <summary>
		/// データが有効かどうかを示します。
		/// </summary>
		public abstract bool IsAvailable { get; }

		/// <summary>
		/// 戦闘をエミュレートします。
		/// </summary>
		/// <param name="hps">各艦のHPリスト。</param>
		/// <param name="damages">各艦の与ダメージリスト。</param>
		public abstract void EmulateBattle(int[] hps, int[] damages);


	}
}
