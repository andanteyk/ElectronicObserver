using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	[DataContract( Name = "ProgressMultiBattle" )]
	public class ProgressMultiBattle : ProgressData {

		[DataMember]
		private ProgressBattle[] ProgressList;

		public ProgressMultiBattle( QuestData quest, IEnumerable<ProgressBattle> progressList )
			: base( quest, 1 ) {
			ProgressList = progressList.ToArray();
			ProgressMax = ProgressList.Sum( p => p.ProgressMax );
		}


		public void Increment( string rank, int areaID, bool isBoss ) {
			foreach ( var p in ProgressList )
				p.Increment( rank, areaID, isBoss );

			Progress = ProgressList.Sum( p => p.Progress );
		}

		public override void Increment() {
			throw new NotSupportedException();
		}

		public override void Decrement() {
			throw new NotSupportedException();
		}

		public override void CheckProgress( QuestData q ) {
			// do nothing
		}

		public override string ToString() {
			if ( ProgressList.All( p => p.IsCleared ) )
				return "達成！";
			else
				return string.Join( ", ", ProgressList.Where( p => !p.IsCleared ).Select( p => p.GetClearCondition() + ": " + p.ToString() ) );
		}

		public override string GetClearCondition() {
			return string.Join( ", ", ProgressList.Select( p => p.GetClearCondition() ) );
		}
	}
}
