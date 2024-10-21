using GameToFunLab.Characters;

namespace GameToFunLab.CharacterMovement
{
    public interface IMovementStrategy
    {
        void Move(DefaultCharacter character);
    }
}