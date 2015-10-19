using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

    public class Lock : APIBase
    {
        bool isShipLockChanged = true;
        string lock_id = null;
        public override void OnRequestReceived(Dictionary<string, string> data)
        {

            KCDatabase db = KCDatabase.Instance;

            if (data.ContainsKey("api.ship.id"))
            {
                isShipLockChanged = true;
                lock_id = data["api.ship.id"];
            }
            if (data.ContainsKey("api.slotitem.id"))
            {
                isShipLockChanged = false;
                lock_id = data["api.slotitem.id"];
            }
            base.OnRequestReceived(data);
        }


        public override void OnResponseReceived(dynamic data)
        {
            if (isShipLockChanged)
            {

            }
            //KCDatabase db = KCDatabase.Instance;

            //var ship = db.Ships[(int)data.api_ship.api_id];
            //if (ship != null)
            //    ship.LoadFromResponse(APIName, data.api_ship);

            //db.Fleet.LoadFromResponse(APIName, data.api_deck);


            //if (Utility.Configuration.Config.Log.ShowSpoiler)
            //    Utility.Logger.Add(2, string.Format("{0} の近代化改修に{1}しました。", ship.NameWithLevel, ((int)data.api_powerup_flag) != 0 ? "成功" : "失敗"));

            //base.OnResponseReceived((object)data);
        }



        public override bool IsRequestSupported { get { return true; } }
        public override bool IsResponseSupported { get { return true; } }


        public override string APIName
        {
            get { return "api_req_kaisou/lock"; }
        }

    }


}
