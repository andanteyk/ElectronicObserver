using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 遠征の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressExpedition" )]
	public class ProgressExpedition : ProgressData {

		/// <summary>
		/// 対象となる海域
		/// </summary>
		[DataMember]
		private HashSet<int> TargetArea { get; set; }


		public ProgressExpedition( QuestData quest, int maxCount, int[] targetArea )
			: base( quest, maxCount ) {

			TargetArea = targetArea == null ? null : new HashSet<int>( targetArea );
		}


		public void Increment( int areaID ) {

			if ( TargetArea != null && !TargetArea.Contains( areaID ) )
				return;

			Increment();
		}


		public override string GetClearCondition() {
			StringBuilder sb = new StringBuilder();
			if ( TargetArea != null ) {
				sb.Append( string.Join( "・", TargetArea.OrderBy( s => s ).Select( s => KCDatabase.Instance.Mission[s].Name ) ) );
			} else {
				sb.Append( "遠征" );
			}
			sb.Append( ProgressMax );

			return sb.ToString();
		}
	}
}
