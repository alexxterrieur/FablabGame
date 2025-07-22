using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimonInputManager : Controller
{
    [SerializeField] private SimonManager simonManager;
  
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
        ClickOnButton(4, context);
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        ClickOnButton(5, context);
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void ResetInputs()
    {
        throw new System.NotImplementedException();
    }
}
