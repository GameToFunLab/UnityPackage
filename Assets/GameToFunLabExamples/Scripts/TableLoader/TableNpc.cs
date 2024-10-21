using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.TableLoader;

namespace Scripts.TableLoader
{
    public class StruckTableNpc
    {
        public int Vnum;
        public string Name;
        public int SpineVnum;
        public string DefaultSkin;
        public float Scale;
        public ICharacter.Grade Grade;
        public float StatHp;
        public float StatAtk;
        public float StatMoveSpeed;
        public int RewardExp;
        public int RewardGold;
    }
    public class TableNpc : DefaultTable
    {
        private static readonly Dictionary<string, ICharacter.Grade> mapGrade;

        static TableNpc()
        {
            mapGrade = new Dictionary<string, ICharacter.Grade>
            {
                { "Common", ICharacter.Grade.Common },
                { "Boss", ICharacter.Grade.Boss },
            };
        }
        public ICharacter.Grade ConvertGrade(string grade) => mapGrade.GetValueOrDefault(grade, ICharacter.Grade.None);

        public StruckTableNpc GetNpcData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableNpc();
            }
            var data = GetData(vnum);
            return new StruckTableNpc
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
    }
}