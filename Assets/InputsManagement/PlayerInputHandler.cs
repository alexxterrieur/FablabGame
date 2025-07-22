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

        [Header("Assemblers Reference")]
        [SerializeField] private SimonInputManager woodAssembler;
        /*
        [SerializeField] private Controller metalAssembler;
        [SerializeField] private Controller plasticAssembler;
        */
        
        private IPlayerInputsControlled currentInputHandler;
        private bool hasChangedInputHandler;

        private void Start()
        {
            if (orderChoiceInputHandler)
            {
                orderChoiceInputHandler.OnOrderSelected += ReceiveOrderSelected;
                
                currentInputHandler = orderChoiceInputHandler;
            }
            else Debug.LogError("OrderChoiceManager is not assigned in the inspector");
            
            if (!characterInputHandler) Debug.LogError("CharacterInputHandler is not assigned in the inspector");

            if (gameManager)
            { 
                gameManager.DeliveryPoint.OnItemDelivered += ReceiveItemDelivered;

                gameManager.WoodAssembler.OnOrderCompleted += ReceiveWoodAssemblerOrderCompleted;
                gameManager.MetalAssembler.OnOrderCompleted += ReceiveMetalAssemblerOrderCompleted;
                gameManager.PlasticAssembler.OnOrderCompleted += ReceivePlasticAssemblerOrderCompleted;
            }
            else Debug.LogError("GameManager is not assigned in the inspector");
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
            
            hasChangedInputHandler = false;
            currentInputHandler.ReceiveMovementUpInput(context);
        }
        
        public void ReceiveMovementDownInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
            
            hasChangedInputHandler = false;
            currentInputHandler.ReceiveMovementDownInput(context);
        }
        
        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
            
            hasChangedInputHandler = false;
            currentInputHandler.ReceiveMovementLeftInput(context);
        }
        
        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
            
            hasChangedInputHandler = false;
            currentInputHandler.ReceiveMovementRightInput(context);
        }
        
        public void ReceiveAInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
            
            hasChangedInputHandler = false;
            
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
            else
                currentInputHandler.ReceiveAInput(context);
        }
        
        public void ReceiveBInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
                
            hasChangedInputHandler = false;
            
            if (gameManager.IsGamePaused())
                gameManager.RestartGame();
        }
        
        public void ReceiveStartInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
                
            hasChangedInputHandler = false;
            
            // Handle X button pressed logic here
            Debug.Log("X button pressed");
        }
        
        public void ReceiveSelectInput(InputAction.CallbackContext context)
        {
            if (!context.started && hasChangedInputHandler) return;
                
            hasChangedInputHandler = false;
            
            gameManager.PauseGame();
        }

        private void ReceiveItemDelivered(int _)
        {
            currentInputHandler.ResetInputs();
            currentInputHandler = orderChoiceInputHandler;
            hasChangedInputHandler = true;
        }

        private void ReceiveOrderSelected()
        {
            currentInputHandler = characterInputHandler;
            hasChangedInputHandler = true;
        }

        private void ReceiveWoodAssemblerOrderCompleted()
        {
            currentInputHandler.ResetInputs();
            currentInputHandler = woodAssembler;
            hasChangedInputHandler = true;

            woodAssembler.SimonManager.OnAssembleurActivityEnd += ReceiveWoodAssemblerActivityEnd;
        }

        private void ReceiveWoodAssemblerActivityEnd(bool _)
        {
            woodAssembler.SimonManager.OnAssembleurActivityEnd -= ReceiveWoodAssemblerActivityEnd;

            currentInputHandler.ResetInputs();
            currentInputHandler = characterInputHandler;
            hasChangedInputHandler = true;
        }

        private void ReceiveMetalAssemblerOrderCompleted()
        {
            currentInputHandler.ResetInputs();
            //currentInputHandler = metalAssembler;
            hasChangedInputHandler = true;
        }

        private void ReceivePlasticAssemblerOrderCompleted()
        {
            currentInputHandler.ResetInputs();
            //currentInputHandler = plasticAssembler;
            hasChangedInputHandler = true;
        }

        private void OnDestroy()
        {
            if (orderChoiceInputHandler)
                orderChoiceInputHandler.OnOrderSelected -= ReceiveOrderSelected;

            if (gameManager)
            {
                gameManager.DeliveryPoint.OnItemDelivered -= ReceiveItemDelivered;

                gameManager.WoodAssembler.OnOrderCompleted -= ReceiveWoodAssemblerOrderCompleted;
                gameManager.MetalAssembler.OnOrderCompleted -= ReceiveMetalAssemblerOrderCompleted;
                gameManager.PlasticAssembler.OnOrderCompleted -= ReceivePlasticAssemblerOrderCompleted;
            }
        }
    }
}