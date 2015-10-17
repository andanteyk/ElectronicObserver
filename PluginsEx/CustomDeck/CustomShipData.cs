using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Script.Serialization;

using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;

namespace CustomDeck
{
    public class CustomEquipmentment
    {
        public int EquipmentmentID = -1;
        public int Level = 0;
        public CustomShip Owner;

        public CustomEquipmentment(int id, int lv)
        {
            EquipmentmentID = id;
            Level = lv;
        }

        public string Text
        {
            get
            {
                return Level > 0 ? Equipmentment.Name + "+" + Level.ToString() : Equipmentment.Name;
            }
        }
        public EquipmentDataMaster Equipmentment
        {
            get
            {
                if (EquipmentmentID > 0)
                    return KCDatabase.Instance.MasterEquipments[EquipmentmentID];
                else
                    return null;
            }
        }
        public CustomEquipmentment Clone()
        {
            return new CustomEquipmentment(EquipmentmentID, Level);
        }

        public string Serialize()
        {
            return "\"id\":" + EquipmentmentID.ToString() + ",\"rf\":" + Level.ToString();
        }
    }
    public class CustomShip
    {
        public int ShipID = -1;
        public int Level = 1;
        public int Luck = 0;
        public CustomEquipmentment[] Equipmentments = new CustomEquipmentment[4];
        public CustomEquipmentment EquipmentmentEx;
        public CustomFleet Owner;

        public ShipDataMaster Ship
        {
            get
            {
                return KCDatabase.Instance.MasterShips[ShipID];
            }
        }

        public CustomShip()
        {
        }

        public string Serialize()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("\"id\":{0},\"lv\":{1},\"luck\":{2},\"items\":{3}", ShipID, Level, Luck > 0 ? Luck : Ship.LuckMin, "{");
            {
                builder.Append("\"ix\":{");
                if (EquipmentmentEx != null)
                    builder.Append(EquipmentmentEx.Serialize());
                builder.Append("}");

                for (int i = 0; i < Equipmentments.Length; i++)
                {
                    if (Equipmentments[i] != null)
                    {
                        builder.AppendFormat(",\"i{0}\":{1}", i + 1, "{");
                        builder.Append(Equipmentments[i].Serialize());
                        builder.Append("}");
                    }
                }
            }
            builder.Append("}");
            return builder.ToString();
        }

