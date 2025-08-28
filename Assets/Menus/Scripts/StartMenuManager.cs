using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour, IPlayerInputsControlled
{
    [Header("Scene Reference")]
    [SerializeField] private int gameSceneIndex = 1;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //private void OpenLogPanel()
    //{
    //    //logPanel.SetActive(true);

    //    Cursor.visible = true;
    //    Cursor.lockState = CursorLockMode.None;
    //}

    public void StartGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(gameSceneIndex);
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