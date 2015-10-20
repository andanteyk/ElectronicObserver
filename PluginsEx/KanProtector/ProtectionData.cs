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
        public bool ProtectPrimary = false;
        
        public ProtectionVersion CurrentVersion;

        const string DataFileName = "\\settings\\KanProtector.xml";
        string DataFile;
        public List<ShipProtection> shipList;
        public List<int> equipmentList;

        int ExistEquipmentMaxID
        {
            get
            {
                return CurrentVersion.MaxIndentifiedEquipmentID;
            }
        }
        List<int> ExistShips;

        static ProtectionData()
        {
            Instance = new ProtectionData();
            //ElectronicObserver.Utility.Logger.Add(2," static ProtectionData()");
        }
        public void LoadConfig()
        {          
            DataFile = System.Windows.Forms.Application.StartupPath + DataFileName;
            int LastVersionID = 0;
            CurrentVersion = ProtectionVersionManager.Instance.GetLatestVersion();

            if (System.IO.File.Exists(DataFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(DataFile);
                var Root = doc.DocumentElement;

                if (!int.TryParse(Root.GetAttribute("CurrentVersion"), out LastVersionID))
                    LastVersionID = 0;
                bool.TryParse(Root.GetAttribute("ProtectPrimary"), out ProtectPrimary);
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

                if (LastVersionID < CurrentVersion.VersionID)
                {
                    CombineVersion(LastVersionID);
                    SaveConfig();
                }
            }
            else
            {
                LoadDefault();
                if (LastVersionID < CurrentVersion.VersionID)
                {
                    CombineVersion(LastVersionID);
                }
                SaveConfig();
            }


            foreach (var shipid in CurrentVersion.IdentifiedShips)
            {
                ExistShips.Add(shipid);
            }
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
                Root.SetAttribute("CurrentVersion", CurrentVersion.VersionID.ToString());
                Root.SetAttribute("ProtectPrimary", ProtectPrimary.ToString());
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
        }

        void LoadDefault()
        {
            equipmentList.Clear();
            equipmentList.AddRange(CurrentVersion.IdentifiedEquipments);

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

        void CombineVersion(int LastVersionID)
        {

            ProtectionVersion oldver = ProtectionVersionManager.Instance.GetVersion(LastVersionID);
            var NewEquipments = CurrentVersion.IdentifiedEquipments.Except(oldver.IdentifiedEquipments);
            foreach (var id in NewEquipments)
            {
                AddEquipment(id);
            }

            var NewShips = CurrentVersion.IdentifiedShips.Except(oldver.IdentifiedShips);
            foreach (var id in NewShips)
            {
                AddShip(id, true, ProtectionType.保护全部, true);
            }

        }

        public int AddShip(int ID, bool derived, ProtectionType type, bool append = false)
        {
            ShipProtection protection = new ShipProtection();
            shipList.Add(protection);
            protection.shipID = ID;
            protection.isContainDerived = derived;
            protection.protectionType = type;

            for (int index = 0; index < shipList.Count; index++)
            {
                ShipProtection sp = shipList[index];
                if (sp.shipID == ID)
                {
                    if (append || sp.isContainDerived == derived)
                        return index;
                }
            }

            shipList.Add(protection);
            return -1;
        }

        public int AddShip(ShipProtection shipProtection, bool append = false)
        {
            for (int index = 0; index < shipList.Count; index++)
            {
                ShipProtection sp = shipList[index];
                if (sp.shipID == shipProtection.shipID)
                {
                    if (append || sp.isContainDerived == shipProtection.isContainDerived)
                    {
                        return index;
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
            if (index < 0)
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
            if (ProtectPrimary)
            {
                return ShipProtection.isPrimaryShipProtected(NowID);
            }
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

        public static bool isPrimaryShipProtected(int NowID)
        {
            ShipDataMaster masterShip = KCDatabase.Instance.Ships[NowID].MasterShip;
            while (masterShip.RemodelBeforeShip != null)
                masterShip = masterShip.RemodelBeforeShip;
            var DerivedShips = ShipUtility.GetDerivedShips(masterShip.ID);

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
    }

    public enum ProtectionType
    {
        保护最高 = 0, 保护全部 = 1
    }


}
