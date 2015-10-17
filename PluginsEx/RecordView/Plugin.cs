using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RecordView
{
	public class Plugin : ServerPlugin
	{

		public override string MenuTitle
		{
			get { return "记录浏览"; }

		}

		public override string Version
		{
			get { return "1.0.0.6"; }
		}

		public override Form GetToolWindow()
		{
            return new RecordViewer();
		}

        public override bool RunService(ElectronicObserver.Window.FormMain main)
        {
            ElectronicObserver.Observer.APIObserver.Instance.APIList["api_req_sortie/battleresult"].ResponseReceived += Plugin_ResponseReceived;
            ElectronicObserver.Observer.APIObserver.Instance.APIList["api_req_practice/battle_result"].ResponseReceived += Plugin_ResponseReceived;
            ElectronicObserver.Observer.APIObserver.Instance.APIList["api_req_combined_battle/battleresult"].ResponseReceived += Plugin_ResponseReceived;
            
            RecordViewer.ConfigFile = Application.StartupPath + "\\Settings\\RecordViewer.xml";//Settings\\
            LoadConfig();
            //api_req_sortie/airbattle
            //api_req_battle_midnight@battle

            for (int i = 0; i < main.MainMenuStrip.Items.Count; i++)
            {
                if (main.MainMenuStrip.Items[i].Name == "StripMenu_Tool")
                {
                    var aa = (ToolStripMenuItem)main.MainMenuStrip.Items[i];
                    aa.DropDownItems.Add("记录浏览").Click += Plugin_Click;
                }
            }

            return true;
        }

        public void LoadConfig()
        {
            try
            {
                if (System.IO.File.Exists(RecordViewer.ConfigFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(RecordViewer.ConfigFile);
                    var Root = doc.DocumentElement;

                    string isSave = Root.GetAttribute("SaveBattleLog");
                    if (isSave == "True")
                    {
                        RecordViewer.SaveBattleRecord = true;
                    }
                    else
                    {
                        RecordViewer.SaveBattleRecord = false;
                    }

                    string FileNamePattern = Root.GetAttribute("FileNamePattern");
                    if (FileNamePattern == null || FileNamePattern == "")
                        FileNamePattern = PatternModifier.DefaultPattern;
                    BattleViewer.FileNamePattern = FileNamePattern;
                }
            }
            catch
            {
            }
        }

        void Plugin_Click(object sender, EventArgs e)
        {
            RecordViewer viewer = new RecordViewer();
            viewer.Show();
        }

        void Plugin_ResponseReceived(string apiname, dynamic data)
        {
            if (RecordViewer.SaveBattleRecord)
                BattleView.GenerateBattleRecord();
        }
	}
}
