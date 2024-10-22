using System.Collections.Generic;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using UnityEngine;

namespace Scripts.TableLoader
{
    public class StruckTableMap
    {
        public int Unum;
        public string Name;
        public MapConstants.Type Type;
        public MapConstants.SubType Subtype;
        public string FolderName;
        public Vector2 PlayerSpawnPosition;
        public int BgmUnum;
    }
    public class TableMap : GameToFunLab.TableLoader.DefaultTable
    {
        private static readonly Dictionary<string, MapConstants.Type> MapType;
        private static readonly Dictionary<string, MapConstants.SubType> MapSubType;

        static TableMap()
        {
            MapType = new Dictionary<string, MapConstants.Type>
            {
                { "Common", MapConstants.Type.Common },
            };
            MapSubType = new Dictionary<string, MapConstants.SubType>
            {
            };
        }
        private static MapConstants.Type ConvertType(string type) => MapType.GetValueOrDefault(type, MapConstants.Type.None);
        private static MapConstants.SubType ConvertTypeSub(string type) => MapSubType.GetValueOrDefault(type, MapConstants.SubType.None);

        
        public StruckTableMap GetMapData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableMap();
            }
            var data = GetData(unum);
            return new StruckTableMap
            {
                Unum = int.Parse(data["Unum"]),
                Name = data["Name"],
                Type = ConvertType(data["Type"]),
                Subtype = ConvertTypeSub(data["Subtype"]),
                FolderName = data["FolderName"],
                PlayerSpawnPosition = ConvertPlayerSpawnPosition(data["PlayerSpawnPosition"]),
                BgmUnum = int.Parse(data["BgmUnum"]),
            };
        }

        private Vector2 ConvertPlayerSpawnPosition(string position)
        {
            Vector2 playerSpawnPosition = new Vector2(0, 0);
            if (position != "")
            {
                var result2 = position.Split(",");
                playerSpawnPosition.x = float.Parse(result2[0]);
                playerSpawnPosition.y = float.Parse(result2[1]);
            }
            return playerSpawnPosition;
        }
    }
}