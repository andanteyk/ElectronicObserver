using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {
	
	/// <summary>
	/// 補給任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressSupply" )]
	public class ProgressSupply : ProgressData {

		public ProgressSupply( QuestData quest, int maxCount )
			: base( quest, maxCount ) {
		}

		public override string GetClearCondition() {
			return "補給" + ProgressMax;
		}
	}
}
