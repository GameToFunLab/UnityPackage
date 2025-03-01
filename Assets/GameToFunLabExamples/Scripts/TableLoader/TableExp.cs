using GameToFunLab.TableLoader;

namespace Scripts.TableLoader
{
    public class TableExp : DefaultTable
    {	
        public long GetNeedExp(int Unum) => long.TryParse(GetDataColumn(Unum, "NeedExp"), out var v) ? v : 0;
    }
}