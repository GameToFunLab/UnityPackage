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
        [HideInInspector] public int vid;
        // 몬스터 테이블 vnum
        private int vnum;
        private GameObject player;
        private SceneGame sceneGame;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            statAtk = 100;
            currentAtk = 100;
            statMoveSpeed = 1f;
            statHp = 100;
            currentHp = 100;
            currentMoveSpeed = 1f;
            transform.localScale = new Vector3(1f, 1f, 0);
            originalScaleX = 1f;
        }
        protected override void Start()
        {
            base.Start();
            sceneGame = SceneGame.Instance;
            player = GameObject.FindWithTag(sceneGame.tagPlayer);
            gameObject.tag = sceneGame.tagEnemy;
        
            InitializationStat();
            Run();
        }
        /// <summary>
        /// 테이블에서 가져온 몬스터 정보 셋팅
        /// </summary>
        void InitializationStat() 
        {
            if (vnum <= 0) return;
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
    }
}
