using GameToFunLab.Characters.Movement;
using GameToFunLab.Runtime.Characters.Movement;
using UnityEngine;

namespace GameToFunLab.Characters
{
    /// <summary>
    /// 기본 캐릭터 클레스
    /// </summary>
    ///<inheritdoc cref="ICharacter"/>
    public class DefaultCharacter : MonoBehaviour, ICharacter
    {
        public long Unum { get; set; }
        public float StatHp { get; set; }
        public float StatAtk { get; set; }
        public float StatMoveSpeed { get; set; }
        public long CurrentHp { get; set; }
        public long CurrentAtk { get; set; }
        public float CurrentMoveSpeed { get; set; }
        public ICharacter.CharacterStatus CurrentStatus { get; set; }
        public ICharacter.CharacterSortingOrder SortingOrder { get; set; }
        public float OriginalScaleX { get; set; }
        public bool IsAttacking { get; set; }
        // 공격 가능 y/n
        public bool PossibleAttack { get; set; }
        public string MyTag { get; set; }
        public string TargetTag { get; set; }
        
        public string[] attackAniNames = new string[] {"attack"};
        public string idleAniName = "idle";
        public string runAniName =  "run";
        public string damageAniName =  "damage";

        private Renderer characterRenderer;

        protected IMovementStrategy MovementStrategy;
        
        protected virtual void Awake()
        {
            IsAttacking = false;
            CurrentStatus = ICharacter.CharacterStatus.None;
        }

        protected virtual void Start()
        {
            characterRenderer = GetComponent<Renderer>();
            
            // statatk 값들은 table 에서 불러올 수 있기 때문에 Start 에서 처리한다.
            CurrentAtk = (long)StatAtk;
            CurrentHp = (long)StatHp;
            CurrentMoveSpeed = StatMoveSpeed;
            OriginalScaleX = transform.localScale.x;
            InitializationStat();
        }
        public void InitializeByEditor()
        {
            InitializationStat();
        }
        /// <summary>
        /// 테이블에서 가져온 몬스터 정보 셋팅
        /// </summary>
        protected virtual void InitializationStat() 
        {
        }
        /// <summary>
        /// 캐릭터가 flip 되었는지 체크
        /// <para>
        /// 디폴트는 왼쪽을 바라봄
        /// </para>
        /// </summary>
        /// <returns></returns>
        public bool IsFlip() {
            return Mathf.Approximately(transform.localScale.x, (OriginalScaleX * -1f));
        }

        public void SetIsPossibleFlip(bool set)
        {
            isPossibleFlip = set;
        }
        private bool isPossibleFlip = true;
        private ICharacter characterImplementation;

        private bool IsPossibleFlip()
        {
            // 공격중이면 flip 하지 않는다
            return isPossibleFlip;
        }
        /// <summary>
        /// 캐릭터 방향 셋팅하기
        /// </summary>
        /// <param name="isFlip"></param>
        protected void SetFlip(bool isFlip)
        {
            if (IsPossibleFlip() != true) return;

            transform.localScale = isFlip ? new Vector3(OriginalScaleX * -1f, transform.localScale.y, transform.localScale.z) : new Vector3(OriginalScaleX, transform.localScale.y, transform.localScale.z);
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
        /// <summary>
        /// 연출 시작할때 캐릭터에 셋팅해주기 
        /// </summary>
        /// <param name="set">true:연출중 / false:연출끝</param>
        protected virtual void SetByDirection(bool set) 
        {
            if (set) {
                CurrentStatus = ICharacter.CharacterStatus.DontMove;
                PossibleAttack = false;
            }
            else {
                CurrentStatus = ICharacter.CharacterStatus.Idle;
                PossibleAttack = true;
            }
        }
        /// <summary>
        /// 공격 버튼 눌렀을때 처리 
        /// </summary>
        protected virtual bool DownAttack()
        {
            //anim.SetTrigger("onAttack");
            if (CurrentStatus == ICharacter.CharacterStatus.Attack || CurrentStatus == ICharacter.CharacterStatus.Dead) return false;
            if (PossibleAttack != true) return false;

            CurrentStatus = ICharacter.CharacterStatus.Attack;
            return true;
        }
        /// <summary>
        /// 캐릭터가 이동할 수 있는 상태인지 
        /// </summary>
        /// <returns></returns>
        private bool IsPossibleRun()
        {
            if (IsAttacking) return false;
            if (CurrentMoveSpeed <= 0) return false;
            
            return (CurrentStatus == ICharacter.CharacterStatus.Idle || CurrentStatus == ICharacter.CharacterStatus.None);
        }
        /// <summary>
        /// 플레이어 이동 시작 
        /// </summary>
        public void Run() 
        {
            if (IsPossibleRun() != true) return;
            if (IsStatusRun()) return;

            // FG_Logger.Log("player Run status: "+player.Status);
            CurrentStatus = ICharacter.CharacterStatus.Run;
        }
        /// <summary>
        ///  플레이어 움직임 멈춤 
        /// </summary>
        public void Stop() {
            // FG_Logger.Log("player Stop");
            CurrentStatus = ICharacter.CharacterStatus.Idle;
        }
        public void SetSortingOrder(ICharacter.CharacterSortingOrder value)
        {
            SortingOrder = value;
        }
        private void UpdatePosition()
        {
            if (SortingOrder == ICharacter.CharacterSortingOrder.Fixed) return;
            int baseSortingOrder;
            // 무조건 위 또는 무조건 아래 플래그 확인
            if (SortingOrder == ICharacter.CharacterSortingOrder.AlwaysOnTop)
            {
                baseSortingOrder = 32767;
            }
            else if (SortingOrder == ICharacter.CharacterSortingOrder.AlwaysOnBottom)
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
            // UpdatePosition();
            
            // 캐릭터의 움직임을 전략 패턴에 위임
            if (MovementStrategy != null)
            {
                MovementStrategy.Move();
            }
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
                CurrentMoveSpeed = StatMoveSpeed;
                return;
            }
            CurrentMoveSpeed = speed;
        }

        public void Move(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 몬스터가 죽은 상태인지 체크 
        /// </summary>
        /// <returns></returns>
        public bool IsStatusDead()
        {
            return CurrentStatus == ICharacter.CharacterStatus.Dead;
        }
        protected bool IsStatusAttack()
        {
            return CurrentStatus == ICharacter.CharacterStatus.Attack;
        }
        protected bool IsStatusRun()
        {
            return CurrentStatus == ICharacter.CharacterStatus.Run;
        }
        protected bool IsSatusIdle()
        {
            return CurrentStatus == ICharacter.CharacterStatus.Idle;
        }
        protected bool IsStatusIdle()
        {
            return CurrentStatus != ICharacter.CharacterStatus.Idle;
        }
        protected bool IsStatusNone()
        {
            return CurrentStatus != ICharacter.CharacterStatus.None;
        }

        /// <summary>
        /// 캐릭터 상태 변화
        /// </summary>
        /// <param name="value"></param>
        private void SetStatus(ICharacter.CharacterStatus value)
        {
            CurrentStatus = value;
        }
        protected void SetStatusDead()
        {
            SetStatus(ICharacter.CharacterStatus.Dead);
        }
        protected void SetStatusIdle()
        {
            SetStatus(ICharacter.CharacterStatus.Idle);
        }
        protected void SetStatusRun()
        {
            SetStatus(ICharacter.CharacterStatus.Run);
        }
    }
}