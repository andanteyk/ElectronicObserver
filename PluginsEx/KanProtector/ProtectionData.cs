using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using ElectronicObserver.Data;

namespace KanProtector
{
    public sealed class ProtectionData
    {
        public static ProtectionData Instance;

        public bool ShipProtectionEnabled = false;
        public bool EquipmentProtectionEnabled = false;

        const string DataFileName = "\\settings\\KanProtector.xml";
        string DataFile;
        public List<ShipProtection> shipList;
        public List<int> equipmentList;

        const int ExistEquipmentMaxID = 150;
        List<int> ExistShips;

        static ProtectionData()
        {
            Instance = new ProtectionData();
            //ElectronicObserver.Utility.Logger.Add(2," static ProtectionData()");
        }
        public void LoadConfig()
        {          
            DataFile = System.Windows.Forms.Application.StartupPath + DataFileName;
            if (System.IO.File.Exists(DataFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(DataFile);
                var Root = doc.DocumentElement;
                bool.TryParse(Root.GetAttribute("ShipProtection"), out ShipProtectionEnabled);
                bool.TryParse(Root.GetAttribute("EquipmentProtection"), out EquipmentProtectionEnabled);

                var Node = Root.SelectSingleNode("ShipProtection");
                if (Node != null)
                {
                    var Ele = Node as XmlElement;
                    foreach (XmlElement subnode in Ele)
                    {
                        int ID;
                        bool derived = false;
                        ProtectionType type = ProtectionType.保护全部;
                        bool LoadSuccess;
                        LoadSuccess = int.TryParse(subnode.GetAttribute("ShipID"), out ID);
                        LoadSuccess = LoadSuccess && bool.TryParse(subnode.GetAttribute("Derived"), out derived);
                        LoadSuccess = LoadSuccess && ProtectionType.TryParse(subnode.GetAttribute("Type"), out type);
                        if (LoadSuccess)
                        {
                            ShipProtection sp = new ShipProtection();
                            sp.protectionType = type;
                            sp.shipID = ID;
                            sp.isContainDerived = derived;
                            shipList.Add(sp);
                        }
                    }
                }

                Node = Root.SelectSingleNode("EquipmentProtection");
                if (Node != null)
                {
                    var Ele = Node as XmlElement;
                    foreach (XmlElement subnode in Ele)
                    {
                        int ID;
                        if (int.TryParse(subnode.GetAttribute("EquipmentID"), out ID))
                        {
                            equipmentList.Add(ID);
                        }
                    }
                }
            }
            else
                LoadDefault();
        }

        public void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!System.IO.File.Exists(DataFile))
                {
                    XmlElement xmlelem = doc.CreateElement("Config");
                    doc.AppendChild(xmlelem);
                }
                else
                {
                    doc.Load(DataFile);
                }
                var Root = doc.DocumentElement;
                Root.SetAttribute("ShipProtection", ShipProtectionEnabled.ToString());
                Root.SetAttribute("EquipmentProtection", EquipmentProtectionEnabled.ToString());
                var NodeShip = Root.SelectSingleNode("ShipProtection");
                if (NodeShip == null)
                {
                    NodeShip = doc.CreateElement("ShipProtection");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    Root.AppendChild(NodeShip);
                }
                NodeShip.RemoveAll();
                foreach (var ship in shipList)
                {
                    XmlElement Element = doc.CreateElement("Ship");
                    Element.SetAttribute("ShipID", ship.shipID.ToString());
                    Element.SetAttribute("Derived", ship.isContainDerived.ToString());
                    Element.SetAttribute("Type", ship.protectionType.ToString());
                    NodeShip.AppendChild(Element);
                }

                var NodeEquipment = Root.SelectSingleNode("EquipmentProtection");
                if (NodeEquipment == null)
                {
                    NodeEquipment = doc.CreateElement("EquipmentProtection");
                    //Node.SetAttribute("Disabled", filter.Value.ToString());
                    Root.AppendChild(NodeEquipment);
                }
                NodeEquipment.RemoveAll();
                foreach (var equipment in equipmentList)
                {
                    XmlElement Element = doc.CreateElement("Equipment");
                    Element.SetAttribute("EquipmentID", equipment.ToString());
                    NodeEquipment.AppendChild(Element);
                }
                
