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
                    ds.Enabled = true;
                    ds.ButtonCaption = "手动下载";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.等待下载:
                    ds.Enabled = true;
                    ds.ButtonCaption = "开始下载";
                    break;
                case Plugins.PluginUpdateProgress.UpdatingProgress.更新成功:
                    ds.Enabled = false;
                    ds.ButtonCaption = "更新成功";
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
                    row.SetValues(Updater.Key, Updater.Value.UpdateProgress.Messages, Status.ButtonCaption);
                    ((DataGridViewDisableButtonCell)row.Cells[2]).Enabled = Status.Enabled;
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
                string name = dataGridView1.Rows[e.RowIndex].Cells["ColName"].Value.ToString();
               
                FormMain.UpdateManager.PluginUpdaters[name].Start();
                
            }
        }

        void UpdateProgress_OnUpdateProgressChanged(Plugins.PluginUpdateProgress UpdateProgress)
        {
            int Row = RowList[UpdateProgress.Updater.Name];
            var Status = GetButtonStatus(UpdateProgress.Progress);
            if (Status.ButtonCaption == null)
                return;
            this.Invoke(new MethodInvoker(() =>
            {
                dataGridView1.Rows[Row].Cells["ColStatus"].Value = UpdateProgress.Messages;
                dataGridView1.Rows[Row].Cells["ColUpdate"].Value = Status.ButtonCaption;
                ((DataGridViewDisableButtonCell)dataGridView1.Rows[Row].Cells["ColUpdate"]).Enabled = Status.Enabled;
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
