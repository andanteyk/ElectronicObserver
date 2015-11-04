using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElectronicObserver.Data;

namespace CustomDeck
{

    public static class ShipUtility
    {
        public static List<int> GetDerivedShips(int ID)
        {
            List<int> ships = new List<int>();
            int id = ID;
            while (id != 0 && !ships.Contains(id))
            {
                ships.Add(id);
                var Ship = KCDatabase.Instance.MasterShips[id];
                if (id == 288)
                {
                    id = 461;
                }
                else if (id == 461)
                {
                    id = 466;
                }
                else
                    id = Ship.RemodelAfterShipID;
            }
            return ships;
        }

        public static List<int> GetRelativedShips(int ID)
        {
            List<int> ships = new List<int>();
            int id = ID;
            while (id != 0 && !ships.Contains(id))
            {
                var Ship = KCDatabase.Instance.MasterShips[id];
                if (Ship.RemodelBeforeShipID > 0)
                    id = Ship.RemodelBeforeShipID;
                else
                    break;
            }
            while (id != 0 && !ships.Contains(id))
            {
                if (id != ID)
                    ships.Add(id);
                var Ship = KCDatabase.Instance.MasterShips[id];
                if (Ship.RemodelAfterShipID > 0)
                    id = Ship.RemodelAfterShipID;
                else
                    break;
            }
            return ships;
        }

        public static List<ShipDataMaster> GetShipList(ShipType type)
        {
            List<ShipDataMaster> list = new List<ShipDataMaster>();
            foreach (var sss in KCDatabase.Instance.MasterShips.Values)
            {
                if (!sss.IsAbyssalShip && sss.ShipType == type.TypeID)
                {
                    list.Add(sss);
                }
            }
            return list;
        }

        public static List<EquipmentDataMaster> GetEquipmentList(EquipmentType type)
        {
            List<EquipmentDataMaster> list = new List<EquipmentDataMaster>();
            foreach (var sss in KCDatabase.Instance.MasterEquipments.Values)
            {
                if (!sss.IsAbyssalEquipment && sss.CategoryTypeInstance.TypeID == type.TypeID)
                {
                    list.Add(sss);
                }
            }
            return list;

            //KCDatabase.Instance.
        }

        public static List<ShipType> GetShipTypeList()
        {
            List<ShipType> list = new List<ShipType>();
            foreach (var sss in KCDatabase.Instance.ShipTypes.Values)
            {
                if (sss.TypeID != 1 && sss.TypeID != 15 && sss.TypeID != 12)
                {
                    list.Add(sss);
                }
            }
            return list;
        }

        public static List<EquipmentType> GetEquipmentTypeList()
        {
            List<EquipmentType> list = new List<EquipmentType>();
            foreach (var sss in KCDatabase.Instance.EquipmentTypes.Values)
            {
                if (sss.TypeID != 16 && sss.TypeID != 20 && sss.TypeID != 38 && sss.TypeID != 93)
                    list.Add(sss);
            }
            return list;
        }

        public static Dictionary<int, string> GetEquipmentTypeListCard()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();

            for (int i = 1; i < 40;i++ )
                list.Add(i, "type " + i.ToString());
                //list.Add(1, "砲");
                //list.Add(2, "魚雷");
                //list.Add(3, "艦載機");
                //list.Add(4, "機銃・特殊弾");
                //list.Add(5, "偵察機・電探");
                //list.Add(6, "強化");
                //list.Add(7, "対潜装備");
                //list.Add(8, "大発動艇・探照灯");
                //list.Add(9, "簡易輸送部材");
                //list.Add(10, "艦艇修理施設");
                //list.Add(11, "照明弾");
                //list.Add(12, "司令部施設");
                //list.Add(13, "航空要員");
                //list.Add(14, "高射装置");
                //list.Add(15, "対地装備");
                //list.Add(16, "水上艦要員");
                //list.Add(17, "大型飛行艇");
                //list.Add(18, "戦闘糧食");
                //list.Add(19, "補給物資");
                return list;
        }

        public static List<EquipmentDataMaster> GetEquipmentListCard(int type)
        {
            List<EquipmentDataMaster> list = new List<EquipmentDataMaster>();
            foreach (var sss in KCDatabase.Instance.MasterEquipments.Values)
            {
                if (!sss.IsAbyssalEquipment && sss.CardType == type)
                {
                    list.Add(sss);
                }
            }
            return list;

            //KCDatabase.Instance.
        }
    }
}
