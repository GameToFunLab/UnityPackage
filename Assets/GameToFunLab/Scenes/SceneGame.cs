using GameToFunLab.Core;
using GameToFunLab.Maps;
using GameToFunLab.Popup;
using GameToFunLab.UI;
using UnityEngine;

namespace GameToFunLab.Scenes
{
    public class SceneGame : DefaultScene
    {
        public static SceneGame Instance { get; private set; }
        public string tagPlayer = "Player";
        public string tagMonster = "Monster";
        
        public enum GameState { Begin, Combat, End, DirectionStart, DirectionEnd };
        public enum GameSubState { Normal, BossChallenge };
        public GameState state;
        public GameSubState stateSub;

        [HideInInspector] public GameObject player;

        public Camera mainCamera;
        
        public SaveDataManager saveDataManager;
        public CalculateManager calculateManager;
        public CameraManager cameraManager;
        public UIWindowManager uIWindowManager;
        public MapManager mapManager;
        public PopupManager popupManager;
        
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            player = GameObject.FindWithTag(tagPlayer);
        }
        public virtual long GetMaxEnemyValue()
        {
            return 10;
        }
        public void SetStateEnd()
        {
            state = GameState.End;
        }
    }
}