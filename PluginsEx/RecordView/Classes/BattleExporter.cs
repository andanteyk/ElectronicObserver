using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;

namespace RecordView
{
    public class BattleExporter
    {
        public static void aa()
        {
            
        }

        unsafe  static string GetMapPoint(BattleRecord* record)
        {
            if (record->BattleResult.AreaID < 1)
                return "演习";
            return record->BattleResult.AreaID.ToString() + "-" + record->BattleResult.InfoID.ToString() + "-" + record->BattleResult.CellID.ToString();
        }
        unsafe public static StringBuilder AnalyseRecord(BattleRecord record)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builderMid = new StringBuilder();
            StringBuilder builderBottom = new StringBuilder();

            builder.AppendLine(@"<html>
<head>
<style type=""text/css"">
body {font-family:""Microsoft JhengHei UI"";}
table {border:none;}
th {background:#eee;}
td,th,tr {text-align:left; padding:1px 2px;}
.changed {color:red;}
.hurt {color:#{0};}
.damaged {color:#{1};}
.broken {color:#{2};}
.sinked {color:grey;}
.damage {background:#FFDDDD;}
</style>
</head>
<body>
");
            //.damage {background:#fcfcfc;color:#822;}
            builder = builder.Replace("{0}", ElectronicObserver.Utility.Configuration.Config.UI.Hp50Color.SerializedColor.Substring(2));
            builder = builder.Replace("{1}", ElectronicObserver.Utility.Configuration.Config.UI.Hp25Color.SerializedColor.Substring(2));
            builder = builder.Replace("{2}", ElectronicObserver.Utility.Configuration.Config.UI.Hp0Color.SerializedColor.Substring(2));

            BattleManager.BattleModes BattleMode = (BattleManager.BattleModes)record.BattleMode;
            bool isWater = ((BattleMode & BattleManager.BattleModes.CombinedSurface) > 0);
            bool isCombined = isWater || (BattleMode > BattleManager.BattleModes.BattlePhaseMask);
            string[] enemys = new string[6];
            for (int i = 0; i < 6; i++)
            {
                if (record.EnemyFleet.ShipID[i] < 0)
                    break;

                enemys[i] = KCDatabase.Instance.MasterShips[record.EnemyFleet.ShipID[i]].NameWithClass;
            }

            string FormationFriend = Constants.GetFormationShort(record.FriendFormation);
            string FormationEnemy = Constants.GetFormationShort(record.EnemyFormation);
            string Formation = Constants.GetEngagementForm(record.T_Status);


            string[] friends = new string[6];
            string[] accompany = new string[6];
            int back = (record.BattleResult.MVP >> 8) & 0xff;
            int back2 = (record.BattleResult.MVP >> 16) & 0xff;
            for (int i = 0; i < 6; i++)
            {
                if (record.FriendFleet.ShipID[i] <= 0)
                    break;
                friends[i] = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[i]].Name + " Lv" + record.FriendFleet.Level[i].ToString();
                if ((back & (1 << i)) > 0)
                {
                    friends[i] += "(退避)";
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if (record.AccompanyFleet.ShipID[i] <= 0)
                    break;
                accompany[i] = KCDatabase.Instance.MasterShips[record.AccompanyFleet.ShipID[i]].Name + " Lv" + record.AccompanyFleet.Level[i].ToString();
                if ((back2 & (1 << i)) > 0)
                {
                    accompany[i] += "(退避)";
                }
            }

            if (BattleMode == BattleManager.BattleModes.Practice)
                enemys = accompany;

            int[] FriendHP = new int[6];
            int[] Accompany = new int[6];
            for (int i = 0; i < 6; i++)
            {
                if (record.FriendFleet.ShipID[i] >= 0)
                    FriendHP[i] = record.FriendFleet.NowHP[i];
                if (record.AccompanyFleet.ShipID[i] >= 0)
                    Accompany[i] = record.AccompanyFleet.NowHP[i];
            }

            string shipdrop = "-";

            if (record.BattleResult.DroppedShipID < 0)
                record.BattleResult.DroppedShipID = 0;

            int shipid = record.BattleResult.DroppedShipID & 0xffff;
            int itemid = (record.BattleResult.DroppedShipID >> 16) & 0xff;
            if (shipid > 0)
                shipdrop = KCDatabase.Instance.MasterShips[shipid].Name;
            if (itemid > 0)
                shipdrop += "/" + KCDatabase.Instance.MasterUseItems[itemid].Name;

            string MVP = null;
            if (isCombined)
            {
                int mvp1 = (record.BattleResult.MVP & 0xf) - 1;
                int mvp2 = (record.BattleResult.MVP & 0xff) >> 4 - 7;
                MVP = friends[mvp1] + "/" + accompany[mvp2];
            }
            else
            {
                int mvp1 = (record.BattleResult.MVP & 0xf) - 1;
                MVP = friends[mvp1];
            }
            builder.AppendFormat(@"<h2>战斗结果</h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90"">战斗海域</th><td>{0}</td>
</tr>
<tr>
<th width=""90"">MVP</th><td>{1}</td>
</tr>
<tr>
<th width=""90"">评价</th><td>{2}</td>
</tr>
<tr>
<th width=""90"">掉落</th><td>{3}</td>
</tr>
</tbody>
</table>
", GetMapPoint(&record), MVP, "" + (char)(record.BattleResult.Rank), shipdrop);



            builder.AppendFormat(@"<h2>战斗阵形</h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90"">我方</th><td>{0}</td>
</tr>
<tr>
<th width=""90"">敌方</th><td>{1}</td>
</tr>
<tr>
<th width=""90"">航向</th><td>{2}</td>
</tr>
</tbody>
</table>
", FormationFriend, FormationEnemy, Formation);





            if (record.AirBattle1.AirSuperiority >= 0)
            {
                int searchFriend = record.SearchingFriend;
                int searchEnemy = record.SearchingEnemy;
                builderBottom.AppendFormat(@"<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90"">我方索敌</th><td>{0}</td><td>{1}</td>
</tr>
<tr>
<th width=""90"">敌方索敌</th><td>{2}</td><td>{3}</td>
</tr>
</tbody>
</table>
", (searchFriend < 4 ? "水上机" : "雷达"), Constants.GetSearchingResult(searchFriend),
 (searchEnemy < 4 ? "水上机" : "雷达"), Constants.GetSearchingResult(searchEnemy));
            }


            if (record.AirBattle1.AirSuperiority >= 0)
            {
                EquipmentDataMaster[] planes =
						{
							KCDatabase.Instance.MasterEquipments[record.AirBattle1.FriendTouch],
							KCDatabase.Instance.MasterEquipments[record.AirBattle2.FriendTouch],
							KCDatabase.Instance.MasterEquipments[record.AirBattle1.EnemyTouch],
							KCDatabase.Instance.MasterEquipments[record.AirBattle2.EnemyTouch]
						};

                bool[] s1available = { true, record.AirBattle2.AirSuperiority >= 0 };
                bool[] s2available = new bool[2];
                bool[] s3available = new bool[2];

                if (record.AirBattle1.EnemyTotalS2 + record.AirBattle1.FriendTotalS2 > 0)
                {
                    s2available[0] = true;
                }
                if (record.AirBattle2.EnemyTotalS2 + record.AirBattle2.FriendTotalS2 > 0)
                {
                    s2available[1] = true;
                }
                for (int i = 0; i < 18; i++)
                {
                    if (record.AirBattle1.Damages[i] > 0)
                    {
                        s3available[0] = true;
                    }
                    if (record.AirBattle2.Damages[i] > 0)
                    {
                        s3available[1] = true;
                    }
                }


                bool[] fire = new bool[] { s2available[0] && record.AirBattle1.AACutInKind >= 0, s2available[1] && record.AirBattle2.AACutInKind >= 0 };
                string AAciShip1 = fire[0] ? KCDatabase.Instance.MasterShips[record.AirBattle1.AACutInID].Name : "";
                string AAciShip2 = fire[1] ? KCDatabase.Instance.MasterShips[record.AirBattle2.AACutInID].Name : "";

                builderBottom.AppendFormat(@"<h2>航空战</h2>
<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90""></th><th width=""110"">我方</th><th width=""110""></th><th width=""110"">敌方</th><th width=""110""></th>
</tr>
<tr>
<th width=""90"">制空</th><td>{0}</td><td>{1}</td><td colspan=""2""></td>
</tr>
<tr>
<th width=""90"">接触信息</th><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>
</tr>
<tr>
<th width=""90"">stage1</th><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td>
</tr>
<tr>
<th width=""90"">stage2</th><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td>
</tr>
<tr>
<th width=""90"">对空</th><td>{14}</td><td>{15}</td><td colspan=""2""></td>
</tr>
</tbody>
</table>
", Constants.GetAirSuperiority(record.AirBattle1.AirSuperiority),
    (record.AirBattle2.AirSuperiority < 0 ? null : Constants.GetAirSuperiority(record.AirBattle2.AirSuperiority)),

    (planes[0] == null ? "-" : planes[0].Name),
    (planes[1] == null ? null : planes[1].Name),
    (planes[2] == null ? "-" : planes[2].Name),
    (planes[3] == null ? null : planes[3].Name),

    (string.Format("-{0}/{1}", record.AirBattle1.FriendLostS1, record.AirBattle1.FriendTotalS1)),
    (!s1available[1] ? null : string.Format("-{0}/{1}", record.AirBattle2.FriendLostS1, record.AirBattle2.FriendTotalS1)),
    (string.Format("-{0}/{1}", record.AirBattle1.EnemyLostS1, record.AirBattle1.EnemyTotalS1)),
    (!s1available[1] ? null : string.Format("-{0}/{1}", record.AirBattle2.EnemyLostS1, record.AirBattle2.EnemyTotalS1)),

    (!s2available[0] ? "-" : string.Format("-{0}/{1}", record.AirBattle1.FriendLostS2, record.AirBattle1.FriendTotalS2)),
    (!s2available[1] ? null : string.Format("-{0}/{1}", record.AirBattle2.FriendLostS2, record.AirBattle2.FriendTotalS2)),
    (!s2available[0] ? null : string.Format("-{0}/{1}", record.AirBattle1.EnemyLostS2, record.AirBattle1.EnemyTotalS2)),
    (!s2available[1] ? null : string.Format("-{0}/{1}", record.AirBattle2.EnemyLostS2, record.AirBattle2.EnemyTotalS2)),

    (!fire[0] && !fire[1] ? "对空炮火" : (!fire[0] ? "-" : string.Format("{0}<br/>{1} (#{2})", AAciShip1, Constants.GetAACutinKind(record.AirBattle1.AACutInKind), record.AirBattle1.AACutInKind))),
    (!fire[1] || !s1available[1] ? "-" : string.Format("{0}<br/>{1} (#{2})", AAciShip2, Constants.GetAACutinKind(record.AirBattle2.AACutInKind), record.AirBattle2.AACutInKind))
    );


                if (s3available[0])
                {
                    FillAirDamage(builderBottom, &record, 1, friends, enemys, accompany);
                }

                if (s3available[1])
                {
                    FillAirDamage(builderBottom, &record, 2, friends, enemys, accompany);
                }
            }

            if (record.Support.SupportFlag > 0)
            {

                string[] supportnames = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    if (record.Support.Supporter[i] < 0)
                        break;
                    supportnames[i] = KCDatabase.Instance.MasterShips[record.Support.Supporter[i]].Name;
                }

                switch (record.Support.SupportFlag)
                {
                    case 1:
                        FillSupportDamage("航空支援", builderBottom, &record, supportnames, enemys);
                        break;
                    case 2:
                        FillSupportDamage("炮击支援", builderBottom, &record, supportnames, enemys);
                        break;
                    case 3:
                        FillSupportDamage("雷击支援", builderBottom, &record, supportnames, enemys);
                        break;
                }

            }

            int ppp = 0;
            if (isCombined&&!isWater)
                ppp = 1;
            if (isWater)
                ppp = 2;

            if (record.OpenTorpedoBattle.IsAvailable > 0)
            {
                FillTorpedoDamage(1, "开幕雷击", builderBottom, &record, isCombined ? accompany : friends, enemys);
            }

            if (record.ShellBattle1.IsAvailable > 0)
            {
                FillShellingDamage(1, ppp, "炮击战1回合", builderBottom, &record, (isCombined && !isWater) ? accompany : friends, enemys);
            }

            if (isCombined && !isWater)
            {
                // 机动部队闭幕雷击
                if (record.CloseTorpedoBattle.IsAvailable > 0)
                {
                    FillTorpedoDamage(2, "闭幕雷击", builderBottom, &record, accompany, enemys);
                }
            }



            if (record.ShellBattle2.IsAvailable > 0)
            {
                FillShellingDamage(2, ppp, "炮击战2回合", builderBottom, &record, friends, enemys);
            }

            if (record.ShellBattle3.IsAvailable > 0)
            {
                FillShellingDamage(3, ppp, "炮击战3回合", builderBottom, &record, isWater ? accompany : friends, enemys);
            }

            // 闭幕雷击
            if ((!isCombined || isWater) && record.CloseTorpedoBattle.IsAvailable > 0)
            {
                FillTorpedoDamage(2, "闭幕雷击", builderBottom, &record, isCombined ? accompany : friends, enemys);
            }

            if (record.NightBattle.IsAvailable > 0)
            {
                string LightFriend = "-", LightEnemy = "-", FriendPlane = "-", EnemyPlane = "-", FlareFriend = "-", FlareEnemy = "-";
                if (record.NightInformation.FriendSearchlight >= 0)
                    LightFriend = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[record.NightInformation.FriendSearchlight]].Name;
                if (record.NightInformation.FriendTouchAircraft >= 0)
                    FriendPlane = KCDatabase.Instance.MasterEquipments[record.NightInformation.FriendTouchAircraft].Name;
                if (record.NightInformation.FriendFlare >= 0)
                    FlareFriend = KCDatabase.Instance.MasterShips[record.FriendFleet.ShipID[record.NightInformation.FriendFlare]].Name;

                if (record.NightInformation.EnemySearchlight >= 0)
                    LightEnemy = KCDatabase.Instance.MasterShips[record.NightInformation.EnemySearchlight].Name;
                if (record.NightInformation.EnemyTouchAircraft >= 0)
                    EnemyPlane = KCDatabase.Instance.MasterEquipments[record.NightInformation.EnemyTouchAircraft].Name;
                if (record.NightInformation.EnemyFlare >= 0)
                    FlareEnemy = KCDatabase.Instance.MasterShips[record.NightInformation.EnemyFlare].Name;

                builderBottom.AppendFormat(@"<h2>夜战</h2>
<hr/>
<table cellspacing=""2"" cellpadding=""0"">
<tbody>
<tr>
<th width=""90""></th><th width=""110"">我方</th><th width=""110"">敌方</th>
</tr>
<tr>
<th width=""90"">探照灯</th><td>{0}</td><td>{1}</td>
</tr>
<tr>
<th width=""90"">夜间接触</th><td>{2}</td><td>{3}</td>
</tr>
<tr>
<th width=""90"">照明弹</th><td>{4}</td><td>{5}</td>
</tr>
</tbody>
</table>
", (LightFriend),
    (LightEnemy),
    (FriendPlane),
    (EnemyPlane),
    (FlareFriend),
    (FlareEnemy)
    );

                if (record.NightBattle.IsAvailable > 0)
                {
                    FillShellingDamage(4,ppp, "夜战", builderBottom, &record, isCombined ? accompany : friends, enemys);
                }
            }

            builderMid.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"">
<thead>
<th width=""160"">我方</th>
<th width=""90"">血量</th>
<th width=""90"">最终</th>
<th width=""40"">火力</th>
<th width=""40"">雷装</th>
<th width=""40"">对空</th>
<th width=""40"">装甲</th>
<th width=""160"">装备1</th>
<th width=""160"">装备2</th>
<th width=""160"">装备3</th>
<th width=""160"">装备4</th>
</thead>");

            for (int i = 0; i < 6; i++)
            {
                if (record.FriendFleet.ShipID[i] < 0)
                    break;
                string[] equip = new string[4];

                if (record.FriendFleet.Equipment1[i] <= 0)
                    equip[0] = "-";
                else
                {
                    int Level = record.FriendFleet.EquipmentLevel1[i] & 0xffff;
                    int Amount = record.FriendFleet.EquipmentLevel1[i] >> 16;
                    if (Level > 0)
                        equip[0] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment1[i]].Name + "+" + Level.ToString();
                    else
                        equip[0] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment1[i]].Name;
                    if (Amount>0)
                    {
                        equip[0] += "[" + Amount.ToString() + "]";
                    }
                }
                if (record.FriendFleet.Equipment2[i] <= 0)
                    equip[1] = "-";
                else
                {
                    int Level = record.FriendFleet.EquipmentLevel2[i] & 0xffff;
                    int Amount = record.FriendFleet.EquipmentLevel2[i] >> 16;
                    if (Level > 0)
                        equip[1] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment2[i]].Name + "+" + Level.ToString();
                    else
                        equip[1] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment2[i]].Name;
                    if (Amount > 0)
                    {
                        equip[1] += "[" + Amount.ToString() + "]";
                    }
                }
                if (record.FriendFleet.Equipment3[i] <= 0)
                    equip[2] = "-";
                else
                {
                    int Level = record.FriendFleet.EquipmentLevel3[i] & 0xffff;
                    int Amount = record.FriendFleet.EquipmentLevel3[i] >> 16;
                    if (Level > 0)
                        equip[2] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment3[i]].Name + "+" + Level.ToString();
                    else
                        equip[2] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment3[i]].Name;
                    if (Amount > 0)
                    {
                        equip[2] += "[" + Amount.ToString() + "]";
                    }
                }
                if (record.FriendFleet.Equipment4[i] <= 0)
                    equip[3] = "-";
                else
                {
                    int Level = record.FriendFleet.EquipmentLevel4[i] & 0xffff;
                    int Amount = record.FriendFleet.EquipmentLevel4[i] >> 16;
                    if (Level > 0)
                        equip[3] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment4[i]].Name + "+" + Level.ToString();
                    else
                        equip[3] = KCDatabase.Instance.MasterEquipments[record.FriendFleet.Equipment4[i]].Name;
                    if (Amount > 0)
                    {
                        equip[3] += "[" + Amount.ToString() + "]";
                    }
                }

