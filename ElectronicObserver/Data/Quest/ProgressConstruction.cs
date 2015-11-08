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

		public ProgressConstruction( QuestData quest, int maxCount )
			: base( quest, maxCount ) {
		}


		public override string GetClearCondition() {
			return "建造" + ProgressMax;
		}
	}
}
