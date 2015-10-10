using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 任務の進捗を管理する基底クラスです。
	/// </summary>
	[DataContract( Name = "ProgressData" )]
	public abstract class ProgressData : IIdentifiable {

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
		public virtual int ProgressMax { get; protected set; }


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
		/// 开始进度时间戳
		/// </summary>
		[DataMember]
		public long StartTimeTicks { get; set; }

		/// <summary>
		/// 任务类型
		/// </summary>
		[DataMember]
		public int Type { get; set; }


		/// <summary>
		/// 進捗率
		/// </summary>
		[IgnoreDataMember]
		public virtual double ProgressPercentage {
			get { return (double)Progress / ProgressMax; }
		}

		/// <summary>
		/// クリア済みかどうか
		/// </summary>
		[IgnoreDataMember]
		public bool IsCleared {
			get { return ProgressPercentage >= 1.0; }
		}


		public ProgressData( int questID, int maxCount ) {
			QuestID = questID;
			ProgressMax = maxCount;
			TemporaryProgress = 0;
			SharedCounterShift = 0;
			StartTimeTicks = DateTime.Now.Ticks;
		}



		/// <summary>
		/// 進捗を1増やします。
		/// </summary>
		public virtual void Increment() {

			var q = KCDatabase.Instance.Quest[QuestID];

			if ( q == null ) {
				TemporaryProgress++;
				return;
			}

			if ( q.State != 2 )
				return;



			CheckProgress( q );


			Progress = Math.Min( Progress + 1, ProgressMax );

			//DEBUG
			//Utility.Logger.Add( 1, string.Format( "Quest++: [{0}] {1} {2}/{3}", QuestID, this.GetType().Name, Progress, ProgressMax ) );
		}

		public override string ToString() {
			return string.Format( "{0}/{1}", Progress, ProgressMax );
		}


		/// <summary>
		/// 実際の進捗データから、進捗度を補正します。
		/// </summary>
		/// <param name="q">任務データ。</param>
		public virtual void CheckProgress( QuestData q ) {

			if ( TemporaryProgress > 0 ) {
				if ( q.State == 2 )
					Progress = Math.Min( Progress + TemporaryProgress, ProgressMax );
				TemporaryProgress = 0;
			}

			switch ( q.Progress ) {
				case 1:		//50%
					Progress = (int)Math.Max( Progress, Math.Ceiling( ( ProgressMax + SharedCounterShift ) * 0.5 ) - SharedCounterShift );
					break;
				case 2:		//80%
					Progress = (int)Math.Max( Progress, Math.Ceiling( ( ProgressMax + SharedCounterShift ) * 0.8 ) - SharedCounterShift );
					break;
			}

			if ( Progress < 0 )
				Progress = 0;

		}


		/// <summary>
		/// この任務の達成に必要な条件を表す文字列を返します。
		/// </summary>
		/// <returns></returns>
		public abstract string GetClearCondition();

		[IgnoreDataMember]
		public int ID {
			get { return QuestID; }
		}
	}
}
