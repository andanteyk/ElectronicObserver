using ElectronicObserver.Data;
using ElectronicObserver.Resource;
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
    public partial class DialogExpeditionCheck : Form
    {
        public DialogExpeditionCheck()
        {
            InitializeComponent();
        }

        private void DialogExpeditionCheck_Load(object sender, EventArgs e)
        {
            if (!KCDatabase.Instance.Mission.Any())
            {
                MessageBox.Show("遠征データが読み込まれていません。\r\n艦これに再ログインし、母港画面を開いてください。",
                    "マスタデータ未ロード", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }


            Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpeditionCheck]);
        }


        private void UpdateCheckView()
        {
            CheckView.SuspendLayout();

            CheckView.Rows.Clear();

            var db = KCDatabase.Instance;
            var rows = new List<DataGridViewRow>(db.Mission.Count);

            var defaultStyle = CheckView.RowsDefaultCellStyle;
            var failedStyle = defaultStyle.Clone();
            failedStyle.BackColor = Color.MistyRose;
            failedStyle.SelectionBackColor = Color.Brown;


            foreach (var mission in db.Mission.Values)
            {
                var results = new[]
                {
                    MissionClearCondition.Check(mission.MissionID, db.Fleet[2]),
                    MissionClearCondition.Check(mission.MissionID, db.Fleet[3]),
                    MissionClearCondition.Check(mission.MissionID, db.Fleet[4]),
                    MissionClearCondition.Check(mission.MissionID, null),
                };


                var row = new DataGridViewRow();
                row.CreateCells(CheckView);
                row.SetValues(
                    mission.MissionID,
                    mission.MissionID,
                    results[0],
                    results[1],
                    results[2],
                    results[3]);

                for (int i = 0; i < 4; i++)
                {
                    var result = results[i];
                    var cell = row.Cells[i + 2];

                    if (result.IsSuceeded || i == 3)
                    {
                        if (!result.FailureReason.Any())
                            cell.Value = "○";
                        else
                            cell.Value = string.Join(", ", result.FailureReason);

                        cell.Style = defaultStyle;
                    }
                    else
                    {
                        cell.Value = string.Join(", ", result.FailureReason);
                        cell.Style = failedStyle;
                    }
                }

                rows.Add(row);
            }

            CheckView.Rows.AddRange(rows.ToArray());

            CheckView.Sort(CheckView_Name, ListSortDirection.Ascending);

            CheckView.ResumeLayout();
        }

        private void DialogExpeditionCheck_Activated(object sender, EventArgs e)
        {
            int displayedRow = CheckView.FirstDisplayedScrollingRowIndex;
            int selectedRow = CheckView.SelectedRows.OfType<DataGridViewRow>().FirstOrDefault()?.Index ?? -1;

            UpdateCheckView();

            if (0 <= displayedRow && displayedRow < CheckView.RowCount)
                CheckView.FirstDisplayedScrollingRowIndex = displayedRow;
            if (0 <= selectedRow && selectedRow < CheckView.RowCount)
            {
                CheckView.ClearSelection();
                CheckView.Rows[selectedRow].Selected = true;
            }
        }



        private void CheckView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == CheckView_Name.Index)
            {
                e.Value = KCDatabase.Instance.Mission[(int)e.Value].Name;
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == CheckView_ID.Index)
            {
                var mission = KCDatabase.Instance.Mission[(int)e.Value];
                e.Value = $"{mission.DisplayID}:{KCDatabase.Instance.MapArea[mission.MapAreaID].Name}";
                e.FormattingApplied = true;
            }
        }

        private void CheckView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == CheckView_Name.Index || e.Column.Index == CheckView_ID.Index)
            {
                var m1 = KCDatabase.Instance.Mission[(int)e.CellValue1];
                var m2 = KCDatabase.Instance.Mission[(int)e.CellValue2];

                int diff = m1.MapAreaID - m2.MapAreaID;
                if (diff == 0)
                    diff = m1.MissionID - m2.MissionID;

                e.SortResult = diff;
                e.Handled = true;
            }
        }

        private void DialogExpeditionCheck_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResourceManager.DestroyIcon(Icon);
        }
    }
}
