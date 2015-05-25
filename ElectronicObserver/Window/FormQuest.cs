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

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter;
		private DataGridViewCellStyle[] CSCategories;


		public FormQuest( FormMain parent ) {
            SuspendLayout();
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

			CSCategories = new DataGridViewCellStyle[8];
			for ( int i = 0; i < 8; i++ ) {
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
					case 8:		//その他
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
			QuestView_Category.DefaultCellStyle = CSCategories[8 - 1];
			QuestView_Name.DefaultCellStyle = CSDefaultLeft;
			QuestView_Progress.DefaultCellStyle = CSDefaultLeft;
            QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            Graphics g = this.CreateGraphics();
            float dy;
            try
            {
                dy = g.DpiY;
            }
            finally
            {
                g.Dispose();
            }
            QuestView.ColumnHeadersHeight = (int)dy / 96 * 23;

            #endregion

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
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

			/*/
			#region - Debug -

			APIObserver o = APIObserver.Instance;
			dynamic data;

			//data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "api_start2.txt" ) ).api_data;
			//o.APIList["api_start2"].OnResponseReceived( data );

			//data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "port.txt" ) ).api_data;
			//o.APIList["api_port/port"].OnResponseReceived( data );

			data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "questlist.txt" ) ).api_data;
			o.APIList["api_get_member/questlist"].OnResponseReceived( data );

			Updated();

			#endregion
			//*/
		}


		void ConfigurationChanged() {

			var conf = Utility.Configuration.Config;

			QuestView.BackgroundColor = conf.UI.BackColor;
			QuestView.GridColor = conf.UI.LineColor;

			if ( CSDefaultCenter != null && CSDefaultLeft != null ) {
				CSDefaultCenter.BackColor =
				CSDefaultCenter.SelectionBackColor =
				CSDefaultLeft.BackColor =
				CSDefaultLeft.SelectionBackColor =
					conf.UI.BackColor;
				CSDefaultCenter.ForeColor =
				CSDefaultCenter.SelectionForeColor =
				CSDefaultLeft.ForeColor =
				CSDefaultLeft.SelectionForeColor =
					conf.UI.ForeColor;
			}

			if ( CSCategories != null && CSCategories.Length >= 8 ) {
				for ( int i = 0; i < 8; i++ ) {

					Color c;
					switch ( i + 1 ) {
						case 1:		//編成
							c = conf.UI.QuestOrganization;
							break;
						case 2:		//出撃
							c = conf.UI.QuestSortie;
							break;
						case 3:		//演習
							c = conf.UI.QuestExercise;
							break;
						case 4:		//遠征
							c = conf.UI.QuestExpedition;
							break;
						case 5:		//補給/入渠
							c = conf.UI.QuestSupplyDocking;
							break;
						case 6:		//工廠
							c = conf.UI.QuestArsenal;
							break;
						case 7:		//改装
							c = conf.UI.QuestRenovated;
							break;
						case 8:		//その他
						default:
							c = CSDefaultCenter.BackColor;
							break;
					}

					CSCategories[i].BackColor =
					CSCategories[i].SelectionBackColor = c;
					CSCategories[i].ForeColor =
					CSCategories[i].SelectionForeColor =
						SystemColors.ControlText;
				}
			}

			QuestView.Font = Font = conf.UI.MainFont;

			MenuMain_ShowRunningOnly.Checked = conf.FormQuest.ShowRunningOnly;
			MenuMain_ShowOnce.Checked = conf.FormQuest.ShowOnce;
			MenuMain_ShowDaily.Checked = conf.FormQuest.ShowDaily;
			MenuMain_ShowWeekly.Checked = conf.FormQuest.ShowWeekly;
			MenuMain_ShowMonthly.Checked = conf.FormQuest.ShowMonthly;

			if ( conf.FormQuest.ColumnFilter == null ) {
				conf.FormQuest.ColumnFilter = new Utility.Storage.SerializableList<bool>( Enumerable.Repeat( true, QuestView.Columns.Count ).ToList() );
			}
			{
				List<bool> list = conf.FormQuest.ColumnFilter;

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
				row.Cells[QuestView_Category.Index].Style = CSCategories[Math.Min( q.Category - 1, 8 - 1 )];
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
