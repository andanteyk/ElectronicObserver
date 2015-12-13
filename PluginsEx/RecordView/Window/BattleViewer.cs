using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Phase;

namespace RecordView
{
    public partial class BattleViewer : Form, IStatusBar
    {
        string DataFileName = Application.StartupPath + "\\battlelogs\\battlelog";
        string ExportPath = Application.StartupPath + "\\BattlelogExports";
        SortedBindingList<BattleData> DataList = new SortedBindingList<BattleData>();
        SortedBindingList<BattleData> FilteredDataList = new SortedBindingList<BattleData>();

        bool SelectChanging = false;
        bool MonthMode = false;

        public static string FileNamePattern = ""; 

        List<string> NameFilter = new List<string>();
        List<string> AreaFilter = new List<string>();
        List<BattleRecord> Records = new List<BattleRecord>();
        public StatusStrip statusStrip
        {
            get;
            set;
        }
        public BattleViewer()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            //ElectronicObserver.Utility.Configuration.Config.UI.Hp100Color.SerializedColor
        }

        private void DropViewer_Shown(object sender, EventArgs e)
        {
            button2.PerformClick();
        }

        void ClearData()
        {
            NameFilter.Clear();
            AreaFilter.Clear();
            Records.Clear();
            FilteredDataList.Clear();
            DataList.Clear();
            dataGridView1.Rows.Clear();
        }

        unsafe static BattleRecord LoadMyStruct(byte* buf)
        {
            int len = sizeof(BattleRecord);
            //MessageBox.Show(len.ToString());
            BattleRecord record;
            byte* p = (byte*)&record;
            for (int i = 0; i < len; i++)
            {
                *p = *buf;
                p++;
                buf++;
            }
            return record;
        }

        List<string> GetDataFiles(DateTime Date1, DateTime Date2)
        {
            int min = Date1.Year * 10000 + Date1.Month * 100 + Date1.Day;
            int max = Date2.Year * 10000 + Date2.Month * 100 + Date2.Day;
            List<string> files = new List<string>();
            if (!Directory.Exists(Application.StartupPath + "\\battlelogs"))
                return null;
            foreach (var fullname in Directory.GetFiles(Application.StartupPath + "\\battlelogs", "battlelog_????_????.dat"))
            {
                string filename = Path.GetFileName(fullname);
                string sDate = filename.Substring(10, 4) + filename.Substring(15, 4);
                int value;
                if (int.TryParse(sDate, out value))
                {
                    if (min <= value && value <= max)
                        files.Add(fullname);
                }
            }
            return files;
        }

