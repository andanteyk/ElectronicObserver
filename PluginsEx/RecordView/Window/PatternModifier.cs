using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RecordView
{
    public partial class PatternModifier : Form
    {
        public static string DefaultPattern = "[时间] [海域]-[地图点] [旗舰名] [MVP] [评价] [掉落]";
        public PatternModifier()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "")
                BattleViewer.FileNamePattern = DefaultPattern;
            else
                BattleViewer.FileNamePattern = textBox1.Text;

            SaveConfig();
            Close();
        }

        public void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!System.IO.File.Exists(RecordViewer.ConfigFile))
                {
                    XmlElement xmlelem = doc.CreateElement("Config");
                    doc.AppendChild(xmlelem);
                }
                else
                {
                    doc.Load(RecordViewer.ConfigFile);
                }
                var Root = doc.DocumentElement;

                Root.SetAttribute("FileNamePattern", BattleViewer.FileNamePattern);

                doc.Save(RecordViewer.ConfigFile);
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PatternModifier_Shown(object sender, EventArgs e)
        {
            textBox1.Text = BattleViewer.FileNamePattern;
            if (textBox1.Text == null || textBox1.Text == "")
                textBox1.Text = DefaultPattern;
        }
    }
}
