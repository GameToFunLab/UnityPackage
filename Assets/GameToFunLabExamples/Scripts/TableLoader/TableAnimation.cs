namespace Scripts.TableLoader
{
    public class TableAnimation : GameToFunLab.TableLoader.DefaultTable
    {
        public string GetPath(int vnum) => GetDataColumn(vnum, "Path");
    }
}