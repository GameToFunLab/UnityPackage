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
    public class Player : GameToFunLab.Characters.Player
    {
        protected override void Awake()
        {
            base.Awake();
            if (TableLoaderManager.Instance != null)
            {
                statAtk = (long)TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatAtk();
                statMoveSpeed = TableLoaderManager.Instance.TableConfig.GetPolyPlayerStatMoveSpeed();
            }
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            hits = new Collider2D[sceneGame.GetMaxEnemyValue()];
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (IsStatusAttack())
            {

            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Status = CharacterStatus.Run;
            }
            else if (Status != CharacterStatus.Attack)
            {
                Status = CharacterStatus.Idle;
            }

            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.up * (currentMoveSpeed * Time.deltaTime));

            if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.Translate(Vector3.left * (currentMoveSpeed * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.down * (currentMoveSpeed * Time.deltaTime));

            if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                transform.Translate(Vector3.right * (currentMoveSpeed * Time.deltaTime));
            }
        }
    
        /// <summary>
        /// 공격 버튼 눌렀을때 처리 
        /// </summary>
        protected override bool DownAttack()
        {
            if (!base.DownAttack()) return false;

            SetFlipToTarget(targetMonster.transform);
            int attackNameIndex = Random.Range(0, attackAniNames.Length);
            PlayAnimationOnceAndThenLoop(attackAniNames[attackNameIndex]);
            return true;
        }
        /// <summary>
        /// 스파인 hit 이벤트 발동되었을때 처리 
        /// </summary>
        /// <param name="eEvent">이벤트 json 문구</param>
        protected override void OnSpineEventHit(Spine.Event eEvent) {
            long totalDamage = sceneGame.calculateManager.GetPlayerTotalAtk();
        
            // 캡슐 콜라이더 2D와 충돌 중인 모든 콜라이더를 검색
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            Vector2 size = new Vector2(capsuleCollider.size.x * Mathf.Abs(transform.localScale.x), capsuleCollider.size.y * transform.localScale.y);
            Vector2 point = (Vector2)transform.position + capsuleCollider.offset * transform.localScale;
            int hitCount = Physics2D.OverlapCapsuleNonAlloc(point, size, capsuleCollider.direction, 0f, hits);

            int countDamageMonster = 0;
            int maxDamageMonster = 10;
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D hit = hits[i];
                if (hit.CompareTag(sceneGame.tagEnemy))
                {
                    GameToFunLab.Characters.Monster monster = hit.GetComponent<GameToFunLab.Characters.Monster>();
                    if (monster != null)
                    {
                        // FgLogger.Log("Player attacked the monster after animation!");
                        monster.OnDamage(totalDamage);
                        ++countDamageMonster;
                        
                        // maxDamageMonster 마리 한테만 데미지 준다 
                        if (countDamageMonster > maxDamageMonster)
                        {
                            break;
                        }
                    }
                }
            }
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
            currentHp = currentHp - damage;
            
            if (currentHp <= 0)
            {
                currentHp = 0;
                sceneGame.state = SceneGame.GameState.End;
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
                if (hit.CompareTag(sceneGame.tagEnemy))
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
        /// <summary>
        /// 애니메이션이 끝나면 호출되는 콜백 함수
        /// </summary>
        /// <param name="entry"></param>
        protected override void OnAnimationCompleteToIdle(TrackEntry entry)
        {
            if (SkeletonAnimation == null) return;
            // 애니메이션 이벤트 리스너 제거
            SkeletonAnimation.AnimationState.Complete -= OnAnimationCompleteToIdle;

            // 연출중일때는 아무것도 하지 않는다
            if (sceneGame.state == SceneGame.GameState.DirectionStart) return;

            bool isCollisionMonster = SearchAndAttackMonsters();
            if (isCollisionMonster)
            {
                Status = CharacterStatus.Idle;
                // 공격시 flip 하기 위해 추가 
                SkeletonAnimation.AnimationState.SetAnimation(0, idleAniName, true);
                DownAttack();
                return;
            }
            // // 다른 애니메이션 loop로 실행
            Status = CharacterStatus.Idle;
            SkeletonAnimation.AnimationState.SetAnimation(0, idleAniName, true);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagEnemy))
            {
                isAttacking = true;
                GameToFunLab.Characters.Monster monster = collision.gameObject.GetComponent<GameToFunLab.Characters.Monster>();
                if (monster.gameObject.GetComponent<GameToFunLab.Characters.Monster>().IsDead())
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
            if (collision.gameObject.CompareTag(sceneGame.tagEnemy))
            {
                isAttacking = false;
            }
        }
    }
}
