using GameManagement;
using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndMenuManager : MonoBehaviour, IPlayerInputsControlled
{
    [Header("Game Reference")]
    [SerializeField] private GameManager gameManager;
    
    [Header("Scene Reference")]
    [SerializeField] private int mainMenuSceneIndex = 0;
    
    
    private void Awake()
    {
        if (gameManager) gameManager.OnGameFinished += ReceiveGameFinished;
        else Debug.LogError("GameManager is not assigned in the inspector");
    }

    private void Start()
    {
        SetVisibility(false);
    }
    
    private void ReceiveGameFinished(bool betterScore)
    {
        SetVisibility(true);
    }
    
    private void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void ReceiveBInput(InputAction.CallbackContext context)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    private void OnDestroy()
    {
        if (gameManager) gameManager.OnGameFinished -= ReceiveGameFinished;
    }

    public void ReceiveMovementUpInput(InputAction.CallbackContext context) { }

    public void ReceiveMovementDownInput(InputAction.CallbackContext context) { }

    public void ReceiveMovementLeftInput(InputAction.CallbackContext context) { }

    public void ReceiveMovementRightInput(InputAction.CallbackContext context) { }

    public void ReceiveAInput(InputAction.CallbackContext context) { }

    public void ReceiveStartInput(InputAction.CallbackContext context) { }

    public void ReceiveSelectInput(InputAction.CallbackContext context) { }

    public void ResetInputs() { }
}