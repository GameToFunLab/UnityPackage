using GameToFunLab.Characters;
using Scripts.Scenes;
using Scripts.TableLoader;
using Spine;
using UnityEngine;

namespace Scripts.Characters
{
    /// <summary>
    /// 몬스터 기본 클레스
    /// </summary>
    public class Monster : DefaultCharacter
    {
        // 스폰될때 vid
        [HideInInspector] public int vid;
        // 몬스터 테이블 vnum
        private int vnum;
        private GameObject player;
        private SceneGame sceneGame;
        private readonly Collider2D[] hits = new Collider2D[10];  // 필요한 크기로 초기화

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            player = GameObject.FindWithTag(sceneGame.tagPlayer);
            gameObject.tag = sceneGame.tagEnemy;
            InitializationStat();
            Run();
        }
        /// <summary>
        /// 몬스터 정보 초기화.
        /// </summary>
        void InitializationStat() 
        {
            if (vnum <= 0) return;
            TableLoaderManager tableLoaderManager = sceneGame.tableLoaderManager;
            var info = tableLoaderManager.TableMonster.GetMonsterData(vnum);
            // FG_Logger.Log("InitializationStat vnum: "+vnum+" / info.vnum: "+info.vnum+" / StatMoveSpeed: "+info.statMoveSpeed);
            statAtk = info.StatAtk;
            currentAtk = (long)statAtk;
            statMoveSpeed = info.StatMoveSpeed;

            statHp = info.StatHp;
            currentHp = (long)statHp;
            
            currentMoveSpeed = statMoveSpeed;
            float scale = info.Scale;
            transform.localScale = new Vector3(scale, scale, 0);
            originalScaleX = scale;
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            UpdateAutoMove();
        }
        /// <summary>    
        /// 몬스터의 hit area 에 플레이어가 있을경우 공격하기 
        /// <para>
        /// monsterAi.cs 에서 충돌 검사중
        /// </para>
        /// </summary>
        protected override bool DownAttack()
        {
            if (!base.DownAttack()) return false;
        
            // PlayAnimationOnceAndThenLoop(attackAniNames[attackNameIndex]);
            // fG_Spine2DController.PlayAnimation(attackAniNames[attackNameIndex], true);
            return true;
        }
        /// <summary>
        /// 몬스터에게 데미지 주기 
        /// </summary>
        /// <param name="damage">데미지 수치</param>
        public virtual bool OnDamage(long damage)
        {
            if (damage <= 0) return false;
            
            if (Status == CharacterStatus.Dead)
            {
                // FG_Logger.Log("monster dead");
                return false;
            }
            // if (monsterStat.damageIgnore >= Random.Range(1f, 100f))
            // {
            //     return;
            // }
                
            currentHp = currentHp - damage;
            // -1 이면 죽지 않는다
            if (statHp < 0)
            {
                currentHp = 1;
            }

        
            if (currentHp <= 0)
            {
                //FG_Logger.Log("dead vid : " + this.vid);
                Status = CharacterStatus.Dead;
                float delay = 0.01f;
                Destroy(this.gameObject, delay);

                OnDead();
            }
            else
            {
                Status = CharacterStatus.Damage;
            
                PlayAnimationOnceAndThenLoop(damageAniName);
            }

            return true;
        }
        /// <summary>
        /// 몬스터가 죽었을때 처리 
        /// </summary>
        private void OnDead()
        {
        }
        /// <summary>
        /// 플레이어의 스킬로 인한 데미지 입었을때 
        /// </summary>
        /// <param name="damage"></param>
        public void DamageFromSkill(long damage) {
            OnDamage(damage);
        }
        protected bool SearchAndAttackPlayer()
        {
            // 캡슐 콜라이더 2D와 충돌 중인 모든 콜라이더를 검색
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            Vector2 size = new Vector2(capsuleCollider.size.x * Mathf.Abs(transform.localScale.x), capsuleCollider.size.y * transform.localScale.y);
            Vector2 point = transform.position;
            int hitCount = Physics2D.OverlapCapsuleNonAlloc(point, size, capsuleCollider.direction, 0f, hits);
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D hit = hits[i];
                if (hit.CompareTag(sceneGame.tagPlayer))
                {
                    Player player = hit.GetComponent<Player>();
                    if (player != null)
                    {
                        // monster.TakeDamage(attackDamage);
                        // FgLogger.Log("Monster attacked the player after animation!");
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
            base.OnAnimationCompleteToIdle(entry);
        
            // 연출중일때는 아무것도 하지 않는다
            if (sceneGame.state == SceneGame.GameState.DirectionStart) return;
        
            bool isCollisionPlayer = SearchAndAttackPlayer();
            if (isCollisionPlayer)
            {
                Status = CharacterStatus.Idle;
                return;
            }
            Status = CharacterStatus.Idle;
            Run();
            // Status = CharacterStatus.idle;
        }
        public void Destroy() {
            Destroy(this.gameObject);
        }

        protected override void SetByDirection(bool set) 
        {
            base.SetByDirection(set);
            if (set == false)
            {
                Run();
            }
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagPlayer))
            {
                isAttacking = true;
                Status = CharacterStatus.Idle;
                // StartCoroutine(AttackCoroutine(collision.gameObject.GetComponent<Player>()));
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagPlayer))
            {
                isAttacking = false;
                Status = CharacterStatus.Idle;
                Invoke(nameof(Run), 0.3f);
            }
        }
        /// <summary>
        /// 몬스터가 플레이어한테 자동 이동하기 
        /// </summary>
        private void UpdateAutoMove()
        {
            // if (IsCurrentAninameIsAttack() == true) return;
            if (Status != CharacterStatus.Run) return;

            SetFlipToTarget(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
                Time.deltaTime * currentMoveSpeed);
        }
        /// <summary>
        /// 보스전 시작할때 일반 몬스터 죽이기 
        /// </summary>
        public void DestroyByChallengeBoss()
        {
            if (sceneGame == null) return;
            Status = CharacterStatus.Dead;
            Destroy(this.gameObject);
        }
    }
}
