using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_quest {
	
	public static class clearitemget {

		public static void LoadFromRequest( string apiname, string data ) {

			KCDatabase db = KCDatabase.Instance;

			db.Quest.LoadFromRequest( apiname, data );


			db.OnQuestUpdated();
		}

	}

}
