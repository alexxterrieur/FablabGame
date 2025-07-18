using GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Script
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Game References")]
        [SerializeField] private GameManager gameManager;
        
        [Header("Player References")]
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
            if (!context.started) return;
            
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
            else
                playerInteraction.Interact();
        }
        
        public void ReceiveBInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (gameManager.IsGamePaused())
                    gameManager.RestartGame();
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
                gameManager.PauseGame();
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