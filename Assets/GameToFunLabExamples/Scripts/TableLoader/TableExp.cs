using GameToFunLab.TableLoader;

namespace FocusGame.Core.TableLoader
{
    public class TableExp : DefaultTable
    {	
        public long GetNeedExp(int vnum) => long.TryParse(GetDataColumn(vnum, "NeedExp"), out var v) ? v : 0;
    }
}