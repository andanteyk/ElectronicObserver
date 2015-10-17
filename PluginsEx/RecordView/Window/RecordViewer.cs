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
using Codeplex.Data;
using ElectronicObserver;
using ElectronicObserver.Data;
using System.Xml;

namespace RecordView
{
    public partial class RecordViewer : Form
    {
        public static bool SaveBattleRecord = false;
        public static string ConfigFile;
        public RecordViewer()
        {
            InitializeComponent();

        }

       

        public void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!System.IO.File.Exists(ConfigFile))
                {
                    XmlElement xmlelem = doc.CreateElement("Config");
                    doc.AppendChild(xmlelem);
                }
                else
                {
                    doc.Load(ConfigFile);
                }
                var Root = doc.DocumentElement;

                Root.SetAttribute("SaveBattleLog", SaveBattleRecord.ToString());

                doc.Save(ConfigFile);
            }
            catch
            {
            }
        }

        Form Viewer = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (Viewer != null)
            {
                Viewer.Close();
                Viewer.Dispose();
            }
            DropViewer viewer = new DropViewer();
            Viewer = viewer;
            Viewer.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Viewer.TopLevel = false;
            Viewer.Dock = DockStyle.Fill;
            Viewer.Parent = panel1;

            IStatusBar StatusBar = viewer;
            StatusBar.statusStrip = this.statusStrip1;
            Viewer.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Viewer != null)
            {
                Viewer.Close();
                Viewer.Dispose();
            }
            ConstructionViewer viewer = new ConstructionViewer();
            Viewer = viewer;
            Viewer.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Viewer.TopLevel = false;
            Viewer.Dock = DockStyle.Fill;
            Viewer.Parent = panel1;

            IStatusBar StatusBar = viewer;
            StatusBar.statusStrip = this.statusStrip1;
            Viewer.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Viewer != null)
            {
                Viewer.Close();
                Viewer.Dispose();
            }
            DevelopmentViewer viewer = new DevelopmentViewer();
            Viewer = viewer;
            Viewer.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Viewer.TopLevel = false;
            Viewer.Dock = DockStyle.Fill;
            Viewer.Parent = panel1;

            IStatusBar StatusBar = viewer;
            StatusBar.statusStrip = this.statusStrip1;
            Viewer.Show();
        }

        unsafe private void button4_Click(object sender, EventArgs e)
        {
            if (Viewer != null)
            {
                Viewer.Close();
                Viewer.Dispose();
            }
            BattleViewer viewer = new BattleViewer();
            Viewer = viewer;
            Viewer.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Viewer.TopLevel = false;
            Viewer.Dock = DockStyle.Fill;
            Viewer.Parent = panel1;

            IStatusBar StatusBar = viewer;
            StatusBar.statusStrip = this.statusStrip1;
            Viewer.Show();
            //OpenFileDialog ofd = new OpenFileDialog();
            //byte[] buff = new byte[10000];
            //if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
            //    fs.Read(buff, 0, sizeof(BattleRecord));
            //    fs.Close();
            //    BattleRecord record;
            //    fixed (byte* p = &buff[0])
            //    {
            //        record = LoadMyStruct(p);
            //    }
            //    BattleExporter.AnalyseRecord(record);
            //}
        }
        unsafe static BattleRecord LoadMyStruct(byte* buf)
        {
            int len = sizeof(BattleRecord);
            //MessageBox.Show(len.ToString());
            BattleRecord record;
            byte* p = (byte*)&record;
            for (int i = 0; i < len; i++)
            {
                *p = *buf;
                p++;
                buf++;
            }
            return record;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveBattleRecord = checkBox1.Checked;
        
        }

        private void RecordViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveConfig();
        }

        private void RecordViewer_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = SaveBattleRecord;
            this.Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;

            LoadConfig();

            //StringBuilder builder = new StringBuilder();
            //foreach (var ship in KCDatabase.Instance.MasterShips)
            //{

            //    ShipDataMaster sdm = ship.Value;
            //    if (sdm.IsAbyssalShip)
            //    {
            //        string s = sdm.NameWithClass;
            //        s = s.Replace(" ", "");
            //        builder.Append(s);
            //        builder.Append("=");
            //        builder.AppendLine(sdm.ID.ToString());
            //    }
            //}
            //File.WriteAllText("D:\\Programs\\ShimakazeGo\\1.txt", builder.ToString(), Encoding.UTF8);


        }

        public void LoadConfig()
        {
            try
            {
                if (System.IO.File.Exists(RecordViewer.ConfigFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(RecordViewer.ConfigFile);
                    var Root = doc.DocumentElement;

            
                    string FileNamePattern = Root.GetAttribute("FileNamePattern");
                    if (FileNamePattern == null || FileNamePattern == "")
                        FileNamePattern = PatternModifier.DefaultPattern;
                    BattleViewer.FileNamePattern = FileNamePattern;
                }
            }
            catch
            {
            }
        }
    }

    public interface IStatusBar
    {
        StatusStrip statusStrip
        {
            get;
            set;
        }
    }
}
