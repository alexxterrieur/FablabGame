using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEInputManager : MonoBehaviour
{
    public QTEKey pressedKey { get; private set; }

    public void Up(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Up);
    }

    public void Down(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Down);
    }

    public void Left(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Left);
    }

    public void Right(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Right);
    }

    public void A(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.A);
    }

    public void B(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.B);
    }

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
}

[System.Flags]
public enum QTEKey
{
    None = 0,
    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
    A = 1 << 4,
    B = 1 << 5
}
