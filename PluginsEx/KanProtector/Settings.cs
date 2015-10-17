using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Notifier;
using ElectronicObserver.Window.Plugins;
using System.IO;

namespace KanProtector
{
    public partial class Settings : PluginSettingControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        bool Initial = false;

        public override bool Save()
        {
            if (Initial)
                ProtectionData.Instance.SaveConfig();
            return true;
        }

        void LoadShipData()
        {
            for (int index = 0; index < ProtectionData.Instance.shipList.Count; index++)
            {
                ShipProtection data = ProtectionData.Instance.shipList[index];
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells["ColIndex"].Value = index;
                dataGridView1.Rows[row].Cells["ColID"].Value = data.shipID;
                dataGridView1.Rows[row].Cells["ColShipType"].Value = KCDatabase.Instance.MasterShips[data.shipID].ShipTypeName;
                dataGridView1.Rows[row].Cells["ColName"].Value = KCDatabase.Instance.MasterShips[data.shipID].Name;
                dataGridView1.Rows[row].Cells["ColDerived"].Value = data.isContainDerived;
                dataGridView1.Rows[row].Cells["ColHighest"].Value = data.protectionType == ProtectionType.保护最高;
                dataGridView1.Rows[row].Cells["ColAll"].Value = data.protectionType == ProtectionType.保护全部;
            }
            dataGridView1.Sort(ColID, ListSortDirection.Ascending);
        }

