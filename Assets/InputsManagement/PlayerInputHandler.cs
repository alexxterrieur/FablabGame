using GameManagement;
using OrderChoice;
using System.Collections;
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
        [SerializeField] private EndMenuManager endMenuInputHandler;

        [Header("Assemblers Reference")]
        [SerializeField] private SimonInputManager plasticAssembler;
        [SerializeField] private QTEInputManager metalAssembler;
        [SerializeField] private MillingInputHandler woodAssembler;
        
        
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
                gameManager.OnGameFinished += ReceiveGameFinished;
                
                gameManager.DeliveryPoint.OnItemDelivered += ReceiveItemDelivered;

                gameManager.WoodAssembler.OnOrderCompleted += ReceiveWoodAssemblerOrderCompleted;
                gameManager.MetalAssembler.OnOrderCompleted += ReceiveMetalAssemblerOrderCompleted;
                gameManager.PlasticAssembler.OnOrderCompleted += ReceivePlasticAssemblerOrderCompleted;
                gameManager.CustomManager.OnEnter += EnterCustom;
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
            else
                currentInputHandler.ReceiveBInput(context);
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

        private void ReceiveGameFinished(bool _)
        {
            currentInputHandler = endMenuInputHandler;
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

            woodAssembler.millingMachine.millingMachineManager.OnAssembleurActivityEnd += ReceiveAssemblerActivityEnd;
        }

        private void ReceiveAssemblerActivityEnd(bool value, Assembler assembler)
        {
            //woodAssembler.SimonManager.OnAssembleurActivityEnd -= ReceiveWoodAssemblerActivityEnd;

            currentInputHandler?.ResetInputs();
            hasChangedInputHandler = true;

            if (value)
            {
                currentInputHandler = null;
                StartCoroutine(WaitForCraft(assembler));
            }
            else
            {
                currentInputHandler = characterInputHandler;
                assembler.OnAssembleurActivityExit(false);
            }

        }

        private IEnumerator WaitForCraft(Assembler assembler)
        {
            yield return new WaitForSeconds(4f);
            currentInputHandler = characterInputHandler;
            currentInputHandler.ResetInputs();
            hasChangedInputHandler = true;
            assembler.OnAssembleurActivityExit(true);
        }

        private void ReceiveMetalAssemblerOrderCompleted()
        {
            metalAssembler.OnActivityEnd += ReceiveAssemblerActivityEnd;
            currentInputHandler.ResetInputs();
            currentInputHandler = metalAssembler;
            hasChangedInputHandler = true;
        }

        private void ReceivePlasticAssemblerOrderCompleted()
        {
            plasticAssembler.SimonManager.OnAssembleurActivityEnd += ReceiveAssemblerActivityEnd;
            currentInputHandler.ResetInputs();
            currentInputHandler = plasticAssembler;
            hasChangedInputHandler = true;
        }

        private void EnterCustom(Mesh _)
        {
            gameManager.CustomManager.OnExit += ExitCustom;
            currentInputHandler.ResetInputs();
            currentInputHandler = gameManager.CustomManager.customInput;
            hasChangedInputHandler = true;
        }

        private void ExitCustom()
        {
            currentInputHandler?.ResetInputs();
            hasChangedInputHandler = true;
            currentInputHandler = characterInputHandler;
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