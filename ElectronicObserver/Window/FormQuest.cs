using ElectronicObserver.Data;
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

namespace ElectronicObserver.Window {

	public partial class FormQuest : DockContent {

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter;
		private DataGridViewCellStyle[] CSCategories;
		private bool IsLoaded = false;

		public FormQuest( FormMain parent ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( QuestView );

			ConfigurationChanged();


			#region set cellstyle

			CSDefaultLeft = new DataGridViewCellStyle();
			CSDefaultLeft.Alignment = DataGridViewContentAlignment.MiddleLeft;
			CSDefaultLeft.BackColor =
			CSDefaultLeft.SelectionBackColor = Utility.Configuration.Config.UI.BackColor;
			CSDefaultLeft.ForeColor =
			CSDefaultLeft.SelectionForeColor = Utility.Configuration.Config.UI.ForeColor;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultCenter.Alignment = DataGridViewContentAlignment.MiddleCenter;

			CSCategories = new DataGridViewCellStyle[9];
			for ( int i = 0; i < CSCategories.Length; i++ ) {
				CSCategories[i] = new DataGridViewCellStyle( CSDefaultCenter );

				Color c;
				switch ( i + 1 ) {
					case 1:		//編成
						c = Utility.Configuration.Config.UI.QuestOrganization;
						break;
					case 2:		//出撃
						c = Utility.Configuration.Config.UI.QuestSortie;
						break;
					case 3:		//演習
						c = Utility.Configuration.Config.UI.QuestExercise;
						break;
					case 4:		//遠征
						c = Utility.Configuration.Config.UI.QuestExpedition;
						break;
					case 5:		//補給/入渠
						c = Utility.Configuration.Config.UI.QuestSupplyDocking;
						break;
					case 6:		//工廠
						c = Utility.Configuration.Config.UI.QuestArsenal;
						break;
					case 7:		//改装
						c = Utility.Configuration.Config.UI.QuestRenovated;
						break;
					case 8:		//出撃(2)
						c = Color.FromArgb( 0xFF, 0xCC, 0xCC );
						break;
					case 9:		//その他
					default:
						c = CSDefaultCenter.BackColor;
						break;
				}

				CSCategories[i].BackColor =
				CSCategories[i].SelectionBackColor = c;
				CSCategories[i].ForeColor =
				CSCategories[i].SelectionForeColor =
					Utility.Configuration.Config.UI.QuestForeColor;
			}

			QuestView.DefaultCellStyle = CSDefaultCenter;
			QuestView.GridColor = Utility.Configuration.Config.UI.LineColor;
			QuestView_Category.DefaultCellStyle = CSCategories[CSCategories.Length - 1];
			QuestView_Name.DefaultCellStyle = CSDefaultLeft;
			QuestView_Progress.DefaultCellStyle = CSDefaultLeft;
			QuestView.ColumnHeadersHeight = this.GetDpiHeight( 24 );

			QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

			if ( Utility.Configuration.Config.UI.ThemeID == 1 ) {
				QuestView.EnableHeadersVisualStyles = false;
				QuestView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
				QuestView.ColumnHeadersDefaultCellStyle = CSDefaultLeft;
			}

			#endregion

			this.ResumeLayoutForDpiScale();
			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}


		private void FormQuest_Load( object sender, EventArgs e ) {

			/*/
			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( APIUpdated ), apiname, data );

			o.APIList["api_req_quest/clearitemget"].RequestReceived += rec;

			o.APIList["api_get_member/questlist"].ResponseReceived += rec;
			//*/

			KCDatabase.Instance.Quest.QuestUpdated += Updated;


			ClearQuestView();

			try {
				int sort = Utility.Configuration.Config.FormQuest.SortParameter;

				QuestView.Sort( QuestView.Columns[sort >> 1], ( sort & 1 ) == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending );

			} catch ( Exception ) {

				QuestView.Sort( QuestView_Name, ListSortDirection.Ascending );
			}


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest] );

