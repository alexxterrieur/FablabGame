using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Script
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        
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
            {
                // Handle A button pressed logic here
                Debug.Log("A button pressed");
            }
        }
        
        public void ReceiveBInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Handle B button pressed logic here
                Debug.Log("B button pressed");
            }
        }
        
        public void ReceiveStartInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Handle X button pressed logic here
                Debug.Log("X button pressed");
            }
        }
        
        public void ReceiveSelectInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Handle Y button pressed logic here
                Debug.Log("Y button pressed");
            }
        }

        private void MovementPlayer(InputAction.CallbackContext context, Vector2 input)
        {
            if (context.started)
                playerMovement.SetDirection(input);
            else if (context.canceled)
                playerMovement.SetDirection(-input);
        }
    }
}