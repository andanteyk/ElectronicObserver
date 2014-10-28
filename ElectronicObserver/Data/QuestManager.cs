using Codeplex.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 任務情報を統括して扱います。
	/// </summary>
	public class QuestManager : APIWrapper {

		public IDDictionary<QuestData> Quests { get; private set; }

		public int Count { get; internal set; }

		private DateTime _prevTime;


		public QuestManager() {
			Quests = new IDDictionary<QuestData>();
			_prevTime = DateTime.Now;
		}
		
		

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );


			//周期任務削除
			if ( DateConverter.IsCrossedDay( _prevTime, 5, 0, 0 ) ) {
				foreach ( var q in Quests ) {
					if ( q.Value.Type == 2 ) {
						Quests.Remove( q.Key );
					} 
				}
			}
			if ( DateConverter.IsCrossedWeek( _prevTime, DayOfWeek.Monday, 5, 0, 0 ) ) {
				foreach ( var q in Quests ) {
					if ( q.Value.Type == 3 ) {
						Quests.Remove( q.Key );
					}
				}
			}
			if ( DateConverter.IsCrossedMonth( _prevTime, 1, 5, 0, 0 ) ) {
				foreach ( var q in Quests ) {
					if ( q.Value.Type == 6 ) {
						Quests.Remove( q.Key );
					}
				}
			}


			Count = (int)RawData.api_count;

			if ( RawData.api_list != null ) {	//任務完遂時orページ遷移時 null になる

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


			_prevTime = DateTime.Now;

		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			//api_req_quest/clearitemget

			Quests.Remove( int.Parse( RequestData["api_quest_id"] ) );
			Count--;

		}

	}

}
