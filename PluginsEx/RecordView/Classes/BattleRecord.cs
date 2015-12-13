using ElectronicObserver;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;

namespace RecordView
{
    public class BattleView
    {

        unsafe public static void Save(BattleRecord* record)
        {
            byte[] buff = SaveMyStruct(record);
            if (!Directory.Exists(Application.StartupPath + "\\battlelogs"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\battlelogs");
            }
            string filename = Application.StartupPath + "\\battlelogs\\battlelog_" + record->BattleTime.ToString("yyyy_MMdd") + ".dat";
            System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Append);
            fs.Write(buff, 0, buff.Length);
            fs.Close();
        }
        unsafe public static void GenerateBattleRecord()
        {

            BattleRecord Record;
            Record.BattleTime = DateTime.Now;
            BattleManager bm = KCDatabase.Instance.Battle;
            if (bm == null
                || bm.BattleMode == BattleManager.BattleModes.Undefined	// 无战斗
                || bm.BattleMode == BattleManager.BattleModes.NightDay)	// TODO: 夜转昼
                //|| bm.BattleMode > BattleManager.BattleModes.BattlePhaseMask )	// TODO：联合舰队
                return;
            Record.BattleMode = (int)bm.BattleMode;
            bool isWater = ((bm.BattleMode & BattleManager.BattleModes.CombinedSurface) > 0);
            bool isCombined = isWater || (bm.BattleMode > BattleManager.BattleModes.BattlePhaseMask);

            if (bm.BattleMode != BattleManager.BattleModes.Practice)
            {
                Record.BattleResult.AreaID = bm.Compass.MapAreaID;
                //bm.Result.
                Record.BattleResult.InfoID = bm.Compass.MapInfoID;
                Record.BattleResult.CellID = bm.Compass.Destination;
                if (bm.Compass.EventID == 5)//BOSS
                {
                    Record.BattleResult.CellID |= 0x10000;
                }
                Record.BattleResult.CellID |= (bm.Compass.MapInfo.EventDifficulty << 20);
                Record.BattleResult.DroppedShipID = bm.Result.DroppedShipID == -1 ? 0 : bm.Result.DroppedShipID;

                if (bm.Result.DroppedItemID >= 0)
                {
                    Record.BattleResult.DroppedShipID |= bm.Result.DroppedItemID << 16;
                }
            }
            else
                Record.BattleResult.DroppedShipID = 0;
            Record.BattleResult.MVP = bm.Result.MVP;
            if (isCombined)
            {
                int mvp = (int)bm.Result.RawData.api_mvp_combined;
                Record.BattleResult.MVP |= mvp << 4;
            }
            //bm.Result.
            Record.BattleResult.Rank = (int)bm.Result.Rank[0];
            //bm.BattleDay.
            var day = bm.BattleDay;
            PhaseInitial init;
            //string[] enemys;
            int[] enemyid;
            int[] hps;
            int[] maxHps;


            if (bm.BattleDay == null)
            {
                init = bm.BattleNight.Initial;

                hps = (int[])init.InitialHPs.Clone();
                maxHps = init.MaxHPs;
                //enemys = ((int[])bm.BattleNight.RawData.api_ship_ke).Skip(1).Select(id => id <= 0 ? null : KCDatabase.Instance.MasterShips[id].NameWithClass).ToArray();
                enemyid = (int[])bm.BattleNight.RawData.api_ship_ke;

                Record.FriendFormation = bm.BattleNight.Searching.FormationFriend;
                Record.EnemyFormation = bm.BattleNight.Searching.FormationEnemy;
                Record.T_Status = bm.BattleNight.Searching.EngagementForm;

                for (int index = 1; index <= 6; index++)
                {
                    Record.EnemyFleet.ShipID[index - 1] = enemyid[index];
                    Record.EnemyFleet.NowHP[index - 1] = hps[index + 5];
                    Record.EnemyFleet.MaxHP[index - 1] = maxHps[index + 5];
                }
            }
            else
            {
                init = bm.BattleDay.Initial;
                hps = (int[])init.InitialHPs.Clone();
                maxHps = init.MaxHPs;
                //enemys = ((int[])bm.BattleDay.RawData.api_ship_ke).Skip(1).Select(id => id <= 0 ? null : KCDatabase.Instance.MasterShips[id].NameWithClass).ToArray();
                enemyid = (int[])bm.BattleDay.RawData.api_ship_ke;

                Record.FriendFormation = bm.BattleDay.Searching.FormationFriend;
                Record.EnemyFormation = bm.BattleDay.Searching.FormationEnemy;
                Record.T_Status = bm.BattleDay.Searching.EngagementForm;

                for (int index = 1; index <= 6; index++)
                {
                    Record.EnemyFleet.ShipID[index - 1] = enemyid[index];
                    Record.EnemyFleet.NowHP[index - 1] = hps[index + 5];
                    Record.EnemyFleet.MaxHP[index - 1] = maxHps[index + 5];
                }
            }

            //[0|1]：1=単縦陣 2=複縦陣, 3=輪形陣, 4=梯形陣, 5=単横陣
            //	[2]：1=同航戦, 2=反航戦, 3=T字有利, 4=T字不利

            //", FormationFriend.Text, FormationEnemy.Text, Formation.Text );
            // 索敌
            if (day != null && day.IsAvailable)
            {
                Record.SearchingFriend = day.Searching.SearchingFriend;
                Record.SearchingEnemy = day.Searching.SearchingEnemy;
            }
            else
            {
                Record.SearchingFriend = -1;
                Record.SearchingEnemy = -1;
            }


            // 战斗过程

            for (int index = 0; index < 6; index++)
            {
                ShipData sd = init.FriendFleet.MembersInstance[index];
                if (sd != null)
                {
                    if (init.FriendFleet.EscapedShipList.Contains(sd.ID))
                    {
                        Record.BattleResult.MVP |= 1 << (8 + index);
                    }

                    Record.FriendFleet.ShipID[index] = sd.MasterShip.ShipID;
                    Record.FriendFleet.Level[index] = sd.Level;
                    Record.FriendFleet.FirePower[index] = sd.FirepowerTotal;
                    Record.FriendFleet.Torpedo[index] = sd.TorpedoTotal;
                    Record.FriendFleet.AA[index] = sd.AATotal;
                    Record.FriendFleet.Armor[index] = sd.ArmorTotal;

                    Record.FriendFleet.NowHP[index] = hps[index];
                    Record.FriendFleet.MaxHP[index] = maxHps[index];
                    //init.FriendFleet.MembersInstance[index].SlotInstance[0].NameWithLevel
                    if (sd.SlotInstance[0] != null)
                    {
                        Record.FriendFleet.Equipment1[index] = sd.SlotInstance[0].EquipmentID;
                        Record.FriendFleet.EquipmentLevel1[index] = sd.SlotInstance[0].Level == 0 ? sd.SlotInstance[0].AircraftLevel : sd.SlotInstance[0].Level;
                        if (Calculator.IsAircraft(Record.FriendFleet.Equipment1[index], true))
                            Record.FriendFleet.EquipmentLevel1[index] |= (sd.Aircraft[0] << 16);
                    }
                    if (sd.SlotInstance[1] != null)
                    {
                        Record.FriendFleet.Equipment2[index] = sd.SlotInstance[1].EquipmentID;
                        Record.FriendFleet.EquipmentLevel2[index] = sd.SlotInstance[1].Level == 0 ? sd.SlotInstance[1].AircraftLevel : sd.SlotInstance[1].Level;
                        if (Calculator.IsAircraft(Record.FriendFleet.Equipment2[index], true))
                            Record.FriendFleet.EquipmentLevel2[index] |= (sd.Aircraft[1] << 16);
                    }
                    if (sd.SlotInstance[2] != null)
                    {
                        Record.FriendFleet.Equipment3[index] = sd.SlotInstance[2].EquipmentID;
                        Record.FriendFleet.EquipmentLevel3[index] = sd.SlotInstance[2].Level == 0 ? sd.SlotInstance[2].AircraftLevel : sd.SlotInstance[2].Level;
                        if (Calculator.IsAircraft(Record.FriendFleet.Equipment3[index], true))
                            Record.FriendFleet.EquipmentLevel3[index] |= (sd.Aircraft[2] << 16);
                    }
                    if (sd.SlotInstance[3] != null)
                    {
                        Record.FriendFleet.Equipment4[index] = sd.SlotInstance[3].EquipmentID;
                        Record.FriendFleet.EquipmentLevel4[index] = sd.SlotInstance[3].Level == 0 ? sd.SlotInstance[3].AircraftLevel : sd.SlotInstance[3].Level;
                        if (Calculator.IsAircraft(Record.FriendFleet.Equipment4[index], true))
                            Record.FriendFleet.EquipmentLevel4[index] |= (sd.Aircraft[3] << 16);
                    }

                }
                else
                {
                    Record.FriendFleet.ShipID[index] = -1;
                }
            }
            if (hps.Length > 12)
                for (int index = 0; index < 6; index++)
                {
                    ShipData sd = init.AccompanyFleet.MembersInstance[index];
                    if (sd != null)
                    {
                        if (init.AccompanyFleet.EscapedShipList.Contains(sd.ID))
                        {
                            Record.BattleResult.MVP |= 1 << (16 + index);
                        }

                        Record.AccompanyFleet.ShipID[index] = sd.MasterShip.ShipID;
                        Record.AccompanyFleet.Level[index] = sd.Level;
                        Record.AccompanyFleet.FirePower[index] = sd.FirepowerTotal;
                        Record.AccompanyFleet.Torpedo[index] = sd.TorpedoTotal;
                        Record.AccompanyFleet.AA[index] = sd.AATotal;
                        Record.AccompanyFleet.Armor[index] = sd.ArmorTotal;

                        Record.AccompanyFleet.NowHP[index] = hps[index + 12];
                        Record.AccompanyFleet.MaxHP[index] = maxHps[index + 12];

                        if (sd.SlotInstance[0] != null)
                        {
                            //sd.SlotInstanceMaster
                            Record.AccompanyFleet.Equipment1[index] = sd.SlotInstance[0].EquipmentID;
                            Record.AccompanyFleet.EquipmentLevel1[index] = sd.SlotInstance[0].Level == 0 ? sd.SlotInstance[0].AircraftLevel : sd.SlotInstance[0].Level;
                            if (Calculator.IsAircraft(Record.AccompanyFleet.Equipment1[index], true))
                                Record.AccompanyFleet.EquipmentLevel1[index] |= (sd.Aircraft[0] << 16);
                        }
                        if (sd.SlotInstance[1] != null)
                        {
                            Record.AccompanyFleet.Equipment2[index] = sd.SlotInstance[1].EquipmentID;
                            Record.AccompanyFleet.EquipmentLevel2[index] = sd.SlotInstance[1].Level == 0 ? sd.SlotInstance[1].AircraftLevel : sd.SlotInstance[1].Level;
                            if (Calculator.IsAircraft(Record.AccompanyFleet.Equipment2[index], true))
                                Record.AccompanyFleet.EquipmentLevel2[index] |= (sd.Aircraft[1] << 16);
                        }
                        if (sd.SlotInstance[2] != null)
                        {
                            Record.AccompanyFleet.Equipment3[index] = sd.SlotInstance[2].EquipmentID;
                            Record.AccompanyFleet.EquipmentLevel3[index] = sd.SlotInstance[2].Level == 0 ? sd.SlotInstance[2].AircraftLevel : sd.SlotInstance[2].Level;
                            if (Calculator.IsAircraft(Record.AccompanyFleet.Equipment3[index], true))
                                Record.AccompanyFleet.EquipmentLevel3[index] |= (sd.Aircraft[2] << 16);
                        }
                        if (sd.SlotInstance[3] != null)
                        {
                            Record.AccompanyFleet.Equipment4[index] = sd.SlotInstance[3].EquipmentID;
                            Record.AccompanyFleet.EquipmentLevel4[index] = sd.SlotInstance[3].Level == 0 ? sd.SlotInstance[3].AircraftLevel : sd.SlotInstance[3].Level;
                            if (Calculator.IsAircraft(Record.AccompanyFleet.Equipment4[index], true))
                                Record.AccompanyFleet.EquipmentLevel4[index] |= (sd.Aircraft[3] << 16);
                        }
                    }
                    else
                    {
                        Record.AccompanyFleet.ShipID[index] = -1;
                    }
                }
            else
            {
                if (bm.BattleMode == BattleManager.BattleModes.Practice)
                {
                    for (int index = 0; index < 6; index++)
                    {
                        var sd = init.EnemyMembersInstance[index];
                        if (sd != null)
                        {
                            Record.AccompanyFleet.ShipID[index] = sd.ShipID;
                            Record.AccompanyFleet.Level[index] = init.EnemyLevels[index];
                            Record.AccompanyFleet.FirePower[index] = init.EnemyParameters[index][0];
                            Record.AccompanyFleet.Torpedo[index] = init.EnemyParameters[index][1];
                            Record.AccompanyFleet.AA[index] = init.EnemyParameters[index][2];
                            Record.AccompanyFleet.Armor[index] = init.EnemyParameters[index][3];

                            if (init.EnemySlotsInstance[index][0] != null)
                            {
                                //sd.SlotInstanceMaster
                                Record.AccompanyFleet.Equipment1[index] = init.EnemySlotsInstance[index][0].EquipmentID;
                                //Record.AccompanyFleet.EquipmentLevel1[index] = sd.SlotInstance[0].Level == 0 ? sd.SlotInstance[0].AircraftLevel : sd.SlotInstance[0].Level;
                            }
                            if (init.EnemySlotsInstance[index][1] != null)
                            {
                                Record.AccompanyFleet.Equipment2[index] = init.EnemySlotsInstance[index][1].EquipmentID;
                                //Record.AccompanyFleet.EquipmentLevel2[index] = sd.SlotInstance[1].Level == 0 ? sd.SlotInstance[1].AircraftLevel : sd.SlotInstance[1].Level;
                            }
                            if (init.EnemySlotsInstance[index][2] != null)
                            {
                                Record.AccompanyFleet.Equipment3[index] = init.EnemySlotsInstance[index][2].EquipmentID;
                                //Record.AccompanyFleet.EquipmentLevel3[index] = sd.SlotInstance[2].Level == 0 ? sd.SlotInstance[2].AircraftLevel : sd.SlotInstance[2].Level;
                            }
                            if (init.EnemySlotsInstance[index][3] != null)
                            {
                                Record.AccompanyFleet.Equipment4[index] = init.EnemySlotsInstance[index][3].EquipmentID;
                                //Record.AccompanyFleet.EquipmentLevel4[index] = sd.SlotInstance[3].Level == 0 ? sd.SlotInstance[3].AircraftLevel : sd.SlotInstance[3].Level;
                            }
                        }
                    }
                }
                else
                    Record.AccompanyFleet.ShipID[0] = -1;
            }


            // day
            {
                Record.AirBattle1.AirSuperiority = -1;
                Record.AirBattle2.AirSuperiority = -1;
                if (day != null && day.IsAvailable)
                {
                    var pd1 = day.AirBattle;
                    var pd2 = (day is BattleAirBattle ? ((BattleAirBattle)day).AirBattle2 : null);

                    bool[] s1available = { pd1.IsStage1Available, (pd2 != null && pd2.IsStage1Available) };
                    bool[] s2available = { pd1.IsStage2Available, (pd2 != null && pd2.IsStage2Available) };
                    int[] touches =
						{
							s1available[0] ? pd1.TouchAircraftFriend : -1, s1available[1] ? pd2.TouchAircraftFriend : -1,
							s1available[0] ? pd1.TouchAircraftEnemy : -1, s1available[1] ? pd2.TouchAircraftEnemy : -1
						};
                    //pd1.
                    Record.AirBattle1.FriendTouch = touches[0];
                    Record.AirBattle1.EnemyTouch = touches[2];
                    Record.AirBattle2.FriendTouch = touches[1];
                    Record.AirBattle2.EnemyTouch = touches[3];

                    bool[] fire = new bool[2];
                    // pd1.IsAACutinAvailable, s1available[1] && pd2.IsAACutinAvailable
                    try
                    {
                        fire[0] = pd1.IsAACutinAvailable;

                        if (s1available[1])
                        {
                            if (pd2 != null)
                                if (pd2.IsAACutinAvailable)
                                    fire[1] = true;
                        }

                        int[] cutinID = new int[]
						{
							fire[0] ? pd1.AACutInKind : -1,
							fire[1] ? pd2.AACutInKind : -1,
						};
                        Record.AirBattle1.AirSuperiority = pd1.AirSuperiority;
                        Record.AirBattle1.AACutInKind = cutinID[0];
                        if (cutinID[0] >= 0)
                            Record.AirBattle1.AACutInID = pd1.AACutInShip.ShipID;

                        Record.AirBattle1.FriendLostS1 = pd1.AircraftLostStage1Friend;
                        Record.AirBattle1.FriendTotalS1 = pd1.AircraftTotalStage1Friend;
                        Record.AirBattle1.EnemyLostS1 = pd1.AircraftLostStage1Enemy;
                        Record.AirBattle1.EnemyTotalS1 = pd1.AircraftTotalStage1Enemy;
                        if (pd1.IsStage2Available)
                        {
                            Record.AirBattle1.FriendLostS2 = pd1.AircraftLostStage2Friend;
                            Record.AirBattle1.FriendTotalS2 = pd1.AircraftTotalStage2Friend;
                            Record.AirBattle1.EnemyLostS2 = pd1.AircraftLostStage2Enemy;
                            Record.AirBattle1.EnemyTotalS2 = pd1.AircraftTotalStage2Enemy;
                        }
                        if (pd2 != null)
                        {
                            Record.AirBattle2.AirSuperiority = pd2.AirSuperiority;
                            Record.AirBattle2.AACutInKind = cutinID[1];

                            if (cutinID[1] >= 0)
                                Record.AirBattle2.AACutInID = pd2.AACutInShip.ShipID;

                            Record.AirBattle2.FriendLostS1 = pd2.AircraftLostStage1Friend;
                            Record.AirBattle2.FriendTotalS1 = pd2.AircraftTotalStage1Friend;
                            Record.AirBattle2.EnemyLostS1 = pd2.AircraftLostStage1Enemy;
                            Record.AirBattle2.EnemyTotalS1 = pd2.AircraftTotalStage1Enemy;
                            if (pd2.IsStage2Available)
                            {
                                Record.AirBattle2.FriendLostS2 = pd2.AircraftLostStage2Friend;
                                Record.AirBattle2.FriendTotalS2 = pd2.AircraftTotalStage2Friend;
                                Record.AirBattle2.EnemyLostS2 = pd2.AircraftLostStage2Enemy;
                                Record.AirBattle2.EnemyTotalS2 = pd2.AircraftTotalStage2Enemy;
                            }
                        }
                    }
                    catch
                    {

                    }
                    //day.AirBattle.StageFlag
                    // 航空战血量变化
                    if (day.AirBattle.IsAvailable && day.AirBattle.IsStage3Available)
                    {
                        //day.AirBattle.
                        var stage3 = day.RawData.api_kouku.api_stage3;
                        //int[] flagsfriend = ( (int[])stage3.api_frai_flag ).Skip( 1 ).Concat( ( (int[])stage3.api_fbak_flag ).Skip( 1 ) ).ToArray();
                        //int[] flagsenemy = ( (int[])stage3.api_erai_flag ).Skip( 1 ).Concat( ( (int[])stage3.api_ebak_flag ).Skip( 1 ) ).ToArray();
                        //FillAirDamage( builder, flagsfriend, flagsenemy, day.AirBattle.Damages, friends, isCombined ? accompany : null, enemys, hps, maxHps );
                        int[] flagsfriend1 = (int[])stage3.api_frai_flag;//舰攻
                        int[] flagsfriend2 = (int[])stage3.api_fbak_flag;//舰爆 水爆
                        int[] flagsfriend3 = (int[])stage3.api_fcl_flag;//暴击
                        int[] flagsenemy1 = (int[])stage3.api_erai_flag;
                        int[] flagsenemy2 = (int[])stage3.api_ebak_flag;
                        int[] flagsenemy3 = (int[])stage3.api_ecl_flag;

                        for (int index = 0; index < 6; index++)
                        {
                            int flag = 0;
                            if (flagsfriend1[index + 1] > 0)
                                flag |= 0x10000;
                            if (flagsfriend2[index + 1] > 0)
                                flag |= 0x20000;
                            if (flagsfriend3[index + 1] > 0)
                                flag |= 0x40000;
                            Record.AirBattle1.Damages[index] = day.AirBattle.Damages[index] | flag;
                        }
                        for (int index = 6; index < 12; index++)
                        {
                            int flag = 0;
                            if (flagsenemy1[index - 5] > 0)
                                flag |= 0x10000;
                            if (flagsenemy2[index - 5] > 0)
                                flag |= 0x20000;
                            if (flagsenemy3[index - 5] > 0)
                                flag |= 0x40000;
                            Record.AirBattle1.Damages[index] = day.AirBattle.Damages[index] | flag;
                        }
                        if (hps.Length > 12)
                            for (int index = 12; index < 18; index++)
                            {
                                int flag = 0;
                                if (flagsfriend1[index -11] > 0)
                                    flag |= 0x10000;
                                if (flagsfriend2[index - 11] > 0)
                                    flag |= 0x20000;
                                if (flagsfriend3[index - 11] > 0)
                                    flag |= 0x40000;
                                Record.AirBattle1.Damages[index] = day.AirBattle.Damages[index] | flag;
                            }

                        if (pd2 != null)
                        {
                            if (pd2.IsStage3Available)
                            {
                                var stage32 = day.RawData.api_kouku2.api_stage3;
                                int[] flagsfriend12 = (int[])stage32.api_frai_flag;//舰攻
                                int[] flagsfriend22 = (int[])stage32.api_fbak_flag;//舰爆 水爆
                                int[] flagsfriend32 = (int[])stage32.api_fcl_flag;//暴击
                                int[] flagsenemy12 = (int[])stage32.api_erai_flag;
                                int[] flagsenemy22 = (int[])stage32.api_ebak_flag;
                                int[] flagsenemy32 = (int[])stage32.api_ecl_flag;
                                int[] FriendDam = (int[])stage32.api_fdam;//友方被
                                int[] EnemyDam = (int[])stage32.api_edam;

                                for (int index = 0; index < 6; index++)
                                {
                                    int flag = 0;
                                    if (flagsfriend12[index + 1] > 0)
                                        flag |= 0x10000;
                                    if (flagsfriend22[index + 1] > 0)
                                        flag |= 0x20000;
                                    if (flagsfriend32[index + 1] > 0)
                                        flag |= 0x40000;
                                    Record.AirBattle2.Damages[index] = FriendDam[index + 1] | flag;
                                }
                                for (int index = 6; index < 12; index++)
                                {
                                    int flag = 0;
                                    if (flagsenemy1[index - 5] > 0)
                                        flag |= 0x10000;
                                    if (flagsenemy2[index - 5] > 0)
                                        flag |= 0x20000;
                                    if (flagsenemy2[index - 5] > 0)
                                        flag |= 0x40000;
                                    Record.AirBattle2.Damages[index] = EnemyDam[index - 5] | flag;
                                }
                                if (hps.Length > 12 && FriendDam.Length > 12)
                                    for (int index = 12; index < 18; index++)
                                    {
                                        int flag = 0;
                                        if (flagsfriend1[index - 11] > 0)
                                            flag |= 0x10000;
                                        if (flagsfriend2[index - 11] > 0)
                                            flag |= 0x20000;
                                        if (flagsfriend3[index - 11] > 0)
                                            flag |= 0x40000;
                                        Record.AirBattle2.Damages[index] = FriendDam[index - 11] | flag;
                                    }
                            }
                        }
                    }


                    // 支援
                    if (day.Support != null && day.Support.SupportFlag > 0)
                    {
                        Record.Support.SupportFlag = day.Support.SupportFlag;
                        //1 航空支援   2,3 炮雷支援
                        for (int index = 0; index < 6; index++)
                        {
                            ShipData sd = day.Support.SupportFleet.MembersInstance[index];
                            if (sd != null)
                                Record.Support.Supporter[index] = sd.ShipID;
                            else
                                Record.Support.Supporter[index] = -1;

                            switch (day.Support.SupportFlag)
                            {
                                case 1:
                                    Record.Support.Damages[index] = day.Support.AirRaidDamages[index];
                                    break;
                                case 2:
                                case 3:
                                    Record.Support.Damages[index] = day.Support.ShellingTorpedoDamages[index];
                                    break;
                            }
                        }


                    }
                    else
                        Record.Support.SupportFlag = -1;




                    Record.OpenTorpedoBattle.IsAvailable = -1;
                    if (day.OpeningTorpedo != null && day.OpeningTorpedo.IsAvailable)
                    {
                        //day.OpeningTorpedo.
                        Record.OpenTorpedoBattle.IsAvailable = 1;
                        int[] friendTarget = ((int[])day.OpeningTorpedo.TorpedoData.api_frai).Skip(1).ToArray();
                        int[] friendDamages = ((int[])day.OpeningTorpedo.TorpedoData.api_fydam).Skip(1).ToArray();
                        int[] friendFlags = ((int[])day.OpeningTorpedo.TorpedoData.api_fcl).Skip(1).ToArray();
                        for (int i = 0; i < 6; i++)
                        {
                            if (friendTarget[i] > 0)
                            {
                                int flag = 0;
                                if (friendFlags[i] == 1)
                                    flag |= 0x10000;
                                if (friendFlags[i] == 2)
                                    flag |= 0x20000;
                                Record.OpenTorpedoBattle.FriendDamages[i] = friendDamages[i] | flag;
                                Record.OpenTorpedoBattle.FriendTarget[i] = friendTarget[i] - 1;
                            }
                            else
                                Record.OpenTorpedoBattle.FriendTarget[i] = -1;
                        }

                        int[] enemyTarget = ((int[])day.OpeningTorpedo.TorpedoData.api_erai).Skip(1).ToArray();
                        int[] enemyDamages = ((int[])day.OpeningTorpedo.TorpedoData.api_eydam).Skip(1).ToArray();
                        int[] enemyFlags = ((int[])day.OpeningTorpedo.TorpedoData.api_ecl).Skip(1).ToArray();

                        for (int i = 0; i < 6; i++)
                        {
                            if (enemyTarget[i] > 0)
                            {
                                int flag = 0;
                                if (enemyFlags[i] == 1)
                                    flag |= 0x10000;
                                if (enemyFlags[i] == 2)
                                    flag |= 0x20000;
                                Record.OpenTorpedoBattle.EnemyDamages[i] = enemyDamages[i] | flag;
                                Record.OpenTorpedoBattle.EnemyTarget[i] = enemyTarget[i] - 1;
                            }
                            else
                                Record.OpenTorpedoBattle.EnemyTarget[i] = -1;
                        }
                        //Record.OpenTorpedoBattle.Damages
                        //FillTorpedoDamage( "开幕雷击", builder, day.OpeningTorpedo.TorpedoData, isCombined ? accompany : friends, enemys, hps, maxHps );
                    }

                    Record.ShellBattle1.IsAvailable = -1;
                    // 炮击战
                    if (day.Shelling1 != null && day.Shelling1.IsAvailable)
                    {
                        Record.ShellBattle1.IsAvailable = 1;
                        var data = day.Shelling1.ShellingData;
                        int[] at_list = (int[])data.api_at_list;
                        int[] at_type = (int[])data.api_at_type;
                        int index = 0;
                        for (int i = 1; i < at_list.Length; i++)
                        {
                            int from = at_list[i] - 1;
                            int[] enemy_list = (int[])data.api_df_list[i];

                            int[] equips = (int[])data.api_si_list[i];
                            int[] flags = (int[])data.api_cl_list[i];
                            int[] damages = (int[])data.api_damage[i];

                            for (int j = 0; j < enemy_list.Length; j++)
                            {
                                int to = enemy_list[j] - 1;

                                Record.ShellBattle1.Attacker[index] = from;
                                Record.ShellBattle1.Target[index] = to;
                                int flag = 0;
                                if (flags[j] == 1)
                                    flag |= 0x10000;
                                if (flags[j] == 2)
                                    flag |= 0x20000;
                                if (at_type[i] > 0)
                                    flag |= at_type[i] << 18;
                                //0=通常, 1=レーザー攻撃, 2=連撃, 3=カットイン(主砲/副砲), 4=カットイン(主砲/電探), 5=カットイン(主砲/徹甲), 6=カットイン(主砲/主砲)
                                Record.ShellBattle1.Damages[index] = damages[j] | flag;
                                index++;
                            }

                        }
                    }


                    // 闭幕雷击
                    Record.CloseTorpedoBattle.IsAvailable = -1;
                    if (day.Torpedo != null && day.Torpedo.IsAvailable)
                    {
                        Record.CloseTorpedoBattle.IsAvailable = 1;
                        int[] friendTarget = ((int[])day.Torpedo.TorpedoData.api_frai).Skip(1).ToArray();
                        int[] friendDamages = ((int[])day.Torpedo.TorpedoData.api_fydam).Skip(1).ToArray();
                        int[] friendFlags = ((int[])day.Torpedo.TorpedoData.api_fcl).Skip(1).ToArray();
                        for (int i = 0; i < 6; i++)
                        {
                            if (friendTarget[i] > 0)
                            {
                                int flag = 0;
                                if (friendFlags[i] == 1)
                                    flag |= 0x10000;
                                if (friendFlags[i] == 2)
                                    flag |= 0x20000;
                                Record.CloseTorpedoBattle.FriendDamages[i] = friendDamages[i] | flag;
                                Record.CloseTorpedoBattle.FriendTarget[i] = friendTarget[i] - 1;
                            }
                            else
                                Record.CloseTorpedoBattle.FriendTarget[i] = -1;
                        }

                        int[] enemyTarget = ((int[])day.Torpedo.TorpedoData.api_erai).Skip(1).ToArray();
                        int[] enemyDamages = ((int[])day.Torpedo.TorpedoData.api_eydam).Skip(1).ToArray();
                        int[] enemyFlags = ((int[])day.Torpedo.TorpedoData.api_ecl).Skip(1).ToArray();

                        for (int i = 0; i < 6; i++)
                        {
                            if (enemyTarget[i] > 0)
                            {
                                int flag = 0;
                                if (enemyFlags[i] == 1)
                                    flag |= 0x10000;
                                if (enemyFlags[i] == 2)
                                    flag |= 0x20000;
                                Record.CloseTorpedoBattle.EnemyDamages[i] = enemyDamages[i] | flag;
                                Record.CloseTorpedoBattle.EnemyTarget[i] = enemyTarget[i] - 1;
                            }
                            else
                                Record.CloseTorpedoBattle.EnemyTarget[i] = -1;
                        }
                    }

                    Record.ShellBattle2.IsAvailable = -1;
                    if (day.Shelling2 != null && day.Shelling2.IsAvailable)
                    {
                        Record.ShellBattle2.IsAvailable = 1;
                        var data = day.Shelling2.ShellingData;
                        int[] at_list = (int[])data.api_at_list;
                        int[] at_type = (int[])data.api_at_type;
                        int index = 0;
                        for (int i = 1; i < at_list.Length; i++)
                        {
                            int from = at_list[i] - 1;
                            int[] enemy_list = (int[])data.api_df_list[i];
                            int[] equips = (int[])data.api_si_list[i];
                            int[] flags = (int[])data.api_cl_list[i];
                            int[] damages = (int[])data.api_damage[i];

                            for (int j = 0; j < enemy_list.Length; j++)
                            {
                                int to = enemy_list[j] - 1;

                                Record.ShellBattle2.Attacker[index] = from;
                                Record.ShellBattle2.Target[index] = to;
                                int flag = 0;
                                if (flags[j] == 1)
                                    flag |= 0x10000;
                                if (flags[j] == 2)
                                    flag |= 0x20000;
                                if (at_type[i] > 0)
                                    flag |= at_type[i] << 18;
                                Record.ShellBattle2.Damages[index] = damages[j] | flag;
                                index++;
                            }

                        }
                    }

                    Record.ShellBattle3.IsAvailable = -1;
                    if (day.Shelling3 != null && day.Shelling3.IsAvailable)
                    {
                        Record.ShellBattle3.IsAvailable = 1;
                        var data = day.Shelling3.ShellingData;
                        int[] at_list = (int[])data.api_at_list;
                        int[] at_type = (int[])data.api_at_type;
                        int index = 0;
                        for (int i = 1; i < at_list.Length; i++)
                        {
                            int from = at_list[i] - 1;
                            int[] enemy_list = (int[])data.api_df_list[i];
                            int[] equips = (int[])data.api_si_list[i];
                            int[] flags = (int[])data.api_cl_list[i];
                            int[] damages = (int[])data.api_damage[i];

                            for (int j = 0; j < enemy_list.Length; j++)
                            {
                                int to = enemy_list[j] - 1;

                                Record.ShellBattle3.Attacker[index] = from;
                                Record.ShellBattle3.Target[index] = to;
                                int flag = 0;
                                if (flags[j] == 1)
                                    flag |= 0x10000;
                                if (flags[j] == 2)
                                    flag |= 0x20000;
                                if (at_type[i] > 0)
                                    flag |= at_type[i] << 18;
                                Record.ShellBattle3.Damages[index] = damages[j] | flag;
                                index++;
                            }

                        }
                    }
                }
            }

            // night
            {
                Record.NightBattle.IsAvailable = -1;
                var night = bm.BattleNight;
                if (night != null && night.IsAvailable)
                {

                    var nightbattle = night.NightBattle;
                    Record.NightBattle.IsAvailable = 1;
                    Record.NightInformation.FriendSearchlight = nightbattle.SearchlightIndexFriend;
                    Record.NightInformation.EnemySearchlight = nightbattle.SearchlightIndexEnemy;
                    Record.NightInformation.FriendTouchAircraft = nightbattle.TouchAircraftFriend;
                    Record.NightInformation.EnemyTouchAircraft = nightbattle.TouchAircraftEnemy;
                    Record.NightInformation.FriendFlare = nightbattle.FlareIndexFriend;
                    Record.NightInformation.EnemyFlare = nightbattle.FlareIndexEnemy;

                    // 战况
                    if (nightbattle.ShellingData != null)
                    {
                        var data = nightbattle.ShellingData;
                        int[] at_list = (int[])data.api_at_list;
                        int[] sp_list = (int[])data.api_sp_list;
                        int index = 0;
                        for (int i = 1; i < at_list.Length; i++)
                        {
                            int from = at_list[i] - 1;
                            int[] enemy_list = (int[])data.api_df_list[i];
                            int[] equips = (int[])data.api_si_list[i];
                            int[] flags = (int[])data.api_cl_list[i];
                            int[] damages = (int[])data.api_damage[i];

                            // 0=通常攻撃, 1=連撃, 2=カットイン(主砲/魚雷), 3=カットイン(魚雷/魚雷), 4=カットイン(主砲/副砲), 5=カットイン(主砲/主砲)
                            // 1 2 3 4 都是2次攻击     5是三次攻击！但是后2次目标均为-1 伤害也是没有效果  所以略过
                            for (int j = 0; j < enemy_list.Length; j++)
                            {
                                int to = enemy_list[j] - 1;
                                if (to < 0)
                                {
                                    continue;
                                }
                                Record.NightBattle.Attacker[index] = from;
                                Record.NightBattle.Target[index] = to;
                                int flag = 0;
                                if (flags[j] == 1)
                                    flag |= 0x10000;
                                if (flags[j] == 2)
                                    flag |= 0x20000;
                                if (sp_list[i] > 0)
                                    flag |= sp_list[i] << 18;
                                Record.NightBattle.Damages[index] = damages[j] | flag;
                                index++;
                            }

                        }
                    }

                }
            }

            Save(&Record);
        }

