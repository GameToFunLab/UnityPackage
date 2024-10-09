using GameToFunLab.TableLoader;

namespace FocusGame.Core.TableLoader
{
    public class TableConfig : DefaultTable
    {
        // enum을 사용하여 컬럼 번호를 명시적으로 나타냄
        private enum ColumnIndex
        {
            MaxChapter = 1,
            MaxStage = 2,
            PolySkillGacha1NeedDia = 3,
            PolySkillGacha10NeedDia = 4,
            PolyWeaponGacha1NeedDia = 5,
            PolyWeaponGacha10NeedDia = 6,
            MaxLevel = 7,
            HudSkillCount = 8,
            ItemGradeLevelMax = 9,
            ComposeNeedCount = 10,
            MonsterSpawnRepeatTime = 11,
            MonsterSpawnMaxcount = 12,
            PolyMonsterHp = 13,
            ProductIdGooglAdMobRewardDia = 14, // 애드몹. 광고 시청 후 보상주는 타입
            PolyWeaponCompseNeedDia = 15, // 무기 합성에 필요한 다이아
            PolyPlayerAtkByLevelUp = 16,
            PolySpawnRepeatTimeRate = 17,
            TimeQuickModeByAds = 18, // 광고보고 퀵모드 유지시간(초)
            ProductIdGooglAdMobRewardQuickMode = 19, // 애드몹. 광고 시청 후 퀵모드 주는 타입
            PolyQuickModeRate = 20,
            CountMaxChat = 21,
            PolyPlayerStatAtk = 23, // 플레이어 기본 atk
            PolyPlayerStatMoveSpeed = 24, // 플레이어 기본 speed
            PolyPlayerDamageRateByLevel = 25, // 최종 데미지 가중치 레벨(%)
            PolyPlayerDamageRateByUpgradeAtkLevel = 26, // 최종 데미지 가중치 공격력레벨업 atk(%)
            PolyPlayerDamageRateByWeapon = 27, // 최종 데미지 가중치 무기(%)
            PolyPlayerDamageRateBySkin = 28, // 최종 데미지 가중치 스킨(%)
            PolyPlayerDamageRateBySkill = 29, // 최종 데미지 가중치 스킬(%)
            PolySkillLevelUpNeedCount = 30, // 스킬 레벨업시 필요한 스킬 개수. 첫 기준 개수
            PolyBossMonsterInfoRate = 31, // 보스몬스터일때 hp, 경험치, 골드 증가 계수
            PolySkillGacha40NeedDia = 32,
            PolyWeaponGacha40NeedDia = 33,
        }

        // 제네릭 메서드를 사용하여 공통 로직 처리
        private T GetConfigValue<T>(ColumnIndex index)
        {
            string data = GetDataColumn((int)index, "Value");
            if (typeof(T) == typeof(int) && int.TryParse(data, out var intValue))
                return (T)(object)intValue;
            if (typeof(T) == typeof(float) && float.TryParse(data, out var floatValue))
                return (T)(object)floatValue;

            return (T)(object)data;
        }

        public int GetMaxChapter() => GetConfigValue<int>(ColumnIndex.MaxChapter);
        public int GetMaxStage() => GetConfigValue<int>(ColumnIndex.MaxStage);
        public int GetPolySkillGacha1NeedDia() => GetConfigValue<int>(ColumnIndex.PolySkillGacha1NeedDia);
        public int GetPolySkillGacha10NeedDia() => GetConfigValue<int>(ColumnIndex.PolySkillGacha10NeedDia);
        public int GetPolySkillGacha40NeedDia() => GetConfigValue<int>(ColumnIndex.PolySkillGacha40NeedDia);
        public int GetPolyWeaponGacha1NeedDia() => GetConfigValue<int>(ColumnIndex.PolyWeaponGacha1NeedDia);
        public int GetPolyWeaponGacha10NeedDia() => GetConfigValue<int>(ColumnIndex.PolyWeaponGacha10NeedDia);
        public int GetPolyWeaponGacha40NeedDia() => GetConfigValue<int>(ColumnIndex.PolyWeaponGacha40NeedDia);
        public int GetMaxLevel() => GetConfigValue<int>(ColumnIndex.MaxLevel);
        public int GetHudSkillCount() => GetConfigValue<int>(ColumnIndex.HudSkillCount);
        public int GetItemGradeLevelMax() => GetConfigValue<int>(ColumnIndex.ItemGradeLevelMax);
        public int GetComposeNeedCount() => GetConfigValue<int>(ColumnIndex.ComposeNeedCount);
        public float GetMonsterSpawnRepeatTime() => GetConfigValue<float>(ColumnIndex.MonsterSpawnRepeatTime);
        public int GetMonsterSpawnMaxcount() => GetConfigValue<int>(ColumnIndex.MonsterSpawnMaxcount);
        public float GetPolyMonsterHp() => GetConfigValue<float>(ColumnIndex.PolyMonsterHp);
        public string GetProductIdGooglAdMobRewardDia() => GetConfigValue<string>(ColumnIndex.ProductIdGooglAdMobRewardDia);
        public string GetProductIdGooglAdMobRewardQuickMode() => GetConfigValue<string>(ColumnIndex.ProductIdGooglAdMobRewardQuickMode);
        public int GetPolyWeaponCompseNeedDia() => GetConfigValue<int>(ColumnIndex.PolyWeaponCompseNeedDia);
        public float GetPolyPlayerAtkByLevelUp() => GetConfigValue<float>(ColumnIndex.PolyPlayerAtkByLevelUp);
        public float GetPolySpawnRepeatTime() => GetConfigValue<float>(ColumnIndex.PolySpawnRepeatTimeRate);
        public int GetTimeQuickModeByAds() => GetConfigValue<int>(ColumnIndex.TimeQuickModeByAds);
        public float GetPolyQuickModeRate() => GetConfigValue<float>(ColumnIndex.PolyQuickModeRate);
        public int GetCountMaxChat() => GetConfigValue<int>(ColumnIndex.CountMaxChat);
        public float GetPolyPlayerStatAtk() => GetConfigValue<float>(ColumnIndex.PolyPlayerStatAtk);
        public float GetPolyPlayerStatMoveSpeed() => GetConfigValue<float>(ColumnIndex.PolyPlayerStatMoveSpeed);
        public int GetPolyPlayerDamageRateByLevel() => GetConfigValue<int>(ColumnIndex.PolyPlayerDamageRateByLevel);
        public int GetPolyPlayerDamageRateByUpgradeAtkLevel() => GetConfigValue<int>(ColumnIndex.PolyPlayerDamageRateByUpgradeAtkLevel);
        public int GetPolyPlayerDamageRateByWeapon() => GetConfigValue<int>(ColumnIndex.PolyPlayerDamageRateByWeapon);
        public int GetPolyPlayerDamageRateBySkin() => GetConfigValue<int>(ColumnIndex.PolyPlayerDamageRateBySkin);
        public int GetPolyPlayerDamageRateBySkill() => GetConfigValue<int>(ColumnIndex.PolyPlayerDamageRateBySkill);
        public int GetPolySkillLevelUpNeedCount() => GetConfigValue<int>(ColumnIndex.PolySkillLevelUpNeedCount);
        public float GetPolyBossMonsterInfoRate() => GetConfigValue<float>(ColumnIndex.PolyBossMonsterInfoRate);
    }
}