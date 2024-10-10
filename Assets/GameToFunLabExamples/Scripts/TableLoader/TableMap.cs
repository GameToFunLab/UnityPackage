using System.Collections.Generic;
using GameToFunLab.Core;
using GameToFunLab.Maps;

namespace Scripts.TableLoader
{
    public class StruckTableMap
    {
        public int Vnum;
        public string Name;
        public MapManager.Type Type;
        public MapManager.SubType Subtype;
        public string TileMapPrefabName;
    }
    public class TableMap : GameToFunLab.TableLoader.DefaultTable
    {
        private static readonly Dictionary<string, MapManager.Type> MapType;
        private static readonly Dictionary<string, MapManager.SubType> MapSubType;

        static TableMap()
        {
            MapType = new Dictionary<string, MapManager.Type>
            {
                { "Common", MapManager.Type.Common },
            };
            MapSubType = new Dictionary<string, MapManager.SubType>
            {
            };
        }
        private static MapManager.Type ConvertType(string type) => MapType.GetValueOrDefault(type, MapManager.Type.None);
        private static MapManager.SubType ConvertTypeSub(string type) => MapSubType.GetValueOrDefault(type, MapManager.SubType.None);

        
        public StruckTableMap GetSpineData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableMap();
            }
            var data = GetData(vnum);
            return new StruckTableMap
            {
                Vnum = int.Parse(data["Vnum"]),
                Name = data["Name"],
                Type = ConvertType(data["Type"]),
                Subtype = ConvertTypeSub(data["Subtype"]),
                TileMapPrefabName = data["TileMapPrefabName"],
            };
        }
    }
}