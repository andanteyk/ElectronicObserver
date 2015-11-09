using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 演習任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressPractice" )]
	public class ProgressPractice : ProgressData {

		/// <summary>
		/// 勝利のみカウントする
		/// </summary>
		[DataMember]
		private bool WinOnly { get; set; }


		public ProgressPractice( QuestData quest, int maxCount, bool winOnly )
			: base( quest, maxCount ) {

			WinOnly = winOnly;
		}


		public void Increment( string rank ) {

			if ( WinOnly && Constants.GetWinRank( rank ) < Constants.GetWinRank( "B" ) )
				return;

			Increment();
		}


		public override string GetClearCondition() {
			return "演習" + ( WinOnly ? "勝利" : "" ) + ProgressMax;
		}
	}
}
