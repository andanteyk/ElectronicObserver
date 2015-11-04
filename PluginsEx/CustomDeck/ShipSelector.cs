using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomDeck
{
    public partial class ShipSelector : Form
    {
        public ElectronicObserver.Data.ShipDataMaster ship = null; 
        public ShipSelector()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ElectronicObserver.Data.ShipType type = btn.Tag as ElectronicObserver.Data.ShipType;
            flowLayoutPanel2.Controls.Clear();
            var list = ShipUtility.GetShipList(type);
            foreach(var ship in list)
            {
                if (ship.ShipType == 2)//驱逐
                {
                    if (ship.RemodelAfterShipID >0)
                        continue;
                }
                var button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Text = ship.Name;
                button.Click += button2_Click;
                button.Tag = ship;
                button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                button.AutoSize = true;
                button.Parent = flowLayoutPanel2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ElectronicObserver.Data.ShipDataMaster shipdata = btn.Tag as ElectronicObserver.Data.ShipDataMaster;
            ship = shipdata;
            Close();
        }

        private void ShipSelector_Load(object sender, EventArgs e)
        {
            var types = ShipUtility.GetShipTypeList();
            foreach (var type in types)
            {
                var button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Text = type.Name;
                button.Click += button1_Click;
                button.Tag = type;
                button.Parent = flowLayoutPanel1;
            }
        }
    }
}
