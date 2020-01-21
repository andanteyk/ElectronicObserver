﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest
{

	/// <summary>
	/// 任務の進捗を管理する基底クラスです。
	/// </summary>
	[DataContract(Name = "ProgressData")]
	public abstract class ProgressData : IIdentifiable
	{

		/// <summary>
		/// 任務ID
		/// </summary>
		[DataMember]
		public int QuestID { get; protected set; }


		/// <summary>
		/// 進捗現在値
		/// </summary>
		[DataMember]
		public int Progress { get; protected set; }

		/// <summary>
		/// 進捗最大値
		/// </summary>
		[DataMember]
		public int ProgressMax { get; protected set; }

		/// <summary>
		/// 任務出現タイプ
		/// </summary>
		[DataMember]
		public int QuestType { get; protected set; }

		/// <summary>
		/// 未ロード時の進捗
		/// </summary>
		[DataMember]
		public int TemporaryProgress { get; protected set; }

		/// <summary>
		/// 共有カウンタの進捗ずれ
		/// 開発任務など、カウンタが共用になっている任務のずれ補正用です
		/// </summary>
		[DataMember]
		public int SharedCounterShift { get; set; }

		/// <summary>
		/// 加算/減算時に進捗カウンタ修正を行うか
		/// </summary>
		[DataMember]
		public bool IgnoreCheckProgress { get; set; }


		/// <summary>
		/// 進捗率
		/// </summary>
		[IgnoreDataMember]
		public virtual double ProgressPercentage => (double)Progress / ProgressMax;

		/// <summary>
		/// クリア済みかどうか
		/// </summary>
		[IgnoreDataMember]
		public bool IsCleared => ProgressPercentage >= 1.0;


		public ProgressData(QuestData quest, int maxCount)
		{
			QuestID = quest.QuestID;
			ProgressMax = maxCount;
			QuestType = quest.Type;
			TemporaryProgress = 0;
			SharedCounterShift = 0;
			IgnoreCheckProgress = false;
		}



		/// <summary>
		/// 進捗を1増やします。
		/// </summary>
		public virtual void Increment()
		{

			var q = KCDatabase.Instance.Quest[QuestID];

			if (q == null)
			{
				TemporaryProgress++;
				return;
			}

			if (q.State != 2)
				return;


			if (!IgnoreCheckProgress)
				CheckProgress(q);

			Progress = Math.Min(Progress + 1, ProgressMax);
		}

		/// <summary>
		/// 進捗を1減らします。
		/// </summary>
		public virtual void Decrement()
		{

			var q = KCDatabase.Instance.Quest[QuestID];

			if (q != null && q.State == 3)      //達成済なら無視
				return;


			Progress = Math.Max(Progress - 1, 0);

			if (!IgnoreCheckProgress)
				CheckProgress(q);
		}


		public override string ToString() => $"{Progress}/{ProgressMax}";



		/// <summary>
		/// 実際の進捗データから、進捗度を補正します。
		/// </summary>
		/// <param name="q">任務データ。</param>
		public virtual void CheckProgress(QuestData q)
		{

			if (TemporaryProgress > 0)
			{
				if (q.State == 2)
					Progress = Math.Min(Progress + TemporaryProgress, ProgressMax);
				TemporaryProgress = 0;
			}

			if (QuestType == 0)     // ver. 1.6.6 以前のデータとの互換性維持
				QuestType = q.Type;

			switch (q.Progress)
			{
				case 1:     //50%
					switch (q.QuestID)
					{
						case 337: // rounds to 2/3
							Progress = 1;
							break;

						default:
							Progress = (int)Math.Max(Progress, Math.Ceiling((ProgressMax + SharedCounterShift) * 0.5) - SharedCounterShift);
							break;
					}
					break;
				case 2:     //80%
					switch (q.QuestID)
					{
						case 330: // rounds to 4/4
							Progress = 3;
							break;

						case 337: // rounds to 3/3
							Progress = 2;
							break;

						default:
							Progress = (int)Math.Max(Progress, Math.Ceiling((ProgressMax + SharedCounterShift) * 0.8) - SharedCounterShift);
							break;
					}
					break;

			}

		}


		/// <summary>
		/// この任務の達成に必要な条件を表す文字列を返します。
		/// </summary>
		/// <returns></returns>
		public abstract string GetClearCondition();

		[IgnoreDataMember]
		public int ID => QuestID;
	}

}
