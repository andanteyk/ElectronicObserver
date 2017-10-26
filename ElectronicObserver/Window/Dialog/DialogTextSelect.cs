using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogTextSelect : Form
	{

		public int SelectedIndex => TextSelect.SelectedIndex;

		public object SelectedItem => TextSelect.SelectedItem;

		public DialogTextSelect()
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(tableLayoutPanel1);
		}

		public DialogTextSelect(string title, string description, object[] items)
			: this()
		{

			Initialize(title, description, items);
		}

		public void Initialize(string title, string description, object[] items)
		{
			this.Text = title;

			tableLayoutPanel1.SuspendLayout();

			Description.Text = description;

			TextSelect.BeginUpdate();
			TextSelect.Items.Clear();
			TextSelect.Items.AddRange(items);
			if (TextSelect.Items.Count > 0)
				TextSelect.SelectedIndex = 0;
			TextSelect.EndUpdate();

			tableLayoutPanel1.ResumeLayout();

		}


		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

	}
}
