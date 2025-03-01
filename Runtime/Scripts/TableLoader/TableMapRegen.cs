using GameToFunLab.Core;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class StruckTableMapRegen
    {
        public int Unum;
        public int MapUnum;
        public int MonsterUnum;
        public string SpawnArea;
        public string SpawnPosition;
        public int SpawnCount;
        public float SpawnRepeatTime;
        public bool SpawnRepeatByDead;
    }
    public class TableMapRegen : GameToFunLab.TableLoader.DefaultTable
    {
        public StruckTableMapRegen GetMapRegenData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableMapRegen();
            }
            var data = GetData(unum);
            return new StruckTableMapRegen
            {
                Unum = int.Parse(data["Unum"]),
                MapUnum = int.Parse(data["MapUnum"]),
                MonsterUnum = int.Parse(data["MonsterUnum"]),
                SpawnArea = data["SpawnArea"],
                SpawnPosition = data["SpawnPosition"],
                SpawnCount = int.Parse(data["SpawnCount"]),
                SpawnRepeatTime = float.Parse(data["SpawnRepeatTime"]),
                SpawnRepeatByDead = data["SpawnRepeatByDead"] == "Y",
            };
        }
    }
}