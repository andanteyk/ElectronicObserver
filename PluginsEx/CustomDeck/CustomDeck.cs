using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using ElectronicObserver.Utility.Data;

namespace CustomDeck
{
    public partial class CustomDeck : Form
    {
        CustomFleet _fleet;
        public CustomFleet Fleet
        {
            get
            {
                return _fleet;
            }
            set
            {
                LoadFleet(value);
                _fleet = value;
            }
        }
        public CustomDeck()
        {
            InitializeComponent();
            TopLevel = false;
            dataGridView1.ForeColor = SystemColors.ControlText;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style = dataGridView1.ColumnHeadersDefaultCellStyle;
            }
            for (int i = 0; i < 6; i++)
            {
                dataGridView1.Rows.Add();
            }
            LoadFleet(_fleet);
        }

        void LoadFleet(CustomFleet fleet)
        {
            for (int i = 0; i < 6; i++)
            {
                if (fleet == null)
                    LoadShip(i, null);
                else
                    LoadShip(i, fleet.Ships[i]);
            }
        }

        void SetEquipment(int row, CustomShip ship, int index)
        {
            int MaxEquipment = index > 3 ? 5 : ship.Ship.SlotSize;
            CustomEquipmentment Equipmentment = index > 3 ? ship.EquipmentmentEx : ship.Equipmentments[index];
            string ColName = index > 3 ? "ColEquipmentX" : "ColEquipment" + (index + 1).ToString();
            if (MaxEquipment > index)
            {
                dataGridView1.Rows[row].Cells[ColName].Tag = 1;
                if (Equipmentment != null)
                {
                    if (index < 4 && Calculator.IsAircraft(Equipmentment.EquipmentmentID, true))
                        dataGridView1.Rows[row].Cells[ColName].Value = "[" + ship.Ship.Aircraft[index].ToString() + "]" + Equipmentment.Text;
                    else
                        dataGridView1.Rows[row].Cells[ColName].Value = Equipmentment.Text;
                }
                else
                {
                    //if (index < 4 && Calculator.IsAircraft(Equipmentment.EquipmentmentID, true))
                    //    dataGridView1.Rows[row].Cells[ColName].Value = "[" + ship.Ship.Aircraft[index].ToString() + "]" + "(未装备)";
                    //else
                        dataGridView1.Rows[row].Cells[ColName].Value = "(未装备)";
                  
                }
            }
            else
            {
                dataGridView1.Rows[row].Cells[ColName].Value = "";
                dataGridView1.Rows[row].Cells[ColName].ToolTipText = "";
                dataGridView1.Rows[row].Cells[ColName].Tag = null;
            }
        }
        void LoadShip(int row, CustomShip ship)
        {
            if (ship == null)
            {
                dataGridView1.Rows[row].Tag = null;
                dataGridView1.Rows[row].Cells["ColName"].Value = "(新舰船...)";
                dataGridView1.Rows[row].Cells["ColName"].Tag = 1;
                dataGridView1.Rows[row].Cells["ColLevel"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment1"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment1"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment2"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment2"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment3"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment3"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment4"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment4"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipmentX"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipmentX"].Tag = null;
            }
            else
            {
                dataGridView1.Rows[row].Tag = 1;
                dataGridView1.Rows[row].Cells["ColName"].Value = ship.Ship.NameWithClass;
                dataGridView1.Rows[row].Cells["ColLevel"].Value = ship.Level;
                dataGridView1.Rows[row].Cells["ColName"].Tag = 1;
                for (int index = 0; index < 5; index++)
                    SetEquipment(row, ship, index);
            }
        }


        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
                {
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "ColName")
                    {
                        MenuShip.Tag = e.RowIndex;
                        MenuShip.Show(dataGridView1, e.X + rect.X, e.Y + rect.Y);
                    }
                    else
                    {
                        MenuEquipment.Tag = new Point(e.RowIndex, e.ColumnIndex - 3);
                        MenuEquipment.Show(dataGridView1, e.X + rect.X, e.Y + rect.Y);
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
            {
                Rectangle rect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                if (dataGridView1.Columns[e.ColumnIndex].Name == "ColName")
                {
                    MenuShip.Tag = e.RowIndex;
                    MenuShip.Show(dataGridView1, e.X + rect.X, e.Y + rect.Y);
                }
                else
                {
                    MenuEquipment.Tag = new Point(e.RowIndex, e.ColumnIndex - 3);
                    MenuEquipment.Show(dataGridView1, e.X + rect.X, e.Y + rect.Y);
                }
            }
        }
    }
}
