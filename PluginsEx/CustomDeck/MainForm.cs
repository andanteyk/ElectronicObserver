using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Window;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Support;

namespace CustomDeck
{
    public partial class MainForm : DockContent
    {
        //DeckData DataList;

        CustomFleets Fleets = new CustomFleets();
        CustomDeck[] Decks = new CustomDeck[4];
        public MainForm(FormMain main)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();
            ImportFleets(s);
        }

        void ImportFleets(string s)
        {
            if (s != null)
            {
                if (Fleets.ImportFromString(s))
                {
                    for (int i = 0; i < Decks.Length; i++)
                    {
                        Decks[i].Fleet = Fleets.Fleets[i];
                    }
                    splitContainer1.Panel2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("配置字符串不正确!无法解析");
                }
            }
            else
            {
                for (int i = 0; i < Decks.Length; i++)
                {
                    Fleets.Fleets[i] = null;
                    Decks[i].Fleet = Fleets.Fleets[i];
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Fleets.Serialize());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DeckData.Instance.LoadConfig();
            LoadList();
            if (listView1.Items.Count>0)
            {
                this.listView1.Items[0].Selected = true;
                this.listView1.Select();
                this.listView1.Items[0].Focused = true; 
            }
        }

        void LoadList()
        {
            listView1.BeginUpdate();
            foreach (var deck in DeckData.Instance.DeckList)
            {
                var item = listView1.Items.Add(deck.Name);
                item.Tag = deck;
                item.SubItems.Add(deck.Flagship);
                item.ToolTipText = "";
            }
            listView1.EndUpdate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                FleetDeck deck = item.Tag as FleetDeck;
                ImportFleets(deck.Json);
            }
            else
            {
                ImportFleets(null);
                splitContainer1.Panel2.Enabled = false;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool enabled = listView1.SelectedItems.Count > 0;
            contextMenuStrip1.Enabled = enabled;
            删除编成ToolStripMenuItem.Enabled = enabled;
            导入游戏当前舰队ToolStripMenuItem.Enabled = enabled;
        }

        private void 删除编成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int index = listView1.Items.IndexOf(listView1.SelectedItems[0]);
                listView1.Items.RemoveAt(index);
                DeckData.Instance.RemoveDeck(index);
                //ImportFleets(null);
                splitContainer1.Panel2.Enabled = false;
            }
        }

        private void 新增编成ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            InputForm input = new InputForm();
            if (input.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var deck = DeckData.Instance.AddDeck(input.InputText, null);
                var item = listView1.Items.Add(deck.Name);
                item.Tag = deck;
                item.SubItems.Add(deck.Flagship);
                item.ToolTipText = "";

                listView1.Items[item.Index].Focused = true;
                listView1.Items[item.Index].Selected = true;
            }

        }

        private void 导入游戏当前舰队ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ImportFleets(CopyFleetDeckBuilder());
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                InputForm input = new InputForm();
                if (input.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var item = listView1.SelectedItems[0];
                    FleetDeck deck = item.Tag as FleetDeck;
                    deck.Name = input.InputText;
                    item.Text = input.InputText;
                }
            }
        }

        private string CopyFleetDeckBuilder()
        {

            StringBuilder sb = new StringBuilder();
            KCDatabase db = KCDatabase.Instance;

            // 手書き json の悲しみ

            sb.Append(@"{""version"":3,");

            foreach (var fleet in db.Fleet.Fleets.Values)
            {
                if (fleet == null) continue;

                sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

                int shipcount = 1;
                foreach (var ship in fleet.MembersInstance)
                {
                    if (ship == null) break;

                    sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
                        shipcount,
                        ship.ShipID,
                        ship.Level,
                        ship.LuckBase);

                    if (ship.ExpansionSlot <= 0)
                        sb.Append(@"""ix"":{},");
                    else
                        sb.AppendFormat(@"""ix"":{{""id"":{0}}},", ship.ExpansionSlotMaster);

                    int eqcount = 1;
                    foreach (var eq in ship.SlotInstance)
                    {
                        if (eq == null) break;
                        sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2}}},", eqcount, eq.EquipmentID, Math.Max(eq.Level, eq.AircraftLevel));

                        eqcount++;
                    }

                    sb.Remove(sb.Length - 1, 1);		// remove ","
                    sb.Append(@"}},");

                    shipcount++;
                }

                sb.Remove(sb.Length - 1, 1);		// remove ","
                sb.Append(@"},");

            }

            sb.Remove(sb.Length - 1, 1);		// remove ","
            sb.Append(@"}");

            return sb.ToString();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < Decks.Length; i++)
            {
                Decks[i] = new CustomDeck();
                Decks[i].Parent = tabControl1.TabPages[i];
                Decks[i].Dock = DockStyle.Fill;
                Decks[i].Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(MousePosition);
        }

       

     
    }
}
