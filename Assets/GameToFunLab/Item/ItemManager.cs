namespace GameToFunLab.Item
{
    public class ItemManager
    {
        public enum Type
        {
            None,
            Currency,
            Consumable,
            Equip
        }
        public enum SubType
        {
            None,
            Gold,
            Dia,
            OpenSkillSlot,
            PlayerTitle,
            Weapon,
            PlusStage,
            SkinFace
        }
        public enum PurchaseCurrencyType
        {
            None,
            Gold,
            Dia,
            Cash,
            GoogleAd
        }

        public const int itemVnumGold = 1;
        public const int itemVnumDia = 2;
        public const int itemVnumDefaultWeapon = 20010001;
        public const int itemVnumDefaultSkinFace = 20020001;
        public const int itemVnumSystemSkinFace = 20020015; // 채팅에서 사용중
        public const int itemVnumDefaultTitle = 10020001;
        public const int itemVnumLastTitle = 10020016; // 마지막 칭호
    }
}