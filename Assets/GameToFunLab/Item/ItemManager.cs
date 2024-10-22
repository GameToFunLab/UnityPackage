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

        public const int itemUnumGold = 1;
        public const int itemUnumDia = 2;
        public const int itemUnumDefaultWeapon = 20010001;
        public const int itemUnumDefaultSkinFace = 20020001;
        public const int itemUnumSystemSkinFace = 20020015; // 채팅에서 사용중
        public const int itemUnumDefaultTitle = 10020001;
        public const int itemUnumLastTitle = 10020016; // 마지막 칭호
    }
}