using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Support;

namespace MissionHelper
{

    public class MissionData
    {
        public MissionData()
        {
            InitData();
        }

        public static MissionData missionData = new MissionData();

        void InitData()
        {
            MissionInformation mi = AddMission(1, "練習航海", 15, 30, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(1, 1, 2);

            mi = AddMission(2, "長距離練習航海", 30, 50, 0, 0, 100, 30, 0, 0, 0, 0, 1, 0, 0, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(2, 1, 4);

            mi = AddMission(3, "警備任務", 20, 30, 20, 30, 30, 40, 0, 0, 0, 0, 0, 0, 0, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 1, 3);

            mi = AddMission(4, "対潜警戒任務", 50, 50, 0, 0, 60, 0, 0, 0, 0, 1, 1, 0, 0, "轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 1, 3, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(5, "海上護衛任務", 90, 50, 0, 200, 200, 20, 20, 0, 0, 0, 0, 0, 0, "轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 1, 4, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(6, "防空射撃演習", 40, 30, 20, 0, 0, 0, 80, 0, 0, 1, 0, 0, 0, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(4, 1, 4);

            mi = AddMission(7, "観艦式予行", 60, 50, 0, 0, 0, 50, 30, 0, 0, 0, 0, 0, 1, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(5, 1, 6);

            mi = AddMission(8, "観艦式", 180, 50, 20, 50, 100, 50, 50, 0, 0, 0, 0, 1, 2, "");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(6, 1, 6);

            mi = AddMission(9, "タンカー護衛任務", 240, 50, 0, 350, 0, 0, 0, 0, 0, 1, 2, 0, 0, "轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 1, 4, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(10, "強行偵察任務", 90, 30, 0, 0, 50, 0, 30, 0, 0, 0, 1, 0, 1, "轻2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 1, 3, new ShipRequirement(ShipTN.cl, 2));

            mi = AddMission(11, "ボーキサイト輸送任務", 300, 50, 0, 0, 0, 0, 250, 0, 0, 1, 1, 0, 0, "驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(6, 1, 4, new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(12, "資源輸送任務", 480, 50, 0, 50, 250, 200, 50, 0, 1, 0, 0, 1, 0, "驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(4, 1, 4, new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(13, "鼠輸送作戦", 240, 50, 40, 240, 300, 0, 0, 0, 0, 1, 2, 0, 0, "轻1驱4");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(5, 1, 6, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 4));

            mi = AddMission(14, "包囲陸戦隊撤収作戦", 360, 50, 0, 0, 240, 200, 0, 0, 0, 0, 1, 1, 0, "轻1驱3");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(6, 1, 6, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 3));

            mi = AddMission(15, "囮機動部隊支援作戦", 720, 50, 40, 0, 0, 300, 400, 1, 0, 0, 0, 1, 0, "空母2驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(9, 1, 6, new ShipRequirement(new string[] { ShipTN.cv, ShipTN.cvl, ShipTN.av, ShipTN.cva }, 2), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(16, "艦隊決戦援護作戦", 900, 50, 40, 500, 500, 200, 200, 0, 0, 0, 0, 2, 2, "轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(11, 1, 6, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(17, "敵地偵察作戦", 45, 30, 40, 70, 70, 50, 0, 0, 0, 0, 0, 0, 0, "轻1驱3");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(20, 1, 6, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 3));

            mi = AddMission(18, "航空機輸送作戦", 300, 50, 20, 0, 0, 300, 100, 0, 0, 0, 1, 0, 0, "空母3驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(15, 1, 6, new ShipRequirement(new string[] { ShipTN.cv, ShipTN.cvl, ShipTN.av, ShipTN.cva }, 3), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(19, "北号作戦", 360, 50, 40, 400, 0, 50, 30, 0, 0, 1, 0, 1, 0, "航战2驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(20, 1, 6, new ShipRequirement(ShipTN.bbv, 2), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(20, "潜水艦哨戒任務", 120, 50, 40, 0, 0, 150, 0, 0, 0, 1, 0, 1, 0, "潜艇1轻1");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(1, 1, 2, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 1), new ShipRequirement(ShipTN.cl, 1));

            mi = AddMission(21, "北方鼠輸送作戦", 140, 80, 70, 320, 270, 0, 0, 0, 0, 1, 0, 0, 0, "轻1驱4 3船3桶");
            mi.AddSpecialRequirement(3, 3, 4, 3, 4);
            mi.shipRequirements = new ShipRequirements(15, 30, 5, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 4));

            mi = AddMission(22, "艦隊演習", 180, 80, 70, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, "重1轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(30, 45, 6, new ShipRequirement(new string[] { ShipTN.ca, ShipTN.cav }, 1), new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(23, "航空戦艦運用演習", 240, 80, 80, 0, 20, 0, 100, 0, 0, 0, 0, 0, 0, "航战2驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(50, 200, 6, new ShipRequirement(ShipTN.bbv, 2), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(24, "北方航路海上護衛", 500, 80, 60, 500, 0, 0, 150, 0, 0, 0, 1, 2, 0, "轻巡旗舰 驱4");
            mi.AddSpecialRequirement(0, 0, 10, 4, 4);
            mi.shipRequirements = new ShipRequirements(50, 200, 6, new ShipRequirement(ShipTN.cl, 1, true), new ShipRequirement(ShipTN.dd, 4));

            mi = AddMission(25, "通商破壊作戦", 2400, 50, 80, 900, 0, 500, 0, 0, 0, 0, 0, 0, 0, "重2驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(25, 1, 4, new ShipRequirement(new string[] { ShipTN.ca, ShipTN.cav }, 2), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(26, "敵母港空襲作戦", 4800, 80, 80, 0, 0, 0, 900, 0, 0, 0, 3, 0, 0, "空母1轻1驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(30, 1, 4, new ShipRequirement(new string[] { ShipTN.cv, ShipTN.cvl, ShipTN.av, ShipTN.cva }, 1), new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 2));

            mi = AddMission(27, "潜水艦通商破壊作戦", 1200, 80, 80, 0, 0, 800, 0, 0, 0, 2, 0, 1, 0, "潜艇2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(1, 1, 2, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 2));

            mi = AddMission(28, "西方海域封鎖作戦", 1500, 80, 80, 0, 0, 900, 350, 0, 2, 0, 0, 2, 0, "潜艇3");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(30, 1, 3, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 3));

            mi = AddMission(29, "潜水艦派遣演習", 1440, 90, 40, 0, 0, 0, 100, 0, 0, 1, 0, 1, 0, "潜艇3");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(50, 1, 3, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 3));

            mi = AddMission(30, "潜水艦派遣作戦", 2880, 90, 70, 0, 0, 0, 100, 0, 0, 0, 0, 3, 0, "潜艇4");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(55, 1, 4, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 4));

            mi = AddMission(31, "海外艦との接触", 120, 50, 0, 0, 30, 0, 0, 0, 0, 1, 0, 0, 0, "潜艇4");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(60, 200, 4, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 4));

            mi = AddMission(32, "遠洋練習航海", 1440, 90, 30, 50, 50, 50, 50, 1, 0, 0, 0, 3, 0, "练巡旗舰 驱2");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(5, 1, 3, new ShipRequirement(ShipTN.pcl, 1, true), new ShipRequirement(ShipTN.dd, 2));

            //mi = AddMission(33, "前衛支援任務", 15, 10, 10, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, "");
            //mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            //mi.shipRequirements = new ShipRequirements(1, 1, 1, new ShipRequirement("", 1));

            //mi = AddMission(34, "艦隊決戦支援任務", 15, 10, 10, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, "");
            //mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            //mi.shipRequirements = new ShipRequirements(1, 1, 1, new ShipRequirement("", 1));

            mi = AddMission(35, "MO作戦", 420, 80, 80, 0, 0, 240, 280, 0, 0, 2, 0, 1, 0, "空母2重1驱1");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(40, 1, 6, new ShipRequirement(new string[] { ShipTN.cv, ShipTN.cvl, ShipTN.av, ShipTN.cva }, 2), new ShipRequirement(new string[] { ShipTN.ca}, 1), new ShipRequirement(ShipTN.dd, 1));

            mi = AddMission(36, "水上機基地建設", 540, 80, 80, 480, 0, 200, 200, 0, 2, 0, 1, 0, 0, "水母2轻1驱1");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(30, 1, 6, new ShipRequirement(ShipTN.av, 2), new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 1));

            mi = AddMission(37, "東京急行", 165, 80, 80, 0, 380, 270, 0, 0, 0, 1, 0, 0, 0, "轻1驱5 3船4桶");
            mi.AddSpecialRequirement(4, 3, 5, 4, 4);
            mi.shipRequirements = new ShipRequirements(50, 200, 6, new ShipRequirement(ShipTN.cl, 1), new ShipRequirement(ShipTN.dd, 5));

            mi = AddMission(38, "東京急行(弐)", 175, 80, 80, 420, 0, 200, 0, 0, 0, 1, 0, 0, 0, "驱5 四船8桶");
            mi.AddSpecialRequirement(8, 4, 10, 4, 4);
            mi.shipRequirements = new ShipRequirements(65, 240, 6, new ShipRequirement(ShipTN.dd, 5));

            mi = AddMission(39, "遠洋潜水艦作戦", 1800, 90, 90, 0, 0, 300, 0, 0, 1, 0, 2, 0, 0, "大鲸 潜4");
            mi.AddSpecialRequirement(0, 0, 0, 0, 0);
            mi.shipRequirements = new ShipRequirements(3, 180, 5, new ShipRequirement(new string[] { ShipTN.ss, ShipTN.ssv }, 4), new ShipRequirement(ShipTN.ssm, 1));

            mi = AddMission(40, "水上機前線輸送", 410, 80, 70, 300, 300, 0, 100, 0, 0, 3, 1, 0, 0, "轻巡旗舰 水母2驱2");
            mi.AddSpecialRequirement(0, 0, 8, 4, 4);
            mi.shipRequirements = new ShipRequirements(25, 150, 6, new ShipRequirement(ShipTN.av, 2), new ShipRequirement(ShipTN.cl, 1, true), new ShipRequirement(ShipTN.dd, 2));
        }

        public List<MissionInformation> Data = new List<MissionInformation>();
        public MissionInformation AddMission(int No, string Name, int Minute, int FuelCost, int AmmoCost, int Fuel, int Ammo, int Steel, int Al, int CoinLarge, int CoinMedium, int CoinSmall, int Bucket, int Material, int FastConstruction, string Detail)
        {
            MissionInformation mi = new MissionInformation();
            mi.No = No;
            mi.Name = Name;
            mi.Minute = Minute;
            mi.FuelCost = FuelCost;
            mi.AmmoCost = AmmoCost;
            mi.Fuel = Fuel;
            mi.Ammo = Ammo;
            mi.Steel = Steel;
            mi.Al = Al;
            mi.CoinLarge = CoinLarge;
            mi.CoinMedium = CoinMedium;
            mi.CoinSmall = CoinSmall;
            mi.Bucket = Bucket;
            mi.ConstrutionMaterial = Material;
            mi.FastConstruction = FastConstruction;
            mi.Detail = Detail;
            Data.Add(mi);
            return mi;
        }

        public MissionInformation GetMission(int ID)
        {
            foreach(MissionInformation mi in Data )
            {
                if (mi.No == ID)
                    return mi;
            }
            return null;
        }

    }

    public class MissionInformation
    {
        public int No
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int Minute
        {
            get;
            set;
        }
        public string Time
        {
            get
            {
                string t = "";
                int h = Minute / 60;
                int m = Minute % 60;
                if (h > 0)
                {
                    t = h.ToString("00") + ":" + m.ToString("00");
                }
                else
                {
                    t = "00:" + m.ToString("00");
                }
                return t;
            }
        }
        public int FuelCost
        {
            get;
            set;
        }

        public int AmmoCost
        {
            get;
            set;
        }
        public int Fuel
        {
            get;
            set;
        }
        public int Ammo
        {
            get;
            set;
        }
        public int Steel
        {
            get;
            set;
        }
        public int Al
        {
            get;
            set;
        }
     
        public string Detail
        {
            get;
            set;
        }
        public int CoinLarge
        {
            get;
            set;
        }
        public int CoinMedium
        {
            get;
            set;
        }
        public int CoinSmall
        {
            get;
            set;
        }
        public int Bucket
        {
            get;
            set;
        }
        public int ConstrutionMaterial
        {
            get;
            set;
        }
        public int FastConstruction
        {
            get;
            set;
        }
        public ShipRequirements shipRequirements
        {
            get;
            set;
        }
        public SpecialRequirement specialRequirement
        {
            get;
            set;
        }
        public GreatRequirement GreatSuccess
        {
            get;
            set;
        }

        public MissionResult GetMissionResult(int FleetID)
        {
            MissionResult result = new MissionResult();
            result.Result = MissionSuccess.Success;

            if (GreatSuccess.CheckRequirement(FleetID) || (FleetData.GetShipSparkle(FleetID) == 6))
            {
                result.Result = MissionSuccess.Great;
            }

            if (FleetData.NeedRefuel(FleetID))
            {
                result.Result = MissionSuccess.Fail;
                result.InvalidCondition.Add("舰队补给未满！");
            }

            MissionResult sunResult = shipRequirements.isMatch(FleetID);

            if (sunResult.Result == MissionSuccess.Fail)
            {
                result.Result = MissionSuccess.Fail;
                result.InvalidCondition.AddRange(sunResult.InvalidCondition);
            }

            if (!specialRequirement.CheckRequirement(FleetID))
            {
                result.Result = MissionSuccess.Fail;
                result.InvalidCondition.Add("带桶条件不符合！");
            }
            //if (shipRequirements.isMatch(FleetID) && specialRequirement.CheckRequirement(FleetID) && !FleetData.NeedRefuel(FleetID))
            //{
            //    if (GreatSuccess.CheckRequirement(FleetID) || (FleetData.GetShipSparkle(FleetID) == 6))
            //    {
            //        result = MissionResult.Great;
            //    }
            //    else
            //        result = MissionResult.Success;
            //}
            //else
            //    result = MissionResult.Fail;
            return result;
        }

        public Resource GetIncome()
        {
            Resource res = new Resource();
            res.Fuel = Fuel;
            res.Ammo = Ammo;
            res.Steel = Steel;
            res.Al = Al;
            return res;
        }

        public string GetCoin()
        {
            if (CoinLarge > 0)
                return "大" + CoinLarge.ToString();
            if (CoinMedium > 0)
                return "中" + CoinMedium.ToString();
            if (CoinSmall > 0)
                return "小" + CoinSmall.ToString();
            return "0";
        }
        public void AddSpecialRequirement(int BucketCount, int ShipwithBucket, int BucketCount2, int ShipwithBucket2, int ShipSparkle)
        {
            specialRequirement = new SpecialRequirement();
            GreatSuccess = new GreatRequirement();
            specialRequirement.BucketCount = BucketCount;
            specialRequirement.ShipwithBucket = ShipwithBucket;
            specialRequirement.ShipSparkle = 0;
            GreatSuccess.BucketCount = BucketCount2;
            GreatSuccess.ShipwithBucket = ShipwithBucket2;
            GreatSuccess.ShipSparkle = ShipSparkle;

            if (BucketCount2 > 0)
            {
                GreatSuccess.Text = "特殊大成功条件：" + ShipwithBucket2.ToString() + "船" + BucketCount2.ToString() + "桶 " + ShipSparkle.ToString() + "闪";
            }
        }
    }

    public class SpecialRequirement
    {
        public bool HaveSpecialRequirement
        {
            get
            {
                return BucketCount != 0;
            }
        }

        public virtual bool CheckRequirement(int FleetID)
        {
            if (HaveSpecialRequirement)
            {
                int Bucket = FleetData.GetBuckets(FleetID);
                int Ships = FleetData.GetShipwithBucket(FleetID);
                int sparkle = FleetData.GetShipSparkle(FleetID);
                if ((BucketCount <= Bucket) && (ShipwithBucket <= Ships) && (ShipSparkle <= sparkle))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public int BucketCount
        {
            get;
            set;
        }
        public int ShipwithBucket
        {
            get;
            set;
        }
        public int ShipSparkle
        {
            get;
            set;
        }
    }

    public class GreatRequirement : SpecialRequirement
    {
        public string Text
        {
            get;
            set;
        }
        public override bool CheckRequirement(int FleetID)
        {
            if (HaveSpecialRequirement)
            {
                int Bucket = FleetData.GetBuckets(FleetID);
                int Ships = FleetData.GetShipwithBucket(FleetID);
                int sparkle = FleetData.GetShipSparkle(FleetID);
                if ((BucketCount <= Bucket) && (ShipwithBucket <= Ships) && (ShipSparkle <= sparkle))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
    public class ShipRequirements
    {
        public ShipRequirements(int shipLevel, int fleetLevel, int shipAmount, params ShipRequirement[] shipRequirement)
        {
            FlagShipLevel = shipLevel;
            FleetLevel = fleetLevel;
            ShipAmount = shipAmount;
            Requirements.AddRange(shipRequirement);
        }

        public MissionResult isMatch(int FleetID)
        {
            MissionResult Result = new MissionResult();
            Result.Result = MissionSuccess.Success;
            try
            {
                if (FleetID <= ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets.Count)
                {
                    var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                    int TotalLevel = 0;
                    int FlagLevel = Fleet.MembersInstance[0].Level;
                    int Total = 0;
                    for (int i = 0; i < Fleet.MembersInstance.Count; i++)
                    {
                        if (Fleet.MembersInstance[i] != null)
                        {
                            Total++;
                            TotalLevel += Fleet.MembersInstance[i].Level;
                        }
                    }
                    if (TotalLevel < FleetLevel)
                    {
                        Result.Result = MissionSuccess.Fail;
                        Result.InvalidCondition.Add("舰队总等级不满足要求(" + TotalLevel.ToString() + "<" + FleetLevel.ToString() + ")");
                    }
                    if (FlagLevel < FlagShipLevel)
                    {
                        Result.Result = MissionSuccess.Fail;
                        Result.InvalidCondition.Add("旗舰等级不满足要求(" + FlagLevel.ToString() + "<" + FlagShipLevel.ToString() + ")");
                    }
                    if (Total < ShipAmount)
                    {
                        Result.Result = MissionSuccess.Fail;
                        Result.InvalidCondition.Add("总舰数不满足要求(" + Total.ToString() + "<" + ShipAmount.ToString() + ")");
                    }
                    foreach (var req in Requirements)
                    {
                        MissionResult subResult = req.isMatch(FleetID);
                        if (subResult.Result== MissionSuccess.Fail)
                        {
                            Result.Result = MissionSuccess.Fail;
                            Result.InvalidCondition.AddRange(subResult.InvalidCondition);
                        }
                    }
                    return Result;
                }
                else
                    return Result;
            }
            catch
            {
                Result.Result = MissionSuccess.Fail;
                return Result;
            }
        }

        public int FlagShipLevel
        {
            get;
            set;
        }
        public int FleetLevel
        {
            get;
            set;
        }
        public int ShipAmount
        {
            get;
            set;
        }
        public List<ShipRequirement> Requirements = new List<ShipRequirement>();
    }

    public class ShipRequirement
    {
        public ShipRequirement()
        {
        }
        public ShipRequirement(string TypeName, int Amount)
        {
            ShipTypeName = new string[] { TypeName };
            MinReq = Amount;
        }
        public ShipRequirement(string[] TypeName, int Amount)
        {
            ShipTypeName = TypeName;
            MinReq = Amount;
        }
        public ShipRequirement(string TypeName, int Amount, bool FlagShipNeeded)
        {
            ShipTypeName = new string[] { TypeName };
            MinReq = Amount;
            FlagShip = FlagShipNeeded;
        }
        public ShipRequirement(string[] TypeName, int Amount, bool FlagShipNeeded)
        {
            ShipTypeName = TypeName;
            MinReq = Amount;
            FlagShip = FlagShipNeeded;
        }

        public MissionResult isMatch(int FleetID)
        {
            MissionResult Result = new MissionResult();
            Result.Result = MissionSuccess.Success;
            int MatchCount = 0;
            try
            {
                if (FleetID <= ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets.Count)
                {
                    var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                    if (FlagShip)
                    {
                        if (Fleet.MembersInstance[0] == null)
                        {
                            Result.Result = MissionSuccess.Fail;
                            return Result;
                        }
                        if (!ShipTypeName.Contains(Fleet.MembersInstance[0].MasterShip.ShipTypeName))
                        {
                            Result.Result = MissionSuccess.Fail;
                            Result.InvalidCondition.Add("旗舰种类不满足要求");
                        }
                    }
                    for (int i = 0; i < Fleet.MembersInstance.Count; i++)
                    {
                        var Ship = Fleet.MembersInstance[i];
                        if (Ship == null)
                            continue;
                        if (ShipTypeName.Contains(Ship.MasterShip.ShipTypeName))
                        {
                            MatchCount++;
                        }
                    }
                    if (MatchCount >= MinReq)
                    {
                        //Result.Result = MissionSuccess.Success;
                    }
                    else
                    {
                        Result.Result = MissionSuccess.Fail;
                        string typeList = "";
                        foreach (var s in ShipTypeName)
                        {
                            typeList += s + " ";
                        }
                        Result.InvalidCondition.Add("船种数量不满足要求 (" + typeList + MatchCount.ToString() + "<" + MinReq.ToString() + ")");
                    }
                }
                else
                    Result.Result = MissionSuccess.Fail;

                return Result;
            }
            catch
            {
                return Result;
            }
        }

        public string[] ShipTypeName
        {
            get;
            set;
        }
        public int MinReq
        {
            get;
            set;
        }
        public bool FlagShip
        {
            get;
            set;
        }
    }

    public static class ShipTN
    {
        public static string cl = "軽巡洋艦";
        public static string dd = "駆逐艦";
        public static string ca = "重巡洋艦";
        public static string cav = "航空巡洋艦";
        public static string bbv = "航空戦艦";
        public static string cv = "正規空母";
        public static string cvl = "軽空母";
        public static string av = "水上機母艦";
        public static string ss = "潜水艦";
        public static string ssv = "潜水空母";
        public static string cva = "装甲空母";
        public static string ssm = "潜水母艦";
        public static string pcl = "練習巡洋艦";
    }

    public static class FleetData
    {
        public static Resource GetCost(int FleetID, double FuelRatio, double AmmoRatio)
        {
            Resource r = new Resource();
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship != null)
                    {
                        r.Fuel += (int)(ship.MasterShip.Fuel * FuelRatio / 100);
                        r.Ammo += (int)(ship.MasterShip.Ammo * AmmoRatio / 100);
                    }
                }
                return r;
            }
            catch
            {
                r.Al = 0;
                r.Al = 0;
                r.Fuel = 0;
                r.Steel = 0;
                return r;
            }
        }
        public static int Get大发(int FleetID)
        {
            int Num = 0;
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship == null)
                        continue;
                    foreach (var equip in ship.SlotInstanceMaster)
                    {
                        if (equip != null)
                            if (equip.Name == "大発動艇")
                                Num++;
                    }
                }
                if (Num > 4)
                    Num = 4;
                return Num;
            }
            catch
            {
                return 0;
            }
        }
        public static int GetShipwithBucket(int FleetID)
        {
            int Num = 0;
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship == null)
                        continue;
                    foreach (var equip in ship.SlotInstanceMaster)
                    {
                        if (equip != null)
                            if (equip.Name == "ドラム缶(輸送用)")
                            {
                                Num++;
                                break;
                            }
                    }
                }
                return Num;
            }
            catch
            {
                return 0;
            }
        }

