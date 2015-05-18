using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ElectronicObserver.Window.Dialog
{
    public partial class DialogResourcesGraph : Form
    {
        public DialogResourcesGraph()
        {
            RecordManager.Instance.Save();//sav current record firstly.

            InitializeComponent();
            this.Text = "资源图表";
            this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourcesGraph]);

            try
            {
                dat = ReadCSV(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Record\\ResourceRecord.csv");
                if (dat.Count <= 1)
                {
                    System.Windows.Forms.MessageBox.Show("资源记录过少.建议运行一段时间后再查看.目前仅有" + dat.Count + "项记录,不足以绘制折线图", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                loadResChart(ref dat);
                this.comboBox1.SelectedIndex = 0;//invoke loadresgrid();
                //loadResGrid(ref dat);
            }
            catch (IOException)
            {
                System.Windows.Forms.MessageBox.Show("载入资源记录失败.请检查程序目录下Record/ResourceRecord.csv是否存在.（程序刚开始运行时不会产生该记录，请运行一段时间后再试）","错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                this.Close();
            }
        }

        private bool loadResGrid(ref List<string[]> dat)
        {
            this.dataGridView1.Rows.Clear();

            this.dataGridView1.ColumnCount = 11;
            this.dataGridView1.Columns[0].Name = "时间";
            this.dataGridView1.Columns[0].Width = 200;
            this.dataGridView1.Columns[1].Name = "燃料";
            this.dataGridView1.Columns[1].Width = 100;
            this.dataGridView1.Columns[2].Name = "弹药";
            this.dataGridView1.Columns[2].Width = 100;
            this.dataGridView1.Columns[3].Name = "钢材";
            this.dataGridView1.Columns[3].Width = 100;
            this.dataGridView1.Columns[4].Name = "铝土";
            this.dataGridView1.Columns[4].Width = 100;
            this.dataGridView1.Columns[5].Name = "高速建造材";
            this.dataGridView1.Columns[5].Width = 100;
            this.dataGridView1.Columns[6].Name = "高速修復材";
            this.dataGridView1.Columns[6].Width = 100;
            this.dataGridView1.Columns[7].Name = "开发资材";
            this.dataGridView1.Columns[7].Width = 80;
            this.dataGridView1.Columns[8].Name = "改修资材";
            this.dataGridView1.Columns[8].Width = 80;
            this.dataGridView1.Columns[9].Name = "司令部Lv";
            this.dataGridView1.Columns[9].Width = 80;
            this.dataGridView1.Columns[10].Name = "提督Exp";
            this.dataGridView1.Columns[10].Width = 120;

            List<String[]> newdat = new List<String[]>();
            int tot=0;
            for (int i = dat.Count - 1; i >= 0 && i >= dat.Count - 50; i--)//mostly 50 logs
            {
                newdat.Add((String[])dat[i].Clone());
                if (i > 0)
                {
                    newdat[tot][0] = dat[i][0]+ "(+" + (System.DateTime.Parse(dat[i][0]) - System.DateTime.Parse(dat[i - 1][0])).ToString() + ')';
                    for (int j = 1; j <= 10; j++)
                    {
                        if (j == 9) continue;
                        int chg = int.Parse(dat[i][j]) - int.Parse(dat[i - 1][j]);
                        string chgstr = "(";
                        if (chg >= 0) chgstr += '+';
                        chgstr += chg.ToString() + ')';
                        newdat[tot][j] += chgstr;
                    }
                }
                this.dataGridView1.Rows.Add(newdat[tot]);
                tot++;
            }

            return true;
        }

        private int day2tbfloor(double day)
        {
            return (int)(Math.Sqrt(day) * 10);
        }

        private int day2tbceil(double day)
        {
            return (int)Math.Ceiling(Math.Sqrt(day) * 10);
        }

        private bool loadResChart(ref List<string[]> dat)
        {
            System.DateTime mindt = System.DateTime.Now;
            System.DateTime maxdt = new System.DateTime(1, 1, 1);


            for (int i = 0; i < dat.Count; i++)
            {
                System.DateTime parsedt = System.DateTime.Parse(dat[i][0]);
                if (parsedt < mindt)
                    mindt = parsedt;
                if (parsedt > maxdt)
                    maxdt = parsedt;
                double dt=parsedt.ToOADate();

                
                this.chart1.Series[0].Points.Add(new DataPoint(dt,dat[i][1]));
                this.chart1.Series[1].Points.Add(new DataPoint(dt, dat[i][2]));
                this.chart1.Series[2].Points.Add(new DataPoint(dt, dat[i][3]));
                this.chart1.Series[3].Points.Add(new DataPoint(dt, dat[i][4]));
                this.chart1.Series[4].Points.Add(new DataPoint(dt, dat[i][6]));
            }

            double maxdays = (System.DateTime.Now - mindt).TotalDays;
            trackBarDate.Maximum = day2tbceil(maxdays);//the farest day

            double mindays = (System.DateTime.Now - maxdt).TotalDays;
            trackBarDate.Minimum = day2tbceil(mindays);//nearest


            this.chart1.ChartAreas[0].AxisX.Maximum = System.DateTime.Now.ToOADate();
            if ((int)Math.Ceiling((System.DateTime.Now - maxdt).TotalDays) > 30)//if the nearly log is over 30 days
            {
                trackBarDate.Value = (trackBarDate.Maximum + trackBarDate.Minimum) >> 1;
                this.chart1.ChartAreas[0].AxisX.Minimum = (System.DateTime.Now - new System.TimeSpan((int)Math.Ceiling((double)trackBarDate.Value * trackBarDate.Value / 100), 0, 0, 0)).ToOADate();
                return true;
            }

            double nowdays = Math.Min(maxdays, 36.0);//show chart in 36 days firstly
            trackBarDate.Value = day2tbceil(nowdays);
            this.chart1.ChartAreas[0].AxisX.Minimum = (System.DateTime.Now - new System.TimeSpan((int)Math.Ceiling((double)trackBarDate.Value * trackBarDate.Value / 100), 0, 0, 0)).ToOADate();

            return true;
        }

        public static List<String[]> ReadCSV(string filePathName)
        {
            List<String[]> dat = new List<String[]>();
            StreamReader fileReader = new StreamReader(filePathName);
            string strLine = "";
            fileReader.ReadLine();
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    dat.Add(strLine.Split(','));
                }
            }
            fileReader.Close();
            return dat;
        }

        private void DialogResourcesGraph_Load(object sender, EventArgs e)
        {
        }

        //chart show infos
        void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("{0}\r\n资源量:{2}\r\n记录时间:{1}", e.HitTestResult.Series.Name, System.DateTime.FromOADate(dp.XValue).ToString(), dp.YValues[0]);
            }
        }  

        //chart scale chg
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = sender as TrackBar;
            this.chart1.ChartAreas[0].AxisX.Minimum = (System.DateTime.Now - new System.TimeSpan((int)Math.Ceiling((double)tb.Value*tb.Value/100), 0, 0, 0)).ToOADate();
        }

        //make sure that more than 1 checkbox checked.
        private void checkBox_Keeper(int order,ref CheckBox cb)
        {
            int showed = 0;
            for (int i = 0; i <= 4; i++)
                showed += this.chart1.Series[i].Enabled ? 1 : 0;
            if (showed==0)
            {
                this.chart1.Series[order].Enabled = cb.Checked = true;
            }
        }

        //fuel
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.chart1.Series[0].Enabled=cb.Checked;

            checkBox_Keeper(0, ref cb);
        }

        //ammo
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.chart1.Series[1].Enabled = cb.Checked;

            checkBox_Keeper(1, ref cb);
        }

        //steel
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.chart1.Series[2].Enabled = cb.Checked;

            checkBox_Keeper(2, ref cb);
        }

        //bauxite
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.chart1.Series[3].Enabled = cb.Checked;

            checkBox_Keeper(3, ref cb);
        }

        //repair
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            this.chart1.Series[4].Enabled = cb.Checked;

            checkBox_Keeper(4, ref cb);
        }

        //sel chart by day
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            double val=chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            chart1.ChartAreas[0].CursorX.SetSelectionPosition(Math.Floor(val),Math.Ceiling(val));
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
        private List<string[]> dat;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb=sender as ComboBox;
            int spandays=0;
            switch(cb.SelectedIndex){
                case 0:spandays=0;break;
                case 1:spandays=1;break;
                case 2:spandays=3;break;
                case 3:spandays=7;break;
                case 4:spandays=15;break;
                case 5:spandays=30;break;
                case 6:spandays=90;break;
            }
            TimeSpan ts=new TimeSpan(spandays,0,0,0);

            List<String[]>newdat=new List<string[]>();
            newdat.Add(dat[dat.Count - 1]);
            Console.WriteLine(dat[dat.Count-1][0]);
            System.DateTime logdt = System.DateTime.Parse(dat[dat.Count - 1][0]);
            for(int i=dat.Count-2;i>=0;i--){
                System.DateTime curdt=System.DateTime.Parse(dat[i][0]);
                if(logdt-curdt>=ts){
                    newdat.Add(dat[i]);
                    logdt=curdt;
                }
            }
            newdat.Reverse();
            loadResGrid(ref newdat);
        }
    }
}