                doc.Save(DataFile);
            }
            catch
            {
            }
        }

        private ProtectionData()
        {
            ExistShips = new List<int>();
            shipList = new List<ShipProtection>();
            equipmentList = new List<int>();

            int[] IdentifiedShips =new int[]{ 1,2,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73
                        ,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132
                        ,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185
                        ,186,187,188,189,190,191,192,193,194,195,196,197,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239
                        ,240,241,242,243,244,245,246,247,248,249,250,251,252,253,254,255,256,257,258,259,260,261,262,263,264,265,266,267,268,269,270,271,272,273,274,275,276,277,278,279,280,281,282,283,284,285,286,287,288,289,290,291
                        ,292,293,294,295,296,297,300,301,302,303,304,305,306,307,308,309,310,311,312,313,314,316,317,318,319,320,321,322,323,324,325,326,327,328,329,330,331,332,334,343,344,345,346,347,348,349,350,351,352,398,399,400
                        ,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,419,420,421,422,424,425,426,427,428,429,430,431,434,435,436,437,441,442,443,445,446,447,450,451,453,458,459,460,461,466};

            foreach (var shipid in IdentifiedShips)
            {
                ExistShips.Add(shipid);
            }
        }

        void LoadDefault()
        {
            int[] eqlist = new int[] {8,9,22,31,36,42,43,45,47,50,52,53,56,58,62,63,67,73,76,79,80,81
                                      ,82,83,85,86,87,88,89,90,93,94,95,99,100,102,103,105,106,107,108
                                      ,109,110,111,112,113,114,116,117,118,120,121,122,123,124,126,127
                                      ,128,129,130,131,132,134,135,136,137,139,140,141,142,143,144,147
                                      , 148,149,150};
            equipmentList.Clear();
            equipmentList.AddRange(eqlist);

            AddShip(120, false, ProtectionType.保护全部);//3v
            AddShip(120, true, ProtectionType.保护最高);//3v
            AddShip(131, true, ProtectionType.保护最高);//大和
            AddShip(139, true, ProtectionType.保护最高);//矢矧
            AddShip(140, true, ProtectionType.保护最高);//酒匂
            AddShip(143, true, ProtectionType.保护最高);//武藏
            AddShip(153, true, ProtectionType.保护最高);//大凤
            AddShip(154, true, ProtectionType.保护最高);//香取
            AddShip(155, true, ProtectionType.保护最高);//401
            AddShip(161, true, ProtectionType.保护最高);//僵尸
            AddShip(163, false, ProtectionType.保护全部);//马路油
            AddShip(167, true, ProtectionType.保护最高);//矶风
            AddShip(171, true, ProtectionType.保护全部);//bsm
            AddShip(174, true, ProtectionType.保护最高);//Z1
            AddShip(175, true, ProtectionType.保护最高);//Z3
            AddShip(176, false, ProtectionType.保护全部);//欧根
            AddShip(176, true, ProtectionType.保护最高);//欧根
            AddShip(181, false, ProtectionType.保护全部);//天津风
            AddShip(181, true, ProtectionType.保护最高);//天津风
            AddShip(182, true, ProtectionType.保护最高);//明石
            AddShip(183, true, ProtectionType.保护最高);//大淀
            AddShip(184, false, ProtectionType.保护最高);//大鲸
            AddShip(185, false, ProtectionType.保护最高);//龙凤
            AddShip(186, true, ProtectionType.保护最高);//失禁风
            AddShip(190, true, ProtectionType.保护最高);//初风
            AddShip(318, false, ProtectionType.保护最高);//龙凤改
            AddShip(331, true, ProtectionType.保护最高);//天城
            AddShip(332, true, ProtectionType.保护最高);//葛成
          
            AddShip(404, true, ProtectionType.保护最高);//云龙
            AddShip(405, true, ProtectionType.保护最高);//春雨
            AddShip(409, true, ProtectionType.保护最高);//早霜
            AddShip(410, true, ProtectionType.保护最高);//清霜
            AddShip(413, true, ProtectionType.保护最高);//朝云
            AddShip(414, true, ProtectionType.保护最高);//山云
            AddShip(415, true, ProtectionType.保护最高);//野分
            AddShip(421, true, ProtectionType.保护最高);//秋月
            AddShip(422, true, ProtectionType.保护最高);//照月
            AddShip(424, true, ProtectionType.保护最高);//高波
            AddShip(425, true, ProtectionType.保护最高);//朝霜
            AddShip(431, true, ProtectionType.保护全部);//U511
            AddShip(441, true, ProtectionType.保护最高);//Lito
            AddShip(442, true, ProtectionType.保护最高);//Roma
            AddShip(443, true, ProtectionType.保护最高);//Libe
            AddShip(445, true, ProtectionType.保护最高);//秋津州
            AddShip(451, true, ProtectionType.保护最高);//瑞穗
            AddShip(453, true, ProtectionType.保护最高);//风云
            AddShip(458, true, ProtectionType.保护最高);//海风
            AddShip(459, true, ProtectionType.保护最高);//江风
            AddShip(460, true, ProtectionType.保护最高);//速吸
        }

        public int AddShip(int ID, bool derived, ProtectionType type, bool rewriten = false)
        {
            ShipProtection               protection = new ShipProtection();
                shipList.Add(protection);
            
            protection.shipID = ID;
            protection.isContainDerived = derived;
            protection.protectionType = type;
            return -1;
    }

        public int AddShip(ShipProtection shipProtection, bool rewriten = false)
        {
            for (int index = 0; index < shipList.Count; index++)
            {
                ShipProtection sp = shipList[index];
                if (sp.shipID == shipProtection.shipID)
                {
                    if (rewriten)
                    {
                        shipList[index] = shipProtection;
                        return -1;
                    }
                    else
                        return index;
                }
            }

            shipList.Add(shipProtection);
            
            return -1;
        }

        public bool DeleteShip(int ID)
        {
            for (int index = 0; index < shipList.Count; index++)
            {
                ShipProtection sp = shipList[index];
                if (sp.shipID == ID)
                {
                    shipList.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        public int AddEquipment(int ID)
        {
            int index = equipmentList.IndexOf(ID);
            if (index<0)
            {
                equipmentList.Add(ID);
            }
            return index;
        }

        public bool DeleteEquipment(int ID)
        {
            return equipmentList.Remove(ID);
        }

        /// <summary>
        /// 判断一个装备是否是受规则保护
        /// </summary>
        /// <param name="ID">MasterEquipment编号</param>
        /// <returns></returns>
        public bool isEquipmentProtected(int ID)
        {
            if (ID > ExistEquipmentMaxID)
                return true;
            return equipmentList.Contains(ID);
        }

        /// <summary>
        /// 判断一个舰娘是否是受规则保护
        /// </summary>
        /// <param name="NowID">这是舰船的Ship编号,而非MasterShipData编号</param>
        /// <returns></returns>
        public bool isShipProtected(int NowID)
        {
            int ID = KCDatabase.Instance.Ships[NowID].ShipID;

            if (!ExistShips.Contains(ID))
                return true;
            foreach (ShipProtection sp in shipList)
            {
                if (sp.isProtected(NowID))
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class ShipProtection
    {
        public int shipID;
        public List<int> DerivedShips
        {
            get
            {
                if (isContainDerived)
                    return ShipUtility.GetDerivedShips(shipID);
                else
                {
                    var list = new List<int>();
                    list.Add(shipID);
                    return list;
                }
            }
        }
        public bool isContainDerived;
        public ProtectionType protectionType;

        public bool isProtected(int NowID)
        {
            int ID = KCDatabase.Instance.Ships[NowID].ShipID;
            switch(protectionType)
            {
                case ProtectionType.保护全部:
                    return DerivedShips.Contains(ID);
                case ProtectionType.保护最高:
                    if (DerivedShips.Contains(ID))
                    {
                        int HighestEXP = 0;
                        int HighestEXPships = 0;
                        foreach (var ship in KCDatabase.Instance.Ships)
                        {
                            if (DerivedShips.Contains(ship.Value.ShipID))
                            {
                                if (ship.Value.ExpTotal == HighestEXP)
                                {
                                    HighestEXPships++; 
                                }
                                if (ship.Value.ExpTotal > HighestEXP)
                                {
                                    HighestEXP = ship.Value.ExpTotal;
                                    HighestEXPships = 1;
                                }
                            }
                        }
                        return (KCDatabase.Instance.Ships[NowID].ExpTotal == HighestEXP) && (HighestEXPships == 1);
                    }
                    else
                        return false;
            }
            return false;
        }
    }

    public enum ProtectionType
    {
        保护最高 = 0, 保护全部 = 1
    }


}