                builderMid.AppendFormat(@"<tr><td>{10}</td>
<td{11}>{0}</td>
<td{12}>{1}</td>
<td>{2}</td>
<td>{3}</td>
<td>{4}</td>
<td>{5}</td>
<td>{6}</td>
<td>{7}</td>
<td>{8}</td>
<td>{9}</td></tr>",
                          FriendHP[i].ToString() + "/" + record.FriendFleet.MaxHP[i].ToString(),
                          record.FriendFleet.NowHP[i].ToString(),
                          record.FriendFleet.FirePower[i].ToString(),
                          record.FriendFleet.Torpedo[i].ToString(),
                          record.FriendFleet.AA[i].ToString(),
                          record.FriendFleet.Armor[i].ToString(),
                          equip[0],
                          equip[1],
                          equip[2],
                          equip[3],
                          friends[i],
                          GetHealthStatus(FriendHP[i], record.FriendFleet.MaxHP[i]),
                          GetHealthStatus(record.FriendFleet.NowHP[i], record.FriendFleet.MaxHP[i])
                );
            }
            builderMid.Append("</table>");
            if (accompany[0] != null)
            {
                string self = "我方";
                if (BattleMode == BattleManager.BattleModes.Practice)
                    self = "敌方";
                builderMid.AppendFormat(@"<table cellspacing=""1"" cellpadding=""0"">
<thead>
<th width=""160"">{0}</th>
<th width=""90"">血量</th>
<th width=""90"">最终</th>
<th width=""40"">火力</th>
<th width=""40"">雷装</th>
<th width=""40"">对空</th>
<th width=""40"">装甲</th>
<th width=""160"">装备1</th>
<th width=""160"">装备2</th>
<th width=""160"">装备3</th>
<th width=""160"">装备4</th>
</thead>", self);

                for (int i = 0; i < 6; i++)
                {
                    if (record.AccompanyFleet.ShipID[i] <= 0)
                        break;
                    string[] equip = new string[4];

                    if (record.AccompanyFleet.Equipment1[i] <= 0)
                        equip[0] = "-";
                    else
                    {
                        int Level = record.AccompanyFleet.EquipmentLevel1[i] & 0xffff;
                        int Amount = record.AccompanyFleet.EquipmentLevel1[i] >> 16;
                        if (Level > 0)
                            equip[0] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment1[i]].Name + "+" + Level.ToString();
                        else
                            equip[0] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment1[i]].Name;
                        if (Amount > 0)
                        {
                            equip[0] += "[" + Amount.ToString() + "]";
                        }
                    }
                    if (record.AccompanyFleet.Equipment2[i] <= 0)
                        equip[1] = "-";
                    else
                    {
                        int Level = record.AccompanyFleet.EquipmentLevel2[i] & 0xffff;
                        int Amount = record.AccompanyFleet.EquipmentLevel2[i] >> 16;
                        if (Level > 0)
                            equip[1] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment2[i]].Name + "+" + Level.ToString();
                        else
                            equip[1] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment2[i]].Name;
                        if (Amount > 0)
                        {
                            equip[1] += "[" + Amount.ToString() + "]";
                        }
                    }
                    if (record.AccompanyFleet.Equipment3[i] <= 0)
                        equip[2] = "-";
                    else
                    {
                        int Level = record.AccompanyFleet.EquipmentLevel3[i] & 0xffff;
                        int Amount = record.AccompanyFleet.EquipmentLevel3[i] >> 16;
                        if (Level > 0)
                            equip[2] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment3[i]].Name + "+" + Level.ToString();
                        else
                            equip[2] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment3[i]].Name;
                        if (Amount > 0)
                        {
                            equip[2] += "[" + Amount.ToString() + "]";
                        }
                    }
                    if (record.AccompanyFleet.Equipment4[i] <= 0)
                        equip[3] = "-";
                    else
                    {
                        int Level = record.AccompanyFleet.EquipmentLevel4[i] & 0xffff;
                        int Amount = record.AccompanyFleet.EquipmentLevel4[i] >> 16;
                        if (Level > 0)
                            equip[3] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment4[i]].Name + "+" + Level.ToString();
                        else
                            equip[3] = KCDatabase.Instance.MasterEquipments[record.AccompanyFleet.Equipment4[i]].Name;
                        if (Amount > 0)
                        {
                            equip[3] += "[" + Amount.ToString() + "]";
                        }
                    }

                    builderMid.AppendFormat(@"<tr><td>{10}</td>
<td>{0}</td>
<td{11}>{1}</td>
<td>{2}</td>
<td>{3}</td>
<td>{4}</td>
<td>{5}</td>
<td>{6}</td>
<td>{7}</td>
<td>{8}</td>
<td>{9}</td></tr>",

                              record.AccompanyFleet.MaxHP[i].ToString() + "/" + record.AccompanyFleet.MaxHP[i].ToString(),
                              record.AccompanyFleet.NowHP[i].ToString(),
                              record.AccompanyFleet.FirePower[i].ToString(),
                              record.AccompanyFleet.Torpedo[i].ToString(),
                              record.AccompanyFleet.AA[i].ToString(),
                              record.AccompanyFleet.Armor[i].ToString(),
                              equip[0],
                              equip[1],
                              equip[2],
                              equip[3],
                              accompany[i],
                              GetHealthStatus(record.AccompanyFleet.NowHP[i], record.AccompanyFleet.MaxHP[i])
                    );
                }
                builderMid.Append("</table>");
            }

            if (BattleMode != BattleManager.BattleModes.Practice)
            {
                builderMid.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"">
<thead><tr>
<th width=""160"">敌方</th>
<th width=""90"">血量</th>
<th width=""90"">最终</th>
</tr></thead>");
                for (int i = 0; i < 6; i++)
                {
                    if (record.EnemyFleet.ShipID[i] < 0)
                        break;

                    builderMid.AppendFormat(@"<tr>
<td>{0}</td>
<td>{1}</td>
<td{3}>{2}</td>
</tr>", KCDatabase.Instance.MasterShips[record.EnemyFleet.ShipID[i]].NameWithClass,
         record.EnemyFleet.MaxHP[i].ToString(),
         record.EnemyFleet.NowHP[i].ToString(),
         GetHealthStatus(record.EnemyFleet.NowHP[i], record.EnemyFleet.MaxHP[i])
         );
                }
                builderMid.Append("</table><hr/>");
            }
            builderBottom.AppendLine("</body>\r\n</html>");

            builder.Append(builderMid);
            builder.Append(builderBottom);

            return builder;
        }

        unsafe static void FillAirDamage(StringBuilder builder, BattleRecord* record, int AirPeriod, string[] friends, string[] enemys, string[] accompany)
        {
            BattleManager.BattleModes BattleMode = (BattleManager.BattleModes)record->BattleMode;
            bool isWater = ((BattleMode & BattleManager.BattleModes.CombinedSurface) > 0);
            bool isCombined = isWater || (BattleMode > BattleManager.BattleModes.BattlePhaseMask);


            builder.AppendLine(@"<table cellspacing=""2"" cellpadding=""0"">
<thead>
<th width=""160"">我方</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>
<th width=""90"">雷爆暴</th>");
            if (accompany[0] != null && isCombined)
            {
                builder.AppendLine(@"<th width=""160"">伴随</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>
<th width=""90"">雷爆暴</th>");
            }
            builder.AppendLine(@"<th width=""160"">敌方</th>
<th width=""90"">所受伤害</th>
<th width=""90"">血量</th>
<th width=""90"">雷爆暴</th>
</tr>
</thead>
<tbody>");
            AirBattleRecord airBattle;
            if (AirPeriod == 1)
                airBattle = record->AirBattle1;
            if (AirPeriod == 2)
                airBattle = record->AirBattle2;


            for (int i = 0; i < 6; i++)
            {
                builder.AppendLine("<tr>");

                // 航空开幕
                if (friends[0] != null)
                {
                    if (friends[i] != null)
                    {
                        int before = record->FriendFleet.NowHP[i];
                        record->FriendFleet.NowHP[i] = Math.Max(record->FriendFleet.NowHP[i] - GetRealDamage(airBattle.Damages[i]), 0);
                        builder.AppendFormat("<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td><td>{7}</td>\r\n",
                            friends[i], before, record->FriendFleet.NowHP[i], record->FriendFleet.MaxHP[i],
                            GetHealthStatus(record->FriendFleet.NowHP[i], record->FriendFleet.MaxHP[i]),
                            (i + 1),
                            (GetRealDamage(airBattle.Damages[i]) > 0 ? GetRealDamage(airBattle.Damages[i]).ToString() : (isAirAttacked(airBattle.Damages[i]) ? "miss" : null)),
                             GetAirFlag(airBattle.Damages[i]));

                    }
                    else
                    {
                        builder.AppendLine("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                    }
                }
                if (accompany[0] != null && isCombined)
                {
                    if (accompany[i] != null)
                    {
                        int before = record->AccompanyFleet.NowHP[i];
                        record->AccompanyFleet.NowHP[i] = Math.Max(record->AccompanyFleet.NowHP[i] - GetRealDamage(airBattle.Damages[i + 12]), 0);
                        builder.AppendFormat("<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td><td>{7}</td>\r\n",
                            accompany[i], before, record->AccompanyFleet.NowHP[i], record->AccompanyFleet.MaxHP[i],
                            GetHealthStatus(record->AccompanyFleet.NowHP[i], record->AccompanyFleet.MaxHP[i]),
                            (i + 1),
                            (GetRealDamage(airBattle.Damages[i + 12]) > 0 ? GetRealDamage(airBattle.Damages[i + 12]).ToString() : (isAirAttacked(airBattle.Damages[i + 12]) ? "miss" : null)),
                            GetAirFlag(airBattle.Damages[i + 12]));
                    }
                    else
                    {
                        builder.AppendLine("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                    }
                }
                if (enemys[0] != null)
                {
                    if (enemys[i] != null)
                    {
                        int before = record->EnemyFleet.NowHP[i];
                        record->EnemyFleet.NowHP[i] = Math.Max(record->EnemyFleet.NowHP[i] - GetRealDamage(airBattle.Damages[i + 6]), 0);
                        builder.AppendFormat("<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td><td>{7}</td>\r\n",
                            enemys[i], before, record->EnemyFleet.NowHP[i], record->EnemyFleet.MaxHP[i],
                             GetHealthStatus(record->EnemyFleet.NowHP[i], record->EnemyFleet.MaxHP[i]),
                            (i + 1),
                            (GetRealDamage(airBattle.Damages[i + 6]) > 0 ? GetRealDamage(airBattle.Damages[i + 6]).ToString() : (isAirAttacked(airBattle.Damages[i + 6]) ? "miss" : null)),
                            GetAirFlag(airBattle.Damages[i + 6]));

                    }
                    else
                    {
                        builder.AppendLine("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                    }
                }

                builder.AppendLine("</tr>");
            }

            builder.AppendLine("</tbody>\r\n</table>");
        }
        static string GetAirFlag(int damage)
        {
            string ss = "";
            if (isAirTorpedo(damage))
                ss+= "√";
            else
                ss += "-";
            if (isAirBomber(damage))
                ss += "√";
            else
                ss += "-";
            if (isAirCritical(damage))
                ss += "√";
            else
                ss += "-";
            return ss;
        }
     static   bool isAirTorpedo(int damage)
        {
            return (damage & 0x10000) > 0;
        }

     static   bool isAirAttacked(int damage)
        {
            return (damage & 0x30000) > 0;
        }

    static    bool isAirBomber(int damage)
        {
            return (damage & 0x20000) > 0;
        }

     static   bool isAirCritical(int damage)
        {
            return (damage & 0x40000) > 0;
        }

        unsafe private static void FillSupportDamage(string name, StringBuilder builder, BattleRecord* record, string[] friends, string[] enemys)
        {

            builder.AppendFormat(@"<h2>{0} <small>（伤害无对应关系）</small></h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""160"">我方</th>
<th width=""40"">&nbsp;</th>
<th width=""160"">敌方</th>
<th width=""90"">伤害</th>
<th width=""90"">血量</th>
</tr>
</thead>
<tbody>
", name);

            for (int i = 0; i < 6; i++)
            {
                builder.AppendLine("<tr>");

                // 支援
                if (friends[i] != null)
                {
                    builder.AppendFormat("<td>{0}.{1}</td><td></td>\r\n", (i + 1), friends[i]);
                }
                else
                {
                    builder.AppendLine("<td>&nbsp;</td><td>&nbsp;</td>");
                }

                if (enemys[i] != null)
                {
                    int before = record->EnemyFleet.NowHP[i];
                    record->EnemyFleet.NowHP[i] = Math.Max(record->EnemyFleet.NowHP[i] - GetRealDamage(record->Support.Damages[i]), 0);
                    builder.AppendFormat("<td>{5}.{0}</td><td>{6}</td><td{4}>{1}→{2}/{3}</td>\r\n",
                        enemys[i], before, record->EnemyFleet.NowHP[i], record->EnemyFleet.MaxHP[i],
                        GetHealthStatus(record->EnemyFleet.NowHP[i], record->EnemyFleet.MaxHP[i]),
                        (i + 1),
                        (GetRealDamage(record->Support.Damages[i]) > 0 ? GetRealDamage(record->Support.Damages[i]).ToString() : "miss"));
                }
                else
                {
                    builder.AppendLine("<td>&nbsp;</td><td>&nbsp;</td>");
                }

                builder.AppendLine("</tr>");
            }

            builder.AppendLine("</tbody>\r\n</table>");

        }

        unsafe private static void FillTorpedoDamage(int Period, string name, StringBuilder builder, BattleRecord* record, string[] friends, string[] enemys)
        {
            BattleManager.BattleModes BattleMode = (BattleManager.BattleModes)record->BattleMode;
            bool isWater = ((BattleMode & BattleManager.BattleModes.CombinedSurface) > 0);
            bool isCombined = isWater || (BattleMode > BattleManager.BattleModes.BattlePhaseMask);


            builder.AppendFormat(@"<h2>{0}</h2>
<hr />
<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""160"">舰</th>
<th width=""90"">血量</th>
<th width=""40"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""90"">伤害</th>
<th width=""90"">暴击</th>
</tr>
</thead>
<tbody>
", name);

            TorpedoRecord torpedo;
            if (Period == 1)
                torpedo = record->OpenTorpedoBattle;
            if (Period == 2)
                torpedo = record->CloseTorpedoBattle;

            int[] FriendDamaged = new int[6];
            int[] EnemyDamaged = new int[6];
            int now, max;
          
            builder.AppendLine(@"<tr><th colspan=""5"">我方攻击</td></tr>");
            for (int i = 0; i < 6; i++)
            {
                if (isCombined)
                {
                    now = record->AccompanyFleet.NowHP[i];
                    max = record->AccompanyFleet.MaxHP[i];
                }
                else
                {
                    now = record->FriendFleet.NowHP[i];
                    max = record->FriendFleet.MaxHP[i];
                }
                    if (torpedo.FriendTarget[i] >= 0)
                    {
                        EnemyDamaged[torpedo.FriendTarget[i]] += GetRealDamage(torpedo.FriendDamages[i]);

                        builder.AppendLine("<tr>");

                        builder.AppendFormat("<td>{4}.{0}<td{6}>{7}</td></td><td>→</td><td>{5}.{1}</td><td>{2}</td><td>{3}</td>\r\n",
                            friends[i], enemys[torpedo.FriendTarget[i]],
                            (GetRealDamage(torpedo.FriendDamages[i]) == 0 ? (isMissed(torpedo.FriendDamages[i]) ? "miss" : "0") : GetRealDamage(torpedo.FriendDamages[i]).ToString()),
                            (isCritical(torpedo.FriendDamages[i]) ? "√" : ""),
                            (i + 1), (torpedo.FriendTarget[i] + 1),
                            GetHealthStatus(now, max), now.ToString() + "/" + max.ToString());

                        builder.AppendLine("</tr>");
                    }

            }

            builder.AppendLine(@"<tr><th colspan=""5"">敌方攻击</td></tr>");
            for (int i = 0; i < 6; i++)
            {
                now = record->EnemyFleet.NowHP[i];
                max = record->EnemyFleet.MaxHP[i];
                if (torpedo.EnemyTarget[i] >= 0)
                {
                    FriendDamaged[torpedo.EnemyTarget[i]] += GetRealDamage(torpedo.EnemyDamages[i]);

                    builder.AppendLine("<tr>");

                    builder.AppendFormat("<td>{4}.{0}<td{6}>{7}</td></td><td>→</td><td>{5}.{1}</td><td>{2}</td><td>{3}</td>\r\n",
                        enemys[i], friends[torpedo.EnemyTarget[i]],
                        (GetRealDamage(torpedo.EnemyDamages[i]) == 0 ? (isMissed(torpedo.EnemyDamages[i]) ? "miss" : "0") : GetRealDamage(torpedo.EnemyDamages[i]).ToString()),
                        (isCritical(torpedo.EnemyDamages[i]) ? "√" : ""),
                        (i + 1), (torpedo.EnemyTarget[i] + 1),
                        GetHealthStatus(now, max), now.ToString() + "/" + max.ToString());

                    builder.AppendLine("</tr>");
                }

            }

            builder.AppendLine("</tbody>\r\n</table>");

            for (int i = 0; i < 6; i++)
            {
                if (isCombined)
                    record->AccompanyFleet.NowHP[i] = Math.Max(record->AccompanyFleet.NowHP[i] - FriendDamaged[i], 0);
                else
                    record->FriendFleet.NowHP[i] = Math.Max(0, record->FriendFleet.NowHP[i] - FriendDamaged[i]);
                record->EnemyFleet.NowHP[i] = Math.Max(0, record->EnemyFleet.NowHP[i] - EnemyDamaged[i]);
            }
        }

        static bool isMissed(int damage)
        {
            return (damage & 0x30000) == 0;
        }

        static bool isCritical(int damage)
        {
            return (damage & 0x20000) > 0;
        }

        static int GetRealDamage(int damage)
        {
            return damage & 0xffff;
        }

        unsafe private static void FillShellingDamage(int Period,int Combined, string name, StringBuilder builder, BattleRecord* record, string[] friends, string[] enemys)
        {
            if (!string.IsNullOrEmpty(name))
            {
                builder.AppendFormat(@"<h2>{0}</h2>
<hr />
", name);
            }

            builder.AppendLine(@"<table cellspacing=""2"" cellpadding=""0"">
<thead>
<tr>
<th width=""24"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""90"">血量</th>
<th width=""40"">&nbsp;</th>
<th width=""24"">&nbsp;</th>
<th width=""160"">舰</th>
<th width=""90"">伤害</th>
<th width=""90"">血量</th>
<th width=""90"">暴击</th>
<th width=""120"">特殊</th>
</tr>
</thead>
<tbody>");
            ShellingRecord shell;
            switch (Period)
            {
                case 1:
                    shell = record->ShellBattle1;
                    break;
                case 2:
                    shell = record->ShellBattle2;
                    break;
                case 3:
                    shell = record->ShellBattle3;
                    break;
                case 4:
                    shell = record->NightBattle;
                    break;
            }


            for (int i = 0; i < 24; i++)
            {
                int from = shell.Attacker[i];
                int to = shell.Target[i];
                int damage = shell.Damages[i];
                string Special = Period == 4 ? GetNightSP(damage) : GetDaySP(damage);
                int RealDamage = GetRealDamage(damage);
                bool Criticl = isCritical(damage);
                bool Missed = isMissed(damage);

                if (from == 0 && to == 0)
                    break;

                int Tag = 0;
                if (Combined == 0 || (Combined == 2 && (Period == 1 || Period == 2)) || ((Combined == 1) && (Period == 2 || Period == 3)))
                    Tag = 1;
                else
                    Tag = 2;

                int ATnow, ATmax;
                if (from >= 6)
                {
                    ATnow = record->EnemyFleet.NowHP[from - 6];
                    ATmax = record->EnemyFleet.MaxHP[from - 6];
                }
                else if (Tag == 1)
                {
                    ATnow = record->FriendFleet.NowHP[from];
                    ATmax = record->FriendFleet.MaxHP[from];
                }
                else
                {
                    ATnow = record->AccompanyFleet.NowHP[from];
                    ATmax = record->AccompanyFleet.MaxHP[from];
                }

                if (to < 6)
                {
                    // 我方受到攻击
                    builder.AppendLine(@"<tr class=""damage"">");
                }
                else
                {
                    builder.AppendLine("<tr>");
                }

                if (i > 0 && from == shell.Attacker[i - 1] && Special != null)
                {
                    builder.Append(@"<td colspan=""5"">&nbsp;</td>");
                }
                else
                {
                    builder.AppendFormat("<td>{0}</td><td>{1}.{2}</td><td{4}>{5}</td><td>→</td><td>{3}</td>",
                        (from < 6 ? "我" : "敌"),
                        (from % 6 + 1),
                        (from < 6 ? friends[from] : enemys[from - 6]),
                        (to < 6 ? "我" : "敌"),
                        GetHealthStatus(ATnow, ATmax), ATnow.ToString() + "/" + ATmax.ToString());
                }

                int before, now, max;
                if (to < 6)
                {
                    if (Tag == 1)
                    {
                        before = record->FriendFleet.NowHP[to];
                        record->FriendFleet.NowHP[to] = Math.Max(0, record->FriendFleet.NowHP[to] - RealDamage);
                        now = record->FriendFleet.NowHP[to];
                        max = record->FriendFleet.MaxHP[to];
                    }
                    else
                    {
                        before = record->AccompanyFleet.NowHP[to];
                        record->AccompanyFleet.NowHP[to] = Math.Max(0, record->AccompanyFleet.NowHP[to] - RealDamage);
                        now = record->AccompanyFleet.NowHP[to];
                        max = record->AccompanyFleet.MaxHP[to];
                    }
                }
                else
                {
                    before = record->EnemyFleet.NowHP[to - 6];
                    record->EnemyFleet.NowHP[to - 6] = Math.Max(0, record->EnemyFleet.NowHP[to - 6] - RealDamage);
                    now = record->EnemyFleet.NowHP[to - 6];
                    max = record->EnemyFleet.MaxHP[to - 6];
                }

                builder.AppendFormat("<td>{0}.{1}</td><td>{2}</td><td{7}>{3}→{4}/{5}</td><td>{6}</td>",
                    (to % 6 + 1),
                    (to < 6 ? friends[to] : enemys[to - 6]),
                    (RealDamage == 0 ? (Missed ? "miss" : "0") : RealDamage.ToString()),
                    Math.Max(before, 0),
                    Math.Max(now, 0), max,
                    (Criticl ? "√" : ""),
                    GetHealthStatus(now,max));


                builder.AppendFormat("<td>{0}</td>", Special);


                builder.AppendLine("</tr>");
            }

            builder.AppendLine("</tbody>\r\n</table>");
        }

    static  string GetDaySP(int damage)
      {
          if (damage >> 18 > 0)
              return Constants.GetDayAttackKind(damage >> 18);
          else
              return null;
      }

    static string GetHealthStatus(int now, int max)
    {
        string status = null;
        int now4 = now * 4;
        if (now4 <= 0)
            status = @" class=""sinked""";
        else if (now4 <= max)
            status = @" class=""broken""";
        else if (now4 <= max * 2)
            status = @" class=""damaged""";
        else if (now4 <= max * 3)
            status = @" class=""hurt""";
        return status;
    }
    static  string GetNightSP(int damage)
      {
          if (damage >> 18 > 0)
              return Constants.GetNightAttackKind(damage >> 18);
          else
              return null;
      }
    }
}
