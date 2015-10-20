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

    public class ProtectionPlugin : ServerPlugin
    {
        public static bool DebugMode = false;

        public override string MenuTitle
        {
            get { return "侠客行"; }
        }

        public override string Version
        {
            get { return "1.0.0.2"; }//MainDockPanel
        }
        public override PluginSettingControl GetSettings()
        {
            return new Settings();
        }

        public override bool RunService(ElectronicObserver.Window.FormMain main)
        {
            ProtectionData.Instance.LoadConfig();
            //ElectronicObserver.Observer.APIObserver.Instance.APIList.
            Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            return true;
        }

        void FiddlerApplication_BeforeRequest(Session oSession)
        {
            //ElectronicObserver.Data.KCDatabase.Instance.MasterShips[288].RemodelAfterShipID = 461;
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
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                            if (!DebugMode)
                            {
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                                oSession.utilCreateResponseAndBypassServer();
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                            }
                        }

                        break;
                    case "api_req_kousyou/destroyitem2":
                        if (!ProtectionData.Instance.EquipmentProtectionEnabled)
                            return;
                        Warning = HandleRequest.OnDestroyItem(oSession.GetRequestBodyAsString());
                        if (Warning != null)
                        {
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                            if (!DebugMode)
                            {
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                                oSession.utilCreateResponseAndBypassServer();
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                            }                        
                        }

                        break;
                    case "api_req_kaisou/powerup":
                        Warning = HandleRequest.OnPowerUp(oSession.GetRequestBodyAsString());
                        if (Warning != null)
                        {
                            ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                            if (!DebugMode)
                            {
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                                oSession.utilCreateResponseAndBypassServer();
                                oSession.oResponse.headers.HTTPResponseCode = 502;
                            }                     
                        }

                        break;
                    case "api_req_kaisou/lock":
                        //Warning = HandleRequest.OnLock(oSession.GetRequestBodyAsString());
                        //if (Warning != null)
                        //{
                        //    ElectronicObserver.Utility.Logger.Add(3, string.Format("{0}", Warning));
                        //}

                        break;
                }
            }
        }

    }
}
