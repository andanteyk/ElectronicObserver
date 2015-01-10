using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_map {
	
	public class start : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Battle.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}


		public override bool IsRequestSupported { get { return true; } }
		
		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase.Instance.Fleet.LoadFromRequest( APIName, data );

			int maparea = int.Parse( data["api_maparea_id"] );
			int mapinfo = int.Parse( data["api_mapinfo_no"] );

			Utility.Logger.Add( 2, string.Format( "{0}が「{1}-{2} {3}」へ出撃しました。", KCDatabase.Instance.Fleet[int.Parse( data["api_deck_id"] )].Name, maparea, mapinfo, KCDatabase.Instance.MapInfo[maparea * 10 + mapinfo].Name ) );

			base.OnRequestReceived( data );
		}


		public override string APIName {
			get { return "api_req_map/start"; }
		}

	}
}
