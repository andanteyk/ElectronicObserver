using ElectronicObserver.Data;
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

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSProgress1, CSProgress50, CSProgress80, CSProgress100;


		public FormQuest( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( QuestView );

			ConfigurationChanged();


			#region set cellstyle

			CSDefaultLeft = new DataGridViewCellStyle();
			CSDefaultLeft.Alignment = DataGridViewContentAlignment.MiddleLeft;
			CSDefaultLeft.BackColor = SystemColors.Control;
			//CSDefaultLeft.Font = Font;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionBackColor = Color.FromArgb( 0xFF, 0xFF, 0xCC );
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultCenter.Alignment = DataGridViewContentAlignment.MiddleCenter;

			CSProgress1 = new DataGridViewCellStyle( CSDefaultLeft );
			CSProgress1.BackColor = Color.FromArgb( 0xFF, 0xFF, 0xBB );

			CSProgress50 = new DataGridViewCellStyle( CSDefaultLeft );
			CSProgress50.BackColor = Color.FromArgb( 0xDD, 0xFF, 0xBB );

			CSProgress80 = new DataGridViewCellStyle( CSDefaultLeft );
			CSProgress80.BackColor = Color.FromArgb( 0xBB, 0xFF, 0xBB );

			CSProgress100 = new DataGridViewCellStyle( CSDefaultLeft );
			CSProgress100.BackColor = Color.FromArgb( 0xBB, 0xFF, 0xFF );

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

			KCDatabase.Instance.Quest.QuestUpdated += () => Invoke( new Action( Updated ) );


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
					DataGridViewCellStyle style;
					
					if ( q.State == 3 ) {
						value = "達成！";
						style = CSProgress100;

					} else {

						if ( KCDatabase.Instance.QuestProgress.Progresses.ContainsKey( q.QuestID ) ) {
							var p = KCDatabase.Instance.QuestProgress.Progresses[q.QuestID];
							value = p.ToString();

							double percentage = p.ProgressPercentage;

							if ( percentage >= 1.00 ) {
								style = CSProgress100;

							} else if ( percentage >= 0.80 ) {
								style = CSProgress80;

							} else if ( percentage >= 0.50 ) {
								style = CSProgress50;

							} else if ( percentage > 0.00 ) {
								style = CSProgress1;

							} else {
								style = CSDefaultLeft;

							} 

						} else {

							switch ( q.Progress ) {
								case 0:
									value = "-";
									style = CSDefaultLeft;
									break;
								case 1:
									value = "50%以上";
									style = CSProgress50;
									break;
								case 2:
									value = "80%以上";
									style = CSProgress80;
									break;
								default:
									value = "???";
									style = CSDefaultLeft;
									break;
							}
						}
					}

					row.Cells[QuestView_Progress.Index].Value = value;
					row.Cells[QuestView_Progress.Index].Style = style;

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
					e.Value = KCDatabase.Instance.Quest[(int)e.Value].Name;
					e.FormattingApplied = true;

				} /*
				else if ( e.ColumnIndex == QuestView_Progress.Index ) {
					switch ( (int)e.Value ) {
						case 0:
							e.Value = "-"; break;
						case 1:
							e.Value = "50%"; break;
						case 2:
							e.Value = "80%"; break;
						case 3:
							e.Value = "達成！"; break;
						default:
							e.Value = "???"; break;
					}

					e.FormattingApplied = true;

				}
				   */
			}


			/*
			if ( e.ColumnIndex == QuestView_Progress.Index ) {
				int? qid = QuestView.Rows[e.RowIndex].Cells[QuestView_Name.Index].Value as int?;

				if ( qid != null ) {
					var q = KCDatabase.Instance.Quest[(int)qid];

					if ( q != null && q.State != 3 && KCDatabase.Instance.QuestProgress.Progresses.ContainsKey( q.QuestID ) ) {
						e.Value = KCDatabase.Instance.QuestProgress.Progresses[q.QuestID].ToString();
						e.FormattingApplied = true;
					}
				}
			}
			*/

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


	}
}
