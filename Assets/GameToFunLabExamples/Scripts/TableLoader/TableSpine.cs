using GameToFunLab.Core;

namespace Scripts.TableLoader
{
    public class StruckTableSpine
    {
        public int Vnum;
        public string Name;
        public string Path;
        public string Skeleton;
    }
    public class TableSpine : GameToFunLab.TableLoader.DefaultTable
    {
        public string GetPath(int vnum) => GetDataColumn(vnum, "Path");
        
        public StruckTableSpine GetSpineData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableSpine();
            }
            var data = GetData(vnum);
            return new StruckTableSpine
            {
                Vnum = int.Parse(data["Vnum"]),
                Name = data["Name"],
                Path = data["Path"],
                Skeleton = data["Skeleton"],
            };
        }
    }
}