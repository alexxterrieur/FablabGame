using UnityEngine;
using UnityEngine.InputSystem;

public class SimonInputManager : MonoBehaviour
{
    [SerializeField] private SimonManager simonManager;
    public void Up(InputAction.CallbackContext context)
    {
        ClickOnButton(0, context);
    }

    public void Down(InputAction.CallbackContext context)
    {
        ClickOnButton(1, context);
    }
    public void Right(InputAction.CallbackContext context)
    {
        ClickOnButton(2, context);
    }
    public void Left(InputAction.CallbackContext context)
    {
        ClickOnButton(3, context);
    }
    public void A(InputAction.CallbackContext context)
    {
        ClickOnButton(4, context);
    }
    public void B(InputAction.CallbackContext context)
    {
        ClickOnButton(5, context);
    }

    private void ClickOnButton(int i, InputAction.CallbackContext context)
    {
        if (context.started)
        {
            simonManager.OnButtonPressed(i);
        }
    }
}
