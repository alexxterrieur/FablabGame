using GameManagement;
using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class MillingInputHandler : MonoBehaviour, IPlayerInputsControlled
{
    public MillingMachine millingMachine;
    [SerializeField] private GameManager gameManager;
    
    private Vector2 movementDir = Vector2.zero;

    private void Awake()
    {
        if (!gameManager) Debug.LogError("GameManager is not assigned in the inspector");
    }

    public void ReceiveAInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
            else
                millingMachine.OnMoveReamer(context);
        }
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameManager.IsGamePaused())
                gameManager.RestartGame();
        }
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

    public void ReceiveSelectInput(InputAction.CallbackContext context){}

    private void MovementPlayer(InputAction.CallbackContext context, Vector2 input, QTEKey key)
    {
        if (context.started)
            movementDir += input;
        else if (context.canceled && pressedKey.HasFlag(key))
            movementDir -= input;

        millingMachine.OnMove(movementDir);

        SetKeyState(context, key);
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        if (context.started)
            gameManager.PauseGame();
    }

    public void ResetInputs()
    {
        movementDir = Vector2.zero;
        pressedKey = QTEKey.None;
        millingMachine.ResetMachine();
    }
}