        unsafe void LoadDataFromFile(DateTime Date1, DateTime Date2)
        {
            ClearData();

            List<string> filenames = GetDataFiles(Date1, Date2);

            foreach (string filename in filenames)
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                try
                {
                    while (true)
                    {
                        byte[] buff = new byte[sizeof(BattleRecord)];
                        int length = fs.Read(buff, 0, buff.Length);
                        if (length < buff.Length)
                            break;
                        fixed (byte* p = &buff[0])
                        {
                            BattleRecord record = LoadMyStruct(p);
                            Records.Add(record);
                        }
                    }
                }
                finally
                { fs.Close(); }
            }

        }

        unsafe void ShowDataFromRecord()
        {
            if (Records.Count <= 0)
                return;
            for (int i = 0; i < Records.Count; i++)
            {
                var record = Records[i];

                BattleManager.BattleModes BattleMode = (BattleManager.BattleModes)record.BattleMode;
                bool isWater = ((BattleMode & BattleManager.BattleModes.CombinedSurface) > 0);
                bool isCombined = isWater || (BattleMode > BattleManager.BattleModes.BattlePhaseMask);

                BattleData data = new BattleData();
                data.No = i;
                if (record.BattleResult.DroppedShipID < 0)
                    record.BattleResult.DroppedShipID = 0;

                int shipid = record.BattleResult.DroppedShipID & 0xffff;
                int itemid = (record.BattleResult.DroppedShipID >> 16) & 0xff;
              
                data.DropShipName = shipid <= 0 ? "-" : KCDatabase.Instance.MasterShips[shipid].Name;
                data.Memo = itemid <= 0 ? "" : KCDatabase.Instance.MasterUseItems[itemid].Name;

                if (!NameFilter.Contains(data.DropShipName))
                    NameFilter.Add(data.DropShipName);
                data.Types = GetTypes(record);
                if (record.BattleResult.AreaID == 0)
                    data.Area = "演习";
                else
                    data.Area = record.BattleResult.AreaID.ToString().PadLeft(2) + "-" + record.BattleResult.InfoID.ToString();

                int Difficulty = record.BattleResult.CellID >> 20;
                if (Difficulty > 0)
                    data.Area += Constants.GetDifficulty(Difficulty);

                if (!AreaFilter.Contains(data.Area))
                    AreaFilter.Add(data.Area);

                int Cell = record.BattleResult.CellID & 0xffff;
                bool BossBattle = (record.BattleResult.CellID & 0x10000) > 0;
               

                data.Flagship = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[0]].Name;
                if (isCombined)
                    data.Flagship += "/" + KCDatabase.Instance.MasterShips[record.AccompanyFleet.ShipID[0]].Name;
                if (isCombined)
                {
                    int mvp1 = (record.BattleResult.MVP & 0xf) - 1;
                    int mvp2 = (record.BattleResult.MVP & 0xff) >> 4 - 7;
                    data.MVP = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[mvp1]].Name +
                        "/" + KCDatabase.Instance.MasterShips[record.AccompanyFleet.ShipID[mvp2]].Name;
                }
                else
                    data.MVP = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[record.BattleResult.MVP - 1]].Name;
                data.Point = Cell.ToString();
                data.Boss = BossBattle;
                data.Time = record.BattleTime;

                data.Rank = "" + (char)(record.BattleResult.Rank);

                DataList.Add(data);
            }

            
            DataList.ReverseSelf();

            AreaFilter.Sort();
            NameFilter.Sort();
            cbArea.Items.Clear();
            cbShip.Items.Clear();
            cbArea.Items.Add("");
            cbArea.Items.AddRange(AreaFilter.ToArray());
            cbShip.Items.AddRange(NameFilter.ToArray());

            FilteredShow();

        }
        //0:ID  1:Name 2:Time 3,4,5:Area 6:Difficulty 7:Boss 8:Enemy 9:Rank 10:Lv


        string GetTypes(RecordView.BattleRecord record)
        {
            StringBuilder s = new StringBuilder();
            if (record.AirBattle2.AirSuperiority >= 0)
                s.Append("2");
            else if (record.AirBattle1.AirSuperiority >= 0)
                s.Append("1");
            else
                s.Append("×");

            if (record.Support.SupportFlag >= 0)
                s.Append("√");
            else
                s.Append("×");

            if (record.OpenTorpedoBattle.IsAvailable>0)
                s.Append("√");
            else
                s.Append("×");

            if (record.ShellBattle3.IsAvailable > 0)
                s.Append("3");
            else if (record.ShellBattle2.IsAvailable > 0)
                s.Append("2");
            else if (record.ShellBattle1.IsAvailable > 0)
                s.Append("1");
            else
                s.Append("×");

            if (record.CloseTorpedoBattle.IsAvailable > 0)
                s.Append("√");
            else
                s.Append("×");

            if (record.NightBattle.IsAvailable > 0)
                s.Append("√");
            else
                s.Append("×");
            return s.ToString();
        }
        void FilteredShow()
        {
            dataGridView1.DataSource = null;
            string Name = cbShip.Text;
            string Area = cbArea.Text;
            FilteredDataList.Clear();
            foreach (var data in DataList)
            {
                if (!data.DropShipName.Contains(Name))
                    continue;
                if (Area != "")
                {
                    if (data.Area != Area)
                        continue;
                }
                if (cbBoss.Checked && !data.Boss)
                    continue;

                if (!CheckPeriod(data.Types))
                    continue;

                FilteredDataList.Add(data);
            }
            statusStrip.Items[0].Text = "符合条件的一共(" + FilteredDataList.Count.ToString() + ")条记录";

            ShowData();
        }

        bool CheckPeriod(string p)
        {
            if (cbAir2.Checked && p[0] != '2')
                return false;
            if (cbAir.Checked && p[0] == '×')
                return false;
            if (cbSup.Checked && p[1] == '×')
                return false;
            if (cbOpT.Checked && p[2] == '×')
                return false;
            if (cbSh3.Checked && p[3] != '3')
                return false;
            if (cbSh2.Checked && !(p[3] == '3' || p[3] == '2'))
                return false;
            if (cbSh1.Checked && p[3] == '×')
                return false;
            if (cbClT.Checked && p[4] == '×')
                return false;
            if (cbNb.Checked && p[5] == '×')
                return false;
            return true;
        }
        void ShowData()
        {
            dataGridView1.DataSource = FilteredDataList;
        }
        private void cbArea_TextChanged(object sender, EventArgs e)
        {
            if (!SelectChanging)
                FilteredShow();
        }

        private void cbShip_TextChanged(object sender, EventArgs e)
        {
            if (!SelectChanging)
                FilteredShow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MonthMode = false;
            SelectChanging = true;

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

            LoadDataByDate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MonthMode = true;
            SelectChanging = true;

            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            dateTimePicker1.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-1");
            int Days = DateTime.DaysInMonth(Year, Month);
            dateTimePicker2.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-" + Days.ToString());

            LoadDataByDate();
        }

        void LoadDataByDate()
        {
            SelectChanging = false;
            LoadDataFromFile(dateTimePicker1.Value, dateTimePicker2.Value);
            ShowDataFromRecord();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            MonthMode = false;
            if (!SelectChanging)
                LoadDataByDate();
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            MonthMode = false;
            if (!SelectChanging)
                LoadDataByDate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MonthMode = false;
            try
            {
                SelectChanging = true;
                dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
                dateTimePicker2.Value = dateTimePicker2.Value.AddDays(-1);
                LoadDataByDate();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MonthMode = false;
            try
            {
                SelectChanging = true;
                dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
                dateTimePicker2.Value = dateTimePicker2.Value.AddDays(1);
                LoadDataByDate();
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SelectChanging = true;
                if (!MonthMode)
                {
                    dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(-1);
                    dateTimePicker2.Value = dateTimePicker2.Value.AddMonths(-1);
                }
                else
                {
                    DateTime LastMonth = dateTimePicker1.Value.AddMonths(-1);
                    int Year = LastMonth.Year;
                    int Month = LastMonth.Month;
                    dateTimePicker1.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-1");
                    int Days = DateTime.DaysInMonth(Year, Month);
                    dateTimePicker2.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-" + Days.ToString());
                }
                LoadDataByDate();
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SelectChanging = true;
                if (!MonthMode)
                {
                    dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(1);
                    dateTimePicker2.Value = dateTimePicker2.Value.AddMonths(1);
                }
                else
                {
                    DateTime NextMonth = dateTimePicker1.Value.AddMonths(1);
                    int Year = NextMonth.Year;
                    int Month = NextMonth.Month;
                    dateTimePicker1.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-1");
                    int Days = DateTime.DaysInMonth(Year, Month);
                    dateTimePicker2.Value = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-" + Days.ToString());
                }
                LoadDataByDate();
            }
            catch { }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = (int)dataGridView1.Rows[e.RowIndex].Cells["colNo"].Value;
                if (Records.Count > index)
                {

                    StringBuilder builder = BattleExporter.AnalyseRecord(Records[index]);
                    new ElectronicObserver.Window.Dialog.DialogBattleReport1(builder.ToString()).Show();
                }
            }
        }

        private void cbBoss_CheckedChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void cbAir_CheckedChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void 导出所选记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> Table = new Dictionary<string, int>();
            for (int index = 0; index < dataGridView1.Columns.Count; index++)
            {
                Table["[" + dataGridView1.Columns[index].HeaderText + "]"] = index;
            }

            if (!Directory.Exists(ExportPath))
                Directory.CreateDirectory(ExportPath);

            for (int index = 0; index < dataGridView1.SelectedRows.Count; index++)
            {
                statusStrip.Items[0].Text = "正在导出第(" + (index + 1).ToString() + "/" + dataGridView1.SelectedRows.Count.ToString() + ")条记录";
                int ID = (int)dataGridView1.SelectedRows[index].Cells["colNo"].Value;
                if (Records.Count > ID)
                {
                    StringBuilder builder = BattleExporter.AnalyseRecord(Records[ID]);
                    File.WriteAllText(ExportPath + "\\" + GetFileName(FileNamePattern, Table, dataGridView1.SelectedRows[index].Index), builder.ToString(), Encoding.UTF8);
                }
            }
            statusStrip.Items[0].Text = "一共导出(" +  dataGridView1.SelectedRows.Count.ToString() + ")条记录";
        }

        private void 导出当前所有记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> Table = new Dictionary<string, int>();
            for (int index = 0; index < dataGridView1.Columns.Count; index++)
            {
                Table["[" + dataGridView1.Columns[index].HeaderText + "]"] = index;
            }

            if (!Directory.Exists(ExportPath))
                Directory.CreateDirectory(ExportPath);

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                statusStrip.Items[0].Text = "正在导出第(" + (index + 1).ToString() + "/" + dataGridView1.Rows.Count.ToString() + ")条记录";
                int ID = (int)dataGridView1.Rows[index].Cells["colNo"].Value;
                if (Records.Count > ID)
                {
                    StringBuilder builder = BattleExporter.AnalyseRecord(Records[ID]);
                    File.WriteAllText(ExportPath + "\\" + GetFileName(FileNamePattern, Table, dataGridView1.Rows[index].Index), builder.ToString(), Encoding.UTF8);
                }
            }
            statusStrip.Items[0].Text = "一共导出(" + dataGridView1.Rows.Count.ToString() + ")条记录";
        }

        private void 设置导出文件名格式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatternModifier pm = new PatternModifier();
            pm.ShowDialog();
        }

        string GetFileName(string pattern, Dictionary<string, int> Table, int row)
        {
            if (pattern == null || pattern == "")
                pattern = PatternModifier.DefaultPattern;
            string Name = pattern;
            foreach (var kp in Table)
            {
                Name = Name.Replace(kp.Key, dataGridView1.Rows[row].Cells[kp.Value].Value.ToString());
            }
            Name = Name.Replace(@"/", "-");
            Name = Name.Replace(@"\", "-");
            Name = Name.Replace(@":", "-");
            return Name + ".html";
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            导出所选记录ToolStripMenuItem.Enabled = dataGridView1.SelectedRows.Count > 0;
            导出当前所有记录ToolStripMenuItem.Enabled = dataGridView1.Rows.Count > 0;
        }

    
     
    }
    class BattleData
    {
        public int No
        {
            get;
            set;
        }
        public DateTime Time
        {
            get;
            set;
        }
        public string Area
        {
            get;
            set;
        }

        public string Point
        {
            get;
            set;
        }
        public string Flagship
        {
            get;
            set;
        }
        public bool Boss
        {
            get;
            set;
        }

        public string MVP
        {
            get;
            set;
        }
        public string Types
        {
            get;
            set;
        }
        public string DropShipName
        {
            get;
            set;
        }

        public string Rank
        {
            get;
            set;
        }

        public string Memo
        {
            get;
            set;
        }
    }
}
