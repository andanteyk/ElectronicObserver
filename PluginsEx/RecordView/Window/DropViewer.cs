using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RecordView
{
    public partial class DropViewer : Form, IStatusBar
    {
        string DataFileName = Application.StartupPath + "\\record\\ShipDropRecord.csv";

        SortedBindingList<DropData> DataList = new SortedBindingList<DropData>();
        SortedBindingList<DropData> FilteredDataList = new SortedBindingList<DropData>();
        DateTime MinDate;

        List<string> NameFilter = new List<string>();
        List<string> AreaFilter = new List<string>();
        Dictionary<int, string> Difficulty = new Dictionary<int, string>();

        public StatusStrip statusStrip
        {
            get;
            set;
        }
        public DropViewer()
        {
            InitializeComponent();
            Difficulty.Add(-1, "常规图");
            Difficulty.Add(0, "丙");
            Difficulty.Add(1, "乙");
            Difficulty.Add(2, "甲");
        }

        private void DropViewer_Shown(object sender, EventArgs e)
        {
          
                LoadDataFromFile();
                
                timeSelection1.SelectToday();
            
        }

        void ClearFilters()
        {
            NameFilter.Clear();
            AreaFilter.Clear();
        }
        void LoadDataFromFile()
        {
            
            ClearFilters();
            MinDate = DateTime.Now;
            var Records = ElectronicObserver.Resource.Record.RecordManager.Instance.ShipDrop.Record;
            for (int i = 1; i < Records.Count; i++)
            {
                var record = Records[i];
              
                DropData data = new DropData();
                data.DropShipName = record.ShipName;
                data.EquipmentName = record.EquipmentName;
                data.ItemName = record.ItemName;
                if (!NameFilter.Contains(record.ShipName))
                    NameFilter.Add(record.ShipName);
                data.Area = record.MapAreaID.ToString().PadLeft(2) + "-" + record.MapInfoID.ToString();
                if (!AreaFilter.Contains(data.Area))
                    AreaFilter.Add(data.Area);
                data.Point = record.CellID.ToString();
                data.Time = record.Date;
                if (MinDate > data.Time)
                    MinDate = data.Time;
                data.Rank = record.Rank;
                data.Boss = record.IsBossNode;
                data.Lv = record.HQLevel.ToString();
                if (Difficulty.ContainsKey(record.Difficulty))
                    data.Difficulty = Difficulty[record.Difficulty];
                else data.Difficulty = "无";
                DataList.Add(data);
            }
            DataList.ReverseSelf();
            DateTime date = new DateTime(MinDate.Year, MinDate.Month, MinDate.Day);
            timeSelection1.MinDate = date;
            AreaFilter.Sort();
            NameFilter.Sort();
            cbArea.Items.Add("");
            cbArea.Items.AddRange(AreaFilter.ToArray());
            cbShip.Items.AddRange(NameFilter.ToArray());
        }
        //0:ID  1:Name 2:Time 3,4,5:Area 6:Difficulty 7:Boss 8:Enemy 9:Rank 10:Lv

        void FilteredShow()
        {
            DateTime dt1 = new DateTime(timeSelection1.Since.Year, timeSelection1.Since.Month, timeSelection1.Since.Day);
            DateTime dd = timeSelection1.Until;
            dd = dd.AddDays(1);
            DateTime dt2 = new DateTime(dd.Year, dd.Month, dd.Day);
            string Name = cbShip.Text;
            string Area = cbArea.Text;
            bool boss = cbBOSS.Checked;
            FilteredDataList.Clear();
            foreach (var data in DataList)
            {
                if (data.Time > dt2 || data.Time < dt1)
                    continue;
                if (!data.DropShipName.Contains(Name))
                    continue;
                if (cbArea.SelectedIndex>=0)
                    if (Area != null && Area != "")
                    {
                        if (data.Area != Area)
                            continue;
                    }
                if (boss && !data.Boss)
                    continue;

                FilteredDataList.Add(data);
            }
            statusStrip.Items[0].Text = "符合条件的一共(" + FilteredDataList.Count.ToString() + ")条记录";
            dataGridView1.DataSource = FilteredDataList;
        }

        private void cbBOSS_CheckedChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void cbArea_TextChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void cbShip_TextChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void timeSelection1_DateChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }
    }

    class DropData
    {
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
        public bool Boss
        {
            get;
            set;
        }
        public string DropShipName
        {
            get;
            set;
        }
        public string EquipmentName
        {
            get;
            set;
        }

        public string ItemName
        {
            get;
            set;
        }
      
        public string Rank
        {
            get;
            set;
        }
        public string Difficulty
        {
            get;
            set;
        }
        public string Lv
        {
            get;
            set;
        }
    }
}
