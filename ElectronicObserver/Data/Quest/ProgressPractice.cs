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


		public ProgressPractice( int questID, int maxCount, bool winOnly )
			: base( questID, maxCount ) {

			WinOnly = winOnly;
		}


		public override void Increment() {
			throw new NotSupportedException();
		}

		public void Increment( string rank ) {

			if ( WinOnly && Constants.GetWinRank( rank ) < Constants.GetWinRank( "B" ) )
				return;

			Increment();
		}

	}
}
