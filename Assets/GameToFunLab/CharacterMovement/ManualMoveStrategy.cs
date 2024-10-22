using GameToFunLab.Characters;
using UnityEngine;

namespace GameToFunLab.CharacterMovement
{
    public class ManualMoveStrategy : IMovementStrategy
    {
        private Vector3 direction;
        private Vector3 directionPrev;
        private void HandleInput()
        {
            direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        
            direction.Normalize();
        }
        public void Move(DefaultCharacter character)
        {
            HandleInput();
            
            if (Input.GetKey(KeyCode.A))
            {
                character.transform.localScale = new Vector3(character.OriginalScaleX, character.transform.localScale.y, character.transform.localScale.z);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                character.transform.localScale = new Vector3(character.OriginalScaleX * -1, character.transform.localScale.y, character.transform.localScale.z);
            }
            
            character.transform.Translate(direction * (character.CurrentMoveSpeed * Time.deltaTime));
        }
    }

}