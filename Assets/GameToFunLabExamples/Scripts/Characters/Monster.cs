using GameToFunLab.Characters;
using Scripts.TableLoader;
using UnityEngine;

namespace Scripts.Characters
{
    /// <summary>
    /// 몬스터 기본 클레스
    /// </summary>
    public class Monster : GameToFunLab.Characters.Monster
    {
        /// <summary>
        /// 몬스터 정보 초기화.
        /// </summary>
        protected override void InitializationStat() 
        {
            base.InitializationStat();
            if (vnum <= 0) return;
            TableLoaderManager tableLoaderManager = TableLoaderManager.Instance;
            var info = tableLoaderManager.TableMonster.GetMonsterData(vnum);
            // FG_Logger.Log("InitializationStat vnum: "+vnum+" / info.vnum: "+info.vnum+" / StatMoveSpeed: "+info.StatMoveSpeed);
            if (info.Vnum > 0)
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
            
            if (Status == ICharacter.CharacterStatus.Dead)
            {
                // FG_Logger.Log("monster dead");
                return false;
            }
            // if (monsterStat.damageIgnore >= Random.Range(1f, 100f))
            // {
            //     return;
            // }
                
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
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * CurrentMoveSpeed);
        }
        /// <summary>
        /// 보스전 시작할때 일반 몬스터 죽이기 
        /// </summary>
        public void DestroyByChallengeBoss()
        {
            if (sceneGame == null) return;
            Status = ICharacter.CharacterStatus.Dead;
            Destroy(this.gameObject);
        }
    }
}
