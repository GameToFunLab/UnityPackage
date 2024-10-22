using GameToFunLab.CharacterMovement;
using GameToFunLab.Maps.Objects;
using GameToFunLab.Scenes;
using Scripts.Maps.Objects;
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
        [HideInInspector] public SceneGame sceneGame;

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
            sceneGame = SceneGame.Instance;
            
            // 자동으로 플레이어에게 다가가는 이동 전략 설정
            MovementStrategy = new ManualMoveStrategy();
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
            CurrentHp = CurrentHp - damage;
            
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                sceneGame.SetStateEnd();
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
            if (collision.gameObject.CompareTag(sceneGame.tagMonster))
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
            else if (collision.gameObject.CompareTag(sceneGame.tagNpc))
            {
                isNpcNearby = true;
            }
            else if (collision.gameObject.CompareTag(sceneGame.tagMapObjectWarp))
            {
                ObjectWarp objectWarp = collision.gameObject.GetComponent<ObjectWarp>();
                if (objectWarp != null && objectWarp.toMapUnum > 0)
                {
                    SceneGame.Instance.mapManager.SetPlaySpawnPosition(objectWarp.toMapPlayerSpawnPosition);
                    SceneGame.Instance.mapManager.LoadMap(objectWarp.toMapUnum);
                }
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagMonster))
            {
                IsAttacking = false;
            }
            else if (collision.gameObject.CompareTag(sceneGame.tagNpc))
            {
                isNpcNearby = false;
            }
        }
        
        
    }
}
