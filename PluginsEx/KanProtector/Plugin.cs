using ElectronicObserver.Window.Plugins;
using ElectronicObserver;
using ElectronicObserver.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;
using Fiddler;

namespace KanProtector
{

    public class MissionPlugin : ServerPlugin
    {
        public override string MenuTitle
        {
            get { return "侠客行"; }
        }

        public override string Version
        {
            get { return "1.0.0.1"; }//MainDockPanel
        }
        public override PluginSettingControl GetSettings()
        {
            return new Settings();
        }

        public override bool RunService(ElectronicObserver.Window.FormMain main)
        {
            ProtectionData.Instance.LoadConfig();
            Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            return true;
        }

        void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (!ProtectionData.Instance.ShipProtectionEnabled && !ProtectionData.Instance.EquipmentProtectionEnabled)
                return;
            int index = oSession.fullUrl.IndexOf("/kcsapi/");
            if (index > 0)
            {
                string api = oSession.fullUrl.Substring(oSession.fullUrl.IndexOf("/kcsapi/") + 8);
                string Warning = null;
                switch (api)
                {
                    case "api_req_kousyou/destroyship":
                        Warning = HandleRequest.OnDestroyShip(oSession.GetRequestBodyAsString());
                        if (Warning != null)
                        {
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            //oSession.utilCreateResponseAndBypassServer();
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                        }
                        else
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", "拆船没有问题"));
                        break;
                    case "api_req_kousyou/destroyitem2":
                        if (!ProtectionData.Instance.EquipmentProtectionEnabled)
                            return;
                        Warning = HandleRequest.OnDestroyItem(oSession.GetRequestBodyAsString());
                        if (Warning != null)
                        {
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            //oSession.utilCreateResponseAndBypassServer();
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                        }
                        else
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", "废弃没有问题"));
                        break;
                    case "api_req_kaisou/powerup":
                        Warning = HandleRequest.OnPowerUp(oSession.GetRequestBodyAsString());
                        if (Warning != null)
                        {
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            //oSession.utilCreateResponseAndBypassServer();
                            //oSession.oResponse.headers.HTTPResponseCode = 502;
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                        }
                        else
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", "强化没有问题"));
                        break;
                }
            }
        }

    }
}
