using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour, IPlayerInputsControlled
{
    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ReceiveMovementUpInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveAInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context)
    {
        StartGame();
    }

    public void ResetInputs()
    {
        StartGame();
    }
}