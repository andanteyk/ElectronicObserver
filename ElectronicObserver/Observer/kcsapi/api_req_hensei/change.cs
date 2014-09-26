using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_hensei {

	public static class change {

		public static void LoadFromRequest( string apiname, Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			db.Fleet.LoadFromRequest( apiname, data );

			db.OnFleetUpdated();

		}

	}

}
