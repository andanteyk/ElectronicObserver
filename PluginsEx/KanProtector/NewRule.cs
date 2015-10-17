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

namespace KanProtector
{
    public partial class NewRule : Form
    {
        public ShipProtection sp = null;
        public NewRule()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ID = 0;
            if (!int.TryParse(textBox1.Text, out ID))
            {
                MessageBox.Show("ID不对啊");
                return;
            }
           
            sp = new ShipProtection();
            sp.shipID = ID;
            sp.isContainDerived = checkBox1.Checked;
            sp.protectionType = radioButton1.Checked ? ProtectionType.保护最高 : ProtectionType.保护全部;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp = null;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShipSelector selector = new ShipSelector();
            selector.ShowDialog();
            if (selector.ship != null)
            {
                textBox1.Text = selector.ship.ShipID.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int ID = 0;
            if (!int.TryParse(textBox1.Text, out ID))
            {
                label2.Text = "ID格式错误";
                label2.ForeColor = Color.Red;
                button1.Enabled = false;
                toolTip1.SetToolTip(checkBox1, "");
                return;
            }
            if (KCDatabase.Instance.MasterShips[ID] == null)
            {
                label2.Text = "不存在此舰船";
                label2.ForeColor = Color.Red;
                button1.Enabled = false;
                toolTip1.SetToolTip(checkBox1, "");
                return;
            }
            if (KCDatabase.Instance.MasterShips[ID].IsAbyssalShip)
            {
                label2.Text = KCDatabase.Instance.MasterShips[ID].Name + "特么是深海的船";
                label2.ForeColor = Color.Red;
                button1.Enabled = false;
                toolTip1.SetToolTip(checkBox1, "");
                return;
            }
            var list = ShipUtility.GetDerivedShips(ID);
            StringBuilder bd = new StringBuilder();
            foreach(var id in list)
            {
                bd.Append(KCDatabase.Instance.MasterShips[id].Name);
                bd.Append(" ");
            }
            toolTip1.SetToolTip(checkBox1, bd.ToString());
            label2.ForeColor = ForeColor;
            label2.Text = KCDatabase.Instance.MasterShips[ID].Name;
            button1.Enabled = true;
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            button3.PerformClick();
        }
    }
}
