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


		public ProgressBattle( QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly )
			: base( quest, maxCount ) {

			LowestRank = Constants.GetWinRank( lowestRank );
			TargetArea = targetArea == null ? null : new HashSet<int>( targetArea );
			IsBossOnly = isBossOnly;
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



		public override string GetClearCondition() {
			StringBuilder sb = new StringBuilder();
			if ( TargetArea != null ) {
				sb.Append( string.Join( "・", TargetArea.OrderBy( s => s ).Select( s => string.Format( "{0}-{1}", s / 10, s % 10 ) ) ) );
			}
			if ( IsBossOnly )
				sb.Append( "ボス" );
			switch ( LowestRank ) {
				case 1:
				default:
					sb.Append( "戦闘" );
					break;
				case 2:
				case 3:
					sb.Append( Constants.GetWinRank( LowestRank ) + "以上" );
					break;
				case 4:
					sb.Append( "勝利" );
					break;
				case 5:
				case 6:
				case 7:
					sb.Append( Constants.GetWinRank( LowestRank ) + "勝利" );
					break;
			}
			sb.Append( ProgressMax );

			return sb.ToString();
		}
	}

}
