using GameToFunLab.Characters;
using GameToFunLab.Characters.Movement;
using Scripts.Scenes;
using Scripts.TableLoader;
using UnityEngine;

namespace Scripts.Characters
{
    /// <summary>
    /// 플레이어 기본 클레스
    /// </summary>
    public class MyPlayer : Player
    {
        private MySceneGame mySceneGame;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            mySceneGame = MySceneGame.MyInstance;
            hits = new Collider2D[mySceneGame.GetMaxEnemyValue()];

            // 수동 플레이 이동 전략 설정
            MovementStrategy = new ManualMoveStrategy(transform, CurrentMoveSpeed, OriginalScaleX);
        }
        
        /// <summary>
        ///  정보 초기화.
        /// </summary>
        protected override void InitializationStat()
        {
            if (TableLoaderManager.Instance != null)
            {
                StatAtk = (long)TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatAtk();
                StatMoveSpeed = TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatMoveSpeed();
                CurrentAtk = (long)StatAtk;
                CurrentHp = (long)StatHp;
                CurrentMoveSpeed = StatMoveSpeed;
                OriginalScaleX = transform.localScale.x;
            }
        }
    }
}
