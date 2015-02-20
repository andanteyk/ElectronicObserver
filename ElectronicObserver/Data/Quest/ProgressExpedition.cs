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


		public ProgressExpedition( int questID, int maxCount, int[] targetArea )
			: base( questID, maxCount ) {

			TargetArea = targetArea == null ? null : new HashSet<int>( targetArea );
		}


		public override void Increment() {
			throw new NotSupportedException();
		}

		public void Increment( int areaID ) {

			if ( TargetArea != null && !TargetArea.Contains( areaID ) )
				return;

			Increment();
		}

	}
}
