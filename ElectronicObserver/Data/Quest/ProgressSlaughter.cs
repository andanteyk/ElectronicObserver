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

		public ProgressSlaughter( int questID, int maxCount, int[] targetShipType )
			: base( questID, maxCount ) {

			TargetShipType = targetShipType == null ? null : new HashSet<int>( targetShipType );

		}


		public override void Increment() {
			throw new NotSupportedException();
		}

		public void Increment( int shipTypeID ) {
			if ( TargetShipType.Contains( shipTypeID ) )
				Increment();
		}

	}
}
