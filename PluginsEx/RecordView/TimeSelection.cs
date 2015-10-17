using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordView
{
    public partial class TimeSelection : UserControl
    {
        public TimeSelection()
        {
            InitializeComponent();
        }

        public event EventHandler DateChanged;

        protected virtual void OnDateChanged(EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, e);
        }  
        public DateTime MinDate
        {
            get
            {
                return dateTimePicker1.MinDate;
            }
            set
            {
                dateTimePicker1.MinDate = value;
                dateTimePicker2.MinDate = value;
            }
        }

        public void SelectToday()
        {
            button1.PerformClick();
        }

        public DateTime Since
        {
            get;
            set;
        }
        public DateTime Until
        {
            get;
            set;
        }

        bool internalChange = false;
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!internalChange)
            {
                internalChange = true;
                dateTimePicker2.MinDate = dateTimePicker1.Value;
                internalChange = false;
            }
            Since = dateTimePicker1.Value;
            Until = dateTimePicker2.Value;
            if (!internalChange)
            {
                OnDateChanged(new EventArgs());
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (!internalChange)
            {
                internalChange = true;
                dateTimePicker1.MaxDate = dateTimePicker2.Value;
                internalChange = false;
            }
            Since = dateTimePicker1.Value;
            Until = dateTimePicker2.Value;
            if (!internalChange)
            {
                OnDateChanged(new EventArgs());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            internalChange = true;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            internalChange = false;
            OnDateChanged(new EventArgs());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            internalChange = true;
            dateTimePicker1.Value = DateTime.Now.AddDays(-6);
            dateTimePicker2.Value = DateTime.Now;
            internalChange = false;
            OnDateChanged(new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            internalChange = true;
            dateTimePicker1.Value = dateTimePicker1.MinDate;
            dateTimePicker2.Value = DateTime.Now;
            internalChange = false;
            OnDateChanged(new EventArgs());
        }
    }
}
