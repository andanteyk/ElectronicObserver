using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 特定艦種撃沈任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressSlaughter" )]
	public class ProgressSlaughter : ProgressData {

		/// <summary>
		/// 対象となる艦種リスト
		/// </summary>
		[DataMember]
		private HashSet<int> TargetShipType { get; set; }

		public ProgressSlaughter( QuestData quest, int maxCount, int[] targetShipType )
			: base( quest, maxCount ) {

			TargetShipType = targetShipType == null ? null : new HashSet<int>( targetShipType );

		}


		public void Increment( int shipTypeID ) {
			if ( TargetShipType.Contains( shipTypeID ) )
				Increment();
		}


		public override string GetClearCondition() {
			StringBuilder sb = new StringBuilder();
			if ( TargetShipType != null ) {
				sb.Append( string.Join( "・", TargetShipType.OrderBy( s => s ).Select( s => KCDatabase.Instance.ShipTypes[s].Name ) ) );
			}

			sb.Append( "撃沈" );
			sb.Append( ProgressMax );

			return sb.ToString();
		}
	}
}
