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
	[KnownType(typeof(ProgressConstruction))]
	[KnownType(typeof(ProgressDestruction))]
	[KnownType(typeof(ProgressDevelopment))]
	[KnownType(typeof(ProgressDiscard))]
	[KnownType(typeof(ProgressDocking))]
	[KnownType(typeof(ProgressExpedition))]
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

			ao.APIList["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
			ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

			ao.APIList["api_req_practice/battle_result"].ResponseReceived += PracticeFinished;

			ao.APIList["api_req_mission/result"].ResponseReceived += ExpeditionCompleted;

			ao.APIList["api_req_nyukyo/start"].RequestReceived += StartRepair;

			ao.APIList["api_req_hokyu/charge"].ResponseReceived += Supplied;

			ao.APIList["api_req_kousyou/createitem"].ResponseReceived += EquipmentDeveloped;

			ao.APIList["api_req_kousyou/createship"].RequestReceived += ShipConstructed;

			ao.APIList["api_req_kousyou/destroyship"].ResponseReceived += ShipDestructed;

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
							Progresses.Add(new ProgressSlaughter(q, 3, new int[] { 7, 11 }));
							break;
						case 212:   //|212|敵輸送船団を叩け！|輸送5
							Progresses.Add(new ProgressSlaughter(q, 5, new int[] { 15 }));
							break;
						case 218:   //|218|敵補給艦を3隻撃沈せよ！|輸送3
							Progresses.Add(new ProgressSlaughter(q, 3, new int[] { 15 }));
							break;
						case 226:   //|226|南西諸島海域の制海権を握れ！|2-(1~5)ボス勝利5
							Progresses.Add(new ProgressBattle(q, 5, "B", new int[] { 21, 22, 23, 24, 25 }, true));
							break;
						case 230:   //|230|敵潜水艦を制圧せよ！|潜水6
							Progresses.Add(new ProgressSlaughter(q, 6, new int[] { 13 }));
							break;

						case 213:   //|213|海上通商破壊作戦|輸送20
							Progresses.Add(new ProgressSlaughter(q, 20, new int[] { 15 }));
							break;
						case 214:   //|214|あ号作戦|出撃36/S勝利6/ボス24/ボス勝利12
							Progresses.Add(new ProgressAGo(q));
							break;
						case 220:   //|220|い号作戦|空母20
							Progresses.Add(new ProgressSlaughter(q, 20, new int[] { 7, 11 }));
							break;
						case 221:   //|221|ろ号作戦|輸送50
							Progresses.Add(new ProgressSlaughter(q, 50, new int[] { 15 }));
							break;
						case 228:   //|228|海上護衛戦|潜水15
							Progresses.Add(new ProgressSlaughter(q, 15, new int[] { 13 }));
							break;
						case 229:   //|229|敵東方艦隊を撃滅せよ！|4-(1~5)ボス勝利12
							Progresses.Add(new ProgressBattle(q, 12, "B", new int[] { 41, 42, 43, 44, 45 }, true));
							break;
						case 242:   //|242|敵東方中枢艦隊を撃破せよ！|4-4ボス勝利1
							Progresses.Add(new ProgressBattle(q, 1, "B", new int[] { 44 }, true));
							break;
						case 243:   //|243|南方海域珊瑚諸島沖の制空権を握れ！|5-2ボスS勝利2
							Progresses.Add(new ProgressBattle(q, 2, "S", new int[] { 52 }, true));
							break;
						case 261:   //|261|海上輸送路の安全確保に努めよ！|1-5ボスA勝利3
							Progresses.Add(new ProgressBattle(q, 3, "A", new int[] { 15 }, true));
							break;
						case 241:   //|241|敵北方艦隊主力を撃滅せよ！|3-(3~5)ボス勝利5
							Progresses.Add(new ProgressBattle(q, 5, "B", new int[] { 33, 34, 35 }, true));
							break;

						case 256:   //|256|「潜水艦隊」出撃せよ！|6-1ボスS勝利3
							Progresses.Add(new ProgressBattle(q, 3, "S", new int[] { 61 }, true));
							break;
						case 265:   //|265|海上護衛強化月間|1-5ボスA勝利10
							Progresses.Add(new ProgressBattle(q, 10, "A", new int[] { 15 }, true));
							break;

						case 822:   //|822|季|沖ノ島海域迎撃戦|2-4ボスS勝利2
							Progresses.Add(new ProgressBattle(q, 2, "S", new int[] { 24 }, true));
							break;
						case 854:   //|854|季|戦果拡張任務！「Z作戦」前段作戦|2-4・6-1・6-3ボスA勝利各1/6-4ボスS勝利1
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressBattle( q, 1, "A", new int[]{ 24 }, true ),
								new ProgressBattle( q, 1, "A", new int[]{ 61 }, true ),
								new ProgressBattle( q, 1, "A", new int[]{ 63 }, true ),
								new ProgressBattle( q, 1, "S", new int[]{ 64 }, true ),
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
							Progresses.Add(new ProgressExpedition(q, 1, new int[] { 37, 38 }));
							break;
						case 411:   //|411|南方への鼠輸送を継続実施せよ！|「東京急行」「東京急行(弐)」成功6
							Progresses.Add(new ProgressExpedition(q, 6, new int[] { 37, 38 }));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;
						case 424:   //|424|月|輸送船団護衛を強化せよ！|「海上護衛任務」成功4
							Progresses.Add(new ProgressExpedition(q, 4, new int[] { 5 }));
							Progresses[q.QuestID].SharedCounterShift = 1;
							break;

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
						case 613:   //|613|資源の再利用|廃棄24回
							Progresses.Add(new ProgressDiscard(q, 24, false, null));
							break;
						case 638:   //|638|対空機銃量産|機銃廃棄6個|回ではない
							Progresses.Add(new ProgressDiscard(q, 6, true, new int[] { 21 }));
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

			OnProgressChanged();
		}

		void ShipDestructed(string apiname, dynamic data)
		{
			foreach (var p in Progresses.Values.OfType<ProgressDestruction>())
			{
				p.Increment();
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
			foreach (var p in Progresses.Values.OfType<ProgressDevelopment>())
			{
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
