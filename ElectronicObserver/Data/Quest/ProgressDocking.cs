using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 入渠任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressDocking" )]
	public class ProgressDocking : ProgressData {

		public ProgressDocking( int questID, int maxCount )
			: base( questID, maxCount ) {
		}

		public override string GetClearCondition() {
			return "入渠" + ProgressMax;
		}
	}
}
