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

		public ProgressDiscard( QuestData quest, int maxCount )
			: base( quest, maxCount ) {
		}

		public override string GetClearCondition() {
			return "廃棄" + ProgressMax;
		}
	}
}
