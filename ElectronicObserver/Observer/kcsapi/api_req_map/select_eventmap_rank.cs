using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_map {

	public class select_eventmap_rank : APIBase {

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			var mapinfo = KCDatabase.Instance.MapInfo[int.Parse( data["api_maparea_id"] ) * 10 + int.Parse( data["api_map_no"] )];
			if ( mapinfo != null )
				mapinfo.LoadFromRequest( APIName, data );

			base.OnRequestReceived( data );
		}


		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }


		public override string APIName {
			get { return "api_req_map/select_eventmap_rank"; }
		}

	}
}
