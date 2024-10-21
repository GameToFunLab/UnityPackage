using GameToFunLab.Core;
using Scripts.TableLoader;
using UnityEngine;

namespace Scripts.Scenes
{
    public class SceneGame : GameToFunLab.Scenes.SceneGame
    {
        [HideInInspector] public TableLoaderManager tableLoaderManager;

        protected override void Awake()
        {
            if (TableLoaderManager.Instance == null)
            {
                SceneManager.LoadSceneByName("Intro");
                return;
            }
            base.Awake();
            tableLoaderManager = TableLoaderManager.Instance;
        }

        public override long GetMaxEnemyValue()
        {
            return 10;
        }
    }
}