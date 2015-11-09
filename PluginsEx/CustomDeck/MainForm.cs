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

using Microsoft.Win32;

using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Window;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Support;

namespace CustomDeck
{
    public partial class DeckMainForm : DockContent
    {

        CustomFleets Fleets = new CustomFleets();
        CustomDeckForm[] Decks = new CustomDeckForm[4];
        public DeckMainForm(FormMain main)
        {
            InitializeComponent();
        }

        void OnDeckChanged(CustomDeckForm sender)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                FleetDeck deck = item.Tag as FleetDeck;

                deck.Fleets = Fleets;

                DeckData.Instance.SaveConfig();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                FleetDeck deck = item.Tag as FleetDeck;
                string s = Clipboard.GetText();
                CustomFleets fleets = new CustomFleets();
                if (!fleets.ImportFromString(s))
                {
                    MessageBox.Show("配置字符串不正确!无法解析");
                    return;
                }
                deck.Fleets = fleets;
                item.ToolTipText = deck.ShipList;
                listView1_SelectedIndexChanged(null, null);
                DeckData.Instance.SaveConfig();
            }
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
                    Fleets.Fleets[i] = new CustomFleet();
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

        }

        void LoadList()
        {
            listView1.BeginUpdate();
            foreach (var deck in DeckData.Instance.DeckList)
            {
                var item = listView1.Items.Add(deck.Name);
                item.Tag = deck;
                item.ToolTipText = deck.ShipList;
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
                splitContainer1.Panel2.Enabled = true;
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

            toolStripMenuItem1.Enabled = enabled;
            删除编成ToolStripMenuItem.Enabled = enabled;
            //导入游戏当前舰队ToolStripMenuItem.Enabled = enabled;
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
                DeckData.Instance.SaveConfig();
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
                item.ToolTipText = deck.ShipList;
                DeckData.Instance.SaveConfig();
                listView1.Items[item.Index].Focused = true;
                listView1.Items[item.Index].Selected = true;
            }

        }

        private void 导入游戏当前舰队ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var deck = DeckData.Instance.AddDeck(KCDatabase.Instance.Fleet.Fleets[1].Name, null);
            var item = listView1.Items.Add(deck.Name);
            item.Tag = deck;
            listView1.Items[item.Index].Focused = true;
            listView1.Items[item.Index].Selected = true;
            ImportGameFleet(KCDatabase.Instance.Fleet.Fleets[1].Name);
        }

        void ImportGameFleet(string name = null)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                FleetDeck deck = item.Tag as FleetDeck;
                if (name != null)
                    deck.Name = name;
                deck.Json = CopyFleetDeckBuilder();
                item.ToolTipText = deck.ShipList;
                listView1_SelectedIndexChanged(null, null);
                DeckData.Instance.SaveConfig();
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

        private string CopyFleetDeckBuilder()//从主程序复制过来
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
                Decks[i] = new CustomDeckForm();
                Decks[i].DeckChanged += OnDeckChanged;
                Decks[i].DeckNo = i;
                Decks[i].Parent = tabControl1.TabPages[i];
                Decks[i].Dock = DockStyle.Fill;
                Decks[i].Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(MousePosition);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImportGameFleet();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string website = @"http://www.kancolle-calc.net/deckbuilder.html?predeck=";
            string json = Fleets.Serialize();
            website += System.Web.HttpUtility.UrlEncode(json);
            //Clipboard.SetText(website);
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            string s = key.GetValue("").ToString();
            int i1 = s.IndexOf("\"", 0);
            int i2 = s.IndexOf("\"", i1 + 1);
            s = s.Substring(i1 + 1, i2 - i1 - 1);
            System.Diagnostics.Process.Start(s, website); 
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(MousePosition);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            contextMenuStrip3.Show(MousePosition);
        }

       

     
    }
}
