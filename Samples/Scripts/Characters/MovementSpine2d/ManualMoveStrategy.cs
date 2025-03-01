using GameToFunLab.Characters.Movement;
using GameToFunLab.Runtime.Characters.Movement;
using Spine.Unity;
using UnityEngine;

namespace Scripts.Characters.MovementSpine2d
{
    public class ManualMoveStrategy : IMovementStrategy
    {
        public Transform PlayerTransform { get; set; }
        public float PlayerSpeed { get; set; }
        public float OriginalScaleX { get; set; }
        
        private Vector3 direction;
        private Vector3 directionPrev;
        private SkeletonAnimation skeletonAnimation;
        public string walkForwardAnim = "walk01_f";
        public string walkBackwardAnim = "walk01_b";
        public string waitForwardAnim = "wait01_f";
        public string waitBackwardAnim = "wait01_b";
        
        public ManualMoveStrategy(Transform transform, float speed, float originalScaleX, SkeletonAnimation pSkeletonAnimation)
        {
            PlayerTransform = transform;
            PlayerSpeed = speed;
            OriginalScaleX = originalScaleX;
            skeletonAnimation = pSkeletonAnimation;
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
            
            
            if (direction.x > 0 && direction.y == 0)
            {
                // 오른쪽
                skeletonAnimation.AnimationName = walkForwardAnim;
                skeletonAnimation.Skeleton.ScaleX = 1;
                directionPrev = direction;
            }
            else if (direction.x < 0 && direction.y == 0)
            {
                // 왼쪽
                skeletonAnimation.AnimationName = walkForwardAnim;
                skeletonAnimation.Skeleton.ScaleX = -1;
                directionPrev = direction;
            }
            else if (direction.y > 0 && direction.x == 0)
            {
                // 위
                skeletonAnimation.AnimationName = walkBackwardAnim;
                skeletonAnimation.Skeleton.ScaleX = 1;
                directionPrev = direction;
            }
            else if (direction.y < 0 && direction.x == 0)
            {
                // 아래
                skeletonAnimation.AnimationName = walkForwardAnim;
                skeletonAnimation.Skeleton.ScaleX = 1;
                directionPrev = direction;
            }
            else if (direction.x > 0 && direction.y > 0)
            {
                // 오른쪽 위
                skeletonAnimation.AnimationName = walkBackwardAnim;
                skeletonAnimation.Skeleton.ScaleX = 1;
                directionPrev = direction;
            }
            else if (direction.x < 0 && direction.y > 0)
            {
                // 왼쪽 위
                skeletonAnimation.AnimationName = walkBackwardAnim;
                skeletonAnimation.Skeleton.ScaleX = -1;
                directionPrev = direction;
            }
            else if (direction.x > 0 && direction.y < 0)
            {
                // 오른쪽 아래
                skeletonAnimation.AnimationName = walkForwardAnim;
                skeletonAnimation.Skeleton.ScaleX = 1;
                directionPrev = direction;
            }
            else if (direction.x < 0 && direction.y < 0)
            {
                // 왼쪽 아래
                skeletonAnimation.AnimationName = walkForwardAnim;
                skeletonAnimation.Skeleton.ScaleX = -1;
                directionPrev = direction;
            }
            else
            {
                if (directionPrev.x > 0 && directionPrev.y == 0)
                {
                    // 오른쪽
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
                else if (directionPrev.x < 0 && directionPrev.y == 0)
                {
                    // 왼쪽
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = -1;
                }
                else if (directionPrev.y > 0 && directionPrev.x == 0)
                {
                    // 위
                    skeletonAnimation.AnimationName = waitBackwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
                else if (directionPrev.y < 0 && directionPrev.x == 0)
                {
                    // 아래
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
                else if (directionPrev.x > 0 && directionPrev.y > 0)
                {
                    // 오른쪽 위
                    skeletonAnimation.AnimationName = waitBackwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
                else if (directionPrev.x < 0 && directionPrev.y > 0)
                {
                    // 왼쪽 위
                    skeletonAnimation.AnimationName = waitBackwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = -1;
                }
                else if (directionPrev.x > 0 && directionPrev.y < 0)
                {
                    // 오른쪽 아래
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
                else if (directionPrev.x < 0 && directionPrev.y < 0)
                {
                    // 왼쪽 아래
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = -1;
                }
                else
                {
                    skeletonAnimation.AnimationName = waitForwardAnim;
                    skeletonAnimation.Skeleton.ScaleX = 1;
                }
            }
            
            PlayerTransform.Translate(direction * (PlayerSpeed * Time.deltaTime));
        }
    }

}