using InputsManagement;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEInputManager : MonoBehaviour, IPlayerInputsControlled
{

    public Action<bool, Assembler> OnActivityEnd;
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
        SetKeyState(context, QTEKey.Up);
    }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Down);
    }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Left);
    }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.Right);
    }

    public void ReceiveAInput(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.A);
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        SetKeyState(context, QTEKey.B);
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void ResetInputs(){}
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
