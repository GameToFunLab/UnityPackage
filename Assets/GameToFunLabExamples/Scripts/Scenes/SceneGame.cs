using Scripts.TableLoader;
using UnityEngine;

namespace Scripts.Scenes
{
    public class SceneGame : GameToFunLab.Scenes.SceneGame
    {
        
        [HideInInspector] public TableLoaderManager tableLoaderManager;

        protected override void Awake()
        {
            base.Awake();
            tableLoaderManager = TableLoaderManager.Instance;
        }

        public override long GetMaxEnemyValue()
        {
            return 10;
        }
    }
}