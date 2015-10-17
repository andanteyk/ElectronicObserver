using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MissionHelper
{
    public partial class MissionHelperForm : DockContent
    {
        MissionData missionData;
        public string ConfigFile;
        Dictionary<int, bool> Filters = new Dictionary<int, bool>();
        public MissionHelperForm()
        {
            InitializeComponent();
            missionData = MissionData.missionData;
            ConfigFile = Application.StartupPath + "\\Settings\\MissionHelper.xml";//Settings\\
        }

        public void DisplayBaseData()
        {
            foreach (var md in missionData.Data)
            {
                if (Filters.ContainsKey(md.No) && Filters[md.No])
                    continue;
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells["colID"].Value = md.No;
                if (md.AmmoCost == 0)
                {
                    dataGridView1.Rows[row].Cells["colID"].Style.BackColor = Color.LimeGreen;
                }
                dataGridView1.Rows[row].Cells["colName"].Value = md.Name;
                dataGridView1.Rows[row].Cells["colName"].ToolTipText = md.GreatSuccess.Text;
                if (md.GreatSuccess.HaveSpecialRequirement)
                {
                    dataGridView1.Rows[row].Cells["colName"].Value = "☆" + md.Name + "☆";
                }
            }
        }

        Resource CalcRealIncome(Resource income, Resource cost, MissionResult result, int NumDF, double TimeRatio)
        {
            Resource res = new Resource();
            if (result.Result == MissionSuccess.Fail)
                return null;
            double ratio = 1;
            if (result.Result == MissionSuccess.Great)
                ratio = 1.5;
            ratio *= (1 + 0.05 * NumDF);
            res.Fuel = (int)(((int)(income.Fuel * ratio) - cost.Fuel) * TimeRatio);
            res.Ammo = (int)(((int)(income.Ammo * ratio) - cost.Ammo) * TimeRatio);
            res.Steel = (int)(income.Steel * ratio * TimeRatio);
            res.Al = (int)(income.Al * ratio * TimeRatio);
            return res;
        }

        public void RefreshSuccess()
        {
            try
            {
                ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[1].Name.ToString();
                //ElectronicObserver.Data.KCDatabase.Instance.MapInfo[1].
                
            }
            catch
            {
                return;
            }
            for (int Index = 0; Index < dataGridView1.Rows.Count; Index++)
            {
                int ID = 0;
                ID = (int)(dataGridView1.Rows[Index].Cells["colID"].Value);
                MissionInformation mi = missionData.GetMission(ID);
                string success = "";
                for (int i = 0; i < 3; i++)
                {
                    string Tps = "";
                    MissionResult r = mi.GetMissionResult(i + 2);
                    if (r.Result == MissionSuccess.Great)
                    {
                        success = "☆";
                        Resource Cost = FleetData.GetCost(i + 2, mi.FuelCost, mi.AmmoCost);
                        Resource Income = mi.GetIncome();
                        Resource Real = CalcRealIncome(Income, Cost, mi.GetMissionResult(i + 2), FleetData.Get大发(i + 2), 1);
                        if (Real != null)
                        {
                            Tps += "油：" + Real.Fuel.ToString() + Environment.NewLine;
                            Tps += "弹：" + Real.Ammo.ToString() + Environment.NewLine;
                            Tps += "钢：" + Real.Steel.ToString() + Environment.NewLine;
                            Tps += "铝：" + Real.Al.ToString() + Environment.NewLine;
                        }
                    }
                    if (r.Result == MissionSuccess.Success)
                    {
                        success = "○";
                        Resource Cost = FleetData.GetCost(i + 2, mi.FuelCost, mi.AmmoCost);
                        Resource Income = mi.GetIncome();
                        Resource Real = CalcRealIncome(Income, Cost, mi.GetMissionResult(i + 2), FleetData.Get大发(i + 2), 1);
                        if (Real != null)
                        {
                            Tps += "油：" + Real.Fuel.ToString() + Environment.NewLine;
                            Tps += "弹：" + Real.Ammo.ToString() + Environment.NewLine;
                            Tps += "钢：" + Real.Steel.ToString() + Environment.NewLine;
                            Tps += "铝：" + Real.Al.ToString() + Environment.NewLine;
                        }
                    }
                    if (r.Result == MissionSuccess.Fail)
                    {
                        success = "×";
                       
                        foreach (var s in r.InvalidCondition)
                        {
                            Tps += s + Environment.NewLine;
                        }
                    }
                    dataGridView1.Rows[Index].Cells["colFleet" + (i + 2).ToString()].ToolTipText = Tps;
                    dataGridView1.Rows[Index].Cells["colFleet" + (i + 2).ToString()].Value = success;
                }
            }
        }

        public void LoadConfig()
        {
            try
            {
                if (System.IO.File.Exists(ConfigFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ConfigFile);
                    var Root = doc.DocumentElement;
                    var Node = Root.SelectSingleNode("Dock/Filter");
                    if (Node != null)
                    {
                        var Ele = Node as XmlElement;
                        foreach (XmlElement subnode in Ele)
                        {
                            int ID;
                            if (int.TryParse(subnode.GetAttribute("ID"), out ID))
                            {
                                bool TF = subnode.GetAttribute("Filter") == "True";
                                Filters[ID] = TF;
                            }
                        }
                    }

                    Node = Root.SelectSingleNode("Dock/Width");
                    if (Node != null)
                    {
                        var Ele = Node as XmlElement;
                        foreach (XmlElement subnode in Ele)
                        {
                            string ColName = subnode.GetAttribute("ColName");
                            int Width;
                            if (int.TryParse(subnode.GetAttribute("ColWidth"), out Width))
                            {
                                if (dataGridView1.Columns.Contains(ColName))
                                {
                                    dataGridView1.Columns[ColName].Width = Width;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!System.IO.File.Exists(ConfigFile))
                {
                    XmlElement xmlelem = doc.CreateElement("Config");
                    doc.AppendChild(xmlelem);
                }
                else
                {
                    doc.Load(ConfigFile);
                }
                var Root = doc.DocumentElement;
                var NodeDock = Root.SelectSingleNode("Dock");
                if (NodeDock == null)
                {
                    NodeDock = doc.CreateElement("Dock");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    Root.AppendChild(NodeDock);
                }
                var NodeFilter = NodeDock.SelectSingleNode("Filter");
                if (NodeFilter == null)
                {
                    NodeFilter = doc.CreateElement("Filter");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    NodeDock.AppendChild(NodeFilter);
                }
                NodeFilter.RemoveAll();
                foreach (var filter in Filters)
                {
                    XmlElement Element = doc.CreateElement("Filter");
                    Element.SetAttribute("ID", filter.Key.ToString());
                    Element.SetAttribute("Filter", filter.Value.ToString());
                    NodeFilter.AppendChild(Element);
                }
                var NodeWidth = NodeDock.SelectSingleNode("Width");
                if (NodeWidth == null)
                {
                    NodeWidth = doc.CreateElement("Width");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    NodeDock.AppendChild(NodeWidth);
                }
                NodeWidth.RemoveAll();
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    XmlElement Element = doc.CreateElement("Width");
                    Element.SetAttribute("ColName", dataGridView1.Columns[i].Name);
                    Element.SetAttribute("ColWidth", dataGridView1.Columns[i].Width.ToString());
                    NodeWidth.AppendChild(Element);
                }
                doc.Save(ConfigFile);
            }
            catch
            {
            }
        }
        private void MissionHelperForm_Load(object sender, EventArgs e)
        {
            this.Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
            this.HideOnClose = true;

            dataGridView1.BackgroundColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
            dataGridView1.DefaultCellStyle.BackColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
            ElectronicObserver.Utility.Configuration.Instance.ConfigurationChanged += Instance_ConfigurationChanged;


            LoadConfig();

            DisplayBaseData();
            RefreshSuccess();
            try
            {
                ElectronicObserver.Utility.SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;

                ElectronicObserver.Observer.APIObserver o = ElectronicObserver.Observer.APIObserver.Instance;

                o.APIList["api_req_nyukyo/start"].RequestReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_nyukyo/speedchange"].RequestReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_hensei/change"].RequestReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_kousyou/destroyship"].RequestReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_kaisou/remodeling"].RequestReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_map/start"].RequestReceived += MissionHelperForm_RequestReceived;
                //o.APIList["api_req_hensei/combined"].RequestReceived += MissionHelperForm_RequestReceived;

                o.APIList["api_port/port"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_get_member/ship2"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_get_member/ndock"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_hokyu/charge"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_get_member/ship3"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_req_kaisou/powerup"].ResponseReceived += MissionHelperForm_RequestReceived;		//requestのほうは面倒なのでこちらでまとめてやる
                o.APIList["api_get_member/deck"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_get_member/slot_item"].ResponseReceived += MissionHelperForm_RequestReceived;
                //o.APIList["api_req_map/next"].ResponseReceived += MissionHelperForm_RequestReceived;
                o.APIList["api_get_member/ship_deck"].ResponseReceived += MissionHelperForm_RequestReceived;
            }
            catch
            {
            }
        }

        void Instance_ConfigurationChanged()
        {
            dataGridView1.BackgroundColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
            dataGridView1.DefaultCellStyle.BackColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
        }

        void SystemEvents_SystemShuttingDown()
        {
            SaveConfig();
        }

        void MissionHelperForm_RequestReceived(string apiname, dynamic data)
        {
            RefreshSuccess();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool Auto = cbAutoFill.Checked;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = Auto ? DataGridViewAutoSizeColumnMode.AllCells : DataGridViewAutoSizeColumnMode.None;
            }
        }

        private void MissionHelperForm_FormClosing(object sender, FormClosingEventArgs e)
        {  
            ElectronicObserver.Observer.APIObserver o = ElectronicObserver.Observer.APIObserver.Instance;

            //o.APIList["api_req_hensei/change"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kousyou/destroyship"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kaisou/remodeling"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kaisou/powerup"].ResponseReceived -= MissionHelperForm_RequestReceived;

            //o.APIList["api_req_nyukyo/start"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_nyukyo/speedchange"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_hensei/change"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kousyou/destroyship"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_member/updatedeckname"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kaisou/remodeling"].RequestReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_map/start"].RequestReceived -= MissionHelperForm_RequestReceived;
            ////o.APIList["api_req_hensei/combined"].RequestReceived -= MissionHelperForm_RequestReceived;

            //o.APIList["api_port/port"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_get_member/ship2"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_get_member/ndock"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kousyou/getship"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_hokyu/charge"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kousyou/destroyship"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_get_member/ship3"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_kaisou/powerup"].ResponseReceived -= MissionHelperForm_RequestReceived;		//requestのほうは面倒なのでこちらでまとめてやる
            //o.APIList["api_get_member/deck"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_get_member/slot_item"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_map/start"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_req_map/next"].ResponseReceived -= MissionHelperForm_RequestReceived;
            //o.APIList["api_get_member/ship_deck"].ResponseReceived -= MissionHelperForm_RequestReceived;

            SaveConfig();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItem2.Checked = cbAutoFill.Checked;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            cbAutoFill.Checked = !cbAutoFill.Checked;
        }

        private void 完整列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MissionHelperPopup form = new MissionHelperPopup();
            form.Show();
        }

        private void 远征过滤ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterForm form = new FilterForm();
            form.Filters = Filters;
            form.ShowDialog();
            dataGridView1.Rows.Clear();
            DisplayBaseData();
            RefreshSuccess();
        }
    }
}

