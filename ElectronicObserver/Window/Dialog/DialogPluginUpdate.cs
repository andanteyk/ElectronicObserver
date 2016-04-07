using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ElectronicObserver.Window.Control;

namespace ElectronicObserver.Window.Dialog
{
    public partial class DialogPluginUpdate : Form
    {
        public DialogPluginUpdate()
        {
            InitializeComponent();
        }


        class DownloadStatus
        {
            public string ButtonCaption;
            public bool Enabled;
            public bool ManualEnabled;
        }

        Dictionary<string, int> RowList = new Dictionary<string, int>();
        DownloadStatus GetButtonStatus(Plugins.PluginUpdateProgress.UpdatingProgress progress)
        {
            DownloadStatus ds = new DownloadStatus();
            switch (progress)
            {
                case Plugins.PluginUpdateProgress.UpdatingProgress.更新失败:
                    ds.Enabled = true;
                    ds.ButtonCaption = "重新更新";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.手动更新:
                    ds.Enabled = false;
                    ds.ButtonCaption = "手动下载";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.等待下载:
                    ds.Enabled = true;
                    ds.ButtonCaption = "开始下载";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.更新成功:
                    ds.Enabled = true;
                    ds.ButtonCaption = "再次检查";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.尚未开始:
                    ds.Enabled = true;
                    ds.ButtonCaption = "开始更新";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.无需更新:
                    ds.Enabled = false;
                    ds.ButtonCaption = "无需更新";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.正在下载:
                    ds.Enabled = false;
                    ds.ButtonCaption = "正在下载";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.检查更新:
                    ds.Enabled = false;
                    ds.ButtonCaption = "正在更新";
                    break;
                default:
                    ds.Enabled = false;
                    ds.ButtonCaption = null;
                    break;
            }
            return ds;
        }



        private void DialogPluginUpdate_Load(object sender, EventArgs e)
        {

            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            int Row = 0;
            foreach (var Updater in FormMain.UpdateManager.PluginUpdaters)
            {
                if (Updater.Value.UpdateInformation.updateType != Plugins.PluginUpdateInformation.UpdateType.None)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    var Status = GetButtonStatus(Updater.Value.UpdateProgress.Progress);
                    row.SetValues(Updater.Key, Updater.Value.UpdateProgress.Messages, Status.ButtonCaption, "手动下载");
                    row.Cells[1].ToolTipText = Updater.Value.UpdateProgress.Changelog;
                    ((DataGridViewDisableButtonCell)row.Cells[2]).Enabled = Status.Enabled;
                    ((DataGridViewDisableButtonCell)row.Cells[3]).Enabled = !string.IsNullOrWhiteSpace(Updater.Value.UpdateInformation.PluginDownloadURI);
                    rows.Add(row);
                    RowList[Updater.Key] = Row;
                    Row++;
                }
            }
            dataGridView1.Rows.AddRange(rows.ToArray());

            foreach (var Updater in FormMain.UpdateManager.PluginUpdaters)
            {
                Updater.Value.UpdateProgress.OnUpdateProgressChanged += UpdateProgress_OnUpdateProgressChanged;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColUpdate.Index)
            {
                if (((DataGridViewDisableButtonCell)dataGridView1.Rows[e.RowIndex].Cells["ColUpdate"]).Enabled)
                {
                    string name = dataGridView1.Rows[e.RowIndex].Cells["ColName"].Value.ToString();
                    FormMain.UpdateManager.PluginUpdaters[name].Start();
                }
                
            }

            if (e.ColumnIndex == ColManual.Index)
            {
                if (((DataGridViewDisableButtonCell)dataGridView1.Rows[e.RowIndex].Cells["ColManual"]).Enabled)
                {
                    string name = dataGridView1.Rows[e.RowIndex].Cells["ColName"].Value.ToString();
                    FormMain.UpdateManager.PluginUpdaters[name].ManualStart();
                }
            }
        }

        void UpdateProgress_OnUpdateProgressChanged(Plugins.PluginUpdateProgress UpdateProgress)
        {
            int Row = RowList[UpdateProgress.Updater.Name];
            var Status = GetButtonStatus(UpdateProgress.Progress);
            if (Status.ButtonCaption == null)
                return;

            FormMain.ActiveForm.Invoke(new MethodInvoker(() =>
            {
                dataGridView1.Rows[Row].Cells["ColStatus"].Value = UpdateProgress.Messages;
                dataGridView1.Rows[Row].Cells["ColUpdate"].Value = Status.ButtonCaption;
                dataGridView1.Rows[Row].Cells[1].ToolTipText = UpdateProgress.Updater.UpdateProgress.Changelog;
                ((DataGridViewDisableButtonCell)dataGridView1.Rows[Row].Cells["ColUpdate"]).Enabled = Status.Enabled;
                ((DataGridViewDisableButtonCell)dataGridView1.Rows[Row].Cells[3]).Enabled = !string.IsNullOrWhiteSpace(UpdateProgress.Updater.UpdateInformation.PluginDownloadURI);
                dataGridView1.Refresh();
            }));
        }

        private void DialogPluginUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var Updater in FormMain.UpdateManager.PluginUpdaters)
            {
                Updater.Value.UpdateProgress.OnUpdateProgressChanged -= UpdateProgress_OnUpdateProgressChanged;
            }
        }
    }
}
