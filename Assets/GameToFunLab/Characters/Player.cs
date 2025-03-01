using GameToFunLab.Characters.Movement;
using GameToFunLab.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToFunLab.Characters
{
    /// <summary>
    /// 플레이어 기본 클레스
    /// </summary>
    public class Player : DefaultCharacter
    {
        [HideInInspector] public GameObject targetMonster;
        [HideInInspector] public Collider2D[] hits;  // 필요한 크기로 초기화

        private bool isNpcNearby;
        
        protected override void Awake()
        {
            base.Awake();
            isNpcNearby = false;
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            // 수동 플레이 이동 전략 설정
            MovementStrategy = new ManualMoveStrategy(transform, CurrentMoveSpeed, OriginalScaleX);
        }
        /// <summary>
        /// 테이블에서 가져온 몬스터 정보 셋팅
        /// </summary>
        protected override void InitializationStat() 
        {
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
            CurrentHp -= damage;
            
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
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
                if (hit.CompareTag(ConfigTags.Monster))
                {
                    Monster monster = hit.GetComponent<Monster>();
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
            if (collision.gameObject.CompareTag(ConfigTags.Monster))
            {
                IsAttacking = true;
                Monster monster = collision.gameObject.GetComponent<Monster>();
                if (monster.gameObject.GetComponent<Monster>().IsStatusDead())
                {
                    // FG_Logger.Log("player / update / monster dead");
                    targetMonster = null;
                }
                else {
                    targetMonster = monster.gameObject;
                    DownAttack();
                }
            }
            else if (collision.gameObject.CompareTag(ConfigTags.Npc))
            {
                isNpcNearby = true;
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(ConfigTags.Monster))
            {
                IsAttacking = false;
            }
            else if (collision.gameObject.CompareTag(ConfigTags.Npc))
            {
                isNpcNearby = false;
            }
        }
        
        
    }
}
