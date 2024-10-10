using UnityEngine;

namespace GameToFunLab.Characters
{
    public class DefaultCharacter : MonoBehaviour
    {
        /// <summary>
        /// 캐릭터 기본 클레스
        /// <para>플레이어, 몬스터의 상위 클레스</para>
        /// </summary>
        protected enum CharacterStatus
        {
            None,
            Idle,
            Run,
            Attack,
            Damage,
            Dead,
            DontMove // 못 움직이게 할 때 
        }
        /// <summary>
        /// 캐릭터 기본 클레스
        /// <para>플레이어, 몬스터의 상위 클레스</para>
        /// </summary>
        public enum Grade
        {
            None,
            Common,
            Boss,
        }
        
        public string[] attackAniNames = new string[] {"attack"};
        public string idleAniName = "idle";
        public string runAniName =  "run";
        public string damageAniName =  "damage";

        private CharacterStatus status = CharacterStatus.None;
    
        public float statHp;
        public float statAtk;
        public float statMoveSpeed;
        public long currentHp;
        public long currentAtk;
        public float currentMoveSpeed;
        [HideInInspector] public bool isAttacking;

        protected CharacterStatus Status
        {
            get => status;
            set
            {
                if (value >= 0)
                {
                    status = value;
                }
            }
        }
        // 공격 가능 y/n
        private bool possibleAttack = true;

        public bool PossibleAttack
        {
            get => possibleAttack;
            set => possibleAttack = value;
        }

        [HideInInspector] public float originalScaleX;

        private Renderer characterRenderer;

        public enum CharacterSortingOrder
        {
            Normal,
            AlwaysOnTop,
            AlwaysOnBottom,
            Fixed
        }

        public CharacterSortingOrder sortingOrder;
 
        /// <summary>
        /// 캐릭터가 flip 되었는지 체크
        /// <para>
        /// 디폴트는 왼쪽을 바라봄
        /// </para>
        /// </summary>
        /// <returns></returns>
        public bool IsFlip() {
            return Mathf.Approximately(transform.localScale.x, (originalScaleX * -1f));
        }

        public void SetIsPossibleFlip(bool set)
        {
            isPossibleFlip = set;
        }
        private bool isPossibleFlip = true;
        private bool IsPossibleFlip()
        {
            // 공격중이면 flip 하지 않는다
            return isPossibleFlip;
        }
        /// <summary>
        /// 캐릭터 방향 셋팅하기
        /// </summary>
        /// <param name="isFlip"></param>
        private void SetFlip(bool isFlip)
        {
            if (IsPossibleFlip() != true) return;

            transform.localScale = isFlip ? new Vector3(originalScaleX * -1f, transform.localScale.y, transform.localScale.z) : new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        }
        /// <summary>
        /// 타겟 오브젝트가 있을경우 방향 셋팅하기
        /// </summary>
        /// <param name="targetTransform"></param>
        protected void SetFlipToTarget(Transform targetTransform)
        {
            Vector3 destination = new Vector2(targetTransform.position.x, transform.position.y);
            SetFlip(!(transform.position.x > destination.x));
        }
        protected virtual void Awake()
        {
            isAttacking = false;
            currentAtk = (long)statAtk;
            currentHp = (long)statHp;
            currentMoveSpeed = statMoveSpeed;

            Status = CharacterStatus.None;
            originalScaleX = transform.localScale.x;
        }

        protected virtual void Start()
        {
            characterRenderer = GetComponent<Renderer>();
        }
        /// <summary>
        /// 연출 시작할때 캐릭터에 셋팅해주기 
        /// </summary>
        /// <param name="set">true:연출중 / false:연출끝</param>
        protected virtual void SetByDirection(bool set) 
        {
            if (set) {
                Status = CharacterStatus.DontMove;
                possibleAttack = false;
            }
            else {
                Status = CharacterStatus.Idle;
                possibleAttack = true;
            }
        }
        /// <summary>
        /// 공격 버튼 눌렀을때 처리 
        /// </summary>
        protected virtual bool DownAttack()
        {
            //anim.SetTrigger("onAttack");
            if (Status == CharacterStatus.Attack || Status == CharacterStatus.Dead) return false;
            if (PossibleAttack != true) return false;

            Status = CharacterStatus.Attack;
            return true;
        }
        /// <summary>
        /// 캐릭터가 이동중인지 
        /// </summary>
        /// <returns></returns>
        private bool IsRunning() {
            return (Status == CharacterStatus.Run);
        }
        /// <summary>
        /// 캐릭터가 이동할 수 있는 상태인지 
        /// </summary>
        /// <returns></returns>
        private bool IsPossibleRun()
        {
            if (isAttacking) return false;
            if (currentMoveSpeed <= 0) return false;
            
            return (Status == CharacterStatus.Idle || Status == CharacterStatus.None);
        }
        /// <summary>
        /// 플레이어 이동 시작 
        /// </summary>
        public void Run() 
        {
            if (IsPossibleRun() != true) return;
            if (IsRunning()) return;

            // FG_Logger.Log("player Run status: "+player.Status);
            Status = CharacterStatus.Run;
        }
        /// <summary>
        ///  플레이어 움직임 멈춤 
        /// </summary>
        public void Stop() {
            // FG_Logger.Log("player Stop");
            Status = CharacterStatus.Idle;
        }
        public void SetSortingOrder(CharacterSortingOrder value)
        {
            sortingOrder = value;
        }
        /// <summary>
        /// 몬스터가 죽은 상태인지 체크 
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return Status == CharacterStatus.Dead;
        }
        public void SetDeadState()
        {
            Status = CharacterStatus.Dead;
        }
        private void UpdatePosition()
        {
            if (sortingOrder == CharacterSortingOrder.Fixed) return;
            int baseSortingOrder;
            // 무조건 위 또는 무조건 아래 플래그 확인
            if (sortingOrder == CharacterSortingOrder.AlwaysOnTop)
            {
                baseSortingOrder = 32767;
            }
            else if (sortingOrder == CharacterSortingOrder.AlwaysOnBottom)
            {
                baseSortingOrder = -32768;
            }
            else
            {
                // y 위치에 따라 sortingOrder 설정
                baseSortingOrder = -(int)(transform.position.y * 100);
            }

            characterRenderer.sortingOrder = baseSortingOrder;
        }
        protected virtual void Update()
        {
            UpdatePosition();
        }
        /// <summary>
        /// 강제로 이동시키기
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveForce(float x, float y)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }
        /// <summary>
        /// sorting layer 바꾸기
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="order"></param>
        public void ChangeSortingLyaer(string layerName, int order)
        {
            GetComponent<Renderer>().sortingLayerName = layerName;
            GetComponent<Renderer>().sortingOrder = order;
            // 그림자 처리
            Transform shadow = transform.Find("Shadow");
            if (shadow)
            {
                shadow.gameObject.GetComponent<Renderer>().sortingLayerName = layerName;
                shadow.gameObject.GetComponent<Renderer>().sortingOrder = order - 1;
            }
        }

        public void SetMoveSpeed(float speed)
        {
            // -1 일때는 리셋 
            if (Mathf.Approximately(speed, -1))
            {
                currentMoveSpeed = statMoveSpeed;
                return;
            }
            currentMoveSpeed = speed;
        }

        protected bool IsStatusAttack()
        {
            return Status == CharacterStatus.Attack;
        }
    }
}