        public CustomShip Clone()
        {
            CustomShip CustomShip = new CustomShip();
            CustomShip.ShipID = this.ShipID;
            CustomShip.Level = this.Level;
            CustomShip.Luck = this.Luck;
            if (Equipmentments != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Equipmentments[i] != null)
                        CustomShip.Equipmentments[i] = Equipmentments[i].Clone();
                }
            }
            CustomShip.EquipmentmentEx = EquipmentmentEx == null ? null : EquipmentmentEx.Clone();

            return CustomShip;
        }

        public void LoadObject(Dictionary<string, object> shipData)
        {
            if (!shipData.ContainsKey("id"))
            {
                throw new Exception("没有舰船ID");
            }
            else
            {
                ShipID = (int)shipData["id"];
                if (shipData.ContainsKey("lv"))
                    Level = (int)shipData["lv"];
                else
                    Level = 1;
                if (shipData.ContainsKey("luck"))
                    Luck = (int)shipData["luck"];
                else
                    Luck = KCDatabase.Instance.MasterShips[ShipID].LuckMin;
                if (shipData.ContainsKey("items"))
                {
                    var itemData = (Dictionary<string, object>)shipData["items"];
                    for (int i = 1; i <= 4; i++)
                    {
                        string Equipmentindex = "i" + i.ToString();

                        var equipment = LoadEquipment(itemData, Equipmentindex);
                        if (equipment != null)
                        {
                            Equipmentments[i - 1] = equipment;
                        }
                    }

                    EquipmentmentEx = LoadEquipment(itemData, "ix");
                }
            }

        }

        CustomEquipmentment LoadEquipment(Dictionary<string, object> data, string name)
        {
            if (data.ContainsKey(name))
            {
                var itemData = (Dictionary<string, object>)data[name];
                if (itemData.ContainsKey("id"))
                {
                    int id = (int)itemData["id"];
                    int lv = 0;
                    if (itemData.ContainsKey("rf"))
                        lv = (int)itemData["rf"];
                    CustomEquipmentment equipmentment = new CustomEquipmentment(id, lv);
                    equipmentment.Owner = this;
                    return equipmentment;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public int Firepower
        {
            get
            {
                int Fire = Ship.FirepowerMax;
                foreach(var eq in Equipmentments)
                {
                    if (eq != null && eq.Equipmentment != null)
                        Fire += eq.Equipmentment.Firepower;
                }
                return Fire;
            }
        }

        public int Torpedo
        {
            get
            {
                int torpedo = Ship.TorpedoMax;
                foreach (var eq in Equipmentments)
                {
                    if (eq != null && eq.Equipmentment != null)
                        torpedo += eq.Equipmentment.Torpedo;
                }
                return torpedo;
            }
        }

        public int AA
        {
            get
            {
                int aa = Ship.AAMax;
                foreach (var eq in Equipmentments)
                {
                    if (eq != null && eq.Equipmentment != null)
                        aa += eq.Equipmentment.AA;
                }
                return aa;
            }
        }

        public int Armor
        {
            get
            {
                int armor = Ship.ArmorMax;
                foreach (var eq in Equipmentments)
                {
                    if (eq != null && eq.Equipmentment != null)
                        armor += eq.Equipmentment.Armor;
                }
                return armor;
            }
        }

        public int Range
        {
            get
            {
                int range = Ship.Range;
                foreach (var eq in Equipmentments)
                {
                    if (eq != null && eq.Equipmentment != null)
                        range = Math.Max(range, eq.Equipmentment.Range);     
                }
                return range;
            }
        }

        public int GetAirSuperiority()
        {
            return 0;
        }

        public int GetSearchingAbilityString()
        {
            return 0;
        }
    }

    public class CustomFleet
    {
        public string FleetName;
        public CustomShip[] Ships = new CustomShip[6];
        public CustomFleets Owner;
        public CustomFleet()
        {
        }
        public void LoadObject(Dictionary<string, object> obj)
        {
            for (int i = 1; i <= 6; i++)
            {
                string shipindex = "s" + i.ToString();
                if (obj.ContainsKey(shipindex))
                {
                    CustomShip ship = new CustomShip();
                    ship.Owner = this;
                    var shipData = (Dictionary<string, object>)obj[shipindex];

                    ship.LoadObject(shipData);
                    Ships[i - 1] = ship;
                }
                else
                    break;
            }
        }

        public string Serialize()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Ships.Length; i++)
            {
                if (Ships[i] != null)
                {
                    if (builder.Length > 0)
                        builder.Append(",");
                    builder.AppendFormat("\"s{0}\":{1}", i + 1,"{");
                    builder.Append(Ships[i].Serialize());
                    builder.Append("}");
                }
            }
            
            return builder.ToString();
        }

        public int this[int i]
        {
            get
            {
                if (Ships[i] != null)
                    return Ships[i].ShipID;
                else
                    return -1;
            }
        }

        public int GetAirSuperiority()
        {
            return 0;
        }

        public int GetSearchingAbilityString()
        {
            return 0;
        }
    }

    public class CustomFleets
    {
        public string FleetsSummary;
        public CustomFleet[] Fleets = new CustomFleet[4];

        public bool ImportFromString(string str)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            try
            {
                var jobject = (Dictionary<string, object>)Serializer.DeserializeObject(str);

                return LoadObject(jobject);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        bool LoadObject(Dictionary<string, object> obj)
        {
            for (int i = 1; i <= 4; i++)
            {
                string fleetindex = "f" + i.ToString();
                if (obj.ContainsKey(fleetindex))
                {
                    var fleetData = (Dictionary<string, object>)obj[fleetindex];
                    CustomFleet fleet = new CustomFleet();
                    fleet.Owner = this;
                    fleet.FleetName = fleetindex;
                    fleet.LoadObject(fleetData);
                    Fleets[i - 1] = fleet;
                }
                else
                    break;
            }
            return true;
        }

        public string Serialize()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"version\":3");
            for (int i = 0; i < Fleets.Length; i++)
            {
                if (Fleets[i] != null)
                {
                    builder.AppendFormat(",\"f{0}\":{1}", i + 1, "{");
                    builder.Append(Fleets[i].Serialize());
                    builder.Append("}");
                }
            }
            builder.Append("}");
            return builder.ToString();
        }

        public string ExportToString()
        {
            return null;
        }
    }
}
