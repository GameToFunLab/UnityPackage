using GameToFunLab.Core;
using UnityEngine;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class StruckTableSpine
    {
        public int Unum;
        public string Name;
        public string Path;
        public string Skeleton;
    }
    public class TableSpine : GameToFunLab.TableLoader.DefaultTable
    {
        public string GetPath(int unum) => GetDataColumn(unum, "Path");
        
        public StruckTableSpine GetSpineData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableSpine();
            }
            var data = GetData(unum);
            return new StruckTableSpine
            {
                Unum = int.Parse(data["Unum"]),
                Name = data["Name"],
                Path = data["Path"],
                Skeleton = data["Skeleton"],
            };
        }
        public GameObject GetPrefab(int unum) {
            var info = GetSpineData(unum);
            if (info.Unum == 0) return null;
        
            string prefabPath = info.Path;
            if (prefabPath == "") {
                FgLogger.Log("prefabPath is ''. shape: "+info.Unum);
                return null;
            }
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null) {
                FgLogger.Log("prefab is null. prefabPath: "+prefabPath);
                return null;
            }
            return prefab;
        }
        public string GetShapePath(int unum)
        {
            var info = GetSpineData(unum);
            return info.Path;
        }
    }
}