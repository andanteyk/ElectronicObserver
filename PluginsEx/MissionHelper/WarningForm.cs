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
    public partial class WarningForm : Form
    {
        public string Fleet = "";
        public WarningForm()
        {
            InitializeComponent();
        }

        private void WarningForm_Shown(object sender, EventArgs e)
        {
            label1.Text = Fleet + label1.Text;
        }
    }
}
