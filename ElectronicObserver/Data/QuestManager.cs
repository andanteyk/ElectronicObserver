using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	public class QuestManager : APIWrapper {

		public IDDictionary<QuestData> Quests { get; private set; }
		

		public QuestManager() {
			Quests = new IDDictionary<QuestData>();
		}
		
		//todo: 5時になった時点でデイリー及びウィークリーをリセットするように！
		
		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			if ( !( RawData.api_list is double ) ) {	//任務完遂時 -1 になる

				foreach ( dynamic elem in RawData.api_list ) {

					if ( !( elem is double ) ) {		//空欄は -1 になるため。

						int id = (int)elem.api_no;
						if ( !Quests.ContainsKey( id ) ) {
							var q = new QuestData();
							q.LoadFromResponse( apiname, elem );
							Quests.Add( q );

						} else {
							Quests[id].LoadFromResponse( apiname, elem );
						}

					}
				}

			}

		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			//api_req_quest/clearitemget

			int id = int.Parse( RequestData["api_quest_id"] );
			Quests.Remove( id );

		}

	}

}
