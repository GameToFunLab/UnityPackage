using GameToFunLab.Core;

namespace Scripts.TableLoader
{
    public class StruckTableMapRegen
    {
        public int Vnum;
        public int MapVnum;
        public int MonsterVnum;
        public string SpawnArea;
        public string SpawnPosition;
        public int SpawnCount;
        public float SpawnRepeatTime;
        public bool SpawnRepeatByDead;
    }
    public class TableMapRegen : GameToFunLab.TableLoader.DefaultTable
    {
        public StruckTableMapRegen GetMapRegenData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableMapRegen();
            }
            var data = GetData(vnum);
            return new StruckTableMapRegen
            {
                Vnum = int.Parse(data["Vnum"]),
                MapVnum = int.Parse(data["MapVnum"]),
                MonsterVnum = int.Parse(data["MonsterVnum"]),
                SpawnArea = data["SpawnArea"],
                SpawnPosition = data["SpawnPosition"],
                SpawnCount = int.Parse(data["SpawnCount"]),
                SpawnRepeatTime = float.Parse(data["SpawnRepeatTime"]),
                SpawnRepeatByDead = data["SpawnRepeatByDead"] == "Y",
            };
        }
    }
}