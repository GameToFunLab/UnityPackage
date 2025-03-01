using GameToFunLab.Runtime.Scripts.TableLoader;
using UnityEngine;

namespace GameToFunLab.Runtime.Scripts.Characters
{
    /// <summary>
    /// 몬스터 기본 클레스
    /// </summary>
    public class MyMonster : GameToFunLab.Characters.Monster
    {
        /// <summary>
        /// 몬스터 정보 초기화.
        /// </summary>
        protected override void InitializationStat() 
        {
            base.InitializationStat();
            if (unum <= 0) return;
            TableLoaderManager tableLoaderManager = TableLoaderManager.Instance;
            var info = tableLoaderManager.TableMonster.GetMonsterData(unum);
            // FG_Logger.Log("InitializationStat unum: "+unum+" / info.unum: "+info.unum+" / StatMoveSpeed: "+info.StatMoveSpeed);
            if (info.Unum > 0)
            {
                StatAtk = info.StatAtk;
                CurrentAtk = (long)StatAtk;
                StatMoveSpeed = info.StatMoveSpeed;
                StatHp = info.StatHp;
                CurrentHp = (long)StatHp;
                CurrentMoveSpeed = StatMoveSpeed;
                float scale = info.Scale;
                transform.localScale = new Vector3(scale, scale, 0);
                OriginalScaleX = scale;
            }
        }
    }
}
