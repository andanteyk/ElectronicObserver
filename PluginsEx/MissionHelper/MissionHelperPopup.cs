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
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace MissionHelper
{
    public partial class MissionHelperPopup : DockContent
    {
        MissionData missionData;
        int CurrentFleet = 2;
        bool perHour = false;
        public string ConfigFile;
        public MissionHelperPopup()
        {
            InitializeComponent();
            missionData = MissionData.missionData;
            ConfigFile = Application.StartupPath + "\\Settings\\MissionHelper.xml";//Settings\\
            this.Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
        }

        public void DisplayBaseData()
        {
            foreach (var md in missionData.Data)
            {
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
                dataGridView1.Rows[row].Cells["colTime"].Value = md.Time;
                dataGridView1.Rows[row].Cells["colFuel"].Value = md.Fuel;
                dataGridView1.Rows[row].Cells["colAmmo"].Value = md.Ammo;
                dataGridView1.Rows[row].Cells["colSteel"].Value = md.Steel;
                dataGridView1.Rows[row].Cells["colAl"].Value = md.Al;
                string Level = md.shipRequirements.FlagShipLevel.ToString();
                if (md.shipRequirements.FleetLevel > 1)
                    Level += "/" + md.shipRequirements.FleetLevel.ToString();
                dataGridView1.Rows[row].Cells["colLevel"].Value = Level;
                dataGridView1.Rows[row].Cells["colCount"].Value = md.shipRequirements.ShipAmount;
                dataGridView1.Rows[row].Cells["colText"].Value = md.Detail;
                dataGridView1.Rows[row].Cells["colConstruc"].Value = md.ConstrutionMaterial;
                dataGridView1.Rows[row].Cells["colFastConstruc"].Value = md.FastConstruction;
                dataGridView1.Rows[row].Cells["colBucket"].Value = md.Bucket;
                string Coin = md.GetCoin();
                dataGridView1.Rows[row].Cells["colCoin"].Value = Coin;
                dataGridView1.Rows[row].Cells["colText"].Value = md.Detail;
            }
        }

        void aaa()
        {
            // DockContent
        }

        public void RefreshResource()
        {
            try
            {
                ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[1].Name.ToString();
            }
            catch
            {
                return;
            }
            for (int Index = 0; Index < missionData.Data.Count; Index++)
            {
                int ID = 0;
                ID = (int)(dataGridView1.Rows[Index].Cells["colID"].Value);
                MissionInformation mi = missionData.GetMission(ID);
                Resource Cost = FleetData.GetCost(CurrentFleet, mi.FuelCost, mi.AmmoCost);
                Resource Income = mi.GetIncome();
                double TimeRatio = 1;
                if (perHour)
                {
                    TimeRatio = 60.0 / mi.Minute;
                }
                dataGridView1.Rows[Index].Cells["colFuel"].Value = (int)(mi.Fuel * TimeRatio);
                dataGridView1.Rows[Index].Cells["colAmmo"].Value = (int)(mi.Ammo * TimeRatio);
                dataGridView1.Rows[Index].Cells["colSteel"].Value = (int)(mi.Steel * TimeRatio);
                dataGridView1.Rows[Index].Cells["colAl"].Value = (int)(mi.Al * TimeRatio);

                Resource Real = CalcRealIncome(Income, Cost, mi.GetMissionResult(CurrentFleet), FleetData.Get大发(CurrentFleet), TimeRatio);
                if (Real != null)
                {
                    dataGridView1.Rows[Index].Cells["colFuel2"].Value = Real.Fuel;
                    dataGridView1.Rows[Index].Cells["colAmmo2"].Value = Real.Ammo;
                    dataGridView1.Rows[Index].Cells["colSteel2"].Value = Real.Steel;
                    dataGridView1.Rows[Index].Cells["colAl2"].Value = Real.Al;
                }
                else
                {
                    dataGridView1.Rows[Index].Cells["colFuel2"].Value = "";
                    dataGridView1.Rows[Index].Cells["colAmmo2"].Value = "";
                    dataGridView1.Rows[Index].Cells["colSteel2"].Value = "";
                    dataGridView1.Rows[Index].Cells["colAl2"].Value = "";
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
            }
            catch
            {
                return;
            }
            for (int Index = 0; Index < missionData.Data.Count; Index++)
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
                        Resource Cost = FleetData.GetCost(CurrentFleet, mi.FuelCost, mi.AmmoCost);
                        Resource Income = mi.GetIncome();
                        Resource Real = CalcRealIncome(Income, Cost, mi.GetMissionResult(CurrentFleet), FleetData.Get大发(CurrentFleet), 1);
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
                        Resource Cost = FleetData.GetCost(CurrentFleet, mi.FuelCost, mi.AmmoCost);
                        Resource Income = mi.GetIncome();
                        Resource Real = CalcRealIncome(Income, Cost, mi.GetMissionResult(CurrentFleet), FleetData.Get大发(CurrentFleet), 1);
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

                    var Node = Root.SelectSingleNode("Popup/Width");
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
                var NodeDock = Root.SelectSingleNode("Popup");
                if (NodeDock == null)
                {
                    NodeDock = doc.CreateElement("Popup");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    Root.AppendChild(NodeDock);
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
            LoadConfig();
            DisplayBaseData();
            RefreshSuccess();
            RefreshResource();

     
        }

        private void rbOneTime_CheckedChanged(object sender, EventArgs e)
        {
            perHour = rbPerHour.Checked;

            RefreshResource();
        }

        private void rbFleet2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFleet2.Checked)
                CurrentFleet = 2;
            else if (rbFleet3.Checked)
                CurrentFleet = 3;
            else
                CurrentFleet = 4;
            RefreshResource();
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            e.Handled = false;
            if (e.Column.Name != "colFuel2" && e.Column.Name != "colAmmo2" && e.Column.Name != "colSteel2" && e.Column.Name != "colAl2")
            {
                return;
            }
            if (e.CellValue1 == null)
            {
                e.SortResult = -1;
                e.Handled = true;
                return;
            }
            if (e.CellValue2 == null)
            {
                e.SortResult = 1;
                e.Handled = true;
                return;
            }
            if (e.CellValue1.ToString() == "")
            {
                e.SortResult = -1;
                e.Handled = true;
            }
            if (e.CellValue2.ToString() == "")
            {
                e.SortResult = 1;
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshSuccess();
            RefreshResource();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool Auto = cbAutoFill.Checked;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = Auto ? DataGridViewAutoSizeColumnMode.AllCells : DataGridViewAutoSizeColumnMode.None;
            }
        }

        private void MissionHelperPopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveConfig();
        }

    }
}

