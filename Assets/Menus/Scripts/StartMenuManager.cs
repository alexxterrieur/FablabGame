using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour, IPlayerInputsControlled
{
    [Header("Scene Reference")]
    [SerializeField] private int gameSceneIndex = 1;
    [SerializeField] private GameObject logPanel;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OpenLogPanel()
    {
        logPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void StarGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(gameSceneIndex);
    }

    public void ReceiveMovementUpInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveAInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveStartInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ReceiveSelectInput(InputAction.CallbackContext context)
    {
        OpenLogPanel();
    }

    public void ResetInputs()
    {
        OpenLogPanel();
    }
}