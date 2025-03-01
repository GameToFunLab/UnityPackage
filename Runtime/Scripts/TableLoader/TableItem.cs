using System.Collections.Generic;
using GameToFunLab.Core;
using GameToFunLab.Item;
using GameToFunLab.TableLoader;
using GameToFunLab.UI;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class StruckTableItem
    {
        public int Unum;
        public ItemManager.Type Type;
        public ItemManager.SubType SubType;
        public string Name;
        public UIIcon.Grade Grade;
        public float Damage;
        public float CoolTime;
        public string IconPath;
        public ItemManager.PurchaseCurrencyType PurchaseCurrencyType;
        public int PurchaseCost;
    }
    
    public class TableItem : DefaultTable
    {
        private static readonly Dictionary<string, ItemManager.Type> MapType;
        private static readonly Dictionary<string, ItemManager.SubType> MapSubType;
        private static readonly Dictionary<string, ItemManager.PurchaseCurrencyType> MapPurchaseCurrency;

        static TableItem()
        {
            MapType = new Dictionary<string, ItemManager.Type>
            {
                { "Currency", ItemManager.Type.Currency },
                { "Consumable", ItemManager.Type.Consumable },
                { "Equip", ItemManager.Type.Equip },
            };
            MapSubType = new Dictionary<string, ItemManager.SubType>
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
        private static ItemManager.SubType ConvertTypeSub(string type) => MapSubType.GetValueOrDefault(type, ItemManager.SubType.None);
        private static ItemManager.PurchaseCurrencyType ConvertPurchaseCurrency(string type) => MapPurchaseCurrency.GetValueOrDefault(type, ItemManager.PurchaseCurrencyType.None);
        
        public StruckTableItem GetItemData(int unum)
        {
            if (unum <= 0)
            {
                FgLogger.LogError("unum is 0.");
                return new StruckTableItem();
            }
            var data = GetData(unum);
            return new StruckTableItem
            {
                Unum = int.Parse(data["Unum"]),
                Type = ConvertType(data["Type"]),
                SubType = ConvertTypeSub(data["TypeSub"]),
                Name = data["Name"],
                Grade = UIIcon.IconGradeEnum[data["Grade"]],
                Damage = float.Parse(data["Damage"]),
                CoolTime = float.Parse(data["CoolTime"]),
                IconPath = data["IconPath"],
                PurchaseCurrencyType = ConvertPurchaseCurrency(data["PurchaseCurrency"]),
                PurchaseCost = int.Parse(data["PurchaseCost"]),
            };
        }
        public Dictionary<int, Dictionary<string, string>> GetWeapons()
        {
            Dictionary<int, Dictionary<string, string>> items = GetDatas();
            Dictionary<int, Dictionary<string, string>> weapons = new Dictionary<int, Dictionary<string, string>>();
            
            // foreach 문을 사용하여 딕셔너리 내용을 출력
            foreach (KeyValuePair<int, Dictionary<string, string>> outerPair in items)
            {
                int itemUnum = outerPair.Key;
                Dictionary<string, string> innerDictionary = outerPair.Value;

                if (innerDictionary == null) continue;
                if (innerDictionary["Unum"] == null || innerDictionary["Type"] != "Equip" || innerDictionary["TypeSub"] != "Weapon") continue;
                weapons.Add(itemUnum, innerDictionary);
            }

            return weapons;
        }
    }
}