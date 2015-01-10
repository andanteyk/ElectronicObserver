using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission {
	
	public class result : APIBase {

		private int _fleetID;


		public override bool IsRequestSupported { get { return true; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			_fleetID = int.Parse( data["api_deck_id"] );
			
			base.OnRequestReceived( data );
		}

		public override void OnResponseReceived( dynamic data ) {

			var fleet = KCDatabase.Instance.Fleet[_fleetID];


			Utility.Logger.Add( 2, string.Format( "{0}が遠征「{1}」から帰投しました。({2})", fleet.Name, data.api_quest_name, Constants.GetExpeditionResult( (int)data.api_clear_result ) ) );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_mission/result"; }
		}
	}
}
