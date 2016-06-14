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

		/// <summary>
		/// 任務リスト
		/// </summary>
		public IDDictionary<QuestData> Quests { get; private set; }

		/// <summary>
		/// 任務数(未ロード含む)
		/// </summary>
		public int Count { get; internal set; }

		/// <summary>
		/// ロードしたかどうか(※全て読み込んでいるとは限らない)
		/// </summary>
		public bool IsLoaded { get; private set; }


		/// <summary>
		/// ロードが完了したかどうか
		/// </summary>
		public bool IsLoadCompleted {
			get { return IsLoaded && Quests.Count == Count; }
		}


		public event Action QuestUpdated = delegate { };



		public QuestManager() {
			Quests = new IDDictionary<QuestData>();
			IsLoaded = false;
		}


		public QuestData this[int key] {
			get { return Quests[key]; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			var progress = KCDatabase.Instance.QuestProgress;


			//周期任務削除
			if ( DateTimeHelper.IsCrossedDay( progress.LastUpdateTime, 5, 0, 0 ) ) {
				// 注: 311 = 精鋭艦隊演習; マンスリーだがデイリーで進捗リセット
				progress.Progresses.RemoveAll( p => ( p.QuestType == 1 || p.QuestType == 5 || p.QuestID == 311 ) );
				Quests.RemoveAll( q => q.Type == 1 || q.Type == 5 || q.QuestID == 311 );
			}
			if ( DateTimeHelper.IsCrossedWeek( progress.LastUpdateTime, DayOfWeek.Monday, 5, 0, 0 ) ) {
				progress.Progresses.RemoveAll( p => p.QuestType == 2 );
				Quests.RemoveAll( q => q.Type == 2 );
			}
			if ( DateTimeHelper.IsCrossedMonth( progress.LastUpdateTime, 1, 5, 0, 0 ) ) {
				progress.Progresses.RemoveAll( p => p.QuestType == 3 );
				Quests.RemoveAll( q => q.Type == 3 );
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


			IsLoaded = true;

		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			//api_req_quest/clearitemget

			Quests.Remove( int.Parse( RequestData["api_quest_id"] ) );
			Count--;

			QuestUpdated();
		}


		public void Clear() {
			Quests.Clear();
			IsLoaded = false;
		}


		// QuestProgressManager から呼ばれます
		internal void OnQuestUpdated() {
			QuestUpdated();
		}
	}

}
