using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 近代化改修任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressModernization" )]
	public class ProgressModernization : ProgressData {

		public ProgressModernization( QuestData quest, int maxCount )
			: base( quest, maxCount ) {
		}

		public override string GetClearCondition() {
			return "近代化改修" + ProgressMax;
		}
	}
}