			IsLoaded = true;
		}


		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			QuestView.BackgroundColor = c.UI.BackColor;
			QuestView.GridColor = c.UI.LineColor;
			QuestView.Font = Font = c.UI.MainFont;

			MenuMain_ShowRunningOnly.Checked = c.FormQuest.ShowRunningOnly;
			MenuMain_ShowOnce.Checked = c.FormQuest.ShowOnce;
			MenuMain_ShowDaily.Checked = c.FormQuest.ShowDaily;
			MenuMain_ShowWeekly.Checked = c.FormQuest.ShowWeekly;
			MenuMain_ShowMonthly.Checked = c.FormQuest.ShowMonthly;

			if ( CSDefaultCenter != null && CSDefaultLeft != null ) {
				CSDefaultCenter.BackColor =
				CSDefaultCenter.SelectionBackColor =
				CSDefaultLeft.BackColor =
				CSDefaultLeft.SelectionBackColor =
					c.UI.BackColor;
				CSDefaultCenter.ForeColor =
				CSDefaultCenter.SelectionForeColor =
				CSDefaultLeft.ForeColor =
				CSDefaultLeft.SelectionForeColor =
					c.UI.ForeColor;
			}

			if ( c.FormQuest.ColumnFilter == null || ( (List<bool>)c.FormQuest.ColumnFilter ).Count != QuestView.Columns.Count ) {
				c.FormQuest.ColumnFilter = Enumerable.Repeat( true, QuestView.Columns.Count ).ToList();
			}
			if ( c.FormQuest.ColumnWidth == null || ( (List<int>)c.FormQuest.ColumnWidth ).Count != QuestView.Columns.Count ) {
				c.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select( column => column.Width ).ToList();
			}
			{
				List<bool> list = c.FormQuest.ColumnFilter;
				List<int> width = c.FormQuest.ColumnWidth;

				for ( int i = 0; i < QuestView.Columns.Count; i++ ) {
					QuestView.Columns[i].Visible =
					( (ToolStripMenuItem)MenuMain_ColumnFilter.DropDownItems[i] ).Checked = list[i];
					QuestView.Columns[i].Width = width[i];
				}
			}

			foreach ( DataGridViewColumn column in QuestView.Columns ) {
				column.SortMode = c.FormQuest.AllowUserToSortRows ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable; 
			}

			Updated();

		}


		void SystemEvents_SystemShuttingDown() {

			try {

				if ( QuestView.SortedColumn != null )
					Utility.Configuration.Config.FormQuest.SortParameter = QuestView.SortedColumn.Index << 1 | ( QuestView.SortOrder == SortOrder.Ascending ? 0 : 1 );

				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select( c => c.Width ).ToList();

			} catch ( Exception ) {
				// *ぷちっ*				
			}
		}



		void Updated() {

			if ( !KCDatabase.Instance.Quest.IsLoaded ) return;

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();

			foreach ( var q in KCDatabase.Instance.Quest.Quests.Values ) {

				if ( MenuMain_ShowRunningOnly.Checked && !( q.State == 2 || q.State == 3 ) )
					continue;

				switch ( q.Type ) {
					case 1:
					case 5:
						if ( !MenuMain_ShowDaily.Checked ) continue;
						break;
					case 2:
						if ( !MenuMain_ShowWeekly.Checked ) continue;
						break;
					case 3:
						if ( !MenuMain_ShowMonthly.Checked ) continue;
						break;
					default:
						if ( !MenuMain_ShowOnce.Checked ) continue;
						break;
				}


				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( QuestView );


				row.Cells[QuestView_State.Index].Value = ( q.State == 3 ) ? ( (bool?)null ) : ( q.State == 2 );
				row.Cells[QuestView_Type.Index].Value = q.Type;
				row.Cells[QuestView_Category.Index].Value = q.Category;
				row.Cells[QuestView_Category.Index].Style = CSCategories[Math.Min( q.Category - 1, 8 - 1 )];
				row.Cells[QuestView_Name.Index].Value = q.QuestID;
				{
					QuestInfo info = QuestLib.GetQuest( q.QuestID );
					var progress = KCDatabase.Instance.QuestProgress[q.QuestID];
					if ( info != null ) {
						string tip = info.Tips;
						string award = info.Award;
						row.Cells[QuestView_Name.Index].ToolTipText = string.Format( "{0} : {1}\r\n{2}\r\n\r\n奖励:{3}\r\n\r\n{4}\r\n{5}", q.QuestID, q.Name, q.Description, award, tip, progress != null ? progress.GetClearCondition() : "" );
					} else {
						row.Cells[QuestView_Name.Index].ToolTipText = string.Format( "{0} : {1}\r\n{2}\r\n{3}", q.QuestID, q.Name, q.Description, progress != null ? progress.GetClearCondition() : "" );
					}
				}
				{
					string value;
					double tag;

					if ( q.State == 3 ) {
						value = "達成！";
						tag = 1.0;

					} else {

						if ( KCDatabase.Instance.QuestProgress.Progresses.ContainsKey( q.QuestID ) ) {
							var p = KCDatabase.Instance.QuestProgress[q.QuestID];

							value = p.ToString();
							tag = p.ProgressPercentage;

						} else {

							switch ( q.Progress ) {
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

				QuestView.Rows.Add( row );
			}


			if ( KCDatabase.Instance.Quest.Quests.Count < KCDatabase.Instance.Quest.Count ) {
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				QuestView.Rows[index].Cells[QuestView_Name.Index].Value = string.Format( "(未取得の任務 x {0})", ( KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count ) );
			}

			if ( KCDatabase.Instance.Quest.Quests.Count == 0 ) {
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "(任務完遂！)";
			}

			//更新時にソートする
			if ( QuestView.SortedColumn != null )
				QuestView.Sort( QuestView.SortedColumn, QuestView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending );


			QuestView.ResumeLayout();
		}


		private void QuestView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.Value is int ) {
				if ( e.ColumnIndex == QuestView_Type.Index ) {
					e.Value = Constants.GetQuestType( (int)e.Value );
					e.FormattingApplied = true;

				} else if ( e.ColumnIndex == QuestView_Category.Index ) {
					e.Value = Constants.GetQuestCategory( (int)e.Value );
					e.FormattingApplied = true;

				} else if ( e.ColumnIndex == QuestView_Name.Index ) {
					var quest = KCDatabase.Instance.Quest[(int)e.Value];
					e.Value = quest != null ? quest.Name : "???";
					e.FormattingApplied = true;

				}

			}

		}



		private void QuestView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == QuestView_State.Index ) {
				e.SortResult = ( e.CellValue1 == null ? 2 : ( (bool)e.CellValue1 ? 1 : 0 ) ) -
					( e.CellValue2 == null ? 2 : ( (bool)e.CellValue2 ? 1 : 0 ) );
			} else {
				e.SortResult = ( e.CellValue1 as int? ?? 99999999 ) - ( e.CellValue2 as int? ?? 99999999 );
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = ( QuestView.Rows[e.RowIndex1].Tag as int? ?? 0 ) - ( QuestView.Rows[e.RowIndex2].Tag as int? ?? 0 );
			}

			e.Handled = true;
		}

		private void QuestView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < QuestView.Rows.Count; i++ ) {
				QuestView.Rows[i].Tag = i;
			}

		}


		private void QuestView_CellPainting( object sender, DataGridViewCellPaintingEventArgs e ) {

			if ( e.ColumnIndex != QuestView_Progress.Index ||
				e.RowIndex < 0 ||
				( e.PaintParts & DataGridViewPaintParts.Background ) == 0 )
				return;


			using ( var bback = new SolidBrush( e.CellStyle.BackColor ) ) {

				Color col;
				double rate = QuestView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as double? ?? 0.0;

				if ( rate < 0.5 ) {
					col = Color.FromArgb( 0xFF, 0x88, 0x00 );

				} else if ( rate < 0.8 ) {
					col = Color.FromArgb( 0x00, 0xCC, 0x00 );

				} else if ( rate < 1.0 ) {
					col = Color.FromArgb( 0x00, 0x88, 0x00 );

				} else {
					col = Color.FromArgb( 0x00, 0x88, 0xFF );

				}

				using ( var bgauge = new SolidBrush( col ) ) {

					const int thickness = 4;

					e.Graphics.FillRectangle( bback, e.CellBounds );
					e.Graphics.FillRectangle( bgauge, new Rectangle( e.CellBounds.X, e.CellBounds.Bottom - thickness, (int)( e.CellBounds.Width * rate ), thickness ) );
				}
			}

			e.Paint( e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.Background );
			e.Handled = true;

		}



		private void MenuMain_ShowRunningOnly_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormQuest.ShowRunningOnly = MenuMain_ShowRunningOnly.Checked;
			Updated();
		}

		private void MenuMain_SaveNow_Click( object sender, EventArgs e ) {
			KCDatabase.Instance.QuestProgress.Save();
			Utility.Logger.Add( 2, "任务进度已保存。" );
		}


		private void MenuMain_ShowOnce_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormQuest.ShowOnce = MenuMain_ShowOnce.Checked;
			Updated();
		}

		private void MenuMain_ShowDaily_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormQuest.ShowDaily = MenuMain_ShowDaily.Checked;
			Updated();
		}

		private void MenuMain_ShowWeekly_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormQuest.ShowWeekly = MenuMain_ShowWeekly.Checked;
			Updated();
		}

		private void MenuMain_ShowMonthly_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormQuest.ShowMonthly = MenuMain_ShowMonthly.Checked;
			Updated();
		}



		private void MenuMain_Initialize_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "任务进度即将初始化。\r\n推荐只在数据混乱的时候使用。\r\n确定吗？", "任务初始化确认",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 ) == System.Windows.Forms.DialogResult.Yes ) {

				KCDatabase.Instance.Quest.Clear();
				KCDatabase.Instance.QuestProgress.Clear();
				ClearQuestView();
			}

		}


		private void ClearQuestView() {

			QuestView.Rows.Clear();

			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( QuestView );
				row.SetValues( null, null, null, "(未取得)", null );
				QuestView.Rows.Add( row );
			}

		}

		private void MenuMain_ColumnFilter_Click( object sender, EventArgs e ) {

			var menu = sender as ToolStripMenuItem;
			if ( menu == null ) return;

			int index = -1;
			for ( int i = 0; i < MenuMain_ColumnFilter.DropDownItems.Count; i++ ) {
				if ( sender == MenuMain_ColumnFilter.DropDownItems[i] ) {
					index = i;
					break;
				}
			}

			if ( index == -1 ) return;

			QuestView.Columns[index].Visible =
			Utility.Configuration.Config.FormQuest.ColumnFilter.List[index] = menu.Checked;
		}


		private void QuestView_ColumnWidthChanged( object sender, DataGridViewColumnEventArgs e ) {
			if ( IsLoaded )
				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select( c => c.Width ).ToList();

		}




		private void QuestView_CellMouseDown( object sender, DataGridViewCellMouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0 ) {
				QuestView.ClearSelection();
				QuestView.Rows[e.RowIndex].Selected = true;
			}

		}

		private void MenuProgress_Increment_Click( object sender, EventArgs e ) {

			var rows = QuestView.SelectedRows;

			if ( rows != null && rows.Count > 0 && rows[0].Index != -1 ) {

				int id = rows[0].Cells[QuestView_Name.Index].Value as int? ?? -1;

				var quest = KCDatabase.Instance.Quest[id];
				var progress = KCDatabase.Instance.QuestProgress[id];

				if ( id != -1 && quest != null && progress != null ) {

					try {
						progress.Increment();
						Updated();

					} catch ( Exception ) {
						Utility.Logger.Add( 3, "この任務の進捗を変更することはできません。" );
						System.Media.SystemSounds.Hand.Play();
					}
				}
			}
		}

		private void MenuProgress_Decrement_Click( object sender, EventArgs e ) {

			var rows = QuestView.SelectedRows;

			if ( rows != null && rows.Count > 0 && rows[0].Index != -1 ) {

				int id = rows[0].Cells[QuestView_Name.Index].Value as int? ?? -1;

				var quest = KCDatabase.Instance.Quest[id];
				var progress = KCDatabase.Instance.QuestProgress[id];

				if ( id != -1 && quest != null && progress != null ) {

					try {
						progress.Decrement();
						Updated();

					} catch ( Exception ) {
						Utility.Logger.Add( 3, "この任務の進捗を変更することはできません。" );
						System.Media.SystemSounds.Hand.Play();
					}
				}
			}
		}




		public override string GetPersistString() {
			return "Quest";
		}

	}
}
