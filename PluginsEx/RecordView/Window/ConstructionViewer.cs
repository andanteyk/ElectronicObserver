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
    public partial class ConstructionViewer : Form, IStatusBar
    {
        string DataFileName = Application.StartupPath + "\\record\\ConstructionRecord.csv";

        SortedBindingList<DropData1> DataList = new SortedBindingList<DropData1>();
        SortedBindingList<DropData1> FilteredDataList = new SortedBindingList<DropData1>();
        DateTime MinDate;

        List<string> NameFilter = new List<string>();

        public StatusStrip statusStrip
        {
            get;
            set;
        }
        public ConstructionViewer()
        {
            InitializeComponent();
        }

        private void DropViewer_Shown(object sender, EventArgs e)
        {
                LoadDataFromFile();
                timeSelection1.SelectToday();
            
        }

        void ClearFilters()
        {
            NameFilter.Clear();

        }
        void LoadDataFromFile()
        {
            ClearFilters();
            MinDate = DateTime.Now;
            var Records = ElectronicObserver.Resource.Record.RecordManager.Instance.Construction.Record;
            for (int i = 1; i < Records.Count; i++)
            {
                var record = Records[i];
                DropData1 data = new DropData1();
                data.ShipName = record.ShipName;
                if (!NameFilter.Contains(record.ShipName))
                    NameFilter.Add(record.ShipName);
                data.Fuel = record.Fuel;
                data.Ammo = record.Ammo;
                data.Steel = record.Steel;
                data.Al = record.Bauxite;
                data.Material = record.DevelopmentMaterial;

                data.Time = record.Date;
                if (MinDate > data.Time)
                    MinDate = data.Time;

                data.GreatConstruction = record.IsLargeDock;
                data.Lv = record.HQLevel.ToString();
                data.Secretary = record.FlagshipName;
                DataList.Add(data);
            }
            DataList.ReverseSelf();
            NameFilter.Sort();
            DateTime date = new DateTime(MinDate.Year, MinDate.Month, MinDate.Day);
            timeSelection1.MinDate = date;
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
            bool boss = cbBOSS.Checked;
            FilteredDataList.Clear();
            foreach (var data in DataList)
            {
                if (data.Time > dt2 || data.Time < dt1)
                    continue;
                if (Name != "")
                {
                    if (data.ShipName != Name)
                        continue;
                }
                //if (Area != "")
                //{
                //    if (data.Area != Area)
                //        continue;
                //}
                if (boss && !data.GreatConstruction)
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

    
        private void cbShip_TextChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void timeSelection1_DateChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }
    }

    class DropData1
    {
        public DateTime Time
        {
            get;
            set;
        }
        public int Fuel
        {
            get;
            set;
        }
        public int Ammo
        {
            get;
            set;
        }
        public int Steel
        {
            get;
            set;
        }
        public int Al
        {
            get;
            set;
        }
        public int Material
        {
            get;
            set;
        }
        public bool GreatConstruction
        {
            get;
            set;
        }
        public string ShipName
        {
            get;
            set;
        }
      
        public string Secretary
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
