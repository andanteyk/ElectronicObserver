using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class QuestData : ResponseWrapper, IIdentifiable {

		public int QuestID {
			get { return (int)RawData.api_no; }
		}

		public int Category {
			get { return (int)RawData.api_category; }
		}

		public int State {
			get { return (int)RawData.api_state; }
		}

		public string Name {
			get { return (string)RawData.api_title; }
		}

		public string Description {
			get { return (string)RawData.api_detail; }
		}

		//undone:api_bonus_flag

		public int Progress {
			get { return (int)RawData.api_progress_flag; }
		}



		public int ID {
			get { return QuestID; }
		}

	}

}
