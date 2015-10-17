using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KanProtector
{
    public partial class EuqipmentSelector : Form
    {
        public ElectronicObserver.Data.EquipmentDataMaster Equipment = null;
        public EuqipmentSelector()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ElectronicObserver.Data.EquipmentType type = btn.Tag as ElectronicObserver.Data.EquipmentType;
            flowLayoutPanel2.Controls.Clear();
            var list = ShipUtility.GetEquipmentList(type);
            foreach(var equipment in list)
            {
                var button = new Button();
                button.ImageAlign = ContentAlignment.MiddleLeft;
                button.FlatStyle = FlatStyle.Flat;
                button.Text = equipment.Name;
                button.Click += button2_Click;
                button.Tag = equipment;
                button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                button.AutoSize = true;
                button.Parent = flowLayoutPanel2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ElectronicObserver.Data.EquipmentDataMaster equipment = btn.Tag as ElectronicObserver.Data.EquipmentDataMaster;
            Equipment = equipment;
            Close();
        }

        private void ShipSelector_Load(object sender, EventArgs e)
        {
            var types = ShipUtility.GetEquipmentTypeList();
            foreach (var type in types)
            {
                var button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Text = type.Name;
                button.Click += button1_Click;
                button.Tag = type;
                button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                button.AutoSize = true;
                button.Parent = flowLayoutPanel1;
            }
        }
    }
}
