﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
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


		public FormQuest( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( QuestView );

			ConfigurationChanged();


			#region set cellstyle

			CSDefaultLeft = new DataGridViewCellStyle();
			CSDefaultLeft.Alignment = DataGridViewContentAlignment.MiddleLeft;
			CSDefaultLeft.BackColor =
			CSDefaultLeft.SelectionBackColor = SystemColors.Control;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultCenter.Alignment = DataGridViewContentAlignment.MiddleCenter;


			QuestView.DefaultCellStyle = CSDefaultCenter;
			QuestView_Name.DefaultCellStyle = CSDefaultLeft;
			QuestView_Progress.DefaultCellStyle = CSDefaultLeft;

			#endregion
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
			QuestView.Sort( QuestView_Name, ListSortDirection.Ascending );


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest] );

		}


		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			QuestView.Font = Font = c.UI.MainFont;

			MenuMain_ShowRunningOnly.Checked = c.FormQuest.ShowRunningOnly;
			MenuMain_ShowOnce.Checked = c.FormQuest.ShowOnce;
			MenuMain_ShowDaily.Checked = c.FormQuest.ShowDaily;
			MenuMain_ShowWeekly.Checked = c.FormQuest.ShowWeekly;
			MenuMain_ShowMonthly.Checked = c.FormQuest.ShowMonthly;

			if ( c.FormQuest.ColumnFilter == null ) {
				c.FormQuest.ColumnFilter = new Utility.Storage.SerializableList<bool>( Enumerable.Repeat( true, QuestView.Columns.Count ).ToList() );
			}
			{
				List<bool> list = c.FormQuest.ColumnFilter;

				for ( int i = 0; i < QuestView.Columns.Count; i++ ) {
					QuestView.Columns[i].Visible =
					((ToolStripMenuItem)MenuMain_ColumnFilter.DropDownItems[i]).Checked = list[i];
				}
			}
			Updated();

		}



		void Updated() {

			if ( !KCDatabase.Instance.Quest.IsLoaded ) return;

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();

			foreach ( var q in KCDatabase.Instance.Quest.Quests.Values ) {

				if ( MenuMain_ShowRunningOnly.Checked && !( q.State == 2 || q.State == 3 ) )
					continue;

				switch ( q.Type ) {
					case 2:
					case 4:
					case 5:
						if ( !MenuMain_ShowDaily.Checked ) continue;
						break;
					case 3:
						if ( !MenuMain_ShowWeekly.Checked ) continue;
						break;
					case 6:
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
				row.Cells[QuestView_Name.Index].Value = q.QuestID;
				row.Cells[QuestView_Name.Index].ToolTipText = string.Format( "{0} : {1}\r\n{2}", q.QuestID, q.Name, q.Description );

				{
					string value;
					double tag;

					if ( q.State == 3 ) {
						value = "達成！";
						tag = 1.0;

					} else {

						if ( KCDatabase.Instance.QuestProgress.Progresses.ContainsKey( q.QuestID ) ) {
							var p = KCDatabase.Instance.QuestProgress.Progresses[q.QuestID];

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


			if ( KCDatabase.Instance.Quest.Quests.Count != KCDatabase.Instance.Quest.Count ) {
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

			if ( MessageBox.Show( "任務データを初期化します。\r\nデータに齟齬が生じている場合以外での使用は推奨しません。\r\nよろしいですか？", "任務初期化の確認",
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

		protected override string GetPersistString() {
			return "Quest";
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



	}
}
