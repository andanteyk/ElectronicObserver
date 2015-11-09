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

namespace ItemUpgrade
{
    public partial class UpgradeHelper : Form
    {
        public List<UpgradeInformation> UpgradeInformations;
        public Dictionary<string, string> TypeGraph;
        public Dictionary<string, bool> Filters;

        public string ConfigFile;

        public UpgradeHelper()
        {
            InitializeComponent();
            UpgradeInformations = new List<UpgradeInformation>();
            TypeGraph = new Dictionary<string, string>();
            Filters = new Dictionary<string, bool>();
            
            InitList();
            ConfigFile = Application.StartupPath + "\\Settings\\ItemUpgrade.xml";

        }

        private void btnNow_Click(object sender, EventArgs e)
        {
            SelectToday();
        }

        private void cmbDayofWeek_TextChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            RefreshList(cb.SelectedIndex, checkBox1.Checked, false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList(cmbDayofWeek.SelectedIndex, checkBox1.Checked, false);
        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList(cmbDayofWeek.SelectedIndex, checkBox1.Checked, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilterForm filter = new FilterForm();
            filter.SetFilter(Filters);
            filter.ShowDialog();
            SaveConfig();
            if (checkBox1.Checked)
                RefreshList(cmbDayofWeek.SelectedIndex, checkBox1.Checked, false);
        }


        private void UpgradeHelper_Load(object sender, EventArgs e)
        {
            this.Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
            LoadConfig();
            SelectToday();
        }

        void aaaa()
        {
            //ElectronicObserver.Data.KCDatabase.Instance.Ships[0].MasterShip.
            //ElectronicObserver.Data.KCDatabase.Instance.Equipments[0].Name
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
                    string Midnight = Root.GetAttribute("Midnight");
                    if (Midnight == "1")
                        cbMidnight.Checked = true;
                    foreach (XmlElement childElement in Root)
                    {
                        string name = childElement.GetAttribute("EquipmentName");
                        string disabled = childElement.GetAttribute("Disabled");
                        Filters[name] = disabled == "True" ? true : false;
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
                Root.RemoveAll();
                Root.SetAttribute("Midnight", cbMidnight.Checked ? "1" : "");
                foreach (var filter in Filters)
                {
                    XmlElement Element = doc.CreateElement("Filter");
                    Element.SetAttribute("EquipmentName", filter.Key);
                    Element.SetAttribute("Disabled", filter.Value.ToString());
                    Root.AppendChild(Element);

                }
                doc.Save(ConfigFile);
            }
            catch
            {
            }
        }

        public void InitList()
        {
            TypeGraph["小口径主砲"] = "Equipment_MainGunS";
            TypeGraph["_小口径主砲"] = "Equipment_HighAngleGun";
            TypeGraph["中口径主砲"] = "Equipment_MainGunM";
            TypeGraph["大口径主砲"] = "Equipment_MainGunL";
            TypeGraph["_副砲"] = "Equipment_HighAngleGun";
            TypeGraph["副砲"] = "Equipment_SecondaryGun";
            TypeGraph["魚雷"] = "Equipment_Torpedo";
            TypeGraph["電探"] = "Equipment_RADAR";
            TypeGraph["ソナー"] = "Equipment_SONAR";
            TypeGraph["爆雷"] = "Equipment_DepthCharge";
            TypeGraph["対艦強化弾"] = "Equipment_APShell";
            TypeGraph["対空機銃"] = "Equipment_AAGun";
            TypeGraph["高射装置"] = "Equipment_AADirector";
            TypeGraph["探照灯"] = "Equipment_Searchlight";

            try
            {
                foreach (var pic in TypeGraph)
                {
                    imageList1.Images.Add(pic.Value, ResourceManager.Instance.Equipments[pic.Value]);
                }
            }
            catch { }

            UpgradeRequirementList RequirementList = new UpgradeRequirementList();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "12.7cm連装砲", 1);
            RequirementList.Refresh.SetCondition(3, 6, "12.7cm連装砲", 2, "12.7cm連装砲B型改二");
            AddInformation(ItemType.小口径主砲, "12.7cm連装砲", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "無", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "12.7cm連装砲B型改二", 1);
            AddInformation(ItemType.小口径主砲, "12.7cm連装砲B型改二", new int[] { 1, 2, 3 }, "夕立改二", RequirementList);
            AddInformation(ItemType.小口径主砲, "12.7cm連装砲B型改二", new int[] { 1, 2, 3 }, "綾波改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, null, 0);
            RequirementList.Steps.SetCondition(4, 7, "10cm連装高角砲", 2);
            AddInformation(ItemType._小口径主砲, "10cm連装高角砲+高射装置", new int[] { 1, 2, 3, 4 }, "秋月", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, null, 0);
            RequirementList.Steps.SetCondition(4, 7, "10cm連装高角砲", 2);
            AddInformation(ItemType._小口径主砲, "10cm連装高角砲+高射装置", new int[] { 0, 4, 5, 6 }, "照月", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "14cm単装砲", 1);
            RequirementList.Refresh.SetCondition(3, 6, "14cm単装砲", 2, "14cm連装砲");
            AddInformation(ItemType.中口径主砲, "14cm単装砲", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "無", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 3, "14cm連装砲", 1);
            AddInformation(ItemType.中口径主砲, "14cm連装砲", new int[] { 1, 4 }, "夕張", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "15.2cm連装砲", 1);
            RequirementList.Refresh.SetCondition(4, 10, "22号対水上電探", 1, "15.2cm連装砲改");
            AddInformation(ItemType.中口径主砲, "15.2cm連装砲", new int[] { 4, 5, 6 }, "阿賀野", RequirementList);
            AddInformation(ItemType.中口径主砲, "15.2cm連装砲", new int[] { 0, 1, 5, 6 }, "能代", RequirementList);
            AddInformation(ItemType.中口径主砲, "15.2cm連装砲", new int[] { 1, 2, 3, 4 }, "矢矧", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "15.2cm連装砲", 1);
            RequirementList.Steps.SetCondition(3, 6, "15.2cm連装砲", 1);
            AddInformation(ItemType.中口径主砲, "15.2cm連装砲改", new int[] { 3, 4, 5, 6 }, "矢矧", RequirementList);
            AddInformation(ItemType.中口径主砲, "15.2cm連装砲改", new int[] { 0, 1, 2, 6 }, "酒匂", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "15.5cm三連装砲", 1);
            AddInformation(ItemType.中口径主砲, "15.5cm三連装砲", new int[] { 5, 6 }, "最上", RequirementList);
            AddInformation(ItemType.中口径主砲, "15.5cm三連装砲", new int[] { 0, 1 }, "大淀", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 3, "20.3cm連装砲", 1);
            RequirementList.Refresh.SetCondition(4, 10, "20.3cm連装砲", 2, "20.3cm(2号)連装砲");
            AddInformation(ItemType.中口径主砲, "20.3cm連装砲", new int[] { 0, 4, 5, 6 }, "青葉", RequirementList);
            AddInformation(ItemType.中口径主砲, "20.3cm連装砲", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "衣笠", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "20.3cm(2号)連装砲", 1);
            RequirementList.Refresh.SetCondition(4, 11, "20.3cm(2号)連装砲", 1, "20.3cm(3号)連装砲");
            AddInformation(ItemType.中口径主砲, "20.3cm(2号)連装砲", new int[] { 0, 1, 2, }, "妙高", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "20.3cm(3号)連装砲", 1);
            AddInformation(ItemType.中口径主砲, "20.3cm(3号)連装砲", new int[] { 2, 3 }, "三隈", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "35.6cm連装砲", 1);
            RequirementList.Refresh.SetCondition(5, 12, "35.6cm連装砲", 3, "試製35.6cm三連装砲");
            AddInformation(ItemType.大口径主砲, "35.6cm連装砲", new int[] { 0, 5, 6 }, "扶桑", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "35.6cm連装砲", 1);
            RequirementList.Steps.SetCondition(3, 5, "35.6cm連装砲", 2);
            RequirementList.Refresh.SetCondition(6, 13, "41cm連装砲", 2, "38cm連装砲改★+3");
            AddInformation(ItemType.大口径主砲, "38cm連装砲", new int[] { 4, 5, 6 }, "Bismarck", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "41cm連装砲", 1);
            RequirementList.Steps.SetCondition(4, 6, "41cm連装砲", 2);
            AddInformation(ItemType.大口径主砲, "38cm連装砲改", new int[] { 0, 1, 2 }, "Bismarck", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "35.6cm連装砲", 1);
            RequirementList.Steps.SetCondition(3, 5, "35.6cm連装砲", 2);
            RequirementList.Refresh.SetCondition(7, 14, "25mm連装機銃", 2, "381mm/50 三連装砲改★+3");
            AddInformation(ItemType.大口径主砲, "381mm/50 三連装砲", new int[] { 2, 3, 4, 5 }, "Littorio", RequirementList);
            AddInformation(ItemType.大口径主砲, "381mm/50 三連装砲", new int[] { 0, 1, 6 }, "Roma", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "41cm連装砲", 1);
            RequirementList.Steps.SetCondition(4, 6, "41cm連装砲", 2);
            AddInformation(ItemType.大口径主砲, "381mm/50 三連装砲改", new int[] { 0, 1, 6 }, "Littorio", RequirementList);
            AddInformation(ItemType.大口径主砲, "381mm/50 三連装砲改", new int[] { 2, 3, 4, 5 }, "Roma", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "41cm連装砲", 1);
            RequirementList.Steps.SetCondition(3, 6, "41cm連装砲", 2);
            AddInformation(ItemType.大口径主砲, "41cm連装砲", new int[] { 2, 5, 6 }, "長門", RequirementList);
            AddInformation(ItemType.大口径主砲, "41cm連装砲", new int[] { 0, 1, 4, }, "陸奥", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 5, "41cm連装砲", 2);
            RequirementList.Steps.SetCondition(4, 7, "41cm連装砲", 3);
            RequirementList.Refresh.SetCondition(8, 14, "41cm連装砲", 4, "46cm三連装砲★+5");
            AddInformation(ItemType.大口径主砲, "試製46cm連装砲", new int[] { 0, 1 }, "大和", RequirementList);
            AddInformation(ItemType.大口径主砲, "試製46cm連装砲", new int[] { 2, 3 }, "武蔵", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(4, 6, "46cm三連装砲", 1);
            RequirementList.Steps.SetCondition(5, 8, "46cm三連装砲", 2);
            AddInformation(ItemType.大口径主砲, "46cm三連装砲", new int[] { 5, 6 }, "大和", RequirementList);
            AddInformation(ItemType.大口径主砲, "46cm三連装砲", new int[] { 0, 1 }, "武蔵", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(5, 7, "46cm三連装砲", 2);
            RequirementList.Steps.SetCondition(7, 10, "46cm三連装砲", 3);
            AddInformation(ItemType.大口径主砲, "試製51cm連装砲", new int[] { 1, 2 }, "大和改", RequirementList);
            AddInformation(ItemType.大口径主砲, "試製51cm連装砲", new int[] { 1, 3 }, "武蔵改", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, "10cm連装高角砲", 1);
            RequirementList.Steps.SetCondition(1, 2, "10cm連装高角砲", 2);
            AddInformation(ItemType._副砲, "90mm単装高角砲", new int[] { 1, 2, 3, 4 }, "Littorio", RequirementList);
            AddInformation(ItemType._副砲, "90mm単装高角砲", new int[] { 0, 4, 5, 6 }, "Roma", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 3, "15.2cm単装砲", 1);
            RequirementList.Refresh.SetCondition(3, 5, "15.2cm単装砲", 2, "15.2cm連装砲");
            AddInformation(ItemType.副砲, "15.2cm単装砲", new int[] { 0, 1, 2 }, "阿賀野", RequirementList);
            AddInformation(ItemType.副砲, "15.2cm単装砲", new int[] { 0, 1, 6 }, "金剛", RequirementList);
            AddInformation(ItemType.副砲, "15.2cm単装砲", new int[] { 1, 2, 3 }, "山城", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "15.5cm三連装砲", 1);
            AddInformation(ItemType.副砲, "OTO 152mm三連装速射砲", new int[] { 0, 2, 3, 6 }, "Littorio", RequirementList);
            AddInformation(ItemType.副砲, "OTO 152mm三連装速射砲", new int[] { 0, 1, 4, 5 }, "Roma", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "61cm三連装魚雷", 1);
            RequirementList.Refresh.SetCondition(2, 4, "61cm三連装魚雷", 2, "61cm三連装(酸素)魚雷");
            AddInformation(ItemType.魚雷, "61cm三連装魚雷", new int[] { 4, 5, 6 }, "吹雪", new string[] { "吹雪改二" }, RequirementList);
            AddInformation(ItemType.魚雷, "61cm三連装魚雷", new int[] { 0, 1, 2 }, "叢雲", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 3, "61cm三連装魚雷", 1);
            RequirementList.Refresh.SetCondition(3, 6, "61cm四連装魚雷", 2, "61cm四連装(酸素)魚雷★+5");
            AddInformation(ItemType.魚雷, "61cm三連装(酸素)魚雷", new int[] { 4, 5, 6 }, "吹雪改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "61cm四連装魚雷", 1);
            RequirementList.Refresh.SetCondition(3, 6, "61cm四連装魚雷", 2, "61cm四連装(酸素)魚雷★+3");
            AddInformation(ItemType.魚雷, "61cm四連装魚雷", new int[] { 0, 1, 2, 5, 6 }, "無", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "61cm四連装(酸素)魚雷", 1);
            RequirementList.Refresh.SetCondition(5, 11, "61cm四連装(酸素)魚雷", 3, "61cm五連装(酸素)魚雷");
            AddInformation(ItemType.魚雷, "61cm四連装(酸素)魚雷", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "大井", RequirementList);
            AddInformation(ItemType.魚雷, "61cm四連装(酸素)魚雷", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "北上", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 5, null, 0);
            RequirementList.Steps.SetCondition(3, 7, "61cm五連装(酸素)魚雷", 1);
            AddInformation(ItemType.魚雷, "61cm五連装(酸素)魚雷", new int[] { 3, 4 }, "島風", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "13号対空電探", 1);
            RequirementList.Refresh.SetCondition(5, 12, "21号対空電探", 1, "13号対空電探改");
            AddInformation(ItemType.電探, "13号対空電探", new int[] { 0, 4, 5, 6 }, "時雨改二", RequirementList);
            AddInformation(ItemType.電探, "13号対空電探", new int[] { 0, 1, 5, 6 }, "五十鈴改二", RequirementList);
            AddInformation(ItemType.電探, "13号対空電探", new int[] { 2, 3, 4 }, "秋月", RequirementList);
            AddInformation(ItemType.電探, "13号対空電探", new int[] { 1, 2, 3 }, "照月", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "13号対空電探", 1);
            RequirementList.Steps.SetCondition(4, 8, "13号対空電探", 2);
            AddInformation(ItemType.電探, "13号対空電探改", new int[] { 0, 5, 6 }, "初霜改二", RequirementList);
            AddInformation(ItemType.電探, "13号対空電探改", new int[] { 4, 5, 6 }, "磯風改", RequirementList);
            AddInformation(ItemType.電探, "13号対空電探改", new int[] { 0, 1, 2, 3 }, "雪風", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "22号対水上電探", 1);
            RequirementList.Steps.SetCondition(3, 5, "22号対水上電探", 2);
            RequirementList.Refresh.SetCondition(8, 14, "22号対水上電探", 3, "22号対水上電探改四");
            AddInformation(ItemType.電探, "22号対水上電探", new int[] { 0, 1, 5, 6 }, "日向", RequirementList);
            AddInformation(ItemType.電探, "22号対水上電探", new int[] { 1, 2, 5, 6 }, "夕雲", RequirementList);
            AddInformation(ItemType.電探, "22号対水上電探", new int[] { 3, 4, 5, 6 }, "島風", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "22号対水上電探", 1);
            RequirementList.Steps.SetCondition(4, 8, "22号対水上電探改四", 1);
            AddInformation(ItemType.電探, "22号対水上電探改四", new int[] { 4, 5, 6 }, "妙高改二", RequirementList);
            AddInformation(ItemType.電探, "22号対水上電探改四", new int[] { 0, 1, 5, 6 }, "羽黒改二", RequirementList);
            AddInformation(ItemType.電探, "22号対水上電探改四", new int[] { 2, 3, 4, 5 }, "金剛改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "21号対空電探", 1);
            RequirementList.Refresh.SetCondition(5, 13, "21号対空電探", 2, "21号対空電探改");
            AddInformation(ItemType.電探, "21号対空電探", new int[] { 0, 1, 5, 6 }, "伊勢", RequirementList);
            AddInformation(ItemType.電探, "21号対空電探", new int[] { 3, 4, 5, 6 }, "日向", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, "21号対空電探", 1);
            RequirementList.Steps.SetCondition(4, 8, "21号対空電探", 2);
            AddInformation(ItemType.電探, "21号対空電探改", new int[] { 0, 4, 5, 6 }, "大和", RequirementList);
            AddInformation(ItemType.電探, "21号対空電探改", new int[] { 2, 3, 4, 5 }, "武蔵", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "22号対水上電探", 1);
            RequirementList.Steps.SetCondition(4, 7, "22号対水上電探", 2);
            RequirementList.Refresh.SetCondition(10, 15, "32号対水上電探", 1, "32号対水上電探改");
            AddInformation(ItemType.電探, "32号対水上電探", new int[] { 3, 4, 5, 6 }, "伊勢", RequirementList);
            AddInformation(ItemType.電探, "32号対水上電探", new int[] { 0, 1, 2 }, "日向", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(5, 6, "22号対水上電探", 3);
            RequirementList.Steps.SetCondition(7, 10, "32号対水上電探", 1);
            AddInformation(ItemType.電探, "32号対水上電探改", new int[] { 0, 1, 2 }, "伊勢", RequirementList);
            AddInformation(ItemType.電探, "32号対水上電探改", new int[] { 3, 4, 5, 6 }, "日向", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 3, "九三式水中聴音機", 1);
            RequirementList.Refresh.SetCondition(3, 5, "九三式水中聴音機", 2, "三式水中探信儀★+3");
            AddInformation(ItemType.ソナー, "九三式水中聴音機", new int[] { 0, 5, 6 }, "夕張", RequirementList);
            AddInformation(ItemType.ソナー, "九三式水中聴音機", new int[] { 1 }, "五十鈴改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 3, "九三式水中聴音機", 1);
            RequirementList.Refresh.SetCondition(6, 12, "三式水中探信儀", 2, "四式水中聴音機");
            AddInformation(ItemType.ソナー, "九三式水中聴音機", new int[] {4, 5 }, "五十鈴改二", RequirementList);
            AddInformation(ItemType.ソナー, "九三式水中聴音機", new int[] { 0, 4, 5, 6 }, "時雨改二", RequirementList);
            AddInformation(ItemType.ソナー, "九三式水中聴音機", new int[] { 0, 5, 6 }, "香取改", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "三式水中探信儀", 1);
            AddInformation(ItemType.ソナー, "三式水中探信儀", new int[] { 2, 3 }, "夕張", RequirementList);
            AddInformation(ItemType.ソナー, "三式水中探信儀", new int[] { 0, 2, 3 }, "五十鈴改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 5, "九三式水中聴音機", 2);
            RequirementList.Steps.SetCondition(4, 6, "四式水中聴音機", 1);
            AddInformation(ItemType.ソナー, "四式水中聴音機", new int[] { 4, 5, 6 }, "五十鈴改二", RequirementList);
            AddInformation(ItemType.ソナー, "四式水中聴音機", new int[] { 0 }, "秋月改", RequirementList);
            AddInformation(ItemType.ソナー, "四式水中聴音機", new int[] { 3 }, "照月改", RequirementList);
            AddInformation(ItemType.ソナー, "四式水中聴音機", new int[] { 1, 2 }, "香取改", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(1, 3, "九四式爆雷投射機", 1);
            RequirementList.Refresh.SetCondition(3, 8, "九四式爆雷投射機", 2, "三式爆雷投射機★+3");
            AddInformation(ItemType.爆雷, "九四式爆雷投射機", new int[] { 3, 4 }, "無", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "三式爆雷投射機", 1);
            AddInformation(ItemType.爆雷, "三式爆雷投射機", new int[] { 3, 4 }, "五十鈴改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "九一式徹甲弾", 1);
            RequirementList.Refresh.SetCondition(4, 9, "九一式徹甲弾", 3, "一式徹甲弾");
            AddInformation(ItemType.対艦強化弾, "九一式徹甲弾", new int[] { 3, 4, 5, 6 }, "比叡", RequirementList);
            AddInformation(ItemType.対艦強化弾, "九一式徹甲弾", new int[] { 0, 1, 5, 6 }, "霧島", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, "九一式徹甲弾", 2);
            RequirementList.Steps.SetCondition(2, 4, "一式徹甲弾", 1);
            AddInformation(ItemType.対艦強化弾, "一式徹甲弾", new int[] { 0, 5, 6 }, "金剛", RequirementList);
            AddInformation(ItemType.対艦強化弾, "一式徹甲弾", new int[] { 1, 2, 3 }, "榛名", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "25mm連装機銃", 1);
            RequirementList.Refresh.SetCondition(1, 2, "25mm連装機銃", 1, "25mm三連装機銃★+3");
            AddInformation(ItemType.対空機銃, "25mm連装機銃", new int[] { 0, 5, 6 }, "五十鈴改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "25mm三連装機銃", 1);
            AddInformation(ItemType.対空機銃, "25mm三連装機銃", new int[] { 2, 3, 4 }, "摩耶", new string[] { "摩耶改二" }, RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 1, null, 0);
            RequirementList.Steps.SetCondition(1, 2, "25mm三連装機銃", 1);
            RequirementList.Refresh.SetCondition(3, 7, "25mm三連装機銃", 5, "25mm三連装機銃 集中配備");
            AddInformation(ItemType.対空機銃, "25mm三連装機銃", new int[] { 1, 2, 3 }, "五十鈴改二", RequirementList);
            AddInformation(ItemType.対空機銃, "25mm三連装機銃", new int[] { 0, 1, 5, 6 }, "摩耶改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(1, 2, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "12.7cm連装高角砲", 1);
            RequirementList.Refresh.SetCondition(4, 7, "10cm連装高角砲", 2, "94式高射装置");
            AddInformation(ItemType.高射装置, "91式高射装置", new int[] { 0, 1, 5, 6 }, "秋月", RequirementList);
            AddInformation(ItemType.高射装置, "91式高射装置", new int[] { 0, 1, 5, 6 }, "摩耶", RequirementList);
            AddInformation(ItemType.高射装置, "91式高射装置", new int[] { 0, 4, 5, 6 }, "照月", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "10cm連装高角砲", 1);
            RequirementList.Refresh.SetCondition(5, 10, "10cm連装高角砲", 2, "10cm高角砲＋高射装置");
            AddInformation(ItemType.高射装置, "94式高射装置", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "秋月", RequirementList);//照月
            AddInformation(ItemType.高射装置, "94式高射装置", new int[] { 0, 1, 2, 3, 4, 5, 6 }, "照月", RequirementList);
            AddInformation(ItemType.高射装置, "94式高射装置", new int[] { 0, 4, 5, 6 }, "吹雪改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(3, 5, "12.7cm連装高角砲", 1);
            RequirementList.Refresh.SetCondition(5, 9, "12.7cm連装高角砲", 2, "12.7cm高角砲＋高射装置");
            AddInformation(ItemType.高射装置, "94式高射装置", new int[] { 0, 4, 5, 6 }, "摩耶改二", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(2, 3, null, 0);
            RequirementList.Steps.SetCondition(2, 4, "探照灯", 1);
            RequirementList.Refresh.SetCondition(3, 7, "熟練見張員", 1, "96式150cm探照灯");
            AddInformation(ItemType.探照灯, "探照灯", new int[] { 4, 5, 6 }, "暁", RequirementList);
            AddInformation(ItemType.探照灯, "探照灯", new int[] { 0, 5, 6 }, "神通", RequirementList);
            AddInformation(ItemType.探照灯, "探照灯", new int[] { 1, 2, 3 }, "青葉", RequirementList);
            AddInformation(ItemType.探照灯, "探照灯", new int[] { 1, 2, 3 }, "綾波", RequirementList);

            RequirementList.New();
            RequirementList.Initial.SetCondition(3, 4, "探照灯", 1);
            RequirementList.Steps.SetCondition(3, 7, "探照灯", 1);
            AddInformation(ItemType.探照灯, "96式150cm探照灯", new int[] { 0, 1, 5, 6 }, "比叡", RequirementList);
            AddInformation(ItemType.探照灯, "96式150cm探照灯", new int[] { 2, 3, 4, 5 }, "霧島", RequirementList);

        }

        public void AddInformation(ItemType Type, string Name, int[] DayofWeeks, string ShipName, UpgradeRequirementList list)
        {
            if (!Filters.ContainsKey(Name))
                Filters.Add(Name, false);

            foreach (int DayofWeek in DayofWeeks)
            {
                UpgradeInformation inf = new UpgradeInformation();
                inf.Type = Type;
                inf.Name = Name;
                inf.DayofWeek = DayofWeek;
                inf.Ship = ShipName;
                inf.Initial = list.Initial;
                inf.Steps = list.Steps;
                inf.Refresh = list.Refresh;
                UpgradeInformations.Add(inf);
            }
        }

        public void AddInformation(ItemType Type, string Name, int[] DayofWeeks, string ShipName, string[] ExceptShip, UpgradeRequirementList list)
        {
            if (!Filters.ContainsKey(Name))
                Filters.Add(Name, false);
            foreach (int DayofWeek in DayofWeeks)
            {
                UpgradeInformation inf = new UpgradeInformation();
                inf.Type = Type;
                inf.Name = Name;
                inf.DayofWeek = DayofWeek;
                inf.Ship = ShipName;
                inf.ExceptShips.AddRange(ExceptShip);
                inf.Initial = list.Initial;
                inf.Steps = list.Steps;
                inf.Refresh = list.Refresh;
                UpgradeInformations.Add(inf);
            }
        }

        public void SelectToday()
        {
            TimeZoneInfo TokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TokyoZone);
            int DayofWeek = (int)(time.DayOfWeek);
            SelectDayofWeek(DayofWeek);
        }

        public void SelectDayofWeek(int DayofWeek)
        {
            if (DayofWeek < 0 || DayofWeek > 6)
            {
                DayofWeek = 0;
            }
            cmbDayofWeek.SelectedIndex = DayofWeek;
        }

        public void RefreshList(int DayofWeek, bool Filtered, bool OnlyShowAvaible)
        {
            bool MidnightWork = cbMidnight.Checked;
            OnlyShowAvaible = checkBox2.Checked;
            lvList.Items.Clear();
            UpgradeInformations.ForEach(x=>x.ShipListDisplayed.List.Clear());
            List<UpgradeInformation> result;
            if (MidnightWork)
            {
                int nextDayofWeek = (DayofWeek + 1) % 7;
                result = UpgradeInformations.Where(x => (x.DayofWeek == DayofWeek) || (x.DayofWeek == nextDayofWeek)).ToList();
            }
            else
                result = UpgradeInformations.Where(x => x.DayofWeek == DayofWeek).ToList();
            int index = 0;

            for (int id = result.Count - 1; id >= 0; id--)//修改  提前删除不符合条件的改修条目
            {
                var inf = result[id];
                if (Filtered && Filters[inf.Name])
                {
                    result.RemoveAt(id);
                    continue;
                }
                if (inf.Ship != "無")
                {
                    if (OnlyShowAvaible && !ShipExists(inf.Ship, inf.ExceptShips))
                    {
                        result.RemoveAt(id);
                        continue;
                    }
                    if (OnlyShowAvaible && !EquipmentExists(inf.Name))
                    {
                        result.RemoveAt(id);
                        continue;
                    }
                }
            }

            while (true)
            {
                result[index].ShipListDisplayed.Add(result[index].Ship);
                if (index >= result.Count - 1)
                    break;

                if ((result[index].Name == result[index + 1].Name) && (result[index].Steps.EquipName == result[index + 1].Steps.EquipName) && (result[index].Refresh.EquipName == result[index + 1].Refresh.EquipName))
                {
                    if (result[index].DayofWeek == DayofWeek)
                    {
                        if (result[index + 1].DayofWeek == DayofWeek)
                        {
                            result[index].ShipListDisplayed.Add(result[index + 1].Ship);
                            result.RemoveAt(index + 1);
                        }
                        else
                        {
                            result.RemoveAt(index + 1);
                        }
                    }
                    else
                    {
                        if (result[index + 1].DayofWeek == DayofWeek)
                        {
                            result.RemoveAt(index);
                        }
                        else
                        {
                            result[index + 1].ShipListDisplayed.Add(result[index].Ship);
                            result.RemoveAt(index);
                        }
                    }

                }
                else
                {
                    index++;
                }
            }

            lvList.BeginUpdate();
            foreach (UpgradeInformation inf in result)
            {
                //if (Filtered && Filters[inf.Name])
                //    continue;
                //if (inf.Ship != "無")
                //{
                //    if (OnlyShowAvaible && !ShipExists(inf.ShipListDisplayed, inf.ExceptShips))
                //        continue;
                //    if (OnlyShowAvaible && !EquipmentExists(inf.Name))
                //        continue;
                //}
                ListViewItem li = new ListViewItem("");
                try
                {
                    li.ImageKey = TypeGraph[inf.Type.ToString()];
                }
                catch
                {
                }
                string Type = inf.Type.ToString();
                if (Type[0] == '_')
                    Type = Type.Substring(1);
                li.SubItems.Add(Type);
                var item = li.SubItems.Add(inf.Name);
                li.SubItems.Add(inf.ShipListDisplayed.ToString());
                li.SubItems.Add(inf.DayofWeek != DayofWeek ? "半夜限定" : "");
                li.ToolTipText = inf.GetHint();
                lvList.Items.Add(li);
            }
            lvList.EndUpdate();
        }

        bool ShipExists(ShipList Ship, List<string> exceptions)
        {
            var ships = Ship.List;
            int Count = 0;
            try
            {
                Count = ElectronicObserver.Data.KCDatabase.Instance.Ships.Count;
            }
            catch
            {
                return true;
            }
            if (Count <= 0)
                return true;
            foreach ( var mship in ElectronicObserver.Data.KCDatabase.Instance.Ships)
            {
                if (mship.Value == null)
                    continue;
                ShipDataMaster Master = mship.Value.MasterShip;

                if (exceptions.Contains(Master.Name))
                {
                    continue;
                }

                while (Master != null)
                {
                    if (ships.Contains(Master.Name))
                    {
                        return true;
                    }
                    Master = Master.RemodelBeforeShip;
                }
            }
            return false;
        }

        bool ShipExists(string Ship, List<string> exceptions)
        {
            int Count = 0;
            try
            {
                Count = ElectronicObserver.Data.KCDatabase.Instance.Ships.Count;
            }
            catch
            {
                return true;
            }
            if (Count <= 0)
                return true;
            foreach (var mship in ElectronicObserver.Data.KCDatabase.Instance.Ships)
            {
                if (mship.Value == null)
                    continue;
                ShipDataMaster Master = mship.Value.MasterShip;

                if (exceptions.Contains(Master.Name))
                {
                    continue;
                }

                while (Master != null)
                {
                    if (Ship == Master.Name)
                    {
                        return true;
                    }
                    Master = Master.RemodelBeforeShip;
                }
            }
            return false;
        }

        bool EquipmentExists(string Equipment)
        {
            int Count = 0;
            try
            {
                Count = ElectronicObserver.Data.KCDatabase.Instance.Equipments.Count;
            }
            catch
            {
                return true;
            }
            if (Count <= 0)
                return true;
            foreach (var equipment in ElectronicObserver.Data.KCDatabase.Instance.Equipments)
            {
                if (equipment.Value == null)
                    continue;
               
                if (Equipment == equipment.Value.Name)
                {
                    return true;
                }

            }
            return false;
        }

        private void lvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0)
                ShowHint(lvList.SelectedItems[0].ToolTipText);
        }

        void ShowHint(string hint)
        {
            string[] Lines = hint.Split('\x0');
            listView1.Items.Clear();
            listView1.BeginUpdate();
            foreach(string s in Lines)
            {
                string[] datas = s.Split('~');
                ListViewItem li = new ListViewItem("");
                li.Text = datas[0];
                li.SubItems.Add(datas[1]);
                li.SubItems.Add(datas[2]);
                if (datas.Length > 3)
                    li.SubItems.Add(datas[3]);
                listView1.Items.Add(li);
            }
            listView1.EndUpdate();
        }

        private void cbMidnight_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList(cmbDayofWeek.SelectedIndex, checkBox1.Checked, false);
        }

        private void UpgradeHelper_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveConfig();
        }
    }

    public class UpgradeInformation
    {
        public ItemType Type
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int DayofWeek
        {
            get;
            set;
        }
        public bool Midnight
        {
            get;
            set;
        }
        public string Ship
        {
            get;
            set;
        }
        public ShipList ShipListDisplayed = new ShipList();

        public List<string> ExceptShips = new List<string>();

        public UpgradeRequirement Initial = new UpgradeRequirement("初期");
        public UpgradeRequirement Steps = new UpgradeRequirement("★6");
        public UpgradeRequirement Refresh = new UpgradeRequirement("更新");

        public string GetHint()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Initial.GetHint());
            builder.Append('\x0');
            builder.Append(Steps.GetHint());
            if (Refresh.PeriodAviable)
            {
                builder.Append('\x0');
                builder.Append(Refresh.GetHint());
            }
            return builder.ToString();
        }
    }


    public class UpgradeRequirement
    {
        public bool PeriodAviable = false;
        string Period = null;
        int[] Gears = new int[2];
        int[] Materials = new int[2];
        int[] Steel = new int[2];
        int[] Fuel = new int[2];
        int[] Ammo = new int[2];
        int[] Al = new int[2];
        public string EquipName = null;
        int EquipNum = 0;
        string RefreshEquipName = null;

        public void Clear()
        {
            PeriodAviable = false;
            Gears[0] = 0;
            Gears[1] = 0;
            Materials[0] = 0;
            Materials[1] = 0;
            Steel[0] = 0;
            Steel[1] = 0;
            Fuel[0] = 0;
            Fuel[1] = 0;
            Ammo[0] = 0;
            Ammo[1] = 0;
            Al[0] = 0;
            Al[1] = 0;
            EquipName = null;
            EquipNum = 0;
            RefreshEquipName = null;
        }
        public void SetCondition(int Gear1,int Gear2,string equipName,int equipNum)
        {
            PeriodAviable = true;
            Gears[0] = Gear1;
            Gears[1] = Gear2;
            EquipName = equipName;
            EquipNum = equipNum;
        }

        public void SetCondition(int Gear1, int Gear2, string equipName, int equipNum, string refreshEquipName)
        {
            SetCondition(Gear1, Gear2, equipName, equipNum);
            RefreshEquipName = refreshEquipName;
        }

        public UpgradeRequirement(string name)
        {
            Period = name;
        }
        public string GetHint()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}~{1}/{2}", Period, Gears[0], Gears[1]);
            if (EquipNum > 0)
                builder.AppendFormat("~{0}*{1}", EquipName, EquipNum);
            else
                builder.Append("~无");
            if (RefreshEquipName != null)
            {
                builder.AppendFormat("~{0}", RefreshEquipName);
            }

            return builder.ToString();
        }
    }

    public class ShipList
    {

        public List<string> List = new List<string>();

        public void Add(string shipList)
        {
            string[] ships = shipList.Split(' ');
            foreach (string ship in ships)
                if (!List.Contains(ship))
                    List.Add(ship);
        }

        public override string ToString()
        {
            string r = "";
            foreach(var s in List)
            {
                r += s + " ";
            }
            return r;
        }
    }

    public enum ItemType
    {
        小口径主砲, _小口径主砲, 中口径主砲, 大口径主砲, 副砲, _副砲, 魚雷, 電探, ソナー, 爆雷, 対艦強化弾, 対空機銃, 高射装置, 探照灯
    }

    public class UpgradeRequirementList
    {
        public UpgradeRequirement Initial = new UpgradeRequirement("初期");
        public UpgradeRequirement Steps = new UpgradeRequirement("★6");
        public UpgradeRequirement Refresh = new UpgradeRequirement("更新");

        public void New()
        {
            Initial = new UpgradeRequirement("初期");
            Steps = new UpgradeRequirement("★6");
            Refresh = new UpgradeRequirement("更新");
        }
    }
}
