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
    public partial class DevelopmentViewer : Form, IStatusBar
    {
        string DataFileName = Application.StartupPath + "\\record\\DevelopmentRecord.csv";

        SortedBindingList<DropData2> DataList = new SortedBindingList<DropData2>();
        SortedBindingList<DropData2> FilteredDataList = new SortedBindingList<DropData2>();
        DateTime MinDate;

        List<string> NameFilter = new List<string>();

        public StatusStrip statusStrip
        {
            get;
            set;
        }
        public DevelopmentViewer()
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
            var Records = ElectronicObserver.Resource.Record.RecordManager.Instance.Development.Record;
            for (int i = 1; i < Records.Count; i++)
            {
                var record = Records[i];
               
                DropData2 data = new DropData2();
                data.Name = record.EquipmentName;
                if (!NameFilter.Contains(record.EquipmentName))
                    NameFilter.Add(record.EquipmentName);
                data.Fuel = record.Fuel;
                data.Ammo = record.Ammo;
                data.Steel = record.Steel;
                data.Al = record.Bauxite;

                data.Time = record.Date;
                if (MinDate > data.Time)
                    MinDate = data.Time;

                data.Secretary = record.FlagshipName;
                data.Lv = record.HQLevel.ToString();
             
                DataList.Add(data);
            }
            DataList.ReverseSelf();
            NameFilter.Sort();
            DateTime date = new DateTime(MinDate.Year, MinDate.Month, MinDate.Day);
            timeSelection1.MinDate = date;
            cbShip.Items.AddRange(NameFilter.ToArray());
        }
        //0:ID  1:Name 2:Time 3,4,5:Area 6:Difficulty 7:Boss 8:Enemy 9:Rank 10:Lv

        int GetTextValue(string text, int defaultValue)
        {
            int value = defaultValue;
            if (text != null)
            {
                if (!int.TryParse(text, out value))
                {
                    value = defaultValue;
                }
            }
            return value;
        }
        void FilteredShow()
        {
            int fmin, fmax, amin, amax, smin, smax, xmin, xmax;
            fmin = GetTextValue(Fmin.Text, 0); fmax = GetTextValue(Fmax.Text, 300);
            amin = GetTextValue(Amin.Text, 0); amax = GetTextValue(Amax.Text, 300);
            smin = GetTextValue(Smin.Text, 0); smax = GetTextValue(Smax.Text, 300);
            xmin = GetTextValue(Xmin.Text, 0); xmax = GetTextValue(Xmax.Text, 300);

            DateTime dt1 = new DateTime(timeSelection1.Since.Year, timeSelection1.Since.Month, timeSelection1.Since.Day);
            DateTime dd = timeSelection1.Until;
            dd = dd.AddDays(1);
            DateTime dt2 = new DateTime(dd.Year, dd.Month, dd.Day);
            string Name = cbShip.Text;

            FilteredDataList.Clear();
            foreach (var data in DataList)
            {
                if (data.Time > dt2 || data.Time < dt1)
                    continue;
                if (Name != "")
                {
                    if (data.Name != Name)
                        continue;
                }
                if (data.Fuel > fmax || data.Fuel < fmin)
                    continue;
                if (data.Ammo > amax || data.Ammo < amin)
                    continue;
                if (data.Steel > smax || data.Steel < smin)
                    continue;
                if (data.Al > xmax || data.Al < xmin)
                    continue;

                FilteredDataList.Add(data);
            }
            statusStrip.Items[0].Text = "符合条件的一共(" + FilteredDataList.Count.ToString() + ")条记录";
            dataGridView1.DataSource = FilteredDataList;
        }

        private void cbShip_TextChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void timeSelection1_DateChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }

        private void Fmin_TextChanged(object sender, EventArgs e)
        {
            FilteredShow();
        }
    }

    class DropData2
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
        public string Name
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
