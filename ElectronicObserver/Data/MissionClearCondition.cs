using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    /// <summary>
    /// 遠征可否判定を行います。
    /// </summary>
    public static class MissionClearCondition
    {

        /// <summary>
        /// 遠征に成功する編成かどうかを判定します。
        /// </summary>
        /// <param name="missionID">遠征ID。</param>
        /// <param name="fleet">対象となる艦隊。達成条件を確認したい場合は null を指定します。</param>
        public static MissionClearConditionResult Check(int missionID, FleetData fleet)
        {
            var result = new MissionClearConditionResult(fleet);

            switch (missionID)
            {
                case 1:     // 練習航海
                    return result
                        .CheckFlagshipLevel(1)
                        .CheckShipCount(2);
                case 2:     // 長距離練習航海
                    return result
                        .CheckFlagshipLevel(2)
                        .CheckShipCount(4);
                case 3:     // 警備任務
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckShipCount(3);
                case 4:     // 対潜警戒任務
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckEscortFleet();
                case 5:     // 海上護衛任務
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckShipCount(4)
                        .CheckEscortFleet();
                case 6:     // 防空射撃演習
                    return result
                        .CheckFlagshipLevel(5)
                        .CheckShipCount(4);
                case 7:     // 観艦式予行
                    return result
                        .CheckFlagshipLevel(5)
                        .CheckShipCount(6);
                case 8:     // 観艦式
                    return result
                        .CheckFlagshipLevel(6)
                        .CheckShipCount(6);
                case 100:   // 兵站強化任務
                    return result
                        .CheckFlagshipLevel(15)
                        .CheckShipCount(4)
                        .CheckSmallShipCount(3);
                case 101:   // 海峡警備行動
                    return result
                        .CheckFlagshipLevel(20)
                        .CheckLevelSum(144)
                        .CheckSmallShipCount(4)
                        .CheckAA(70)
                        .CheckASW(180)
                        .CheckLOS(73);
                case 102:   // 長時間対潜警戒
                    return result
                        .CheckFlagshipLevel(35)
                        .CheckLevelSum(185)
                        .CheckShipCount(5)
                        .CheckEscortFleet()
                        .CheckAA(162)
                        .CheckASW(280)
                        .CheckLOS(60);

                case 9:     // タンカー護衛任務
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckShipCount(4)
                        .CheckEscortFleet();
                case 10:    // 強行偵察任務
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckShipCount(3)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 2);
                case 11:    // ボーキサイト輸送任務
                    return result
                        .CheckFlagshipLevel(6)
                        .CheckShipCount(4)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 12:    // 資源輸送任務
                    return result
                        .CheckFlagshipLevel(4)
                        .CheckShipCount(4)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 13:    // 鼠輸送作戦
                    return result
                        .CheckFlagshipLevel(5)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 4);
                case 14:    // 包囲陸戦隊撤収作戦
                    return result
                        .CheckFlagshipLevel(6)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 3);
                case 15:    // 囮機動部隊支援作戦
                    return result
                        .CheckFlagshipLevel(8)
                        .CheckShipCount(6)
                        .CheckAircraftCarrierCount(2)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 16:    // 艦隊決戦援護作戦
                    return result
                        .CheckFlagshipLevel(10)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 110:   // 南西方面航空偵察作戦
                    return result
                        .CheckFlagshipLevel(40)
                        .CheckLevelSum(150)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.SeaplaneTender, 1)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2)
                        .CheckAA(200)
                        .CheckASW(200)
                        .CheckLOS(140);
                case 111:   // 敵泊地強襲反撃作戦
                    return result
                        .CheckFlagshipLevel(50)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 3)
                        .CheckFirepower(360);

                case 17:    // 敵地偵察作戦
                    return result
                        .CheckFlagshipLevel(20)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 3);
                case 18:    // 航空機輸送作戦
                    return result
                        .CheckFlagshipLevel(15)
                        .CheckShipCount(6)
                        .CheckAircraftCarrierCount(3)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 19:    // 北号作戦
                    return result
                        .CheckFlagshipLevel(20)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.AviationBattleship, 2)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 20:    // 潜水艦哨戒任務
                    return result
                        .CheckFlagshipLevel(1)
                        .CheckSubmarineCount(1)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1);
                case 21:    // 北方鼠輸送作戦
                    return result
                        .CheckFlagshipLevel(15)
                        .CheckLevelSum(30)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 4)
                        .CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3);
                case 22:    // 艦隊演習
                    return result
                        .CheckFlagshipLevel(30)
                        .CheckLevelSum(45)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 23:    // 航空戦艦運用演習
                    return result
                        .CheckFlagshipLevel(50)
                        .CheckLevelSum(200)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.AviationBattleship, 2)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 24:    // 北方航路海上護衛
                    return result
                        .CheckFlagshipLevel(50)
                        .CheckLevelSum(200)
                        .CheckShipCount(6)
                        .CheckFlagshipType(ShipTypes.LightCruiser)
                        .CheckShipCountByType(ShipTypes.Destroyer, 4);

                case 25:    // 通商破壊作戦
                    return result
                        .CheckFlagshipLevel(25)
                        .CheckShipCountByType(ShipTypes.HeavyCruiser, 2)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 26:    // 敵母港空襲作戦
                    return result
                        .CheckFlagshipLevel(30)
                        .CheckAircraftCarrierCount(1)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 27:    // 潜水艦通商破壊作戦
                    return result
                        .CheckFlagshipLevel(1)
                        .CheckSubmarineCount(2);
                case 28:    // 潜水艦通商破壊作戦
                    return result
                        .CheckFlagshipLevel(30)
                        .CheckSubmarineCount(3);
                case 29:    // 潜水艦派遣演習
                    return result
                        .CheckFlagshipLevel(50)
                        .CheckSubmarineCount(3);
                case 30:    // 潜水艦派遣作戦
                    return result
                        .CheckFlagshipLevel(55)
                        .CheckSubmarineCount(4);
                case 31:    // 海外艦との接触
                    return result
                        .CheckFlagshipLevel(60)
                        .CheckLevelSum(200)
                        .CheckSubmarineCount(4);
                case 32:    // 遠洋練習航海
                    return result
                        .CheckFlagshipLevel(5)
                        .CheckFlagshipType(ShipTypes.TrainingCruiser)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);

                case 33:    // 前衛支援任務
                    return result
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 34:    // 艦隊決戦支援任務
                    return result
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);
                case 35:    // MO作戦
                    return result
                        .CheckFlagshipLevel(40)
                        .CheckShipCount(6)
                        .CheckAircraftCarrierCount(2)
                        .CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 1);
                case 36:    // 水上機基地建設
                    return result
                        .CheckFlagshipLevel(30)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.SeaplaneTender, 2)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 1);
                case 37:    // 東京急行
                    return result
                        .CheckFlagshipLevel(50)
                        .CheckLevelSum(200)
                        .CheckShipCountByType(ShipTypes.LightCruiser, 1)
                        .CheckShipCountByType(ShipTypes.Destroyer, 5)
                        .CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3)
                        .CheckEquipmentCount(EquipmentTypes.TransportContainer, 4);
                case 38:    // 東京急行(弐)
                    return result
                        .CheckFlagshipLevel(65)
                        .CheckLevelSum(240)
                        .CheckShipCount(6)
                        .CheckShipCountByType(ShipTypes.Destroyer, 5)
                        .CheckEquippedShipCount(EquipmentTypes.TransportContainer, 4)
                        .CheckEquipmentCount(EquipmentTypes.TransportContainer, 8);
                case 39:    // 遠洋潜水艦作戦
                    return result
                        .CheckFlagshipLevel(3)
                        .CheckLevelSum(180)
                        .CheckShipCountByType(ShipTypes.SubmarineTender, 1)
                        .CheckSubmarineCount(4);
                case 40:    // 水上機前線輸送
                    return result
                        .CheckFlagshipLevel(25)
                        .CheckLevelSum(150)
                        .CheckShipCount(6)
                        .CheckFlagshipType(ShipTypes.LightCruiser)
                        .CheckShipCountByType(ShipTypes.SeaplaneTender, 2)
                        .CheckShipCountByType(ShipTypes.Destroyer, 2);

                default:
                    {
                        // イベント海域での支援遠征への対応
                        var mission = KCDatabase.Instance.Mission[missionID];

                        if (mission != null && (mission.Name == "前衛支援任務" || mission.Name == "艦隊決戦支援任務"))
                        {
                            return result
                                .CheckShipCountByType(ShipTypes.Destroyer, 2);
                        }

                        return result
                            .AddMessage($"未対応(ID:{missionID})");
                    }
            }
        }


        /// <summary>
        /// 遠征可否判定の結果を保持します。
        /// </summary>
        public class MissionClearConditionResult
        {

            /// <summary>
            /// 遠征が成功するかどうか
            /// </summary>
            public bool IsSuceeded { get; private set; }

            private List<string> failureReason;

            /// <summary>
            /// 遠征が失敗した理由 / 未対応遠征の場合のメッセージ
            /// </summary>
            public ReadOnlyCollection<string> FailureReason => failureReason.AsReadOnly();

            // nullable!
            private FleetData targetFleet;
            private IEnumerable<ShipData> members => (targetFleet?.MembersInstance ?? Enumerable.Empty<ShipData>()).Where(s => s != null);


            public MissionClearConditionResult(FleetData targetFleet)
            {
                this.targetFleet = targetFleet;
                IsSuceeded = true;
                failureReason = new List<string>();
            }


            private void Assert(bool condition, Func<string> failedMessage)
            {
                if (!condition)
                {
                    IsSuceeded = false;
                    failureReason.Add(failedMessage());
                }
            }

            private string CurrentValue(int value) => targetFleet != null ? (value.ToString() + "/") : "";

            public MissionClearConditionResult AddMessage(string message)
            {
                failureReason.Add(message);
                return this;
            }
            public MissionClearConditionResult Fail(string reason)
            {
                Assert(false, () => reason);
                return this;
            }

            public MissionClearConditionResult CheckFlagshipLevel(int leastLevel)
            {
                int actualLevel = members.FirstOrDefault()?.Level ?? 0;
                Assert(actualLevel >= leastLevel,
                    () => $"旗艦Lv{CurrentValue(actualLevel)}{leastLevel}");
                return this;
            }

            public MissionClearConditionResult CheckLevelSum(int leastSum)
            {
                int actualSum = members.Sum(s => s.Level);
                Assert(actualSum >= leastSum,
                    () => $"Lv合計{CurrentValue(actualSum)}{leastSum}");
                return this;
            }

            public MissionClearConditionResult CheckShipCount(int leastCount)
            {
                int actualCount = members.Count();
                Assert(
                    actualCount >= leastCount,
                    () => $"艦船数{CurrentValue(actualCount)}{leastCount}");
                return this;
            }

            
            public MissionClearConditionResult CheckShipCount(Func<ShipData, bool> predicate, int leastCount, string whatis)
            {
                int actualCount = members.Count(predicate);
                Assert(
                    actualCount >= leastCount,
                    () => $"{whatis}{CurrentValue(actualCount)}{leastCount}");
                return this;
            }

            public MissionClearConditionResult CheckShipCountByType(ShipTypes shipType, int leastCount) =>
                CheckShipCount(s => s.MasterShip.ShipType == shipType, leastCount, KCDatabase.Instance.ShipTypes[(int)shipType].Name);

            public MissionClearConditionResult CheckSmallShipCount(int leastCount) =>
                CheckShipCount(s => s.MasterShip.ShipType == ShipTypes.Destroyer || s.MasterShip.ShipType == ShipTypes.Escort, leastCount, "(駆逐+海防)");

            public MissionClearConditionResult CheckAircraftCarrierCount(int leastCount) =>
                CheckShipCount(s => s.MasterShip.IsAircraftCarrier || s.MasterShip.ShipType == ShipTypes.SeaplaneTender, leastCount, "空母系");

            public MissionClearConditionResult CheckSubmarineCount(int leastCount) =>
               CheckShipCount(s => s.MasterShip.IsSubmarine, leastCount, "潜水艦系");

            public MissionClearConditionResult CheckEscortFleet()
            {
                int lightCruiser = members.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser);
                int destroyer = members.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer);
                int trainingCruiser = members.Count(s => s.MasterShip.ShipType == ShipTypes.TrainingCruiser);
                int escort = members.Count(s => s.MasterShip.ShipType == ShipTypes.Escort);
                int escortAircraftCarrier = members.Count(s => s.MasterShip.ShipType == ShipTypes.LightAircraftCarrier && s.ASWBase > 0);

                Assert(
                    ((lightCruiser >= 1 || escortAircraftCarrier >= 1) && (destroyer >= 2 || escort >= 2)) ||
                    (destroyer >= 1 && escort >= 3) ||
                    (trainingCruiser >= 1 && escort >= 2),
                    //() => "[(軽巡 or 護衛空母)+(駆逐2 or 海防2) or 駆逐+海防3 or 練巡+海防2]"       // 厳密だけど長いので
                    () => "護衛隊(軽巡1駆逐2他)"
                    );
                return this;
            }

            public MissionClearConditionResult CheckFlagshipType(ShipTypes shipType)
            {
                Assert(
                   members.FirstOrDefault()?.MasterShip?.ShipType == shipType,
                    () => $"旗艦:{KCDatabase.Instance.ShipTypes[(int)shipType].Name}");
                return this;
            }

            public MissionClearConditionResult CheckParameter(Func<ShipData, int> selector, int leastSum, string parameterName)
            {
                int actualSum = members.Sum(s => selector(s));
                Assert(
                    actualSum >= leastSum,
                    () => $"{parameterName}{CurrentValue(actualSum)}{leastSum}");
                return this;
            }

            public MissionClearConditionResult CheckFirepower(int leastSum) =>
               CheckParameter(s => s.FirepowerTotal, leastSum, "火力");

            public MissionClearConditionResult CheckAA(int leastSum) =>
                CheckParameter(s => s.AATotal, leastSum, "対空");

            public MissionClearConditionResult CheckLOS(int leastSum) =>
               CheckParameter(s => s.LOSTotal, leastSum, "索敵");


            public MissionClearConditionResult CheckASW(int leastSum) =>
                CheckParameter(s => s.ASWTotal - s.AllSlotInstance.Sum(eq =>
                {
                    if (eq == null) return 0;
                    switch (eq.MasterEquipment.CategoryType)
                    {
                        case EquipmentTypes.SeaplaneRecon:
                        case EquipmentTypes.SeaplaneBomber:
                        case EquipmentTypes.FlyingBoat:
                            return eq.MasterEquipment.ASW;
                        default:
                            return 0;
                    }
                }), leastSum, "対潜");


            public MissionClearConditionResult CheckEquipmentCount(Func<EquipmentData, bool> predicate, int leastCount, string whatis)
            {
                int actualCount = members.Sum(s => s.AllSlotInstance.Count(eq => eq != null && predicate(eq)));
                Assert(actualCount >= leastCount,
                    () => $"{whatis}:装備数{CurrentValue(actualCount)}{leastCount}");
                return this;
            }

            public MissionClearConditionResult CheckEquipmentCount(EquipmentTypes equipmentType, int leastCount) =>
                CheckEquipmentCount(eq => eq.MasterEquipment.CategoryType == equipmentType, leastCount, KCDatabase.Instance.EquipmentTypes[(int)equipmentType].Name);


            public MissionClearConditionResult CheckEquippedShipCount(Func<EquipmentData, bool> predicate, int leastCount, string whatis)
            {
                int actualCount = members.Count(s => s.AllSlotInstance.Any(eq => eq != null && predicate(eq)));
                Assert(actualCount >= leastCount,
                    () => $"{whatis}:装備艦船数{CurrentValue(actualCount)}{leastCount}");
                return this;
            }

            public MissionClearConditionResult CheckEquippedShipCount(EquipmentTypes equipmentType, int leastCount) =>
                CheckEquippedShipCount(eq => eq.MasterEquipment.CategoryType == equipmentType, leastCount, KCDatabase.Instance.EquipmentTypes[(int)equipmentType].Name);



            public override string ToString()
            {
                return (IsSuceeded ? "成功" : "失敗") + (FailureReason.Count == 0 ? "" : (" - " + string.Join(", ", FailureReason)));
            }
        }

    }
}
