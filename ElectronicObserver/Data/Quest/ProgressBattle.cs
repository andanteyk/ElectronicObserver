using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 戦闘系の任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressBattle" )]
	public class ProgressBattle : ProgressData {

		/// <summary>
		/// 条件を満たす最低ランク
		/// </summary>
		[DataMember]
		private int LowestRank { get; set; }

		/// <summary>
		/// 対象となる海域
		/// </summary>
		[DataMember]
		private HashSet<int> TargetArea { get; set; }

		/// <summary>
		/// ボス限定かどうか
		/// </summary>
		[DataMember]
		private bool IsBossOnly { get; set; }


		public ProgressBattle( int questID, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly )
			: base( questID, maxCount ) {

			LowestRank = Constants.GetWinRank( lowestRank );
			TargetArea = targetArea == null ? null : new HashSet<int>( targetArea );
			IsBossOnly = isBossOnly;
		}


		public override void Increment() {
			throw new NotSupportedException();
		}

		public void Increment( string rank, int areaID, bool isBoss ) {

			if ( TargetArea != null && !TargetArea.Contains( areaID ) )
				return;

			if ( Constants.GetWinRank( rank ) < LowestRank )
				return;

			if ( IsBossOnly && !isBoss )
				return;


			Increment();
		}


	}

}