        unsafe static byte[] SaveMyStruct(BattleRecord* st)
        {
            int len = sizeof(BattleRecord);
            //MessageBox.Show(len.ToString());
            byte[] buf = new byte[len];
            byte* p = (byte*)st;
            for (int i = 0; i < len; i++)
            {
                buf[i] = *p++;
            }
            return buf;
        }
    }

    public struct BattleRecord
    {
        public DateTime BattleTime;
        public BattleResultRecord BattleResult;
        public FriendFleetRecord FriendFleet;
        public FriendFleetRecord AccompanyFleet;
        public EnemyFleetRecord EnemyFleet;
        public int BattleMode;

        public int SearchingFriend;
        public int SearchingEnemy;
        /// <summary>
        /// [0|1]：1=単縦陣 2=複縦陣, 3=輪形陣, 4=梯形陣, 5=単横陣 11-14 1-4阵形
        /// </summary>
        public int FriendFormation;
        /// <summary>
        /// [0|1]：1=単縦陣 2=複縦陣, 3=輪形陣, 4=梯形陣, 5=単横陣 11-14 1-4阵形
        /// </summary>
        public int EnemyFormation;
        /// <summary>
        /// 1=同航戦, 2=反航戦, 3=T字有利, 4=T字不利
        /// </summary>
        public int T_Status;


