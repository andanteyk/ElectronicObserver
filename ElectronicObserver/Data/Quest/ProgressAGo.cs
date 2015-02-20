using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 任務「あ号作戦」の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressAGo" )]
	public class ProgressAGo : ProgressData {

		/// <summary>
		/// 達成に必要な出撃回数
		/// </summary>
		[IgnoreDataMember]
		private int sortieMax { get { return 36; } }
		
		/// <summary>
		/// 達成に必要なS勝利回数
		/// </summary>
		[IgnoreDataMember]
		private int sWinMax { get { return 6; } }
		
		/// <summary>
		/// 達成に必要なボス戦闘回数
		/// </summary>
		[IgnoreDataMember]
		private int bossMax { get { return 24; } }

		/// <summary>
		/// 達成に必要なボス勝利回数
		/// </summary>
		[IgnoreDataMember]
		private int bossWinMax { get { return 12; } }


		/// <summary>
		/// 現在の出撃回数
		/// </summary>
		[IgnoreDataMember]
		private int sortieCount {
			get { return Progress & 0xFF; }
			set { Progress = ( Progress & ~0xFF ) | ( value & 0xFF ); }
		}

		/// <summary>
		/// 現在のS勝利回数
		/// </summary>
		[IgnoreDataMember]
		private int sWinCount {
			get { return ( Progress >> 8 ) & 0xFF; }
			set { Progress = ( Progress & ~( 0xFF << 8 ) ) | ( ( value & 0xFF ) << 8 ); }
		}

		/// <summary>
		/// 現在のボス戦闘回数
		/// </summary>
		[IgnoreDataMember]
		private int bossCount {
			get { return ( Progress >> 16 ) & 0xFF; }
			set { Progress = ( Progress & ~( 0xFF << 16 ) ) | ( ( value & 0xFF ) << 16 ); }
		}

		/// <summary>
		/// 現在のボス勝利回数
		/// </summary>
		[IgnoreDataMember]
		private int bossWinCount {
			get { return ( Progress >> 24 ) & 0xFF; }
			set { Progress = ( Progress & ~( 0xFF << 24 ) ) | ( ( value & 0xFF ) << 24 ); }
		}



		public ProgressAGo( int questID )
			: base( questID, 0 ) {
		}


		public override double ProgressPercentage {
			get {
				double prog = 0;
				prog += Math.Min( (double)sortieCount / sortieMax, 1.0 ) * 0.25;
				prog += Math.Min( (double)sWinCount / sWinMax, 1.0 ) * 0.25;
				prog += Math.Min( (double)bossCount / bossMax, 1.0 ) * 0.25;
				prog += Math.Min( (double)bossWinCount / bossWinMax, 1.0 ) * 0.25;
				return prog;
			}
		}



		public override void Increment() {
			throw new NotSupportedException();
		}


		public override void CheckProgress( int progressFlag ) {
			//なにもしない
		}


		/// <summary>
		/// 出撃回数を増やします。
		/// </summary>
		public void IncrementSortie() {
			sortieCount = Math.Min( sortieCount + 1, sortieMax );
		}

		/// <summary>
		/// 戦闘回数を増やします。
		/// </summary>
		public void IncrementBattle( string rank, bool isBoss ) {

			int irank = Constants.GetWinRank( rank );

			if ( isBoss ) {
				bossCount = Math.Min( bossCount + 1, bossMax );

				if ( irank >= Constants.GetWinRank( "B" ) )
					bossWinCount = Math.Min( bossWinCount + 1, bossWinMax );
			}

			if ( irank >= Constants.GetWinRank( "S" ) )
				sWinCount = Math.Min( sWinCount + 1, sWinMax );

		}


		public override string ToString() {
			return string.Format( "出撃 {0}/{1}, S勝利 {2}/{3}, ボス {4}/{5}, ボス勝利 {6}/{7} ({8:p})",
				sortieCount, sortieMax,
				sWinCount, sWinMax,
				bossCount, bossMax,
				bossWinCount, bossMax,
				ProgressPercentage );
		}

	}

}
