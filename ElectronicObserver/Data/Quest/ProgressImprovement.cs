using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 装備改修任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressImprovement" )]
	public class ProgressImprovement : ProgressData {

		public ProgressImprovement( int questID, int maxCount )
			: base( questID, maxCount ) {
		}
	}
}
