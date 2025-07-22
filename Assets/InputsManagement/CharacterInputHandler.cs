using Player.Script;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputsManagement
{
    public class CharacterInputHandler : MonoBehaviour, IPlayerInputsControlled
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerInteraction playerInteraction;
        
        public void ReceiveMovementUpInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.up);
        }
        
        public void ReceiveMovementDownInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.down);
        }
        
        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.left);
        }
        
        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.right);
        }

        public void ReceiveAInput(InputAction.CallbackContext context)
        { 
            if (context.started)
                playerInteraction.Interact();
        }

        public void ReceiveBInput(InputAction.CallbackContext context)
        {
        }

        public void ReceiveStartInput(InputAction.CallbackContext context)
        {
        }

        public void ReceiveSelectInput(InputAction.CallbackContext context)
        {
        }

        private void MovementPlayer(InputAction.CallbackContext context, Vector2 input)
        {
            if (context.started)
                playerMovement.SetDirection(input);
            else if (context.canceled)
                playerMovement.SetDirection(-input);
        }
        
        public void ResetInputs()
        {
            Debug.Log("Resetting character inputs");
            playerMovement.ResetDirection();
        }
    }
}