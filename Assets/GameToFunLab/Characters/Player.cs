using GameToFunLab.Scenes;
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
        public Collider2D[] hits;  // 필요한 크기로 초기화
        public SceneGame sceneGame;

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            sceneGame = SceneGame.Instance;
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
                transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.Translate(Vector3.left * currentMoveSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.down * currentMoveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                transform.Translate(Vector3.right * currentMoveSpeed * Time.deltaTime);
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
            currentHp = currentHp - damage;
            
            if (currentHp <= 0)
            {
                currentHp = 0;
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
                if (hit.CompareTag(sceneGame.tagEnemy))
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
            if (collision.gameObject.CompareTag(sceneGame.tagEnemy))
            {
                isAttacking = true;
                Monster monster = collision.gameObject.GetComponent<Monster>();
                if (monster.gameObject.GetComponent<Monster>().IsDead())
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
