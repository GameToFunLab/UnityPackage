using UnityEngine;

namespace GameToFunLab.Characters.Movement
{
    public interface IMovementStrategy
    {
        Transform PlayerTransform { get; set; }
        float PlayerSpeed { get; set; }
        float OriginalScaleX { get; set; }

        void Move();
    }
}