        public AirBattleRecord AirBattle1;
        public AirBattleRecord AirBattle2;
        public SupportRecord Support;
        public TorpedoRecord OpenTorpedoBattle;

        public ShellingRecord ShellBattle1;
        public ShellingRecord ShellBattle2;
        public ShellingRecord ShellBattle3;

        public TorpedoRecord CloseTorpedoBattle;

        public NightRecord NightInformation;
        public ShellingRecord NightBattle;
    }

    public struct BattleResultRecord
    {
        public int DroppedShipID;
        /// <summary>
        /// //其实是个字符 SABCD
        /// </summary>
        public int Rank;
        /// <summary>
        /// //1-6
        /// </summary>
        public int MVP;
        /// <summary>
        /// 3-2-2 第一个数字
        /// </summary>
        public int AreaID;
        /// <summary>
        ///  3-2-2 第2个数字
        /// </summary>
        public int InfoID;
        /// <summary>
        ///  3-2-2 第3个数字
        /// </summary>
        public int CellID;
    }

    unsafe public struct FriendFleetRecord
    {
        public fixed int ShipID[6];
        public fixed int Level[6];
        public fixed int MaxHP[6];
        public fixed int NowHP[6];

        /// <summary>
        /// 装备后数值
        /// </summary>
        public fixed int FirePower[6];
        /// <summary>
        /// 装备后数值
        /// </summary>
        public fixed int Torpedo[6];
        /// <summary>
        /// 装备后数值
        /// </summary>
        public fixed int AA[6];
        /// <summary>
        /// 装备后数值
        /// </summary>
        public fixed int Armor[6];

