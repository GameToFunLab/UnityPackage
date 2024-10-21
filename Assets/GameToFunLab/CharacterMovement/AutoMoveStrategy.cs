using GameToFunLab.Characters;
using UnityEngine;

namespace GameToFunLab.CharacterMovement
{
    public class AutoMoveStrategy : IMovementStrategy
    {
        private Transform playerTransform;

        public AutoMoveStrategy(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }

        public void Move(DefaultCharacter character)
        {
            // 플레이어 방향으로 자동 이동
            Vector3 direction = (playerTransform.position - character.transform.position).normalized;
            character.transform.position += direction * (character.CurrentMoveSpeed * Time.deltaTime);
        }
    }

}