        public static int GetShipSparkle(int FleetID)
        {
            int Num = 0;
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship != null)
                        if (ship.Condition >= 50)
                            Num++;
                }
                return Num;
            }
            catch
            {
                return 0;
            }
        }

        public static bool NeedRefuel(int FleetID)
        {
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship == null)
                        continue;
                    if (ship.Fuel < ship.MasterShip.Fuel)
                        return true;
                    if (ship.Ammo < ship.MasterShip.Ammo)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static int GetBuckets(int FleetID)
        {
            int Num = 0;
            try
            {
                var Fleet = ElectronicObserver.Data.KCDatabase.Instance.Fleet.Fleets[FleetID];
                foreach (var ship in Fleet.MembersInstance)
                {
                    if (ship == null)
                        continue;
                    foreach (var equip in ship.SlotInstanceMaster)
                    {
                        if (equip != null)
                            if (equip.Name == "ドラム缶(輸送用)")
                                Num++;
                    }
                }
                return Num;
            }
            catch
            {
                return 0;
            }
        }
    }

    public class Resource
    {
        public Resource()
        {
            Fuel = 0;
            Ammo = 0;
            Steel = 0;
            Al = 0;
        }
        public int Fuel
        {
            get;
            set;
        }
        public int Ammo
        {
            get;
            set;
        }
        public int Steel
        {
            get;
            set;
        }
        public int Al
        {
            get;
            set;
        }
    }

    public class MissionResult
    {
        public MissionSuccess Result
        {
            get;
            set;
        }
        public List<string> InvalidCondition = new List<string>();
    }
    public enum MissionSuccess
    {
        Fail, Success, Great
    }
}
