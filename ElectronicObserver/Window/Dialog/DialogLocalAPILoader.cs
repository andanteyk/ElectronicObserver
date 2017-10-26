using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogLocalAPILoader : Form
	{

		public string FilePath => TextFilePath.Text;

		public string FileData
		{
			get
			{
				try
				{

					using (var sr = new StreamReader(TextFilePath.Text))
					{
						return sr.ReadToEnd();
					}

				}
				catch (Exception)
				{

					return null;
				}
			}
		}

		public string APIName
		{
			get
			{
				if (APIList.SelectedIndex != -1)
					return APIList.SelectedItem.ToString();
				else
					return null;
			}
		}

		public string APIPath
		{
			get
			{
				if (APIList.SelectedIndex != -1)
					return "/kcsapi/" + APIList.SelectedItem.ToString();
				else
					return null;
			}
		}

		public bool IsRequest => APICategory.SelectedIndex == 0;

		public bool IsResponse => APICategory.SelectedIndex == 1;


		public DialogLocalAPILoader()
		{
			InitializeComponent();
		}


		private void DialogLocalAPILoader_Load(object sender, EventArgs e)
		{

			Icon iconWarning = SystemIcons.Warning;
			Bitmap bmp = new Bitmap(PictureWarning.Width, PictureWarning.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{

				g.DrawIcon(iconWarning, 0, 0);
				PictureWarning.Image = bmp;

			}


			APICategory.SelectedIndex = 1;

			FileOpener.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;
		}


		private void APICategory_SelectedIndexChanged(object sender, EventArgs e)
		{

			APIList.Items.Clear();
			if (APICategory.SelectedIndex == 0)
			{
				//request
				foreach (string s in APIObserver.Instance.APIList.Values.Where(a => a.IsRequestSupported).Select(a => a.APIName))
				{
					APIList.Items.Add(s);
				}

			}
			else
			{
				//response
				foreach (string s in APIObserver.Instance.APIList.Values.Where(a => a.IsResponseSupported).Select(a => a.APIName))
				{
					APIList.Items.Add(s);
				}
			}

			APIList.SelectedIndex = 0;

		}


		private void ButtonSearchFilePath_Click(object sender, EventArgs e)
		{

			if (File.Exists(TextFilePath.Text))
				FileOpener.FileName = TextFilePath.Text;

			FileOpener.Filter = APIList.SelectedItem.ToString() + "|*" + (APICategory.SelectedIndex == 0 ? "Q" : "S") + "@" + APIList.SelectedItem.ToString().Replace('/', '@') + ".json|JSON|*.json;*.js|File|*";

			if (FileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				TextFilePath.Text = FileOpener.FileName;
			}

		}



		private void ButtonOpen_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}


		private void TextFilePath_DragEnter(object sender, DragEventArgs e)
		{

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;

			}
			else
			{
				e.Effect = DragDropEffects.None;
			}

		}

		private void TextFilePath_DragDrop(object sender, DragEventArgs e)
		{

			string[] path = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (path.Length > 0 && File.Exists(path[0]))
				TextFilePath.Text = path[0];

		}

	}
}