        void LoadEquipmentData()
        {
            for (int index = 0; index < ProtectionData.Instance.equipmentList.Count; index++)
            {
                int ID = ProtectionData.Instance.equipmentList[index];
                int row = dataGridView2.Rows.Add();
                dataGridView2.Rows[row].Cells["EColIndex"].Value = index;
                dataGridView2.Rows[row].Cells["EColID"].Value = ID;
                dataGridView2.Rows[row].Cells["EColImage"].Value = KCDatabase.Instance.MasterEquipments[ID].IconType;
                dataGridView2.Rows[row].Cells["EColType"].Value = KCDatabase.Instance.MasterEquipments[ID].CategoryTypeInstance.Name;
                dataGridView2.Rows[row].Cells["EColName"].Value = KCDatabase.Instance.MasterEquipments[ID].Name;
            }
            dataGridView2.Sort(EColImage, ListSortDirection.Ascending);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (KCDatabase.Instance.MasterShips.Count < 1)
            {
                this.Enabled = false;
                return;
            }
            Initial = true;
            checkBox1.Checked = ProtectionData.Instance.ShipProtectionEnabled;
            checkBox2.Checked = ProtectionData.Instance.EquipmentProtectionEnabled;

            LoadShipData();
            LoadEquipmentData();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ColDerived")
            {
                int index = (int)(dataGridView1.Rows[e.RowIndex].Cells["ColIndex"].Value);
                ProtectionData.Instance.shipList[index].isContainDerived = (bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                ProtectionData.Instance.SaveConfig();
            }

        }

        private void 删除该条目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        void DeleteRows()
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                int ID = (int)(dataGridView1.SelectedRows[i].Cells["ColID"].Value);
                ProtectionData.Instance.DeleteShip(ID);
            }
            foreach (var obj in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove((DataGridViewRow)obj);
            }
            ProtectionData.Instance.SaveConfig();
        }

        void ShowAlbum()
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;
            var row = dataGridView1.SelectedRows[0];


            int ID = (int)(dataGridView1.Rows[row.Index].Cells["ColID"].Value);
            new DialogAlbumMasterShip(ID).Show(this);

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRows();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewRule nr = new NewRule();
            nr.ShowDialog();

            if (nr.sp != null)
            {
                int index = ProtectionData.Instance.AddShip(nr.sp);
                if (index >= 0)
                {
                    MessageBox.Show("已经存在这个舰船了");
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        int Index = (int)dataGridView1.Rows[i].Cells["ColIndex"].Value;
                        if (Index == index)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    ProtectionData.Instance.SaveConfig();
                    int newr = dataGridView1.Rows.Add();
                    dataGridView1.Rows[newr].Cells["ColIndex"].Value = ProtectionData.Instance.shipList.Count - 1;
                    dataGridView1.Rows[newr].Cells["ColID"].Value = nr.sp.shipID;
                    dataGridView1.Rows[newr].Cells["ColShipType"].Value = KCDatabase.Instance.MasterShips[nr.sp.shipID].ShipTypeName;
                    dataGridView1.Rows[newr].Cells["ColName"].Value = KCDatabase.Instance.MasterShips[nr.sp.shipID].Name;
                    dataGridView1.Rows[newr].Cells["ColDerived"].Value = nr.sp.isContainDerived;
                    dataGridView1.Rows[newr].Cells["ColHighest"].Value = nr.sp.protectionType == ProtectionType.保护最高;
                    dataGridView1.Rows[newr].Cells["ColAll"].Value = nr.sp.protectionType == ProtectionType.保护全部;

                    var col = dataGridView1.SortedColumn == null ? dataGridView1.Columns[1] : dataGridView1.SortedColumn;
                    ListSortDirection dir = ListSortDirection.Ascending;
                    if (dataGridView1.SortOrder == SortOrder.Descending)
                        dir = ListSortDirection.Descending;
                    dataGridView1.Sort(col, dir);

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        int Index = (int)dataGridView1.Rows[i].Cells["ColIndex"].Value;
                        if (Index == ProtectionData.Instance.shipList.Count - 1)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                if (column.Name == "ColHighest")
                {
                    int index = (int)(dataGridView1.Rows[e.RowIndex].Cells["ColIndex"].Value);
                    bool tf = !(bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tf;
                    ProtectionData.Instance.shipList[index].protectionType = tf ? ProtectionType.保护最高 : ProtectionType.保护全部;
                    dataGridView1.Rows[e.RowIndex].Cells["ColAll"].Value = !tf;
                }
                if (column.Name == "ColAll")
                {
                    int index = (int)(dataGridView1.Rows[e.RowIndex].Cells["ColIndex"].Value);
                    bool tf = !(bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tf;
                    ProtectionData.Instance.shipList[index].protectionType = tf ? ProtectionType.保护全部 : ProtectionType.保护最高;
                    dataGridView1.Rows[e.RowIndex].Cells["ColHighest"].Value = !tf;

                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ProtectionData.Instance.ShipProtectionEnabled = checkBox1.Checked;
            ProtectionData.Instance.SaveConfig();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ProtectionData.Instance.EquipmentProtectionEnabled = checkBox2.Checked;
            ProtectionData.Instance.SaveConfig();
        }

        private void 查看图鉴双击ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAlbum();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowAlbum();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ID = 0;
            if (!int.TryParse(textBox2.Text, out ID))
            {
                MessageBox.Show("ID不对啊");
                return;
            }


            int index = ProtectionData.Instance.AddEquipment(ID);
            if (index >= 0)
            {
                MessageBox.Show("已经存在这个装备了");
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int Index = (int)dataGridView2.Rows[i].Cells["EColIndex"].Value;
                    if (Index == index)
                    {
                        dataGridView2.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }
            }
            else
            {
                ProtectionData.Instance.SaveConfig();
                int newr = dataGridView2.Rows.Add();
                dataGridView2.Rows[newr].Cells["EColIndex"].Value = ProtectionData.Instance.equipmentList.Count - 1;
                dataGridView2.Rows[newr].Cells["EColID"].Value = ID;
                dataGridView2.Rows[newr].Cells["EColImage"].Value = KCDatabase.Instance.MasterEquipments[ID].IconType;
                dataGridView2.Rows[newr].Cells["EColType"].Value = KCDatabase.Instance.MasterEquipments[ID].CategoryTypeInstance.Name;
                dataGridView2.Rows[newr].Cells["EColName"].Value = KCDatabase.Instance.MasterEquipments[ID].Name;


                var col = dataGridView2.SortedColumn == null ? dataGridView2.Columns[1] : dataGridView2.SortedColumn;
                ListSortDirection dir = ListSortDirection.Ascending;
                if (dataGridView2.SortOrder == SortOrder.Descending)
                    dir = ListSortDirection.Descending;
                dataGridView2.Sort(col, dir);

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int Index = (int)dataGridView2.Rows[i].Cells["EColIndex"].Value;
                    if (Index == ProtectionData.Instance.equipmentList.Count - 1)
                    {
                        dataGridView2.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }
            }
        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            EuqipmentSelector selector = new EuqipmentSelector();
            selector.ShowDialog();
            if (selector.Equipment != null)
            {
                textBox2.Text = selector.Equipment.EquipmentID.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int ID = 0;
            if (!int.TryParse(textBox2.Text, out ID))
            {
                label1.Text = "ID格式错误";
                label1.ForeColor = Color.Red;
                button2.Enabled = false;
                return;
            }
            if (KCDatabase.Instance.MasterEquipments[ID] == null)
            {
                label1.Text = "不存在此装备";
                label1.ForeColor = Color.Red;
                button2.Enabled = false;
                return;
            }
            if (KCDatabase.Instance.MasterEquipments[ID].IsAbyssalEquipment)
            {
                label1.Text = KCDatabase.Instance.MasterEquipments[ID].Name + "特么是深海的装备";
                label1.ForeColor = Color.Red;
                button2.Enabled = false;
                return;
            }

            label1.ForeColor = ForeColor;
            label1.Text = KCDatabase.Instance.MasterEquipments[ID].Name;
            button2.Enabled = true;
            button2.PerformClick();
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == EColImage.Index)
            {
                if (e.Value == null)
                    return;
                e.Value = ElectronicObserver.Resource.ResourceManager.GetEquipmentImage((int)e.Value);
                e.FormattingApplied = true;
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeleteRows2();
            ProtectionData.Instance.SaveConfig();
        }
        void DeleteRows2()
        {
            for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
            {
                int ID = (int)(dataGridView2.SelectedRows[i].Cells["EColID"].Value);
                ProtectionData.Instance.DeleteEquipment(ID);
            }
            foreach (var obj in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove((DataGridViewRow)obj);
            }
            ProtectionData.Instance.SaveConfig();
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ShowAlbum2();
        }
        void ShowAlbum2()
        {
            if (dataGridView2.SelectedRows.Count < 1)
                return;
            var row = dataGridView2.SelectedRows[0];


            int ID = (int)(dataGridView2.Rows[row.Index].Cells["EColID"].Value);
            new DialogAlbumMasterEquipment(ID).Show(this);
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRows2();
                ProtectionData.Instance.SaveConfig();
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            ShowAlbum2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = System.IO.File.ReadAllText("d:\\1.json");
            string Warning = HandleRequest.OnDestroyShip(s);
            s = System.IO.File.ReadAllText("d:\\2.json");
            Warning = HandleRequest.OnDestroyItem(s);
            //s = System.IO.File.ReadAllText("d:\\3.json");
            //Warning = HandleRequest.OnPowerUp(s);
        }
    }
}
