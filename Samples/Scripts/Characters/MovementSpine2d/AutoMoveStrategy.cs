using GameToFunLab.Characters.Movement;
using GameToFunLab.Runtime.Characters.Movement;
using UnityEngine;

namespace Scripts.Characters.MovementSpine2d
{
    public class AutoMoveStrategy : IMovementStrategy
    {
        public Transform PlayerTransform { get; set; }
        public float PlayerSpeed { get; set; }
        public float OriginalScaleX { get; set; }
        
        public AutoMoveStrategy(Transform transform, float speed)
        {
            PlayerTransform = transform;
            PlayerSpeed = speed;
        }

        public void Move()
        {
            // 플레이어 방향으로 자동 이동
            Vector3 direction = (PlayerTransform.position - PlayerTransform.position).normalized;
            PlayerTransform.position += direction * (PlayerSpeed * Time.deltaTime);
        }
    }

}