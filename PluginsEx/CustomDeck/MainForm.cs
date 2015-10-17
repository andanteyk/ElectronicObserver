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
    public partial class MainForm : Form
    {
        CustomFleets Fleets = new CustomFleets();
        CustomDeck[] Decks = new CustomDeck[4];
        public MainForm()
        {
            InitializeComponent();
            for (int i = 0; i < Decks.Length; i++)
            {
                Decks[i] = new CustomDeck();
                Decks[i].Parent = tabControl1.TabPages[i];
                Decks[i].Dock = DockStyle.Fill;
                Decks[i].Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Fleets.Serialize());
        }
    }
}
