using GameToFunLab.Characters;
using Scripts.Scenes;
using Scripts.TableLoader;
using Spine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Characters
{
    /// <summary>
    /// 플레이어 기본 클레스
    /// </summary>
    public class MyPlayer : GameToFunLab.Characters.Player
    {
        protected override void Awake()
        {
            base.Awake();
            if (TableLoaderManager.Instance != null)
            {
                StatAtk = (long)TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatAtk();
                StatMoveSpeed = TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatMoveSpeed();
            }
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            hits = new Collider2D[sceneGame.GetMaxEnemyValue()];
        }
        /// <summary>
        /// 공격 버튼 눌렀을때 처리 
        /// </summary>
        protected override bool DownAttack()
        {
            if (!base.DownAttack()) return false;

            SetFlipToTarget(targetMonster.transform);
            int attackNameIndex = Random.Range(0, attackAniNames.Length);
            return true;
        }
        /// <summary>
        /// 내가 데미지 받았을때 처리 
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        /// <param name="monster">누가 때렸는지</param>
        public void OnDamage(long damage, GameObject monster)
        {
            // soundAttack.PlayOneShot(audioClipDamage);
            if (damage <= 0) return;
            CurrentHp = CurrentHp - damage;
            
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                sceneGame.state = MySceneGame.GameState.End;
                return;
            }
        } 
        public bool SearchAndAttackMonsters()
        {
            // 캡슐 콜라이더 2D와 충돌 중인 모든 콜라이더를 검색
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            Vector2 size = new Vector2(capsuleCollider.size.x * Mathf.Abs(transform.localScale.x), capsuleCollider.size.y * transform.localScale.y);
            Vector2 point = transform.position;
            int hitCount = Physics2D.OverlapCapsuleNonAlloc(point, size, capsuleCollider.direction, 0f, hits);
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D hit = hits[i];
                if (hit.CompareTag(sceneGame.tagMonster))
                {
                    GameToFunLab.Characters.Monster monster = hit.GetComponent<GameToFunLab.Characters.Monster>();
                    if (monster != null)
                    {
                        // FgLogger.Log("Player attacked the monster after animation!");
                        targetMonster = monster.gameObject;
                        return true;
                    }
                }
            }

            return false;
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagMonster))
            {
                IsAttacking = true;
                GameToFunLab.Characters.Monster monster = collision.gameObject.GetComponent<GameToFunLab.Characters.Monster>();
                if (monster.gameObject.GetComponent<GameToFunLab.Characters.Monster>().IsStatusDead())
                {
                    // FG_Logger.Log("player / update / monster dead");
                    targetMonster = null;
                }
                else {
                    targetMonster = monster.gameObject;
                    DownAttack();
                }
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagMonster))
            {
                IsAttacking = false;
            }
        }
    }
}
