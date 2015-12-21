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
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Data;

namespace CustomDeck
{
    public partial class CustomDeckForm : Form
    {
        public delegate void DeckChangedEvent(CustomDeckForm sender);
        public event DeckChangedEvent DeckChanged;
        public int DeckNo
        {
            get;
            set;
        }

        public bool ShowEquipmentImage
        {
            set
            {
                ColEquipment1.Visible = !value;
                ColEquipment2.Visible = !value;
                ColEquipment3.Visible = !value;
                ColEquipment4.Visible = !value;
                ColEquipmentX.Visible = !value;

                ColEquipmentG1.Visible = value;
                ColEquipmentG2.Visible = value;
                ColEquipmentG3.Visible = value;
                ColEquipmentG4.Visible = value;
                ColEquipmentGX.Visible = value;
            }
        }

        void OnDeckChanged()
        {
            if (DeckChanged != null)
                DeckChanged(this);
        }

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
        public CustomDeckForm()
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
            string ColGraph = index > 3 ? "ColEquipmentGX" : "ColEquipmentG" + (index + 1).ToString();
            if (MaxEquipment > index)
            {
                if (Equipmentment != null)
                {
                    dataGridView1.Rows[row].Cells[ColName].Tag = Equipmentment.EquipmentmentID;
                    if (index < 4 && Calculator.IsAircraft(Equipmentment.EquipmentmentID, true))
                        dataGridView1.Rows[row].Cells[ColName].Value = "[" + ship.Ship.Aircraft[index].ToString() + "]" + Equipmentment.Text;
                    else
                        dataGridView1.Rows[row].Cells[ColName].Value = Equipmentment.Text;
                    dataGridView1.Rows[row].Cells[ColGraph].ToolTipText = dataGridView1.Rows[row].Cells[ColName].Value.ToString();

                    int IconID = KCDatabase.Instance.MasterEquipments[Equipmentment.EquipmentmentID].IconType;
                    dataGridView1.Rows[row].Cells[ColGraph].Value = ElectronicObserver.Resource.ResourceManager.GetEquipmentImage(IconID);
                }
                else
                {
                    dataGridView1.Rows[row].Cells[ColName].Tag = 0;
                    //if (index < 4 && Calculator.IsAircraft(Equipmentment.EquipmentmentID, true))
                    //    dataGridView1.Rows[row].Cells[ColName].Value = "[" + ship.Ship.Aircraft[index].ToString() + "]" + "(未装备)";
                    //else
                        dataGridView1.Rows[row].Cells[ColName].Value = "(未装备)";
                        dataGridView1.Rows[row].Cells[ColGraph].Value = ElectronicObserver.Resource.ResourceManager.GetEquipmentImage(0);
                }
            }
            else
            {
                dataGridView1.Rows[row].Cells[ColName].Value = "";
                dataGridView1.Rows[row].Cells[ColName].ToolTipText = "";
                
                dataGridView1.Rows[row].Cells[ColName].Tag = null;
                dataGridView1.Rows[row].Cells[ColGraph].Value = ElectronicObserver.Resource.ResourceManager.GetEquipmentImage(-1);
                dataGridView1.Rows[row].Cells[ColGraph].ToolTipText = "";
            }
            dataGridView1.Rows[row].Cells[ColGraph].Tag = dataGridView1.Rows[row].Cells[ColName].Tag;
        }
        void LoadShip(int row, CustomShip ship)
        {
            if (ship == null)
            {
                dataGridView1.Rows[row].Tag = null;
                dataGridView1.Rows[row].Cells["ColName"].Value = "(新舰船...)";
                dataGridView1.Rows[row].Cells["ColName"].Tag = 0;
                dataGridView1.Rows[row].Cells["ColLevel"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment1"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment1"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment1"].ToolTipText = "";
                dataGridView1.Rows[row].Cells["ColEquipment2"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment2"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment2"].ToolTipText = "";
                dataGridView1.Rows[row].Cells["ColEquipment3"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment3"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment3"].ToolTipText = "";
                dataGridView1.Rows[row].Cells["ColEquipment4"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipment4"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipment4"].ToolTipText = "";
                dataGridView1.Rows[row].Cells["ColEquipmentX"].Value = "";
                dataGridView1.Rows[row].Cells["ColEquipmentX"].Tag = null;
                dataGridView1.Rows[row].Cells["ColEquipmentX"].ToolTipText = "";
            }
            else
            {
                dataGridView1.Rows[row].Tag = 1;
                dataGridView1.Rows[row].Cells["ColName"].Value = ship.Ship.NameWithClass;
                dataGridView1.Rows[row].Cells["ColLevel"].Value = ship.Level;
                dataGridView1.Rows[row].Cells["ColName"].Tag = ship.ShipID;
                for (int index = 0; index < 5; index++)
                    SetEquipment(row, ship, index);
            }
        }

        int GetEquipmentIndex(string ColName)
        {
            string LastChar = ColName.Substring(ColName.Length - 1);
            return LastChar == "X" ? 4 : int.Parse(LastChar) - 1;
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
                        int index = GetEquipmentIndex(dataGridView1.Columns[e.ColumnIndex].Name);
                        MenuEquipment.Tag = new Point(e.RowIndex, index);
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
                    int index = GetEquipmentIndex(dataGridView1.Columns[e.ColumnIndex].Name);
                    MenuEquipment.Tag = new Point(e.RowIndex, index);
                    MenuEquipment.Show(dataGridView1, e.X + rect.X, e.Y + rect.Y);
                }
            }
        }

        private void 更换舰船ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;
            if (ti.GetCurrentParent().Tag != null)
            {
                int id = (int)ti.GetCurrentParent().Tag;

                ShipSelector selector = new ShipSelector();
                selector.ShowDialog();
                if (selector.ship == null)
                    return;
                if (Fleet.Ships[id] == null)
                    Fleet.Ships[id] = new CustomShip();
                Fleet.Ships[id].ShipID = selector.ship.ShipID;
                LoadShip(id, Fleet.Ships[id]);
                OnDeckChanged();
            }
        }

        private void 清除舰船_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;
            if (ti.GetCurrentParent().Tag != null)
            {
                int id = (int)ti.GetCurrentParent().Tag;
                Fleet.Ships[id] = null;
                LoadShip(id, null);
                OnDeckChanged();
            }
        }

        private void 更换装备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             ToolStripMenuItem ti = sender as ToolStripMenuItem;

             if (ti.GetCurrentParent().Tag != null)
             {
                 Point pt = (Point)ti.GetCurrentParent().Tag;

                 EuqipmentSelector selector = new EuqipmentSelector();
                 selector.ShowDialog();
                 if (selector.Equipment == null)
                     return;
                 int id = selector.Equipment.EquipmentID;

                 if (pt.Y < 4)
                 {
                     if (Fleet.Ships[pt.X].Equipmentments[pt.Y] == null)
                         Fleet.Ships[pt.X].Equipmentments[pt.Y] = new CustomEquipmentment(id, 0);
                     else
                         Fleet.Ships[pt.X].Equipmentments[pt.Y].EquipmentmentID = id;
                 }
                 else
                 {
                     if (Fleet.Ships[pt.X].EquipmentmentEx == null)
                         Fleet.Ships[pt.X].EquipmentmentEx = new CustomEquipmentment(id, 0);
                     else
                         Fleet.Ships[pt.X].EquipmentmentEx.EquipmentmentID = id;
                 }
                 SetEquipment(pt.X, Fleet.Ships[pt.X], pt.Y);

                 OnDeckChanged();
             }
        }

        private void 清除装备_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;

            if (ti.GetCurrentParent().Tag != null)
            {
                Point id = (Point)ti.GetCurrentParent().Tag;
                if (id.Y < 4)
                    Fleet.Ships[id.X].Equipmentments[id.Y] = null;
                else
                    Fleet.Ships[id.X].EquipmentmentEx = null;
                SetEquipment(id.X, Fleet.Ships[id.X], id.Y);

                OnDeckChanged();
            }
        }

        private void toolStrip舰船图鉴_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;
            if (ti.GetCurrentParent().Tag != null)
            {
                int id = (int)ti.GetCurrentParent().Tag;
                new DialogAlbumMasterShip(Fleet.Ships[id].Ship.ShipID).Show(Parent);
            }
        }

        private void 查看图鉴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;

            if (ti.GetCurrentParent().Tag != null)
            {
                Point id = (Point)ti.GetCurrentParent().Tag;
                if (id.Y < 4)
                    new DialogAlbumMasterEquipment(Fleet.Ships[id.X].Equipmentments[id.Y].EquipmentmentID).Show(Parent);
                else
                    new DialogAlbumMasterEquipment(Fleet.Ships[id.X].EquipmentmentEx.EquipmentmentID).Show(Parent);
            }
        }

        private void MenuShip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip ti = sender as ContextMenuStrip;
            if (ti.Tag != null)
            {
                int index = (int)ti.Tag;
                int id = Fleet.Ships[index] == null ? 0 : Fleet.Ships[index].ShipID;
                if (id == 0)
                {
                    更换舰船ToolStripMenuItem.Enabled = true;
                    改造ToolStripMenuItem.Enabled = false;
                    toolStripMenu舰船图鉴.Enabled = false;
                    清除DToolStripMenuItem.Enabled = false;
                }
                else
                {
                    更换舰船ToolStripMenuItem.Enabled = true;
                    改造ToolStripMenuItem.Enabled = true;
                    toolStripMenu舰船图鉴.Enabled = true;
                    清除DToolStripMenuItem.Enabled = true;

                    var lists = ShipUtility.GetRelativedShips(id);
                    改造ToolStripMenuItem.DropDownItems.Clear();
                    foreach(int shipid in lists)
                    {
                        var item = 改造ToolStripMenuItem.DropDownItems.Add(KCDatabase.Instance.MasterShips[shipid].Name);
                        item.Tag = new Point(index, shipid);
                        item.Click += Remodel_Click;
                    }
                }
            }
        }

        void Remodel_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;
            if (ti.Tag != null)
            {
                Point id = (Point)ti.Tag;
                int row = id.X;
                int shipid = id.Y;
                Fleet.Ships[row].ShipID = shipid;
                LoadShip(row, Fleet.Ships[row]);
                OnDeckChanged();
            }
        }

        private void SetLevel_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = sender as ToolStripMenuItem;
            var Strip = ti.OwnerItem.Owner;
            if (Strip.Tag != null)
            {
                int Level = int.Parse(ti.Text);
                Point pt = (Point)Strip.Tag;
                int row = pt.X;
                int index = pt.Y;

                if (index < 4)
                    Fleet.Ships[row].Equipmentments[index].Level = Level;
                else
                    Fleet.Ships[row].EquipmentmentEx.Level = Level;
                SetEquipment(row, Fleet.Ships[row], index);

                OnDeckChanged();
            }
        }

        private void MenuEquipment_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip ti = sender as ContextMenuStrip;
            if (ti.Tag != null)
            {
                Point pt = (Point)ti.Tag;

                CustomEquipmentment eq;
                if (pt.Y > 3)
                {
                    eq = Fleet.Ships[pt.X].EquipmentmentEx;
                }
                else
                {
                    eq = Fleet.Ships[pt.X].Equipmentments[pt.Y];
                }
                if (eq == null)
                {
                    更换装备ToolStripMenuItem.Enabled = true;
                    设置等级ToolStripMenuItem.Enabled = false;
                    查看图鉴ToolStripMenuItem.Enabled = false;
                    清除DToolStripMenuItem1.Enabled = false;
                }
                else
                {
                    更换装备ToolStripMenuItem.Enabled = true;
                    设置等级ToolStripMenuItem.Enabled = true;
                    查看图鉴ToolStripMenuItem.Enabled = true;
                    清除DToolStripMenuItem1.Enabled = true;
                }
            }
        }
    }
}
