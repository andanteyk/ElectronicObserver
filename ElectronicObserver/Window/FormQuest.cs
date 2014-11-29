using ElectronicObserver.Data;
using ElectronicObserver.Observer;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormQuest : DockContent {

		public FormQuest( FormMain parent ) {
			InitializeComponent();

		}

		
		private void FormQuest_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_req_quest/clearitemget"].RequestReceived += rec;

			o.APIList["api_get_member/questlist"].ResponseReceived += rec;


			//こうしないとフォントがなぜかデフォルトにされる
			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			QuestView.Font = Font;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.HQQuest] );

		}


		void Updated( string apiname, dynamic data ) {

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();
			foreach ( var q in KCDatabase.Instance.Quest.Quests ) {
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[0].Value = ( q.Value.State == 3 ) ? ( (bool ?)null ) : ( q.Value.State == 2 );
				QuestView.Rows[index].Cells[1].Value = q.Value.Name;
				
				{
					string message = "#null!";
					if ( q.Value.State == 3 ) {
						message = "達成！";
					} else {
						switch ( q.Value.Progress ) {
							case 0:
								message = "-"; break;
							case 1:
								message = "50%"; break;
							case 2:
								message = "80%"; break;
						}
					}

					QuestView.Rows[index].Cells[2].Value = message;
				}
			}

			if ( KCDatabase.Instance.Quest.Quests.Count != KCDatabase.Instance.Quest.Count ) {
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[0].Value = null;	//intermediate
				
				QuestView.Rows[index].Cells[1].Value = "(未取得の任務 x " + ( KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count ) + " )";
			}

			//更新時にソートする！
			//フィルタも適用可能ならしておく
			QuestView.ResumeLayout();
		}


		protected override string GetPersistString() {
			return "Quest";
		}

	}
}
