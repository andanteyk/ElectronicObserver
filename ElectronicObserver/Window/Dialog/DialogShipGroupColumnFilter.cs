using ElectronicObserver.Data;
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
	public partial class DialogShipGroupColumnFilter : Form
	{

		public List<ShipGroupData.ViewColumnData> Result { get; private set; }
		public int ScrollLockColumnCount { get; private set; }

		public DialogShipGroupColumnFilter(DataGridView target, ShipGroupData group)
		{
			InitializeComponent();


			var rows = new LinkedList<DataGridViewRow>();
			var row = new DataGridViewRow();

			row.CreateCells(ColumnView);
			row.SetValues("(全て)", null, null, "-");
			row.Cells[ColumnView_Width.Index].ReadOnly = true;
			rows.AddLast(row);

			foreach (var c in group.ViewColumns.Values.OrderBy(c => c.DisplayIndex))
			{
				row = new DataGridViewRow();
				row.CreateCells(ColumnView);
				row.SetValues(target.Columns[c.Name].HeaderText, c.Visible, c.AutoSize, c.Width);
				row.Cells[ColumnView_Width.Index].ValueType = typeof(int);
				row.Tag = c.Name;
				rows.AddLast(row);
			}

			ColumnView.Rows.AddRange(rows.ToArray());


			ScrLkColumnCount.Minimum = 0;
			ScrLkColumnCount.Maximum = group.ViewColumns.Count;
			ScrLkColumnCount.Value = group.ScrollLockColumnCount;
		}

		private void DialogShipGroupColumnFilter_Load(object sender, EventArgs e)
		{
			if (Owner != null)
				Icon = Owner.Icon;
		}



		// (全て)の処理
		private void ColumnView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex == 0)
			{

				if (e.ColumnIndex == ColumnView_Visible.Index)
				{
					for (int i = 1; i < ColumnView.Rows.Count; i++)
					{
						ColumnView.Rows[i].Cells[ColumnView_Visible.Index].Value =
							ColumnView.Rows[0].Cells[ColumnView_Visible.Index].Value;
					}
				}

				if (e.ColumnIndex == ColumnView_AutoSize.Index)
				{
					for (int i = 1; i < ColumnView.Rows.Count; i++)
					{
						ColumnView.Rows[i].Cells[ColumnView_AutoSize.Index].Value =
							ColumnView.Rows[0].Cells[ColumnView_AutoSize.Index].Value;
					}
				}
			}

		}

		// ボタン処理
		private void ColumnView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 1) return;

			if (e.ColumnIndex == ColumnView_Up.Index && e.RowIndex > 1)
			{
				ControlHelper.RowMoveUp(ColumnView, e.RowIndex);

			}
			else if (e.ColumnIndex == ColumnView_Down.Index && e.RowIndex < ColumnView.RowCount - 1)
			{
				ControlHelper.RowMoveDown(ColumnView, e.RowIndex);
			}
		}

        private void ButtonSelectedUp_Click(object sender, EventArgs e)
        {
            var selectedCell = ColumnView.SelectedCells.OfType<DataGridViewCell>().FirstOrDefault();
            if (selectedCell != null && selectedCell.RowIndex > 1)
            {
                ControlHelper.RowMoveUp(ColumnView, selectedCell.RowIndex);
            }
        }


        private void ButtonSelectedDown_Click(object sender, EventArgs e)
        {
            var selectedCell = ColumnView.SelectedCells.OfType<DataGridViewCell>().FirstOrDefault();
            if (selectedCell != null && selectedCell.RowIndex < ColumnView.RowCount - 1)
            {
                ControlHelper.RowMoveDown(ColumnView, selectedCell.RowIndex);
            }
        }

        // チェックボックスを即時反映
        private void ColumnView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{

			if (ColumnView.Columns[ColumnView.CurrentCellAddress.X] is DataGridViewCheckBoxColumn)
			{
				if (ColumnView.IsCurrentCellDirty)
				{
					ColumnView.CommitEdit(DataGridViewDataErrorContexts.Commit);
				}
			}
		}

		// エラー値は元に戻す
		private void ColumnView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.Cancel = false;
		}

		// 幅には数値以外の入力を拒否
		private void ColumnView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{

			if (e.ColumnIndex == ColumnView_Width.Index)
			{

				if (!int.TryParse(e.FormattedValue.ToString(), out int value))
				{
					ColumnView.CancelEdit();
				}

			}

		}


		private void ButtonOK_Click(object sender, EventArgs e)
		{

			Result = new List<ShipGroupData.ViewColumnData>(ColumnView.Rows.Count - 1);

			for (int i = 1; i < ColumnView.Rows.Count; i++)
			{

				var row = ColumnView.Rows[i];
				var r = new ShipGroupData.ViewColumnData((string)row.Tag)
				{
					DisplayIndex = row.Index - 1,
					Visible = (bool)row.Cells[ColumnView_Visible.Index].Value,
					AutoSize = (bool)row.Cells[ColumnView_AutoSize.Index].Value,
					Width = Convert.ToInt32(row.Cells[ColumnView_Width.Index].Value)
				};

				Result.Add(r);
			}

			ScrollLockColumnCount = (int)ScrLkColumnCount.Value;

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}



		private void CopyRow(DataGridView dgv, int source, int destination)
		{
			var clone = new DataGridViewRow();
			var src = dgv.Rows[source];
			clone.CreateCells(dgv);

			for (int i = 0; i < clone.Cells.Count; i++)
			{
				clone.Cells[i].Value = src.Cells[i].Value;
			}
			clone.Tag = src.Tag;

			dgv.Rows.Insert(destination, clone);
		}


		private void CheckRow()
		{

			var rows = ColumnView.Rows.Cast<DataGridViewRow>();
			var allrow = ColumnView.Rows[0];

			if (rows.All(r => r.Cells[ColumnView_Visible.Index].Value as bool? ?? false == true))
			{
				allrow.Cells[ColumnView_Visible.Index].Value = true;
			}
			else if (rows.All(r => r.Cells[ColumnView_Visible.Index].Value as bool? ?? true == false))
			{
				allrow.Cells[ColumnView_Visible.Index].Value = false;
			}
			else
			{
				allrow.Cells[ColumnView_Visible.Index].Value = null;
			}

			if (rows.All(r => r.Cells[ColumnView_AutoSize.Index].Value as bool? ?? false == true))
			{
				allrow.Cells[ColumnView_AutoSize.Index].Value = true;
			}
			else if (rows.All(r => r.Cells[ColumnView_AutoSize.Index].Value as bool? ?? true == false))
			{
				allrow.Cells[ColumnView_AutoSize.Index].Value = false;
			}
			else
			{
				allrow.Cells[ColumnView_AutoSize.Index].Value = null;
			}


		}

    }
}
