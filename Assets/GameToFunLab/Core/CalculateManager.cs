using GameToFunLab.Scenes;
using UnityEngine;

namespace GameToFunLab.Core
{
    public class CalculateManager : MonoBehaviour
    {
        private SaveDataManager saveDataManager;

        private void Start()
        {
            saveDataManager = SceneGame.Instance.saveDataManager;
        }

        protected long GetPlayerWeaponAtk()
        {
            return 0;
        }
        public long GetPlayerTotalAtk()
        {
            return 0;
        }
    }
}