using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ElectronicObserver.Data.Quest
{

	/// <summary>
	/// 任務の進捗を管理します。
	/// </summary>
	[DataContract(Name = "QuestProgress")]
	[KnownType(typeof(ProgressData))]
	[KnownType(typeof(ProgressAGo))]
	[KnownType(typeof(ProgressBattle))]
	[KnownType(typeof(ProgressMultiBattle))]
	[KnownType(typeof(ProgressSpecialBattle))]
	[KnownType(typeof(ProgressConstruction))]
	[KnownType(typeof(ProgressDestruction))]
	[KnownType(typeof(ProgressDevelopment))]
	[KnownType(typeof(ProgressDiscard))]
	[KnownType(typeof(ProgressMultiDiscard))]
	[KnownType(typeof(ProgressDocking))]
	[KnownType(typeof(ProgressExpedition))]
	[KnownType(typeof(ProgressMultiExpedition))]
	[KnownType(typeof(ProgressImprovement))]
	[KnownType(typeof(ProgressModernization))]
	[KnownType(typeof(ProgressPractice))]
	[KnownType(typeof(ProgressSlaughter))]
	[KnownType(typeof(ProgressSupply))]
	public sealed class QuestProgressManager : DataStorage
	{


		public const string DefaultFilePath = @"Settings\QuestProgress.xml";


		[IgnoreDataMember]
		public IDDictionary<ProgressData> Progresses { get; private set; }

		[DataMember]
		private List<ProgressData> SerializedProgresses
		{
			get
			{
				return Progresses.Values.ToList();
			}
			set
			{
				Progresses = new IDDictionary<ProgressData>(value);
			}
		}

		[DataMember]
		public DateTime LastUpdateTime { get; set; }

		/*
		[DataMember]
		private string LastUpdateTimeSerializer {
			get { return DateTimeHelper.TimeToCSVString( LastUpdateTime ); }
			set { LastUpdateTime = DateTimeHelper.CSVStringToTime( value ); }
		}
		*/

		[IgnoreDataMember]
		private DateTime _prevTime;


		public QuestProgressManager()
		{
			Initialize();
		}


		public override void Initialize()
		{
			Progresses = new IDDictionary<ProgressData>();
			LastUpdateTime = DateTime.Now;

			RemoveEvents();     //二重登録防止


			var ao = APIObserver.Instance;

			ao.APIList["api_get_member/questlist"].ResponseReceived += QuestUpdated;

			ao.APIList["api_req_map/start"].ResponseReceived += StartSortie;

			ao.APIList["api_req_map/next"].ResponseReceived += NextSortie;

			ao.APIList["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
			ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

			ao.APIList["api_req_practice/battle_result"].ResponseReceived += PracticeFinished;

			ao.APIList["api_req_mission/result"].ResponseReceived += ExpeditionCompleted;

			ao.APIList["api_req_nyukyo/start"].RequestReceived += StartRepair;

			ao.APIList["api_req_hokyu/charge"].ResponseReceived += Supplied;

			ao.APIList["api_req_kousyou/createitem"].ResponseReceived += EquipmentDeveloped;

			ao.APIList["api_req_kousyou/createship"].RequestReceived += ShipConstructed;

			ao.APIList["api_req_kousyou/destroyship"].RequestReceived += ShipDestructed;

			// 装備廃棄はイベント前に装備データが削除されてしまうので destroyitem2 から直接呼ばれる

			ao.APIList["api_req_kousyou/remodel_slot"].ResponseReceived += EquipmentRemodeled;

			ao.APIList["api_req_kaisou/powerup"].ResponseReceived += Modernized;

			ao.APIList["api_port/port"].ResponseReceived += TimerSave;


			_prevTime = DateTime.Now;
		}

		public void RemoveEvents()
		{

			var ao = APIObserver.Instance;

			ao.APIList["api_get_member/questlist"].ResponseReceived -= QuestUpdated;

			ao.APIList["api_req_map/start"].ResponseReceived -= StartSortie;

			ao.APIList["api_req_map/next"].ResponseReceived -= NextSortie;

			ao.APIList["api_req_sortie/battleresult"].ResponseReceived -= BattleFinished;
			ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived -= BattleFinished;

			ao.APIList["api_req_practice/battle_result"].ResponseReceived -= PracticeFinished;

			ao.APIList["api_req_mission/result"].ResponseReceived -= ExpeditionCompleted;

			ao.APIList["api_req_nyukyo/start"].RequestReceived -= StartRepair;

			ao.APIList["api_req_hokyu/charge"].ResponseReceived -= Supplied;

			ao.APIList["api_req_kousyou/createitem"].ResponseReceived -= EquipmentDeveloped;

			ao.APIList["api_req_kousyou/createship"].RequestReceived -= ShipConstructed;

			ao.APIList["api_req_kousyou/destroyship"].ResponseReceived -= ShipDestructed;

			// 装備廃棄は(ry

			ao.APIList["api_req_kousyou/remodel_slot"].ResponseReceived -= EquipmentRemodeled;

			ao.APIList["api_req_kaisou/powerup"].ResponseReceived -= Modernized;

			ao.APIList["api_port/port"].ResponseReceived -= TimerSave;

		}

		public ProgressData this[int key] => Progresses[key];



		void TimerSave(string apiname, dynamic data)
		{

			bool iscleared;

			switch (Utility.Configuration.Config.FormQuest.ProgressAutoSaving)
			{
				case 0:
				default:
					iscleared = false;
					break;
				case 1:
					iscleared = DateTimeHelper.IsCrossedHour(_prevTime);
					break;
				case 2:
					iscleared = DateTimeHelper.IsCrossedDay(_prevTime, 0, 0, 0);
					break;
			}


			if (iscleared)
			{
				_prevTime = DateTime.Now;

				Save();
				Utility.Logger.Add(1, "任務進捗のオートセーブを行いました。");
			}

		}


		void QuestUpdated(string apiname, dynamic data)
		{


			var quests = KCDatabase.Instance.Quest;

			//消えている・達成済みの任務の進捗情報を削除
			if (quests.IsLoadCompleted)
				Progresses.RemoveAll(q => !quests.Quests.ContainsKey(q.QuestID) || quests[q.QuestID].State == 3);


			foreach (var q in quests.Quests.Values)
			{

				//達成済みはスキップ
				if (q.State == 3) continue;

				// 進捗情報の生成
				if (!Progresses.ContainsKey(q.QuestID))
				{

					#region 地 獄 の 任 務 I D べ た 書 き 祭 り

					switch (q.QuestID)
					{

						case 201:   //|201|敵艦隊を撃破せよ！|勝利1
							Progresses.Add(new ProgressBattle(q, 1, "B", null, false));
							break;
						case 216:   //|216|敵艦隊主力を撃滅せよ！|戦闘1
							Progresses.Add(new ProgressBattle(q, 1, "E", null, false));
							break;
						case 210:   //|210|敵艦隊を10回邀撃せよ！|戦闘10
							Progresses.Add(new ProgressBattle(q, 10, "E", null, false));
							break;
						case 211:   //|211|敵空母を3隻撃沈せよ！|空母3
							Progresses.Add(new ProgressSlaughter(q, 3, new[] { 7, 11 }));
							break;
						case 212:   //|212|敵輸送船団を叩け！|輸送5
							Progresses.Add(new ProgressSlaughter(q, 5, new[] { 15 }));
							break;
						case 218:   //|218|敵補給艦を3隻撃沈せよ！|輸送3
							Progresses.Add(new ProgressSlaughter(q, 3, new[] { 15 }));
							break;
						case 226:   //|226|南西諸島海域の制海権を握れ！|2-(1~5)ボス勝利5
							Progresses.Add(new ProgressBattle(q, 5, "B", new[] { 21, 22, 23, 24, 25 }, true));
							break;
						case 230:   //|230|敵潜水艦を制圧せよ！|潜水6
							Progresses.Add(new ProgressSlaughter(q, 6, new[] { 13 }));
							break;

						case 213:   //|213|海上通商破壊作戦|輸送20
							Progresses.Add(new ProgressSlaughter(q, 20, new[] { 15 }));
							break;
						case 214:   //|214|あ号作戦|出撃36/S勝利6/ボス24/ボス勝利12
							Progresses.Add(new ProgressAGo(q));
							break;
						case 220:   //|220|い号作戦|空母20
							Progresses.Add(new ProgressSlaughter(q, 20, new[] { 7, 11 }));
							break;
						case 221:   //|221|ろ号作戦|輸送50
							Progresses.Add(new ProgressSlaughter(q, 50, new[] { 15 }));
							break;
						case 228:   //|228|海上護衛戦|潜水15
							Progresses.Add(new ProgressSlaughter(q, 15, new[] { 13 }));
							break;
						case 229:   //|229|敵東方艦隊を撃滅せよ！|4-(1~5)ボス勝利12
							Progresses.Add(new ProgressBattle(q, 12, "B", new[] { 41, 42, 43, 44, 45 }, true));
							break;
						case 242:   //|242|敵東方中枢艦隊を撃破せよ！|4-4ボス勝利1
							Progresses.Add(new ProgressBattle(q, 1, "B", new[] { 44 }, true));
							break;
						case 243:   //|243|南方海域珊瑚諸島沖の制空権を握れ！|5-2ボスS勝利2
							Progresses.Add(new ProgressBattle(q, 2, "S", new[] { 52 }, true));
							break;
						case 261:   //|261|海上輸送路の安全確保に努めよ！|1-5ボスA勝利3
							Progresses.Add(new ProgressBattle(q, 3, "A", new[] { 15 }, true));
							break;
						case 241:   //|241|敵北方艦隊主力を撃滅せよ！|3-(3~5)ボス勝利5
							Progresses.Add(new ProgressBattle(q, 5, "B", new[] { 33, 34, 35 }, true));
							break;

						case 249:   //|249|月|「第五戦隊」出撃せよ！|2-5ボスS勝利1|要「那智」「妙高」「羽黒」
							Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true));
							break;
						case 256:   //|256|「潜水艦隊」出撃せよ！|6-1ボスS勝利3
							Progresses.Add(new ProgressBattle(q, 3, "S", new[] { 61 }, true));
							break;
						case 257:   //|257|月|「水雷戦隊」南西へ！|1-4ボスS勝利1|要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止
							Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 14 }, true));
							break;
						case 259:   //|259|月|「水上打撃部隊」南方へ！|5-1ボスS勝利1|要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止
							Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 51 }, true));
							break;
						case 264:   //|264|月|「空母機動部隊」西へ！|4-2ボスS勝利1|要(空母or軽母or装母)2/駆逐2
							Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 42 }, true));
							break;
						case 266:   //|266|月|「水上反撃部隊」突入せよ！|2-5ボスS勝利1|要駆逐旗艦、重巡1軽巡1駆逐4
							Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true));
							break;
						case 280:   //|280|月|兵站線確保！海上警備を強化実施せよ！|1-2・1-3・1-4・2-1ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							}));
							break;

						case 265:   //|265|海上護衛強化月間|1-5ボスA勝利10
							Progresses.Add(new ProgressBattle(q, 10, "A", new[] { 15 }, true));
							break;

						case 822:   //|822|季|沖ノ島海域迎撃戦|2-4ボスS勝利2
							Progresses.Add(new ProgressBattle(q, 2, "S", new[] { 24 }, true));
							break;
						case 854:   //|854|季|戦果拡張任務！「Z作戦」前段作戦|2-4・6-1・6-3ボスA勝利各1/6-4ボスS勝利1
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressBattle(q, 1, "A", new[]{ 24 }, true),
								new ProgressBattle(q, 1, "A", new[]{ 61 }, true),
								new ProgressBattle(q, 1, "A", new[]{ 63 }, true),
								new ProgressBattle(q, 1, "S", new[]{ 64 }, true),
							}));
							break;
						case 861:   //|861|季|強行輸送艦隊、抜錨！|1-6終点到達2|要(航空戦艦or補給艦)2
							Progresses.Add(new ProgressSpecialBattle(q, 2, "x", new[] { 16 }, true));
							break;
						case 862:   //|862|季|前線の航空偵察を実施せよ！|6-3ボスA勝利2|要水母1軽巡2
							Progresses.Add(new ProgressSpecialBattle(q, 2, "A", new[] { 63 }, true));
							break;
						case 872:   //|872|季|戦果拡張任務！「Z作戦」後段作戦|5-5・6-2・6-5・7-2(第二)ボスS勝利各1|要第一艦隊？
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressBattle(q, 1, "S", new[]{ 55 }, true),
								new ProgressBattle(q, 1, "S", new[]{ 62 }, true),
								new ProgressBattle(q, 1, "S", new[]{ 65 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 72 }, true, 2),
							}));
							break;
						case 873:   //|873|季|北方海域警備を実施せよ！|3-1・3-2・3-3ボスA勝利各1|要軽巡1, 1エリア達成で50%,2エリアで80%
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressSpecialBattle(q, 1, "A", new[]{ 31 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[]{ 32 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[]{ 33 }, true),
							}));
							break;
						case 875:   //|875|季|精鋭「三一駆」、鉄底海域に突入せよ！|5-4ボスS勝利2|要長波改二/(高波改or沖波改or朝霜改)
							Progresses.Add(new ProgressSpecialBattle(q, 2, "S", new[] { 54 }, true));
							break;
						case 888:   //|888|季|新編成「三川艦隊」、鉄底海峡に突入せよ！|5-1・5-3・5-4ボスS勝利各1|要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressSpecialBattle(q, 1, "S", new[]{ 51 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 53 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 54 }, true),
							}));
							break;
						case 893:   //|893|季|泊地周辺海域の安全確保を徹底せよ！|1-5・7-1・7-2(第一＆第二)ボスS勝利各3|3エリア達成時点で80%
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressBattle(q, 3, "S", new[]{ 15 }, true),
								new ProgressBattle(q, 3, "S", new[]{ 71 }, true),
								new ProgressSpecialBattle(q, 3, "S", new[]{ 72 }, true, 1),
								new ProgressSpecialBattle(q, 3, "S", new[]{ 72 }, true, 2),
							})); break;
						case 894:   //|894|季|空母戦力の投入による兵站線戦闘哨戒|1-3・1-4・2-1・2-2・2-3ボスS勝利各1?|要空母系
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							}));
							break;
						case 284:   //|284|季|南西諸島方面「海上警備行動」発令！|1-4・2-1・2-2・2-3ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
							Progresses.Add(new ProgressMultiBattle(q, new[]{
								new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							}));
							break;
						case 845:   //|845|季|発令！「西方海域作戦」|4-1・4-2・4-3・4-4・4-5ボスS勝利各1
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressBattle(q, 1, "S", new[] { 41 }, true),
								new ProgressBattle(q, 1, "S", new[] { 42 }, true),
								new ProgressBattle(q, 1, "S", new[] { 43 }, true),
								new ProgressBattle(q, 1, "S", new[] { 44 }, true),
								new ProgressBattle(q, 1, "S", new[] { 45 }, true),
							}));
							break;
						case 903:   //|903|季|拡張「六水戦」、最前線へ！|5-1・5-4・6-4・6-5ボスS勝利各1|要旗艦夕張改二(|特|丁), 由良改二or(睦月/如月/弥生/卯月/菊月/望月2)|進捗3/4で80%
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "S", new[] { 51 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 54 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 64 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 65 }, true),
							}));
							break;
						
						case 904:   //|904|年(2月)|精鋭「十九駆」、躍り出る！|2-5・3-4・4-5・5-3ボスS勝利各1|要綾波改二/敷波改二
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 34 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 45 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 53 }, true),
							}));
							break;
						case 905:   //|905|年(2月)|「海防艦」、海を護る！|1-1・1-2・1-3・1-5ボスA勝利各1/1-6終点到達1|要海防艦3, 5隻以下の編成
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 11 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 12 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 15 }, true),
								new ProgressSpecialBattle(q, 1, "x", new[] { 16 }, true),
							}));
							break;
						case 912:   //|912|年(3月)|工作艦「明石」護衛任務|1-3・2-1・2-2・2-3ボスA勝利各1/1-6終点到達1|要明石旗艦, 駆逐艦3
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 21 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 22 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 23 }, true),
								new ProgressSpecialBattle(q, 1, "x", new[] { 16 }, true),
							}));
							break;
						case 914:   //|914|３|重巡戦隊、西へ！|4-1・4-2・4-3・4-4ボスA勝利各1|要重巡3/駆逐1
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 41 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 42 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 43 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 44 }, true),
							}));
							break;
						case 928:   //|928|９|歴戦「第十方面艦隊」、全力出撃！|4-2・7-2(第二)・7-3(第二)ボスS勝利各2|要(羽黒/足柄/妙高/高雄/神風)2
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 2, "S", new[] { 42 }, true),
								new ProgressSpecialBattle(q, 2, "S", new[] { 72 }, true, 2),
								new ProgressSpecialBattle(q, 2, "S", new[] { 73 }, true, 2),
							}));
							break;

						case 303:   //|303|「演習」で練度向上！|演習3
							Progresses.Add(new ProgressPractice(q, 3, false));
							break;
						case 304:   //|304|「演習」で他提督を圧倒せよ！|演習勝利5
							Progresses.Add(new ProgressPractice(q, 5, true));
							break;
						case 302:   //|302|大規模演習|演習勝利20
							Progresses.Add(new ProgressPractice(q, 20, true));
							break;
						case 311:   //|311|精鋭艦隊演習|演習勝利7|マンスリーだが1日で進捗リセット
							Progresses.Add(new ProgressPractice(q, 7, true));
							break;

						case 402:   //|402|「遠征」を3回成功させよう！|遠征成功3
							Progresses.Add(new ProgressExpedition(q, 3, null));
							break;
						case 403:   //|403|「遠征」を10回成功させよう！|遠征成功10
							Progresses.Add(new ProgressExpedition(q, 10, null));
							break;
						case 404:   //|404|大規模遠征作戦、発令！|遠征成功30
							Progresses.Add(new ProgressExpedition(q, 30, null));
							break;
						case 410:   //|410|南方への輸送作戦を成功させよ！|「東京急行」「東京急行(弐)」成功1
							Progresses.Add(new ProgressExpedition(q, 1, new[] { 37, 38 }));
							break;
						case 411:   //|411|南方への鼠輸送を継続実施せよ！|「東京急行」「東京急行(弐)」成功6
							Progresses.Add(new ProgressExpedition(q, 6, new[] { 37, 38 }));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 424:   //|424|月|輸送船団護衛を強化せよ！|「海上護衛任務」成功4
							Progresses.Add(new ProgressExpedition(q, 4, new[] { 5 }));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 426:   //|426|季|海上通商航路の警戒を厳とせよ！|「警備任務」「対潜警戒任務」「海上護衛任務」「強行偵察任務」成功各1|3エリア達成時点で80%				 
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 3 }),
								new ProgressExpedition(q, 1, new[]{ 4 }),
								new ProgressExpedition(q, 1, new[]{ 5 }),
								new ProgressExpedition(q, 1, new[]{ 10 }),
							}));
							break;
						case 428:   //|428|季|近海に侵入する敵潜を制圧せよ！|「対潜警戒任務」「海峡警備行動」「長時間対潜警戒」成功各2|1エリア達成ごとに進捗が進む
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 2, new[]{ 4 }),
								new ProgressExpedition(q, 2, new[]{ 101 }),
								new ProgressExpedition(q, 2, new[]{ 102 }),
							}));
							break;
						case 434:   //|434|年(2月)|特設護衛船団司令部、活動開始！|「警備任務」「海上護衛任務」「兵站強化任務」「海峡警備行動」「タンカー護衛任務」成功各1|
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 3 }),
								new ProgressExpedition(q, 1, new[]{ 5 }),
								new ProgressExpedition(q, 1, new[]{ 100 }),
								new ProgressExpedition(q, 1, new[]{ 101 }),
								new ProgressExpedition(q, 1, new[]{ 9 }),
							}));
							break;
						case 436:   //|436|年(3月)|練習航海及び警備任務を実施せよ！|「練習航海」「長距離練習航海」「警備任務」「対潜警戒任務」「強行偵察任務」成功各1|
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 1 }),
								new ProgressExpedition(q, 1, new[]{ 2 }),
								new ProgressExpedition(q, 1, new[]{ 3 }),
								new ProgressExpedition(q, 1, new[]{ 4 }),
								new ProgressExpedition(q, 1, new[]{ 10 }),
							})); break;
						case 437:   //|437|年(5月)|小笠原沖哨戒線の強化を実施せよ！|「対潜警戒任務」「小笠原沖哨戒線遠征」「小笠原沖戦闘哨戒」「南西方面航空偵察作戦」成功各1?|
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 4 }),
								new ProgressExpedition(q, 1, new[]{ 104 }),
								new ProgressExpedition(q, 1, new[]{ 105 }),
								new ProgressExpedition(q, 1, new[]{ 110 }),
							})); break;
						case 438:   //|438|年(8月)|南西諸島方面の海上護衛を強化せよ！|「対潜警戒任務」「兵站強化任務」「タンカー護衛任務」「南西諸島捜索撃滅戦」成功各1|
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 4 }),
								new ProgressExpedition(q, 1, new[]{ 100 }),
								new ProgressExpedition(q, 1, new[]{ 9 }),
								new ProgressExpedition(q, 1, new[]{ 114 }),
							})); break;
						case 439:   //|439|年(9月)|兵站強化遠征任務【基本作戦】|「海上護衛任務」「兵站強化任務」「ボーキサイト輸送任務」「南西方面航空偵察作戦」成功各1
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 5 }),
								new ProgressExpedition(q, 1, new[]{ 100 }),
								new ProgressExpedition(q, 1, new[]{ 11 }),
								new ProgressExpedition(q, 1, new[]{ 110 }),
							})); break;
						case 440:   //|440|９|兵站強化遠征任務【拡張作戦】|「ブルネイ泊地沖哨戒」「海上護衛任務」「水上機前線輸送」「強行鼠輸送作戦」「南西海域戦闘哨戒」成功各1
							Progresses.Add(new ProgressMultiExpedition(q, new[]{
								new ProgressExpedition(q, 1, new[]{ 41 }),
								new ProgressExpedition(q, 1, new[]{ 5 }),
								new ProgressExpedition(q, 1, new[]{ 40 }),
								new ProgressExpedition(q, 1, new[]{ 142 }),
								new ProgressExpedition(q, 1, new[]{ 46 }),
							})); break;

						case 503:   //|503|艦隊大整備！|入渠5
							Progresses.Add(new ProgressDocking(q, 5));
							break;
						case 504:   //|504|艦隊酒保祭り！|補給15回
							Progresses.Add(new ProgressSupply(q, 15));
							break;

						case 605:   //|605|新装備「開発」指令|開発1
							Progresses.Add(new ProgressDevelopment(q, 1));
							break;
						case 606:   //|606|新造艦「建造」指令|建造1
							Progresses.Add(new ProgressConstruction(q, 1));
							break;
						case 607:   //|607|装備「開発」集中強化！|開発3
							Progresses.Add(new ProgressDevelopment(q, 3));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 608:   //|608|艦娘「建造」艦隊強化！|建造3
							Progresses.Add(new ProgressConstruction(q, 3));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 609:   //|609|軍縮条約対応！|解体2
							Progresses.Add(new ProgressDestruction(q, 2));
							break;
						case 619:   //|619|装備の改修強化|装備改修1(失敗可)
							Progresses.Add(new ProgressImprovement(q, 1));
							break;
						case 673:   //|673|装備開発力の整備|小口径主砲廃棄4個|進捗は1/5から始まる(3個廃棄時点で80%達成になる)
							Progresses.Add(new ProgressDiscard(q, 4, true, new[] { 1 }));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 674:   //|674|工廠環境の整備|機銃廃棄3個,鋼材300保有|進捗は2/5から始まる(2個廃棄時点で80%達成になる)
							Progresses.Add(new ProgressDiscard(q, 3, true, new[] { 21 }));
							Progresses[q.QuestID].SharedCounterShift = 2;
							break;
						case 613:   //|613|資源の再利用|廃棄24回
							Progresses.Add(new ProgressDiscard(q, 24, false, null));
							break;
						case 638:   //|638|対空機銃量産|機銃廃棄6個|回ではない
							Progresses.Add(new ProgressDiscard(q, 6, true, new[] { 21 }));
							break;
						case 676:   //|676|週|装備開発力の集中整備|(中口径主砲x3, 副砲x3, 簡易輸送部材x1)廃棄, 鋼材2400保有|進捗は n/7 で1つごとに進む
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 3, true, new[]{ 2 }),
								new ProgressDiscard(q, 3, true, new[]{ 4 }),
								new ProgressDiscard(q, 1, true, new[]{ 30 }),
								}));
							break;
						case 677:   //|677|週|継戦支援能力の整備|(大口径主砲x4, 水上偵察機x2, 魚雷x3)廃棄, 鋼材3600保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 4, true, new[]{ 3 }),
								new ProgressDiscard(q, 2, true, new[]{ 10 }),
								new ProgressDiscard(q, 3, true, new[]{ 5 }),
								}));
							break;
						case 626:   //|626|月|精鋭「艦戦」隊の新編成|熟練搭乗員, 零式艦戦21型>>装備の鳳翔旗艦, (零式艦戦21型x2,九六式艦戦x1)廃棄
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 2, true, new[]{ 20 }, -1),
								new ProgressDiscard(q, 1, true, new[]{ 19 }, -1),
							}));
							break;
						case 628:   //|628|月|機種転換|零式艦戦21型(熟練)>>装備の空母旗艦, 零式艦戦52型x2廃棄
							Progresses.Add(new ProgressDiscard(q, 2, true, new[] { 21 }, -1));
							break;
						case 645:   //|645|月|「洋上補給」物資の調達|三式弾廃棄, (燃料750, 弾薬750, ドラム缶(輸送用)x2, 九一式徹甲弾)保有
							Progresses.Add(new ProgressDiscard(q, 1, true, new[] { 18 }));
							break;
						case 643:   //|643|季|主力「陸攻」の調達|零式艦戦21型x2廃棄, (九六式陸攻x1, 九七式艦攻x2)保有
							Progresses.Add(new ProgressDiscard(q, 2, true, new[] { 20 }, -1));
							break;
						case 653:   //|653|季|工廠稼働！次期作戦準備！|14cm単装砲x6廃棄, (家具コイン6000, 35.6cm連装砲x3, 九六式艦戦x3)保有
							Progresses.Add(new ProgressDiscard(q, 6, true, new[] { 4 }, -1));
							break;
						case 663:   //|663|季|新型艤装の継続研究|大口径主砲x10廃棄, 鋼材18000保有
							Progresses.Add(new ProgressDiscard(q, 10, true, new[] { 3 }));
							break;
						case 675:   //|675|季|運用装備の統合整備|(艦上戦闘機x6, 機銃x4)廃棄, ボーキ800保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 6, true, new[]{ 6 }),
								new ProgressDiscard(q, 4, true, new[]{ 21 }),
								}));
							break;
						case 678:   //|678|季|主力艦上戦闘機の更新|(九六式艦戦x3, 零式艦戦21型x5)廃棄, 秘書艦の第1・第2スロットに零式艦戦52型装備, ボーキ4000保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 3, true, new[]{ 19 }, -1),
								new ProgressDiscard(q, 5, true, new[]{ 20 }, -1),
							}));
							break;
						case 680:   //|680|季|対空兵装の整備拡充|(対空機銃x4, (小型電探or大型電探)x4)廃棄, ボーキ1500保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 4, true, new[]{ 21 }),
								new ProgressDiscard(q, 4, true, new[]{ 12, 13 }),
							}));
							break;
						case 686:   //|686|季|戦時改修A型高角砲の量産|12.7cm連装砲A型改二★10を第一スロ装備の特型駆逐艦旗艦, (10cm連装高角砲x4, 94式高射装置x1)廃棄, (開発資材30, 鋼材900, 新型砲熕兵装資材1)保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 4, true, new[]{ 3 }, -1),
								new ProgressDiscard(q, 1, true, new[]{ 121 }, -1),
							}));
							break;
						case 688:   //|688|季|航空戦力の強化|(艦上戦闘機x3, 艦上爆撃機x3, 艦上攻撃機x3, 水上偵察機x3)廃棄, (熟練搭乗員x1, ボーキサイトx1800)保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 3, true, new[]{ 6 }),
								new ProgressDiscard(q, 3, true, new[]{ 7 }),
								new ProgressDiscard(q, 3, true, new[]{ 8 }),
								new ProgressDiscard(q, 3, true, new[]{ 10 }),
							}));
							break;
						case 657:   //|657|年(9月)|新型兵装開発整備の強化|(小口径主砲x6, 中口径主砲x5, 魚雷x4)廃棄, 鋼材4000保有|
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 6, true, new[]{ 1 }),
								new ProgressDiscard(q, 5, true, new[]{ 2 }),
								new ProgressDiscard(q, 4, true, new[]{ 5 }),
							}));
							break;
						case 655:   //|655|11|工廠フル稼働！新兵装を開発せよ！|(小口径主砲x5, 中口径主砲x5, 大口径主砲x5, 水上偵察機x5, 艦上攻撃機x5)廃棄, (燃料x1500, 鋼材x1500, ボーキx1500)保有
							Progresses.Add(new ProgressMultiDiscard(q, new[]{
								new ProgressDiscard(q, 5, true, new[]{ 1 }),
								new ProgressDiscard(q, 5, true, new[]{ 2 }),
								new ProgressDiscard(q, 5, true, new[]{ 3 }),
								new ProgressDiscard(q, 5, true, new[]{ 8 }),
								new ProgressDiscard(q, 5, true, new[]{ 10 }),
							}));
							break;

						case 702:   //|702|艦の「近代化改修」を実施せよ！|改修成功2
							Progresses.Add(new ProgressModernization(q, 2));
							break;
						case 703:   //|703|「近代化改修」を進め、戦備を整えよ！|改修成功15
							Progresses.Add(new ProgressModernization(q, 15));
							break;

					}

					#endregion

				}

				// 進捗度にずれがあった場合補正する
				var p = Progresses[q.QuestID];
				if (p != null)
					p.CheckProgress(q);

			}

			LastUpdateTime = DateTime.Now;
			OnProgressChanged();

		}


		void BattleFinished(string apiname, dynamic data)
		{

			var bm = KCDatabase.Instance.Battle;
			var battle = bm.SecondBattle ?? bm.FirstBattle;

			var hps = battle.ResultHPs;
			if (hps == null)
				return;


			#region Slaughter

			var slaughterList = Progresses.Values.OfType<ProgressSlaughter>();

			for (int i = 0; i < 6; i++)
			{
				if (hps[Battle.BattleIndex.Get(Battle.BattleSides.EnemyMain, i)] <= 0)
				{
					var ship = battle.Initial.EnemyMembersInstance[i];
					if (ship == null)
						continue;

					foreach (var p in slaughterList)
						p.Increment(ship.ShipType);
				}

				if (bm.IsEnemyCombined && hps[Battle.BattleIndex.Get(Battle.BattleSides.EnemyEscort, i)] <= 0)
				{
					var ship = battle.Initial.EnemyMembersEscortInstance[i];
					if (ship == null)
						continue;

					foreach (var p in slaughterList)
						p.Increment(ship.ShipType);
				}
			}

			#endregion


			#region Battle

			foreach (var p in Progresses.Values.OfType<ProgressBattle>())
			{
				p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);
			}

			foreach (var p in Progresses.Values.OfType<ProgressMultiBattle>())
			{
				p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);
			}

			#endregion


			var pago = Progresses.Values.OfType<ProgressAGo>().FirstOrDefault();
			if (pago != null)
				pago.IncrementBattle(bm.Result.Rank, bm.Compass.EventID == 5);


			OnProgressChanged();
		}

		void PracticeFinished(string apiname, dynamic data)
		{

			foreach (var p in Progresses.Values.OfType<ProgressPractice>())
			{
				p.Increment(data.api_win_rank);
			}

			OnProgressChanged();
		}

		void ExpeditionCompleted(string apiname, dynamic data)
		{

			if ((int)data.api_clear_result == 0)
				return;     //遠征失敗

			FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.Members.Contains((int)data.api_ship_id[1]));

			int areaID = fleet.ExpeditionDestination;

			foreach (var p in Progresses.Values.OfType<ProgressExpedition>())
			{
				p.Increment(areaID);
			}
			foreach (var p in Progresses.Values.OfType<ProgressMultiExpedition>())
			{
				p.Increment(areaID);
			}

			OnProgressChanged();
		}


		void StartRepair(string apiname, dynamic data)
		{

			foreach (var p in Progresses.Values.OfType<ProgressDocking>())
			{
				p.Increment();
			}

			OnProgressChanged();
		}

		void Supplied(string apiname, dynamic data)
		{

			foreach (var p in Progresses.Values.OfType<ProgressSupply>())
			{
				p.Increment();
			}

			OnProgressChanged();
		}

		void EquipmentRemodeled(string apiname, dynamic data)
		{

			foreach (var p in Progresses.Values.OfType<ProgressImprovement>())
			{
				p.Increment();
			}

			OnProgressChanged();
		}



		void Modernized(string apiname, dynamic data)
		{

			if ((int)data.api_powerup_flag == 0) return;    //近代化改修失敗

			foreach (var p in Progresses.Values.OfType<ProgressModernization>())
			{
				p.Increment();
			}

			OnProgressChanged();
		}

		public void EquipmentDiscarded(string apiname, Dictionary<string, string> data)
		{

			var ids = data["api_slotitem_ids"].Split(",".ToCharArray()).Select(s => int.Parse(s));

			foreach (var p in Progresses.Values.OfType<ProgressDiscard>())
			{
				p.Increment(ids);
			}
			foreach (var p in Progresses.Values.OfType<ProgressMultiDiscard>())
			{
				p.Increment(ids);
			}

			OnProgressChanged();
		}

		void ShipDestructed(string apiname, dynamic data)
		{
			int amount = (data["api_ship_id"] as string).Split(",".ToCharArray()).Count();

			foreach (var p in Progresses.Values.OfType<ProgressDestruction>())
			{
				p.Increment(amount);
			}

			OnProgressChanged();
		}

		void ShipConstructed(string apiname, dynamic data)
		{
			foreach (var p in Progresses.Values.OfType<ProgressConstruction>())
			{
				p.Increment();
			}

			OnProgressChanged();
		}

		void EquipmentDeveloped(string apiname, dynamic data)
		{
			int trials = KCDatabase.Instance.Development.DevelopmentTrials;

			foreach (var p in Progresses.Values.OfType<ProgressDevelopment>())
			{
				for (int i = 0; i < trials; i++)
					p.Increment();
			}

			OnProgressChanged();
		}

		void StartSortie(string apiname, dynamic data)
		{
			foreach (var p in Progresses.Values.OfType<ProgressAGo>())
			{
				p.IncrementSortie();
			}

			OnProgressChanged();
		}

		private void NextSortie(string apiname, dynamic data)
		{
			var compass = KCDatabase.Instance.Battle.Compass;

			// 船団護衛成功イベント
			if (compass?.EventID == 8)
			{
				foreach (var p in Progresses.Values.OfType<ProgressBattle>())
				{
					p.Increment("x", compass.MapAreaID * 10 + compass.MapInfoID, compass.IsEndPoint);
				}

				foreach (var p in Progresses.Values.OfType<ProgressMultiBattle>())
				{
					p.Increment("x", compass.MapAreaID * 10 + compass.MapInfoID, compass.IsEndPoint);
				}

				OnProgressChanged();
			}
		}


		public void Clear()
		{
			Progresses.Clear();
			LastUpdateTime = DateTime.Now;
		}


		public QuestProgressManager Load()
		{
			return (QuestProgressManager)Load(DefaultFilePath);
		}

		public void Save()
		{
			Save(DefaultFilePath);
		}

		private void OnProgressChanged()
		{
			KCDatabase.Instance.Quest.OnQuestUpdated();
		}
	}

}
