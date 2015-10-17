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

namespace MapAreaInfo
{
    public partial class Main : Form
    {
        List<FormFleetData> forms = new List<FormFleetData>();
        static string LastMap = null;

        public Main()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //listBox1.Items.AddRange(MapInformation.MapLists.Keys.ToArray());


            forms.Add(new FormFleetData());
            forms.Add(new FormFleetData());
            forms.Add(new FormFleetData());
            forms.Add(new FormFleetData());
            forms.Add(new FormFleetData());
            forms.Add(new FormFleetData());

            foreach (var form in forms)
            {
                form.Parent = flowLayoutPanel1;
            }

            if (LastMap != null)
                ChangeMap(LastMap);
            //form.Parent = flowLayoutPanel1;

            //form.Show();
            //form.UpdateEnemyFleetInstant(MapData.MapArea["3-2"][3].Parts[0]);  

            toolTip1.Active = true;
            //toolTip1.SetToolTip(pictureBox1, "    1111");

            BackColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
            //Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
            ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.ForeColor;
            foreach (var btn in tableLayoutPanel1.Controls)
            {
                if (btn is Button)
                {
                    Button button = btn as Button;
                    //button.BackColor = System.Drawing.SystemColors.Control;
                    button.ForeColor = System.Drawing.SystemColors.ControlText;
                }
            }
        }

        MapArea CurrentArea = null;

        void ChangeMap(string mapName)
        {
            LastMap = mapName;
            CurrentArea = MapInformation.MapLists[mapName];

            string name = mapName.Replace("-", "_");
            pictureBox1.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("MAP" + name);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeMap();
        }

        void refreshFleet(MapCell cell)
        {
            if (KCDatabase.Instance.MasterShips.Count <= 0)
            {
                MessageBox.Show("游戏数据还未载入,无法查看  请等待进入游戏");
                return;
            }
            for (int i = 0; i < forms.Count;i++ )
            {
                if (cell.Parts.Count > i)
                {
                    forms[i].Visible = true;
                    forms[i].UpdateEnemyFleet(cell.Parts[i]);
                }
                else
                    forms[i].Visible = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = e.X.ToString() + "," + e.Y.ToString();

            if (CurrentArea == null)
                return;
            Point point = new Point(e.X, e.Y);
            point.X = (int)(point.X * 600.0 / pictureBox1.Width);
            point.Y = (int)(point.Y * 300.0 / pictureBox1.Height);
            string A = CurrentArea.CheckPoint(point);
            if (A != null)
            {
                if (pictureBox1.Cursor != System.Windows.Forms.Cursors.Hand)
                {
                    pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
                    if (CurrentArea.Cells.ContainsKey(A))
                        if (CurrentArea.Cells[A].Drops.Drops != null)
                        {
                            toolTip1.Active = true;
                            toolTip1.SetToolTip(pictureBox1, CurrentArea.Cells[A].Drops.Drops);
                        }
                }
            }
            else
            {
                if (pictureBox1.Cursor != System.Windows.Forms.Cursors.Default)
                {
                    pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
                    toolTip1.Active = false;
                    //
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Point p = pictureBox1.PointToClient(MousePosition);
            //if (textBox1.Text != null && textBox1.Text != "")
            //    textBox1.Text += "|";
            //textBox1.Text += alpha + "=" + p.X.ToString() + "," + p.Y.ToString();
            //alpha++;
            //Clipboard.SetText(textBox1.Text);

            if (CurrentArea == null)
                return;
            Point point = pictureBox1.PointToClient(MousePosition);
            point.X = (int)(point.X * 600.0 / pictureBox1.Width);
            point.Y = (int)(point.Y * 300.0 / pictureBox1.Height);
            string A = CurrentArea.CheckPoint(point);
            //label1.Text = A;
            if (A == null)
                return;
            if (CurrentArea.Cells.ContainsKey(A))
            {
                MapCell cell = CurrentArea.Cells[A];
                label1.Text = "正在载入...";
                refreshFleet(cell);
                label1.Text = A + ":" + cell.CellName + "      " + cell.LeadingInfo;
            }
        }


        char alpha = '@';
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            alpha = '@';
            textBox1.Text = "";
        }

        private void buttonMap_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ChangeMap(btn.Text);
        }
    }

}
