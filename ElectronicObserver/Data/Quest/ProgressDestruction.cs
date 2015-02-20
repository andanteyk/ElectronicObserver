using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 艦船解体任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressDestruction" )]
	public class ProgressDestruction : ProgressData {

		public ProgressDestruction( int questID, int maxCount )
			: base( questID, maxCount ) {
		}
	}
}