        public fixed int Equipment1[6];
        public fixed int Equipment2[6];
        public fixed int Equipment3[6];
        public fixed int Equipment4[6];
        //public fixed int Equipment5[6];

        public fixed int EquipmentLevel1[6];
        public fixed int EquipmentLevel2[6];
        public fixed int EquipmentLevel3[6];
        public fixed int EquipmentLevel4[6];
    }

    unsafe public struct EnemyFleetRecord
    {
        public fixed int ShipID[6];
        public fixed int MaxHP[6];
        public fixed int NowHP[6];
    }

    unsafe public struct AirBattleRecord
    {
        /// <summary>
        /// -1:没有航空站 0=制空均衡, 1=制空権確保, 2=航空優勢, 3=航空劣勢, 4=制空権喪失
        /// </summary>
        public int AirSuperiority;
        public int FriendTouch;
        public int EnemyTouch;
        public int AACutInID;
        /// <summary>
        /// -1:没有对空ci
        /// 1：高角砲x2/電探
        ///	 2：高角砲/電探
        ///		 3：高角砲x2
        ///	 4：大口径主砲/三式弾/高射装置/電探
        ///			 5：高角砲+高射装置x2/電探
        ///	 6：大口径主砲/三式弾/高射装置
        ///		 7：高角砲/高射装置/電探
        ///		 8：高角砲+高射装置/電探
        ///		 9：高角砲/高射装置
        ///					10：高角砲/集中機銃/電探
        ///			11：高角砲/集中機銃
        ///					12：集中機銃/機銃/電探
        /// </summary>
        public int AACutInKind;
        public int FriendTotalS1;
        public int EnemyTotalS1;
        public int FriendLostS1;
        public int EnemyLostS1;
        public int FriendTotalS2;
        public int EnemyTotalS2;
        public int FriendLostS2;
        public int EnemyLostS2;
        public fixed int Damages[18];
    }

