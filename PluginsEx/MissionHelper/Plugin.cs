using ElectronicObserver.Window.Plugins;
using ElectronicObserver;
using ElectronicObserver.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;

namespace MissionHelper
{

    public class MissionPlugin : ServerPlugin
    {
        MissionHelperForm form = new MissionHelperForm();
        ElectronicObserver.Window.FormMain Main = null;
        public override string MenuTitle
        {
            get { return "远征助手"; }
        }

        public override string Version
        {
            get { return "1.0.0.5"; }//MainDockPanel
        } 

        public override bool RunService(ElectronicObserver.Window.FormMain main)
        {
            Main = main;
            Main.SubForms.Add(form);
            for (int i = 0; i < Main.MainMenuStrip.Items.Count; i++)
            {
                if (Main.MainMenuStrip.Items[i].Name == "StripMenu_Tool")
                {
                    var aa = (ToolStripMenuItem)Main.MainMenuStrip.Items[i];
                    aa.DropDownItems.Add("远征助手").Click += MissionWarningPlugin_Click;
                }
            }
           //StripMenu_Tool
            //MissionHelperForm
            try
            {
                ElectronicObserver.Observer.APIObserver.Instance.APIList["api_req_mission/start"].RequestReceived += Plugin_RequestReceived;
            }
            catch
            {
                MessageBox.Show("远征插件 RunService 出错啦");
            }
            return true;
        }

        void MissionWarningPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                form.Show(Main.MainPanel);
            }
            catch
            {
                form = new MissionHelperForm();
                form.Show(Main.MainPanel);
            }
        }

        void Plugin_RequestReceived(string apiname, dynamic data)
        {
            try
            {
                int deckID = int.Parse(data["api_deck_id"]);
                int destination = int.Parse(data["api_mission_id"]);
                MissionInformation mi = MissionData.missionData.GetMission(destination);
                if (mi == null)
                {
                    return;
                }
                if (mi.GetMissionResult(deckID).Result == MissionSuccess.Fail)
                {
                   
                    //ElectronicObserver.Notifier.NotifierManager.
                    WarningForm wf = new WarningForm();
                    wf.Fleet = "舰队" + deckID.ToString();
                    wf.TopMost = true;
                    wf.Show();
                    
                }
            }
            catch
            {
                MessageBox.Show("远征插件  Plugin_RequestReceived 出错啦");
            }

        }

    }
}
