using GameToFunLab.TableLoader;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class TableExp : DefaultTable
    {	
        public long GetNeedExp(int Unum) => long.TryParse(GetDataColumn(Unum, "NeedExp"), out var v) ? v : 0;
    }
}