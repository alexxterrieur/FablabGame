using GameManagement;
using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimonInputManager : MonoBehaviour, IPlayerInputsControlled
{
    [SerializeField] private SimonManager simonManager;
    [SerializeField] private GameManager gameManager;
  
    private void ClickOnButton(int i, InputAction.CallbackContext context)
    {
        if (context.started)
        {
            simonManager.OnButtonPressed(i);
        }
    }

    public void ReceiveMovementUpInput(InputAction.CallbackContext context)
    {
        ClickOnButton(0, context);
    }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context)
    {
        ClickOnButton(1, context);
    }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
    {
        ClickOnButton(3, context);
    }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context)
    {
        ClickOnButton(2, context);
    }

    public void ReceiveAInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
            else
                ClickOnButton(4, context);
        }
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameManager.IsGamePaused())
                gameManager.RestartGame();
            else
                ClickOnButton(5, context);
        }
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        if (context.started)
            gameManager.PauseGame();
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context) { }

    public void ResetInputs() { }
    
    public SimonManager SimonManager => simonManager;
}
