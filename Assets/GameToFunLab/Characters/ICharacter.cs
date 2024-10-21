using UnityEngine;

namespace GameToFunLab.Characters
{
    public interface ICharacter
    {
        /// <summary>
        /// 캐릭터 기본 클레스
        /// <para>플레이어, 몬스터의 상위 클레스</para>
        /// </summary>
        enum CharacterStatus
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
        enum Grade
        {
            None,
            Common,
            Boss,
        }
        enum CharacterSortingOrder
        {
            Normal,
            AlwaysOnTop,
            AlwaysOnBottom,
            Fixed
        }
        
        long Vnum { get; set; }
        float StatHp { get; set; }
        float StatAtk { get; set; }
        float StatMoveSpeed { get; set; }
        long CurrentHp { get; set; }
        long CurrentAtk { get; set; }
        float CurrentMoveSpeed { get; set; }
        CharacterStatus CurrentStatus { get; set; }
        CharacterSortingOrder SortingOrder { get; set; }
        bool PossibleAttack { get; set; }
        float OriginalScaleX { get; set; }
        bool IsAttacking { get; set; }
        
        void Move(Vector3 direction);
        void Attack();
        void TakeDamage(int damage);
        
    }
}