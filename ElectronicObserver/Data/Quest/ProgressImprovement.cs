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

		public ProgressImprovement( QuestData quest, int maxCount )
			: base( quest, maxCount ) {
		}

		public override string GetClearCondition() {
			return "装備改修" + ProgressMax;
		}
	}
}
