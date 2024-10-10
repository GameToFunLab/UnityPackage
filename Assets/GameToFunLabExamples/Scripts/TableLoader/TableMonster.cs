using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.TableLoader;
using UnityEngine;

namespace Scripts.TableLoader
{
    public class StruckTableMonster
    {
        public int Vnum;
        public string Name;
        public int SpineVnum;
        public string DefaultSkin;
        public float Scale;
        public DefaultCharacter.Grade Grade;
        public float StatHp;
        public float StatAtk;
        public float StatMoveSpeed;
        public int RewardExp;
        public int RewardGold;
    }
    public class TableMonster : DefaultTable
    {
        private static readonly Dictionary<string, DefaultCharacter.Grade> mapGrade;

        static TableMonster()
        {
            mapGrade = new Dictionary<string, DefaultCharacter.Grade>
            {
                { "Common", DefaultCharacter.Grade.Common },
                { "Boss", DefaultCharacter.Grade.Boss },
            };
        }
        public DefaultCharacter.Grade ConvertGrade(string grade) => mapGrade.GetValueOrDefault(grade, DefaultCharacter.Grade.None);

        public StruckTableMonster GetMonsterData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableMonster();
            }
            var data = GetData(vnum);
            return new StruckTableMonster
            {
                Vnum = int.Parse(data["Vnum"]),
                Name = data["Name"],
                SpineVnum = int.Parse(data["SpineVnum"]),
                DefaultSkin = data["DefaultSkin"],
                Scale = float.Parse(data["Scale"]),
                Grade = ConvertGrade(data["Grade"]),
                StatHp = float.Parse(data["StatHp"]),
                StatAtk = float.Parse(data["StatAtk"]),
                StatMoveSpeed = float.Parse(data["StatMoveSpeed"]),
                RewardExp = int.Parse(data["RewardExp"]),
                RewardGold = int.Parse(data["RewardGold"]),
            };
        }
        
        public GameObject GetPrefab(int vnum) {
            var info = GetMonsterData(vnum);
            if (info.SpineVnum == 0) return null;
        
            string prefabPath = TableLoaderManager.Instance.TableSpine.GetPath(info.SpineVnum);
            if (prefabPath == "") {
                FgLogger.Log("prefabPath is ''. shape: "+info.SpineVnum);
                return null;
            }
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null) {
                FgLogger.Log("prefab is null. prefabPath: "+prefabPath);
                return null;
            }
            return prefab;
        }
        public string GetShapePath(int vnum)
        {
            var info = GetMonsterData(vnum);
            return info.SpineVnum <= 0 ? "" : TableLoaderManager.Instance.TableSpine.GetPath(info.SpineVnum);
        }
    }
}