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


			Utility.Logger.Add( 2, string.Format( "#{0}「{1}」的远征「{2}: {3}」已归来。({4})", fleet.FleetID, fleet.Name, fleet.ExpeditionDestination, data.api_quest_name, Constants.GetExpeditionResult( (int)data.api_clear_result ) ) );

			base.OnResponseReceived( (object)data );
		}

		public override string APIName {
			get { return "api_req_mission/result"; }
		}
	}
}
