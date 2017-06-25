using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class remodeling : APIBase {

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {

			int id = int.Parse( data["api_id"] );
			var ship = KCDatabase.Instance.Ships[id];
			Utility.Logger.Add( 2, string.Format( "{0} Lv. {1} への改装が完了しました。", ship.MasterShip.RemodelAfterShip.NameWithClass, ship.Level ) );

			KCDatabase.Instance.Fleet.LoadFromRequest( APIName, data );
			
			base.OnRequestReceived( data );
		}

		public override string APIName {
			get { return "api_req_kaisou/remodeling"; }
		}
	}
}
