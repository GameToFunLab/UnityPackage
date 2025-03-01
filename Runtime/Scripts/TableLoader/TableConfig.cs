using GameToFunLab.TableLoader;

namespace GameToFunLab.Runtime.Scripts.TableLoader
{
    public class TableConfig : DefaultTable
    {
        // enum을 사용하여 컬럼 번호를 명시적으로 나타냄
        private enum ColumnIndex
        {
            PolyPlayerStatAtk = 1, // 플레이어 기본 atk
            PolyPlayerStatMoveSpeed = 2, // 플레이어 기본 speed
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

        public float GetPolyPlayerStatAtk() => GetConfigValue<float>(ColumnIndex.PolyPlayerStatAtk);
        public float GetPolyPlayerStatMoveSpeed() => GetConfigValue<float>(ColumnIndex.PolyPlayerStatMoveSpeed);
    }
}