using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Data;

namespace EquipmentsDetail
{
    public partial class FilterForm : Form
    {
        List<int> myfilters = null;
        public void SetFilter(List<int> filters)
        {
            myfilters = filters;

            foreach (var f in KCDatabase.Instance.MasterEquipments)
            {
                if (f.Value.IsAbyssalEquipment)
                    break;
                checkedListBox1.Items.Add(f.Value.Name, !myfilters.Contains(f.Value.EquipmentID));
            }
        }

        public FilterForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FilterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myfilters.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count;i++ )
            {
                if (!checkedListBox1.GetItemChecked(i))
                    myfilters.Add(i + 1);
            }
              
        }
    }
}
