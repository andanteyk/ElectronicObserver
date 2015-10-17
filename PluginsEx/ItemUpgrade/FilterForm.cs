using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemUpgrade
{
    public partial class FilterForm : Form
    {
        Dictionary<string, bool> myfilters = null;
        public void SetFilter(Dictionary<string, bool> filters)
        {
            myfilters = filters;
            foreach (var f in filters)
            {
                checkedListBox1.Items.Add(f.Key, !f.Value);
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
            for (int i = 0; i < checkedListBox1.Items.Count;i++ )
            {
                myfilters[checkedListBox1.GetItemText(checkedListBox1.Items[i])] = !checkedListBox1.GetItemChecked(i);
            }
              
        }
    }
}
