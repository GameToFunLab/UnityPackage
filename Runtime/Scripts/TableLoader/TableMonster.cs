using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.TableLoader;
using UnityEngine;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class StruckTableMonster
    {
        public int Unum;
        public string Name;
        public int SpineUnum;
        public string DefaultSkin;
        public float Scale;
        public ICharacter.Grade Grade;
        public float StatHp;
        public float StatAtk;
        public float StatMoveSpeed;
        public int RewardExp;
        public int RewardGold;
    }
    public class TableMonster : DefaultTable
    {
        private static readonly Dictionary<string, ICharacter.Grade> mapGrade;

        static TableMonster()
        {
            mapGrade = new Dictionary<string, ICharacter.Grade>
            {
                { "Common", ICharacter.Grade.Common },
                { "Boss", ICharacter.Grade.Boss },
            };
        }
        public ICharacter.Grade ConvertGrade(string grade) => mapGrade.GetValueOrDefault(grade, ICharacter.Grade.None);

        public StruckTableMonster GetMonsterData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableMonster();
            }
            var data = GetData(unum);
            return new StruckTableMonster
            {
                Unum = int.Parse(data["Unum"]),
                Name = data["Name"],
                SpineUnum = int.Parse(data["SpineUnum"]),
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
        
        public GameObject GetPrefab(int unum) {
            var info = GetMonsterData(unum);
            if (info.SpineUnum == 0) return null;
        
            string prefabPath = TableLoaderManager.Instance.TableSpine.GetPath(info.SpineUnum);
            if (prefabPath == "") {
                FgLogger.Log("prefabPath is ''. shape: "+info.SpineUnum);
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
            var info = GetMonsterData(unum);
            return info.SpineUnum <= 0 ? "" : TableLoaderManager.Instance.TableSpine.GetPath(info.SpineUnum);
        }
    }
}