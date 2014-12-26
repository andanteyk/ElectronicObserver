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

		public FormQuest( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( QuestView );

		}


		private void FormQuest_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_req_quest/clearitemget"].RequestReceived += rec;

			o.APIList["api_get_member/questlist"].ResponseReceived += rec;


			//こうしないとフォントがなぜかデフォルトにされる
			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			QuestView.Font = Font;
			
			
			//デフォルト行の追加
			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( QuestView );
				row.SetValues( null, "", "(未取得)", "" );
				QuestView.Rows.Add( row );
			}


			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.HQQuest] );

		}


		void Updated( string apiname, dynamic data ) {

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();

			foreach ( var q in KCDatabase.Instance.Quest.Quests.Values ) {

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( QuestView );


				row.Cells[QuestView_State.Index].Value = ( q.State == 3 ) ? ( (bool ?)null ) : ( q.State == 2 );
				row.Cells[QuestView_Type.Index].Value = Constants.GetQuestType( q.Type );
				row.Cells[QuestView_Name.Index].Value = q.Name;
				

				{
					string message = "#null!";
					if ( q.State == 3 ) {
						message = "達成！";
					} else {
						switch ( q.Progress ) {
							case 0:
								message = "-"; break;
							case 1:
								message = "50%"; break;
							case 2:
								message = "80%"; break;
						}
					}

					row.Cells[QuestView_Progress.Index].Value = message;
				}

				QuestView.Rows.Add( row );
			}


			if ( KCDatabase.Instance.Quest.Quests.Count != KCDatabase.Instance.Quest.Count ) {
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;	//intermediate
				
				QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "(未取得の任務 x " + ( KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count ) + " )";
			}

			//更新時にソートする！
			//フィルタも適用可能ならしておく
			QuestView.ResumeLayout();
		}


		protected override string GetPersistString() {
			return "Quest";
		}

		private void QuestView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == QuestView_State.Index ) {
				e.SortResult = ( e.CellValue1 == null ? 2 : ( (bool)e.CellValue1 ? 1 : 0 ) ) -
					( e.CellValue2 == null ? 2 : ( (bool)e.CellValue2 ? 1 : 0 ) );
			} else {
				//undone: 疲れたので寝るから手抜き工事;後で修正せよ
				e.SortResult = ( (string)e.CellValue1 ).CompareTo( e.CellValue2 );
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( QuestView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( QuestView.Rows[e.RowIndex2].Tag ?? 0 );
			}
		}

		private void QuestView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < QuestView.Rows.Count; i++ ) {
				QuestView.Rows[i].Tag = i;
			}

		}

	}
}
