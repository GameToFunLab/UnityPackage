using GameToFunLab.CharacterMovement;
using GameToFunLab.Scenes;
using UnityEngine;

namespace GameToFunLab.Characters
{
    /// <summary>
    /// 몬스터 기본 클레스
    /// </summary>
    public class Monster : DefaultCharacter
    {
        // 스폰될때 vid
        public int vid;
        // 몬스터 테이블 vnum
        public int vnum;
        [HideInInspector] public GameObject player;
        [HideInInspector] public Collider2D[] hits;  // 필요한 크기로 초기화
        [HideInInspector] public SceneGame sceneGame;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            StatAtk = 100;
            CurrentAtk = 100;
            StatMoveSpeed = 1f;
            StatHp = 100;
            CurrentHp = 100;
            CurrentMoveSpeed = 1f;
            transform.localScale = new Vector3(1f, 1f, 0);
            OriginalScaleX = 1f;
        }
        protected override void Start()
        {
            base.Start();
            sceneGame = SceneGame.Instance;
            player = GameObject.FindWithTag(sceneGame.tagPlayer);
            gameObject.tag = sceneGame.tagMonster;
        
            // 자동으로 플레이어에게 다가가는 이동 전략 설정
            MovementStrategy = new AutoMoveStrategy(player.transform);
            
            InitializationStat();
            Run();
        }
        /// <summary>
        /// 테이블에서 가져온 몬스터 정보 셋팅
        /// </summary>
        protected virtual void InitializationStat() 
        {
            if (vnum <= 0) return;
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
            return true;
        }
        /// <summary>
        /// 몬스터에게 데미지 주기 
        /// </summary>
        /// <param name="damage">데미지 수치</param>
        public virtual bool OnDamage(long damage)
        {
            if (damage <= 0) return false;
            
            if (Status == ICharacter.CharacterStatus.Dead)
            {
                // FG_Logger.Log("monster dead");
                return false;
            }
                
            CurrentHp = CurrentHp - damage;
            // -1 이면 죽지 않는다
            if (StatHp < 0)
            {
                CurrentHp = 1;
            }

            if (CurrentHp <= 0)
            {
                //FG_Logger.Log("dead vid : " + this.vid);
                Status = ICharacter.CharacterStatus.Dead;
                float delay = 0.01f;
                Destroy(this.gameObject, delay);

                OnDead();
            }
            else
            {
                Status = ICharacter.CharacterStatus.Damage;
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
            return false;
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
                IsAttacking = true;
                Status = ICharacter.CharacterStatus.Idle;
                // StartCoroutine(AttackCoroutine(collision.gameObject.GetComponent<Player>()));
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagPlayer))
            {
                IsAttacking = false;
                Status = ICharacter.CharacterStatus.Idle;
                Invoke(nameof(Run), 0.3f);
            }
        }
        /// <summary>
        /// 몬스터가 플레이어한테 자동 이동하기 
        /// </summary>
        private void UpdateAutoMove()
        {
            // if (IsCurrentAninameIsAttack() == true) return;
            if (Status != ICharacter.CharacterStatus.Run) return;

            SetFlipToTarget(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
                Time.deltaTime * CurrentMoveSpeed);
        }
    }
}
