using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ElectronicObserver.Utility.Data
{
    public static class MapCell2WikiPoint
    {
        static Dictionary<string, object> MapCellData;
        static MapCell2WikiPoint()
        {
            var data = System.Text.Encoding.Default.GetString(Properties.Resources.MapCell);
            try
            {
                MapCellData = new JavaScriptSerializer().DeserializeObject(data) as Dictionary<string, object>;
            }
            catch
            {
                MapCellData = null;
            }
        }

        public static string GetWikiPointName(int MapID, int MapNo, int CellID)
        {
            string MapString = MapID.ToString() + "-" + MapNo.ToString();

            return GetWikiPointName(MapString, CellID);
        }

        public static string GetWikiPointName(string MapString, int CellID)
        {
            if (MapCellData != null && MapCellData.ContainsKey(MapString))
            {
                var Map = MapCellData[MapString] as Dictionary<string, object>;
                string Cell = CellID.ToString();
                if (Map != null && Map.ContainsKey(Cell))
                {
                    return Map[Cell].ToString();
                }
            }
            return null;
        }
    }
}
