using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 艦船建造任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressConstruction" )]
	public class ProgressConstruction : ProgressData {

		public ProgressConstruction( int questID, int maxCount )
			: base( questID, maxCount ) {
		}

	}
}
