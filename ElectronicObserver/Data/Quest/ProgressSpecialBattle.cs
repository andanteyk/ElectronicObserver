using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest
{
	[DataContract(Name = "ProgressSpecialBattle")]
	public class ProgressSpecialBattle : ProgressBattle
	{
		/// <summary>
		/// 対象となるゲージ本数（-1は任意のゲージ）
		/// </summary>
		[DataMember]
		private int GaugeIndex = -1;

		public ProgressSpecialBattle(QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly) : base(quest, maxCount, lowestRank, targetArea, isBossOnly)
		{
		}

		public ProgressSpecialBattle(QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly, int gaugeIndex) : base(quest, maxCount, lowestRank, targetArea, isBossOnly)
		{
			GaugeIndex = gaugeIndex;
		}


		public override void Increment(string rank, int areaID, bool isBoss)
		{
			// 邪悪
			var Empty = (ShipTypes)(-1);

			// 邪悪
			var bm = KCDatabase.Instance.Battle;

			var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

			if (fleet == null)
			{
				// 出撃中ではない - たぶん UI 操作経由のコール?
				base.Increment(rank, areaID, isBoss);
				return;
			}

			var members = fleet.MembersWithoutEscaped;
			var memberstype = members.Select(s => s?.MasterShip?.ShipType ?? Empty).ToArray();

			bool isAccepted = false;


			switch (QuestID)
			{
				// |249|月|「第五戦隊」出撃せよ！|2-5ボスS勝利1|要「那智」「妙高」「羽黒」
				case 249:
					{
						bool nachi = false, myoukou = false, haguro = false;
						foreach (var ship in members)
						{
							switch (ship?.MasterShip?.NameReading)
							{
								case "なち":
									nachi = true;
									break;
								case "みょうこう":
									myoukou = true;
									break;
								case "はぐろ":
									haguro = true;
									break;
							}
						}
						isAccepted = nachi && myoukou && haguro;
					}
					break;

				// |257|月|「水雷戦隊」南西へ！|1-4ボスS勝利1|要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止
				case 257:
					isAccepted =
						memberstype[0] == ShipTypes.LightCruiser &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) <= 3 &&
						memberstype.All(t => t == ShipTypes.Destroyer || t == ShipTypes.LightCruiser || t == Empty);
					break;

				// |259|月|「水上打撃部隊」南方へ！|5-1ボスS勝利1|要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止
				case 259:
					{
						int battleships = 0;
						bool hasLightCruiser = false;
						foreach (var ship in members)
						{
							switch (ship?.MasterShip?.ShipType)
							{
								case ShipTypes.Battleship:
								case ShipTypes.AviationBattleship:
									switch (ship?.MasterShip?.ShipClass)
									{
										case 2:     // 伊勢型
										case 19:    // 長門型
										case 26:    // 扶桑型
										case 37:    // 大和型
											battleships++;
											break;
										default:
											battleships = -9999;
											break;
									}
									break;

								case ShipTypes.Battlecruiser:
									battleships = -9999;
									break;

								case ShipTypes.LightCruiser:
									hasLightCruiser = true;
									break;
							}
						}
						isAccepted = battleships == 3 && hasLightCruiser;
					}
					break;

				// |264|月|「空母機動部隊」西へ！|4-2ボスS勝利1|要(空母or軽母or装母)2/駆逐2
				case 264:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.AircraftCarrier || t == ShipTypes.LightAircraftCarrier || t == ShipTypes.ArmoredAircraftCarrier) >= 2 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 2;
					break;

				// |266|月|「水上反撃部隊」突入せよ！|2-5ボスS勝利1|要駆逐旗艦、重巡1軽巡1駆逐4
				case 266:
					isAccepted =
						memberstype[0] == ShipTypes.Destroyer &&
						memberstype.Count(t => t == ShipTypes.HeavyCruiser) == 1 &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) == 1 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) == 4;
					break;

				// |280|月|兵站線確保！海上警備を強化実施せよ！|1-2・1-3・1-4・2-1ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
				// |284|季|南西諸島方面「海上警備行動」発令！|1-4・2-1・2-2・2-3ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
				case 280:
				case 284:
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightAircraftCarrier || t == ShipTypes.LightCruiser || t == ShipTypes.TorpedoCruiser || t == ShipTypes.TrainingCruiser) &&
						memberstype.Count(t => t == ShipTypes.Destroyer || t == ShipTypes.Escort) >= 3;
					break;

				// |854|季|戦果拡張任務！「Z作戦」前段作戦|2-4・6-1・6-3ボスA勝利各1/6-4ボスS勝利1|要第一艦隊
				case 854:
					isAccepted =
						fleet.FleetID == 1;
					break;

				// |861|季|強行輸送艦隊、抜錨！|1-6終点到達2|要(航空戦艦or補給艦)2
				case 861:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.AviationBattleship || t == ShipTypes.FleetOiler) >= 2;
					break;

				// |862|季|前線の航空偵察を実施せよ！|6-3ボスA勝利2|要水母1軽巡2
				case 862:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.SeaplaneTender) >= 1 &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) >= 2;
					break;

				// |873|季|北方海域警備を実施せよ！|3-1・3-2・3-3ボスA勝利各1|要軽巡1, 1エリア達成で50%,2エリアで80%
				case 873:
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightCruiser);
					break;

				// |875|季|精鋭「三一駆」、鉄底海域に突入せよ！|5-4ボスS勝利2|要長波改二/(高波改or沖波改or朝霜改)
				case 875:
					isAccepted =
						members.Any(s => s?.ShipID == 543) &&
						members.Any(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "たかなみ":
								case "おきなみ":
								case "あさしも":
									return s.MasterShip.RemodelTier >= 1;
								default:
									return false;
							}
						});
					break;

				// |888|季|新編成「三川艦隊」、鉄底海峡に突入せよ！|5-1・5-3・5-4ボスS勝利各1|要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4
				case 888:
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "ちょうかい":
								case "あおば":
								case "きぬがさ":
								case "かこ":
								case "ふるたか":
								case "てんりゅう":
								case "ゆうばり":
									return true;
								default:
									return false;
							}
						}) >= 4;
					break;

				case 872:   // |872|季|戦果拡張任務！「Z作戦」後段作戦|5-5・6-2・6-5・7-2(第二)ボスS勝利各1|要第一艦隊
					isAccepted = fleet.FleetID == 1 && CheckGaugeIndex72(bm.Compass);
					break;

				case 893:   // |893|季|泊地周辺海域の安全確保を徹底せよ！|1-5・7-1・7-2(第一＆第二)ボスS勝利各3|3エリア達成時点で80%
					isAccepted = CheckGaugeIndex72(bm.Compass);
					break;

				// |894|季|空母戦力の投入による兵站線戦闘哨戒|1-3・1-4・2-1・2-2・2-3ボスS勝利各1?|要空母系
				case 894:
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightAircraftCarrier || t == ShipTypes.AircraftCarrier || t == ShipTypes.ArmoredAircraftCarrier);
					break;

				case 903:   // |903|季|拡張「六水戦」、最前線へ！|5-1・5-4・6-4・6-5ボスS勝利各1|要旗艦夕張改二(|特|丁), 由良改二or(睦月/如月/弥生/卯月/菊月/望月2)|進捗3/4で80%
					isAccepted = members[0]?.MasterShip?.NameReading == "ゆうばり" && members[0]?.MasterShip?.RemodelTier >= 2 &&
						(members.Any(s => s?.ShipID == 488) || members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "むつき":
								case "きさらぎ":
								case "やよい":
								case "もちづき":
								case "きくづき":
								case "うづき":
									return true;
								default:
									return false;
							}
						}) >= 2);
					break;

				case 904:   // |904|年(2月)|精鋭「十九駆」、躍り出る！|2-5・3-4・4-5・5-3ボスS勝利各1|要綾波改二/敷波改二
					isAccepted = members.Any(s => s?.ShipID == 195) && members.Any(s => s?.ShipID == 627);
					break;

				case 905:   // |905|年(2月)|「海防艦」、海を護る！|1-1・1-2・1-3・1-5ボスA勝利各1/1-6終点到達1|要海防艦3, 5隻以下の編成
					isAccepted = members.Count(s => s != null) <= 5 && memberstype.Count(t => t == ShipTypes.Escort) >= 3;
					break;

				case 912:   // |912|年(3月)|工作艦「明石」護衛任務|1-3・2-1・2-2・2-3ボスA勝利各1/1-6終点到達1|要明石旗艦, 駆逐艦3
					isAccepted = members.FirstOrDefault()?.MasterShip?.NameReading == "あかし" &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 3;
					break;

				case 914:   // |914|３|重巡戦隊、西へ！|4-1・4-2・4-3・4-4ボスA勝利各1|要重巡3/駆逐1
					isAccepted = memberstype.Count(t => t == ShipTypes.HeavyCruiser) >= 3 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 1;
					break;

				case 928:   //|928|９|歴戦「第十方面艦隊」、全力出撃！|4-2・7-2(第二)・7-3(第二)ボスS勝利各2|要(羽黒/足柄/妙高/高雄/神風)2
					isAccepted = members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "はぐろ":
								case "あしがら":
								case "みょうこう":
								case "たかお":
								case "かみかぜ":
									return true;
								default:
									return false;
							}
						}) >= 2 && CheckGaugeIndex72(bm.Compass) && CheckGaugeIndex73(bm.Compass);
					break;


				case 840:   //|840|週|【節分任務】令和三年節分作戦|2-(1~3)ボスA勝利各1|要(軽母or軽巡or雷巡or練巡)旗艦/(駆逐or海防)3, 期間限定(2021/01/13～????/??/??)
					isAccepted =
						new[] {
							ShipTypes.LightAircraftCarrier,
							ShipTypes.LightCruiser,
							ShipTypes.TorpedoCruiser,
							ShipTypes.TrainingCruiser }
						.Contains(memberstype.FirstOrDefault()) &&
						memberstype.Count(t => t == ShipTypes.Destroyer || t == ShipTypes.Escort) >= 3;
					break;

				case 841:   //|841|週|【節分任務】令和三年西方海域節分作戦|4-(1~3)ボスS勝利各1|要(水母2or航巡2or重巡2)旗艦, 期間限定(2021/01/13～????/??/??)
					isAccepted =
						new[] {
							ShipTypes.SeaplaneTender,
							ShipTypes.HeavyCruiser,
							ShipTypes.AviationCruiser
						}.Contains(memberstype.FirstOrDefault()) &&
						memberstype.Count(t => t == memberstype.FirstOrDefault()) >= 2;
					break;
				case 843:   //|843|週|【節分拡張任務】令和三年節分作戦、全力出撃！|5-2・5-5・6-4ボスS勝利各1|要(戦艦系or空母系)旗艦/駆逐2, 期間限定(2021/01/13～????/??/??)
					isAccepted =
						new[] {
							ShipTypes.Battlecruiser,
							ShipTypes.Battleship,
							ShipTypes.AviationBattleship,
							ShipTypes.LightAircraftCarrier,
							ShipTypes.AircraftCarrier,
							ShipTypes.ArmoredAircraftCarrier,
						}.Contains(memberstype.FirstOrDefault()) &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 2;
					break;
			}

			// 第二ゲージでも第一ボスに行ける場合があるので、個別対応が必要
			//if (GaugeIndex != -1)
			//	isAccepted &= bm.Compass.MapInfo.CurrentGaugeIndex == GaugeIndex;

			if (isAccepted)
				base.Increment(rank, areaID, isBoss);
		}



		private bool CheckGaugeIndex72(CompassData compass)
		{
			if (compass.MapAreaID == 7 && compass.MapInfoID == 2)
			{
				switch (compass.Destination)
				{
					case 7:
						return GaugeIndex == 1;
					case 15:
						return GaugeIndex == 2;
					default:
						return false;
				}
			}
			return true;
		}

		private bool CheckGaugeIndex73(CompassData compass)
		{
			if (compass.MapAreaID == 7 && compass.MapInfoID == 3)
			{
				switch (compass.Destination)
				{
					case 5:
					case 8:
						return GaugeIndex == 1;
					case 18:
					case 23:
					case 24:
					case 25:
						return GaugeIndex == 2;
					default:
						return false;
				}
			}
			return true;
		}


		public override string GetClearCondition()
		{
			var sb = new StringBuilder(base.GetClearCondition());

			if (GaugeIndex != -1)
				sb.AppendFormat("(第{0}ゲージ)", GaugeIndex);

			/*
			switch (QuestID)
			{
				// |249|月|「第五戦隊」出撃せよ！|2-5ボスS勝利1|要「那智」「妙高」「羽黒」
				case 249:
					sb.Append("【要「那智」「妙高」「羽黒」】");
					break;

				// |257|月|「水雷戦隊」南西へ！|1-4ボスS勝利1|要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止
				case 257:
					sb.Append("【要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止】");
					break;

				// |259|月|「水上打撃部隊」南方へ！|5-1ボスS勝利1|要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止
				case 259:
					sb.Append("【要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止】");
					break;

				// |264|月|「空母機動部隊」西へ！|4-2ボスS勝利1|要(空母or軽母or装母)2/駆逐2
				case 264:
					sb.Append("【要空母系2/駆逐2】");
					break;

				// |266|月|「水上反撃部隊」突入せよ！|2-5ボスS勝利1|要駆逐旗艦、重巡1軽巡1駆逐4
				case 266:
					sb.Append("【要駆逐旗艦、重巡1軽巡1駆逐4】");
					break;

				// |861|季|強行輸送艦隊、抜錨！|1-6終点到達2|要(航空戦艦or補給艦)2
				case 861:
					sb.Append("【要(航空戦艦or補給艦)2】");
					break;

				// |862|季|前線の航空偵察を実施せよ！|6-3ボスA勝利2|要水母1軽巡2
				case 862:
					sb.Append("【要水母1軽巡2】");
					break;

				// |873|季|北方海域警備を実施せよ！|3-1・3-2・3-3ボスA勝利各1|要軽巡1, 1エリア達成で50%,2エリアで80%
				case 873:
					sb.Append("【要軽巡1】");
					break;

				// |875|季|精鋭「三一駆」、鉄底海域に突入せよ！|5-4ボスS勝利2|要長波改二/(高波改or沖波改or朝霜改)
				case 875:
					sb.Append("【要長波改二/(高波改or沖波改or朝霜改)】");
					break;

				// |888|季|新編成「三川艦隊」、鉄底海峡に突入せよ！|5-1・5-3・5-4ボスS勝利各1|要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4
				case 888:
					sb.Append("【要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4】");
					break;

				// |893|季|泊地周辺海域の安全確保を徹底せよ！|1-5・7-1・7-2(第一＆第二)ボスS勝利各3|3エリア達成時点で80%
				case 893:
					sb.Append("【7-2: #1/#2ゲージ両方】");
					break;

				// |894|季|空母戦力の投入による兵站線戦闘哨戒|1-3・1-4・2-1・2-2・2-3ボスS勝利各1?|要空母系
				case 894:
					sb.Append("【要空母系】");
					break;
			}
			*/

			return sb.ToString();
		}
	}
}
