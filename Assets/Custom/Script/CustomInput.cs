using InputsManagement;
using System;
using GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInput : MonoBehaviour, IPlayerInputsControlled
{
    [SerializeField] private GameManager gameManager;
    
    public Action<Vector2Int> OnMove;
    public Action<Vector2> OnMoveDirection;
    public Action OnSelect;

    private Vector2 movementDir = Vector2.zero;
    public QTEKey pressedKey { get; private set; }

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
                OnSelect();
        }
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameManager.IsGamePaused())
                gameManager.ResumeGame();
        }
    }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context)
    {
        MovementPlayer(context, Vector2.down, QTEKey.Down);
        if (context.started)
            OnMove?.Invoke(new Vector2Int(0,-1));
    }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
    {
        MovementPlayer(context, Vector2.left, QTEKey.Left);
        if (context.started)
            OnMove?.Invoke(new Vector2Int(-1, 0));
    }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context)
    {
        MovementPlayer(context, Vector2.right, QTEKey.Right);
        if (context.started)
            OnMove?.Invoke(new Vector2Int(1, 0));
    }

    public void ReceiveMovementUpInput(InputAction.CallbackContext context)
    {
        MovementPlayer(context, Vector2.up, QTEKey.Up);
        if (context.started)
            OnMove?.Invoke(new Vector2Int(0, 1));
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context)
    {

    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        if (context.started)
            gameManager.PauseGame();
    }

    public void ResetInputs()
    {

    }


    private void MovementPlayer(InputAction.CallbackContext context, Vector2 input, QTEKey key)
    {
        if (context.started)
            movementDir += input;
        else if (context.canceled && pressedKey.HasFlag(key))
            movementDir -= input;

        OnMoveDirection?.Invoke(movementDir);

        SetKeyState(context, key);
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
