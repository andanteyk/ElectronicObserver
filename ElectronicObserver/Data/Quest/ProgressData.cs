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
		}



		/// <summary>
		/// 進捗を1増やします。
		/// </summary>
		public virtual void Increment() {
			
			var q = KCDatabase.Instance.Quest[QuestID];

			// 任務が存在しないか遂行中でない場合スキップ
			if ( q == null ||q.State != 2 )
				return;

			CheckProgress( q.Progress );


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
		/// <param name="progressFlag">任務データの進捗度。</param>
		public virtual void CheckProgress( int progressFlag ) {

			switch ( progressFlag ) {
				case 1:
					Progress = (int)Math.Max( Progress, Math.Ceiling( ProgressMax * 0.5 ) );
					break;
				case 2:
					Progress = (int)Math.Max( Progress, Math.Ceiling( ProgressMax * 0.8 ) );
					break;
			}

		}


		[IgnoreDataMember]
		public int ID {
			get { return QuestID; }
		}
	}
}
