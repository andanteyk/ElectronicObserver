using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 装備廃棄任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressDiscard" )]
	public class ProgressDiscard : ProgressData {

		public ProgressDiscard( int questID, int maxCount )
			: base( questID, maxCount ) {
		}
	}
}