    unsafe public struct SupportRecord
    {
        /// <summary>
        /// -1为没有支援  0=到着せず, 1=空撃?, 2=砲撃, 3=雷撃?
        /// </summary>
        public int SupportFlag;
        public fixed int Supporter[6];
        public fixed int Damages[6];
    }

    unsafe public struct TorpedoRecord
    {
        public int IsAvailable;
        /// <summary>
        /// 低16位 伤害值   高16位 最低位1:命中  第二位2：暴击
        /// </summary>
        public fixed int FriendDamages[6];
        /// <summary>
        ///  //雷击战目标全是0-5，敌我全是     -1：没有攻击
        /// </summary>
        public fixed int FriendTarget[6];
        /// <summary>
        /// 低16位 伤害值   高16位 最低位1:命中  第二位2：暴击
        /// </summary>
        public fixed int EnemyDamages[6];
        /// <summary>
        ///  //雷击战目标全是0-5，敌我全是     -1：没有攻击
        /// </summary>
        public fixed int EnemyTarget[6];


    }

    unsafe public struct ShellingRecord
    {
        public int IsAvailable;
        public fixed int Attacker[24];
        /// <summary>
        /// 低16位 伤害值   高16位 最低位1:命中  第二位2：暴击  其他3-5位：特殊攻击
        /// </summary>
        public fixed int Damages[24];
        /// <summary>
        /// 0-5 自己 6-11敌人 12+ 联合舰队2队
        /// </summary>
        public fixed int Target[24];
    }

    public struct NightRecord
    {
        public int FriendSearchlight;
        public int EnemySearchlight;
        public int FriendTouchAircraft;
        public int EnemyTouchAircraft;
        public int FriendFlare;
        public int EnemyFlare;
    }
}
