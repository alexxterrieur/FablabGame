using GameManagement;
using Player.Script;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputsManagement
{
    public class CharacterInputHandler : MonoBehaviour, IPlayerInputsControlled
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            if (!playerMovement) Debug.LogError("PlayerMovement is not assigned in the inspector");
            if (!playerInteraction) Debug.LogError("PlayerInteraction is not assigned in the inspector");
            if (!gameManager) Debug.LogError("GameManager is not assigned in the inspector");
        }

        public QTEKey pressedKey { get; private set; }

        private void SetKeyState(InputAction.CallbackContext context, QTEKey key)
        {
            if (context.started || context.performed)
            {
                pressedKey |= key; // Add flag
            }
            else if (context.canceled)
            {
                pressedKey &= ~key; // Remove flag
            }
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.up, QTEKey.Up);
        }
        
        public void ReceiveMovementDownInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.down, QTEKey.Down);
        }
        
        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.left, QTEKey.Left);
        }
        
        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            MovementPlayer(context, Vector2.right, QTEKey.Right);
        }

        public void ReceiveAInput(InputAction.CallbackContext context)
        { 
            if (context.started)
            {
                if (gameManager.IsGamePaused())
                    gameManager.ResumeGame();
                else
                    playerInteraction.Interact();
            }

        }

        public void ReceiveBInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (gameManager.IsGamePaused())
                    gameManager.RestartGame();
                else
                    playerInteraction.ThrowItem();
            }
        }

        public void ReceiveStartInput(InputAction.CallbackContext context)
        {
        }

        public void ReceiveSelectInput(InputAction.CallbackContext context)
        {
            gameManager.PauseGame();
        }

        private void MovementPlayer(InputAction.CallbackContext context, Vector2 input, QTEKey key)
        {
            if (context.started)
                playerMovement.SetDirection(input);
            else if (context.canceled && pressedKey.HasFlag(key))
                playerMovement.SetDirection(-input);

            SetKeyState(context, key);
        }
        
        public void ResetInputs()
        {
            Debug.Log("Resetting character inputs");
            playerMovement.ResetDirection();
        }
    }
}