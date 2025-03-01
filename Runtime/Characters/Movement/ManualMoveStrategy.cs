using UnityEngine;

namespace GameToFunLab.Characters.Movement
{
    public class ManualMoveStrategy : IMovementStrategy
    {
        public Transform PlayerTransform { get; set; }
        public float PlayerSpeed { get; set; }
        public float OriginalScaleX { get; set; }
        
        private Vector3 direction;
        private Vector3 directionPrev;
        
        public ManualMoveStrategy(Transform transform, float speed, float originalScaleX)
        {
            PlayerTransform = transform;
            PlayerSpeed = speed;
            OriginalScaleX = originalScaleX;
        }
        private void HandleInput()
        {
            direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        
            direction.Normalize();
        }
        public void Move()
        {
            HandleInput();
            
            if (Input.GetKey(KeyCode.A))
            {
                PlayerTransform.localScale = new Vector3(OriginalScaleX, PlayerTransform.localScale.y, PlayerTransform.localScale.z);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                PlayerTransform.localScale = new Vector3(OriginalScaleX * -1, PlayerTransform.localScale.y, PlayerTransform.localScale.z);
            }
            
            PlayerTransform.Translate(direction * (PlayerSpeed * Time.deltaTime));
        }
    }

}