using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionHelper
{
    public partial class FilterForm : Form
    {
        public Dictionary<int, bool> Filters
        {
            get;
            set;
        }
        
        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Shown(object sender, EventArgs e)
        {
            foreach (var mi in MissionData.missionData.Data)
            {
                int ID = mi.No;
                FilterInformation fi = new FilterInformation(ID);
                bool TF = true;
                if (Filters.ContainsKey(ID) && Filters[ID])
                    TF = false;
                checkedListBox1.Items.Add(fi, TF);
            }
        }

        private void FilterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i=0; i< checkedListBox1.Items.Count;i++)
            {
                FilterInformation fi = (FilterInformation)checkedListBox1.Items[i];
                Filters[fi.ID] = !checkedListBox1.GetItemChecked(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    class FilterInformation
    {
        public FilterInformation(int id)
        {
            ID = id;
        }
        public string Text
        {
            get
            {
                 var mi = MissionData.missionData.GetMission(ID);
                 return mi.Name;
            }
        }

        public int ID
        {
            get;
            set;
        }

        public override string ToString()
        {
            return ID.ToString() + " " + Text;
        }
    }
}
