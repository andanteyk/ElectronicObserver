using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserverUpdater
{
    public partial class FormUpdater : Form
    {
		#region Variables/Fields
		/// <summary>
		/// Base Url of update file.
		/// Two parameters should be replaced when download file.
		/// </summary>
		private static string BASE_URL = "https://github.com/Tsukumoyarei/ElectronicObserver/releases/tag/v{0}-mk{1}/74eo-joint.zip";
		public string Version { get; set; } = "";
		public bool IsSetBaseSize { get; private set; }

		private string FileName { get; set; } = "";

		public Progress<ZipProgress> _progress;

		private delegate void CSafeSetText(string text);
		private delegate void CSafeSetMaximum(int value);
		private delegate void CSafeSetValue(int value);
		private readonly CSafeSetText csst;
		private readonly CSafeSetMaximum cssm;
		private readonly WebClient wc;
		#endregion

		#region Invoke Methods
		void CrossSafeSetTextMethod(string text)
		{
			if (labelDescription.InvokeRequired)
				labelDescription.Invoke(csst, text);
			else
				labelDescription.Text = text;
		}

		void CrossSafeSetValueMethod(int value)
		{
			if (progressBar.InvokeRequired)
				progressBar.Invoke(cssm, value);
			else
				progressBar.Value = value;
		}
		void CrossSafeSetMaximumMethod(int value)
		{
			if (progressBar.InvokeRequired)
				progressBar.Invoke(cssm, value);
			else
				progressBar.Maximum = value;
		}
		#endregion

		/// <summary>
		/// Initializer of FormUpdater class
		/// </summary>
		/// <param name="v">current version. this should be {n}.{n}.{n}.{n} template. (ElectronicObserver.Utility.SoftwareInformation.VersionEnglish)</param>
		public FormUpdater(string v)
        {
			wc = new WebClient();
			InitializeComponent();
			Version = v;

			_progress = new Progress<ZipProgress>();
			_progress.ProgressChanged += Report;
		}


		private void FormUpdater_Load(object sender, EventArgs e)
		{
			KillEOProcess();

			//turn on TLS/SSL Options
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

			FileName = Version.Replace(".", string.Empty);
			//If minor version exists
			if(FileName.Length > 3)
			{
				FileName = FileName.Insert(3, "_");
			}
			string url = string.Format(BASE_URL, Version, FileName);
			FileName = string.Format(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\{0}", System.IO.Path.GetFileName(url));
			try
			{
				wc.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadCompleted);
				wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FileDownloadProgressChanged);
				wc.DownloadFileAsync(new Uri(url), FileName);

				progressBar.Value = 0;
				labelStatus.Text = "Downloading...";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().FullName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>
		/// Kill ElectronicObserver process before update.
		/// </summary>
		private void KillEOProcess()
		{
			Process[] processes = Process.GetProcessesByName("ElectronicObserver");
			foreach(var p in processes)
			{
				p.Kill();
			}
		}

		private void Report(object sender, ZipProgress zipProgress)
		{
			//Update value asynchronously
			CrossSafeSetMaximumMethod(zipProgress.Total);
			CrossSafeSetValueMethod(zipProgress.Processed);
			CrossSafeSetTextMethod(zipProgress.CurrentItem);

			if(zipProgress.Total == zipProgress.Processed)
			{
				var eo = "ElectronicObserver.exe";
				var eoFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ElectronicObserver.exe";
				var result = MessageBox.Show("アップデートが完了しました。", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
				//When update completed, automatically start EO again.
				if(result == DialogResult.OK)
				{
					if (System.IO.File.Exists(eo))
						Process.Start(eo);
					else   
						Process.Start(eoFile);
					Application.Exit();
				}	
			}
		}

		void FileDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (!IsSetBaseSize)
			{
				CrossSafeSetMaximumMethod((int)e.TotalBytesToReceive);
				IsSetBaseSize = true;
			}

			CrossSafeSetValueMethod((int)e.BytesReceived);

			CrossSafeSetTextMethod(String.Format("{0:N0} Bytes / {1:N0} Bytes ({2:P})", e.BytesReceived, e.TotalBytesToReceive, (Double)e.BytesReceived / (Double)e.TotalBytesToReceive));
		}

		void FileDownloadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			//Extract File
			labelStatus.Text = "Unzipping Files...";
			ZipArchive zip = ZipFile.OpenRead(FileName);
			//Extract zip asynchronously
			Task.Run(() =>
			{
				Utillity.ZipFileExtensions.ExtractToDirectory(zip, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), _progress, true);
			});
		}
	}
}
