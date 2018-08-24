﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{

	public partial class FormQuest : DockContent
	{

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter;
		private DataGridViewCellStyle[] CSCategories;
		private bool IsLoaded = false;

		public FormQuest(FormMain parent)
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(QuestView);

			ConfigurationChanged();


			#region set cellstyle

			CSDefaultLeft = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft
			};
			CSDefaultLeft.BackColor =
			CSDefaultLeft.SelectionBackColor = SystemColors.Control;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleCenter
			};

			CSCategories = new DataGridViewCellStyle[9];
			for (int i = 0; i < CSCategories.Length; i++)
			{
				CSCategories[i] = new DataGridViewCellStyle(CSDefaultCenter);

				Color c;
				switch (i + 1)
				{
					case 1:     //編成
						c = Color.FromArgb(0xAA, 0xFF, 0xAA);
						break;
					case 2:     //出撃
						c = Color.FromArgb(0xFF, 0xCC, 0xCC);
						break;
					case 3:     //演習
						c = Color.FromArgb(0xDD, 0xFF, 0xAA);
						break;
					case 4:     //遠征
						c = Color.FromArgb(0xCC, 0xFF, 0xFF);
						break;
					case 5:     //補給/入渠
						c = Color.FromArgb(0xFF, 0xFF, 0xCC);
						break;
					case 6:     //工廠
						c = Color.FromArgb(0xDD, 0xCC, 0xBB);
						break;
					case 7:     //改装
						c = Color.FromArgb(0xDD, 0xCC, 0xFF);
						break;
					case 8:     //出撃(2)
						c = Color.FromArgb(0xFF, 0xCC, 0xCC);
						break;
					case 9:     //その他
					default:
						c = CSDefaultCenter.BackColor;
						break;
				}

				CSCategories[i].BackColor =
				CSCategories[i].SelectionBackColor = c;
			}

			QuestView.DefaultCellStyle = CSDefaultCenter;
			QuestView_Category.DefaultCellStyle = CSCategories[CSCategories.Length - 1];
			QuestView_Name.DefaultCellStyle = CSDefaultLeft;
			QuestView_Progress.DefaultCellStyle = CSDefaultLeft;

			#endregion


			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}



		private void FormQuest_Load(object sender, EventArgs e)
		{

			/*/
			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( APIUpdated ), apiname, data );

			o.APIList["api_req_quest/clearitemget"].RequestReceived += rec;

			o.APIList["api_get_member/questlist"].ResponseReceived += rec;
			//*/

			KCDatabase.Instance.Quest.QuestUpdated += Updated;


			ClearQuestView();

			try
			{
				int sort = Utility.Configuration.Config.FormQuest.SortParameter;

				QuestView.Sort(QuestView.Columns[sort >> 1], (sort & 1) == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending);

			}
			catch (Exception)
			{

				QuestView.Sort(QuestView_Name, ListSortDirection.Ascending);
			}


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest]);

			IsLoaded = true;
		}


		void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			QuestView.Font = Font = c.UI.MainFont;

			MenuMain_ShowRunningOnly.Checked = c.FormQuest.ShowRunningOnly;
			MenuMain_ShowOnce.Checked = c.FormQuest.ShowOnce;
			MenuMain_ShowDaily.Checked = c.FormQuest.ShowDaily;
			MenuMain_ShowWeekly.Checked = c.FormQuest.ShowWeekly;
			MenuMain_ShowMonthly.Checked = c.FormQuest.ShowMonthly;
			MenuMain_ShowOther.Checked = c.FormQuest.ShowOther;

			if (c.FormQuest.ColumnFilter == null || ((List<bool>)c.FormQuest.ColumnFilter).Count != QuestView.Columns.Count)
			{
				c.FormQuest.ColumnFilter = Enumerable.Repeat(true, QuestView.Columns.Count).ToList();
			}
			if (c.FormQuest.ColumnWidth == null || ((List<int>)c.FormQuest.ColumnWidth).Count != QuestView.Columns.Count)
			{
				c.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(column => column.Width).ToList();
			}
			{
				List<bool> list = c.FormQuest.ColumnFilter;
				List<int> width = c.FormQuest.ColumnWidth;

				for (int i = 0; i < QuestView.Columns.Count; i++)
				{
					QuestView.Columns[i].Visible =
					((ToolStripMenuItem)MenuMain_ColumnFilter.DropDownItems[i]).Checked = list[i];
					QuestView.Columns[i].Width = width[i];
				}
			}

			foreach (DataGridViewColumn column in QuestView.Columns)
			{
				column.SortMode = c.FormQuest.AllowUserToSortRows ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
			}

			if (c.UI.IsLayoutFixed)
			{
				QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
				QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			}
			else
			{
				QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
				QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			}

			foreach (DataGridViewRow row in QuestView.Rows)
			{
				row.Height = 21;
			}

			Updated();

		}


		void SystemEvents_SystemShuttingDown()
		{

			try
			{

				if (QuestView.SortedColumn != null)
					Utility.Configuration.Config.FormQuest.SortParameter = QuestView.SortedColumn.Index << 1 | (QuestView.SortOrder == SortOrder.Ascending ? 0 : 1);

				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

			}
			catch (Exception)
			{
				// *ぷちっ*				
			}
		}



		void Updated()
		{

			if (!KCDatabase.Instance.Quest.IsLoaded) return;

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();

			foreach (var q in KCDatabase.Instance.Quest.Quests.Values)
			{

				if (MenuMain_ShowRunningOnly.Checked && !(q.State == 2 || q.State == 3))
					continue;

				switch (q.Type)
				{
					case 1:
						if (!MenuMain_ShowDaily.Checked) continue;
						break;
					case 2:
						if (!MenuMain_ShowWeekly.Checked) continue;
						break;
					case 3:
						if (!MenuMain_ShowMonthly.Checked) continue;
						break;
					case 4:
					default:
						if (!MenuMain_ShowOnce.Checked) continue;
						break;
					case 5:
						if (q.QuestID == 211 || q.QuestID == 212)
						{   // 空母3か輸送5
							if (!MenuMain_ShowDaily.Checked) continue;
						}
						else
						{
							if (!MenuMain_ShowOther.Checked) continue;
						}
						break;
				}

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(QuestView);
				row.Height = 21;

				row.Cells[QuestView_State.Index].Value = (q.State == 3) ? ((bool?)null) : (q.State == 2);
				row.Cells[QuestView_Type.Index].Value = q.Type;
				row.Cells[QuestView_Category.Index].Value = q.Category;
				row.Cells[QuestView_Category.Index].Style = CSCategories[Math.Min(q.Category - 1, 8 - 1)];
				row.Cells[QuestView_Name.Index].Value = q.QuestID;
				row.Cells[QuestView_PageNum.Index].Value = q.DisplayPage;
				{
					var progress = KCDatabase.Instance.QuestProgress[q.QuestID];
					row.Cells[QuestView_Name.Index].ToolTipText = $"{q.QuestID} : {q.Name}\r\n{q.Description}\r\n{progress?.GetClearCondition() ?? ""}\r\n任務ページ : {q.DisplayPage}";
				}
				{
					string value;
					double tag;

					if (q.State == 3)
					{
						value = "達成！";
						tag = 1.0;

					}
					else
					{

						if (KCDatabase.Instance.QuestProgress.Progresses.ContainsKey(q.QuestID))
						{
							var p = KCDatabase.Instance.QuestProgress[q.QuestID];

							value = p.ToString();
							tag = p.ProgressPercentage;

						}
						else
						{

							switch (q.Progress)
							{
								case 0:
									value = "-";
									tag = 0.0;
									break;
								case 1:
									value = "50%以上";
									tag = 0.5;
									break;
								case 2:
									value = "80%以上";
									tag = 0.8;
									break;
								default:
									value = "???";
									tag = 0.0;
									break;
							}
						}
					}

					row.Cells[QuestView_Progress.Index].Value = value;
					row.Cells[QuestView_Progress.Index].Tag = tag;
				}

				QuestView.Rows.Add(row);
			}


			if (KCDatabase.Instance.Quest.Quests.Count < KCDatabase.Instance.Quest.Count)
			{
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				QuestView.Rows[index].Cells[QuestView_Name.Index].Value = string.Format("(未取得の任務 x {0})", (KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count));
			}

			if (KCDatabase.Instance.Quest.Quests.Count == 0)
			{
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "(任務完遂！)";
			}

			//更新時にソートする
			if (QuestView.SortedColumn != null)
				QuestView.Sort(QuestView.SortedColumn, QuestView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);


			QuestView.ResumeLayout();
		}


		private void QuestView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.Value is int)
			{
				if (e.ColumnIndex == QuestView_Type.Index)
				{
					e.Value = Constants.GetQuestType((int)e.Value);
					e.FormattingApplied = true;

				}
				else if (e.ColumnIndex == QuestView_Category.Index)
				{
					e.Value = Constants.GetQuestCategory((int)e.Value);
					e.FormattingApplied = true;

				}
				else if (e.ColumnIndex == QuestView_Name.Index)
				{
					var quest = KCDatabase.Instance.Quest[(int)e.Value];
					e.Value = quest != null ? quest.Name : "???";
					e.FormattingApplied = true;

				}

			}

		}



		private void QuestView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{

			if (e.Column.Index == QuestView_State.Index)
			{
				e.SortResult = (e.CellValue1 == null ? 2 : ((bool)e.CellValue1 ? 1 : 0)) -
					(e.CellValue2 == null ? 2 : ((bool)e.CellValue2 ? 1 : 0));
			}
			else
			{
				e.SortResult = (e.CellValue1 as int? ?? 99999999) - (e.CellValue2 as int? ?? 99999999);
			}

			if (e.SortResult == 0)
			{
				e.SortResult = (QuestView.Rows[e.RowIndex1].Tag as int? ?? 0) - (QuestView.Rows[e.RowIndex2].Tag as int? ?? 0);
			}

			e.Handled = true;
		}

		private void QuestView_Sorted(object sender, EventArgs e)
		{

			for (int i = 0; i < QuestView.Rows.Count; i++)
			{
				QuestView.Rows[i].Tag = i;
			}

		}


		private void QuestView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{

			if (e.ColumnIndex != QuestView_Progress.Index ||
				e.RowIndex < 0 ||
				(e.PaintParts & DataGridViewPaintParts.Background) == 0)
				return;


			using (var bback = new SolidBrush(e.CellStyle.BackColor))
			{

				Color col;
				double rate = QuestView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as double? ?? 0.0;

				if (rate < 0.5)
				{
					col = Color.FromArgb(0xFF, 0x88, 0x00);

				}
				else if (rate < 0.8)
				{
					col = Color.FromArgb(0x00, 0xCC, 0x00);

				}
				else if (rate < 1.0)
				{
					col = Color.FromArgb(0x00, 0x88, 0x00);

				}
				else
				{
					col = Color.FromArgb(0x00, 0x88, 0xFF);

				}

				using (var bgauge = new SolidBrush(col))
				{

					const int thickness = 4;

					e.Graphics.FillRectangle(bback, e.CellBounds);
					e.Graphics.FillRectangle(bgauge, new Rectangle(e.CellBounds.X, e.CellBounds.Bottom - thickness, (int)(e.CellBounds.Width * rate), thickness));
				}
			}

			e.Paint(e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.Background);
			e.Handled = true;

		}



		private void MenuMain_ShowRunningOnly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowRunningOnly = MenuMain_ShowRunningOnly.Checked;
			Updated();
		}


		private void MenuMain_ShowOnce_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowOnce = MenuMain_ShowOnce.Checked;
			Updated();
		}

		private void MenuMain_ShowDaily_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowDaily = MenuMain_ShowDaily.Checked;
			Updated();
		}

		private void MenuMain_ShowWeekly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowWeekly = MenuMain_ShowWeekly.Checked;
			Updated();
		}

		private void MenuMain_ShowMonthly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowMonthly = MenuMain_ShowMonthly.Checked;
			Updated();
		}

		private void MenuMain_ShowOther_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowOther = MenuMain_ShowOther.Checked;
			Updated();
		}



		private void MenuMain_Initialize_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("任務データを初期化します。\r\nデータに齟齬が生じている場合以外での使用は推奨しません。\r\nよろしいですか？", "任務初期化の確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
			{

				KCDatabase.Instance.Quest.Clear();
				KCDatabase.Instance.QuestProgress.Clear();
				ClearQuestView();
			}

		}


		private void ClearQuestView()
		{

			QuestView.Rows.Clear();

			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(QuestView);
				row.SetValues(null, null, null, "(未取得)", null);
				QuestView.Rows.Add(row);
			}

		}


		private void MenuMain_ColumnFilter_Click(object sender, EventArgs e)
		{

			var menu = sender as ToolStripMenuItem;
			if (menu == null) return;

			int index = -1;
			for (int i = 0; i < MenuMain_ColumnFilter.DropDownItems.Count; i++)
			{
				if (sender == MenuMain_ColumnFilter.DropDownItems[i])
				{
					index = i;
					break;
				}
			}

			if (index == -1) return;

			QuestView.Columns[index].Visible =
			Utility.Configuration.Config.FormQuest.ColumnFilter.List[index] = menu.Checked;
		}


		private void QuestView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{

			if (IsLoaded)
				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

		}




		private void QuestView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{

			if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
			{
				QuestView.ClearSelection();
				QuestView.Rows[e.RowIndex].Selected = true;
			}

		}

		private void MenuProgress_Increment_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();

			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && quest != null && progress != null)
			{

				try
				{
					progress.Increment();
					Updated();

				}
				catch (Exception)
				{
					Utility.Logger.Add(3, string.Format("任務『{0}』の進捗を変更することはできません。", quest.Name));
					System.Media.SystemSounds.Hand.Play();
				}
			}
		}

		private void MenuProgress_Decrement_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();
			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && quest != null && progress != null)
			{

				try
				{
					progress.Decrement();
					Updated();

				}
				catch (Exception)
				{
					Utility.Logger.Add(3, string.Format("任務『{0}』の進捗を変更することはできません。", quest.Name));
					System.Media.SystemSounds.Hand.Play();
				}
			}
		}

		private void MenuProgress_Reset_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();

			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && (quest != null || progress != null))
			{

				if (MessageBox.Show("任務" + (quest != null ? ("『" + quest.Name + "』") : ("ID: " + id.ToString() + " ")) + "を一覧から削除し、進捗をリセットします。\r\nよろしいですか？\r\n(艦これ本体の任務画面を開くと正しく更新されます。)", "任務削除の確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
				{

					if (quest != null)
						KCDatabase.Instance.Quest.Quests.Remove(quest);

					if (progress != null)
						KCDatabase.Instance.QuestProgress.Progresses.Remove(progress);

					Updated();
				}
			}

		}


		// デフォルトのツールチップは消える時間が速すぎるので、自分で制御する
		private void QuestView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= QuestView.RowCount || e.ColumnIndex >= QuestView.ColumnCount)
			{
				ToolTipInfo.SetToolTip(QuestView, null);
				return;
			}

			if (!string.IsNullOrWhiteSpace(QuestView[e.ColumnIndex, e.RowIndex].ToolTipText))
			{
				ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].ToolTipText);

			}
			else if (e.ColumnIndex == QuestView_Progress.Index && QuestView[e.ColumnIndex, e.RowIndex].Value != null)
			{
				ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].Value.ToString());

			}
			else
			{
				ToolTipInfo.SetToolTip(QuestView, null);
			}

		}

		private void QuestView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			ToolTipInfo.SetToolTip(QuestView, null);
		}


		private void MenuMain_Opening(object sender, CancelEventArgs e)
		{

			var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

			if (quest != null)
			{
				MenuMain_GoogleQuest.Enabled = true;
				MenuMain_GoogleQuest.Text = string.Format("『{0}』でGoogle検索(&G)", quest.Name);
			}
			else
			{
				MenuMain_GoogleQuest.Enabled = false;
				MenuMain_GoogleQuest.Text = "任務名でGoogle検索(&G)";
			}
		}

		private void MenuMain_GoogleQuest_Click(object sender, EventArgs e)
		{
			var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

			if (quest != null)
			{
				try
				{

					// google <任務名> 艦これ
					System.Diagnostics.Process.Start(@"https://www.google.co.jp/search?q=" + Uri.EscapeDataString(quest.Name) + "+%E8%89%A6%E3%81%93%E3%82%8C");

				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "任務名の Google 検索に失敗しました。");
				}
			}

		}

		private int GetSelectedRowQuestID()
		{
			var rows = QuestView.SelectedRows;

			if (rows != null && rows.Count > 0 && rows[0].Index != -1)
			{

				return rows[0].Cells[QuestView_Name.Index].Value as int? ?? -1;
			}

			return -1;
		}


		protected override string GetPersistString()
		{
			return "Quest";
		}


	}
}
