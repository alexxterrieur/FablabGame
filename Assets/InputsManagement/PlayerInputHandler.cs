using GameManagement;
using OrderChoice;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputsManagement
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Game References")]
        [SerializeField] private GameManager gameManager;
        
        [Header("Player References")]
        [SerializeField] private CharacterInputHandler characterInputHandler;
        
        [Header("Input Handlers")]
        [SerializeField] private OrderChoiceManager orderChoiceInputHandler;
        
        private IPlayerInputsControlled currentInputHandler;

        private void Start()
        {
            if (orderChoiceInputHandler)
            {
                orderChoiceInputHandler.OnOrderSelected += ReceiveOrderSelected;
                
                currentInputHandler = orderChoiceInputHandler;
            }
            else Debug.LogError("OrderChoiceManager is not assigned in the inspector");
            
            if (!characterInputHandler) Debug.LogError("CharacterInputHandler is not assigned in the inspector");
            
            if (gameManager) gameManager.DeliveryPoint.OnItemDelivered += ReceiveItemDelivered;
            else Debug.LogError("GameManager is not assigned in the inspector");
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context)
        {
            currentInputHandler.ReceiveMovementUpInput(context);
        }
        
        public void ReceiveMovementDownInput(InputAction.CallbackContext context)
        {
            currentInputHandler.ReceiveMovementDownInput(context);
        }
        
        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            currentInputHandler.ReceiveMovementLeftInput(context);
        }
        
        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            currentInputHandler.ReceiveMovementRightInput(context);
        }
        
        public void ReceiveAInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
            else
                currentInputHandler.ReceiveAInput(context);
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

        private void ReceiveItemDelivered(int _)
        {
            currentInputHandler = orderChoiceInputHandler;
        }

        private void ReceiveOrderSelected()
        {
            currentInputHandler = characterInputHandler;
        }

        private void OnDestroy()
        {
            if (orderChoiceInputHandler)
                orderChoiceInputHandler.OnOrderSelected -= ReceiveOrderSelected;

            if (gameManager)
                gameManager.DeliveryPoint.OnItemDelivered -= ReceiveItemDelivered;
        }
    }
}