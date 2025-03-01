using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.TableLoader;

namespace Scripts.TableLoader
{
    public class StruckTableNpc
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

        public StruckTableNpc GetNpcData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableNpc();
            }
            var data = GetData(unum);
            return new StruckTableNpc
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
    }
}