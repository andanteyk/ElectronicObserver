using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 戦闘情報を保持するデータの基底です。
	/// </summary>
	public abstract class BattleData : ResponseWrapper
	{

		protected int[] _resultHPs;
		/// <summary>
		/// 戦闘終了時の各艦のHP
		/// </summary>
		public ReadOnlyCollection<int> ResultHPs => Array.AsReadOnly(_resultHPs);

		protected int[] _attackDamages;
		/// <summary>
		/// 各艦の与ダメージ
		/// </summary>
		public ReadOnlyCollection<int> AttackDamages => Array.AsReadOnly(_attackDamages);


		public PhaseInitial Initial { get; protected set; }
		public PhaseSearching Searching { get; protected set; }
		public PhaseSupport Support { get; protected set; }


		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			Initial = new PhaseInitial(this, "戦力");
			Searching = new PhaseSearching(this, "索敵");

			_resultHPs = new int[24];
			Array.Copy(Initial.FriendInitialHPs, 0, _resultHPs, 0, Initial.FriendInitialHPs.Length);
			Array.Copy(Initial.EnemyInitialHPs, 0, _resultHPs, 12, Initial.EnemyInitialHPs.Length);
			if (Initial.FriendInitialHPsEscort != null)
				Array.Copy(Initial.FriendInitialHPsEscort, 0, _resultHPs, 6, 6);
			if (Initial.EnemyInitialHPsEscort != null)
				Array.Copy(Initial.EnemyInitialHPsEscort, 0, _resultHPs, 18, 6);



			if (_attackDamages == null)
				_attackDamages = new int[_resultHPs.Length];
		}


		/// <summary>
		/// MVP 取得候補艦のインデックス [0-6]
		/// </summary>
		public IEnumerable<int> MVPShipIndexes
		{
			get
			{
				int memberCount = Initial.FriendFleet.Members.Count;
				int max = _attackDamages.Take(memberCount).Max();
				if (max == 0)
				{       // 全員ノーダメージなら旗艦MVP
					yield return 0;

				}
				else
				{
					for (int i = 0; i < memberCount; i++)
					{
						if (_attackDamages[i] == max)
							yield return i;
					}
				}
			}
		}


		/// <summary>
		/// 連合艦隊随伴艦隊の MVP 取得候補艦のインデックス [0-5]
		/// </summary>
		public IEnumerable<int> MVPShipCombinedIndexes
		{
			get
			{
				int max = _attackDamages.Skip(6).Take(6).Max();
				if (max == 0)
				{       // 全員ノーダメージなら旗艦MVP
					yield return 0;

				}
				else
				{
					for (int i = 0; i < 6; i++)
					{
						if (_attackDamages[i + 6] == max)
							yield return i;
					}
				}
			}
		}


		/// <summary>
		/// 前回の戦闘データからパラメータを引き継ぎます。
		/// </summary>
		internal void TakeOverParameters(BattleData prev)
		{
			_attackDamages = (int[])prev._attackDamages.Clone();
		}



		/// <summary>
		/// 対応しているAPIの名前を取得します。
		/// </summary>
		public abstract string APIName { get; }

		/// <summary>
		/// 戦闘形態の名称
		/// </summary>
		public abstract string BattleName { get; }


		public virtual bool IsPractice => false;
		public virtual bool IsFriendCombined => Initial.IsFriendCombined;
		public virtual bool IsEnemyCombined => Initial.IsEnemyCombined;
		public virtual bool IsBaseAirRaid => false;



		/// <summary>
		/// すべての戦闘詳細データを取得します。
		/// </summary>
		public string GetBattleDetail()
		{
			return GetBattleDetail(-1);
		}

		/// <summary>
		/// 指定したインデックスの艦の戦闘詳細データを取得します。
		/// </summary>
		/// <param name="index">インデックス。[0-23]</param>
		public string GetBattleDetail(int index)
		{
			var sb = new StringBuilder();

			foreach (var phase in GetPhases())
			{
				string bd = phase.GetBattleDetail(index);

				if (!string.IsNullOrEmpty(bd))
				{
					sb.AppendLine("《" + phase.Title + "》").Append(bd);
				}
			}
			return sb.ToString();
		}


		public abstract IEnumerable<PhaseBase> GetPhases();

	}

}
