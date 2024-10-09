using System.Collections.Generic;
using GameToFunLab.Core;
using GameToFunLab.Core.UI;
using GameToFunLab.Item;
using GameToFunLab.TableLoader;
using GameToFunLab.UI;

namespace Scripts.TableLoader
{
    public class StruckTableItem
    {
        public int Vnum;
        public ItemManager.Type Type;
        public ItemManager.SubType SubType;
        public string Name;
        public UIIcon.Grade Grade;
        public int GradeLevel;
        public float Damage;
        public float CoolTime;
        public string IconPath;
        public string IconPathFaceSkin;
        public string SkinName;
        public ItemManager.PurchaseCurrencyType PurchaseCurrencyType;
        public int PurchaseCost;
        public string ProductID;
        
        public bool IsPlayerTitle;
        public bool IsSkillSlotOpen;
        public bool IsPlusStage;
    }
    
    public class TableItem : DefaultTable
    {
        private static readonly Dictionary<string, ItemManager.Type> MapType;
        private static readonly Dictionary<string, ItemManager.SubType> MapTypeSub;
        private static readonly Dictionary<string, ItemManager.PurchaseCurrencyType> MapPurchaseCurrency;

        static TableItem()
        {
            MapType = new Dictionary<string, ItemManager.Type>
            {
                { "Currency", ItemManager.Type.Currency },
                { "Consumable", ItemManager.Type.Consumable },
                { "Equip", ItemManager.Type.Equip },
            };
            MapTypeSub = new Dictionary<string, ItemManager.SubType>
            {
                { "Dia", ItemManager.SubType.Dia },
                { "Gold", ItemManager.SubType.Gold },
                { "Weapon", ItemManager.SubType.Weapon },
                { "OpenSkillSlot", ItemManager.SubType.OpenSkillSlot },
                { "PlayerTitle", ItemManager.SubType.PlayerTitle },
                { "PlusStage", ItemManager.SubType.PlusStage },
                { "SkinFace", ItemManager.SubType.SkinFace },
            };
            MapPurchaseCurrency = new Dictionary<string, ItemManager.PurchaseCurrencyType>
            {
                { "Dia", ItemManager.PurchaseCurrencyType.Dia },
                { "Gold", ItemManager.PurchaseCurrencyType.Gold },
                { "Cash", ItemManager.PurchaseCurrencyType.Cash },
            };
        }
        private static ItemManager.Type ConvertType(string type) => MapType.GetValueOrDefault(type, ItemManager.Type.None);
        private static ItemManager.SubType ConvertTypeSub(string type) => MapTypeSub.GetValueOrDefault(type, ItemManager.SubType.None);
        private static ItemManager.PurchaseCurrencyType ConvertPurchaseCurrency(string type) => MapPurchaseCurrency.GetValueOrDefault(type, ItemManager.PurchaseCurrencyType.None);
        
        public StruckTableItem GetItemData(int vnum)
        {
            if (vnum <= 0)
            {
                FgLogger.LogError("vnum is 0.");
                return new StruckTableItem();
            }
            var data = GetData(vnum);
            return new StruckTableItem
            {
                Vnum = int.Parse(data["Vnum"]),
                Type = ConvertType(data["Type"]),
                SubType = ConvertTypeSub(data["TypeSub"]),
                Name = data["Name"],
                Grade = UIIcon.IconGradeEnum[data["Grade"]],
                GradeLevel = int.Parse(data["GradeLevel"]),
                Damage = float.Parse(data["Damage"]),
                CoolTime = float.Parse(data["CoolTime"]),
                IconPath = data["IconPath"],
                IconPathFaceSkin = data["IconPath"].Replace("Skin","Face"),
                SkinName = data["SkinName"],
                PurchaseCurrencyType = ConvertPurchaseCurrency(data["PurchaseCurrency"]),
                PurchaseCost = int.Parse(data["PurchaseCost"]),
                ProductID = data["ProductID"],
                IsPlayerTitle = ConvertType(data["Type"]) == ItemManager.Type.Consumable && ConvertTypeSub(data["TypeSub"]) == ItemManager.SubType.PlayerTitle,
                IsSkillSlotOpen = ConvertType(data["Type"]) == ItemManager.Type.Consumable && ConvertTypeSub(data["TypeSub"]) == ItemManager.SubType.OpenSkillSlot,
                IsPlusStage = ConvertType(data["Type"]) == ItemManager.Type.Consumable && ConvertTypeSub(data["TypeSub"]) == ItemManager.SubType.PlusStage,
            };
        }
        public Dictionary<int, Dictionary<string, string>> GetWeapons()
        {
            Dictionary<int, Dictionary<string, string>> items = GetDatas();
            Dictionary<int, Dictionary<string, string>> weapons = new Dictionary<int, Dictionary<string, string>>();
            
            // foreach 문을 사용하여 딕셔너리 내용을 출력
            foreach (KeyValuePair<int, Dictionary<string, string>> outerPair in items)
            {
                int itemVnum = outerPair.Key;
                Dictionary<string, string> innerDictionary = outerPair.Value;

                if (innerDictionary == null) continue;
                if (innerDictionary["Vnum"] == null || innerDictionary["Type"] != "Equip" || innerDictionary["TypeSub"] != "Weapon") continue;
                weapons.Add(itemVnum, innerDictionary);
            }

            return weapons;
        }
        public Dictionary<int, Dictionary<string, string>> GetSkins()
        {
            Dictionary<int, Dictionary<string, string>> items = GetDatas();
            Dictionary<int, Dictionary<string, string>> skins = new Dictionary<int, Dictionary<string, string>>();
            
            // foreach 문을 사용하여 딕셔너리 내용을 출력
            foreach (KeyValuePair<int, Dictionary<string, string>> outerPair in items)
            {
                int itemVnum = outerPair.Key;
                Dictionary<string, string> innerDictionary = outerPair.Value;

                if (innerDictionary == null) continue;
                if (innerDictionary["Vnum"] == null || innerDictionary["Type"] != "Equip" || innerDictionary["TypeSub"] != "SkinFace") continue;
                skins.Add(itemVnum, innerDictionary);
            }

            return skins;
        }
        public Dictionary<UIIcon.Grade, int[]> GetInitGradeCount()
        {
            Dictionary<int, Dictionary<string, string>> items = GetWeapons();

            Dictionary<UIIcon.Grade, int[]> gradeCount = new Dictionary<UIIcon.Grade, int[]>();
            foreach (KeyValuePair<int, Dictionary<string, string>> outerPair in items)
            {
                Dictionary<string, string> innerDictionary = outerPair.Value;
                UIIcon.Grade grade = UIIcon.IconGradeEnum[innerDictionary["Grade"]];
                gradeCount.TryAdd(grade, new int[5]);
            }
            return gradeCount;
        }
    }
}