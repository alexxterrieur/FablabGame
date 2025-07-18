using GameManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        if (gameManager) gameManager.OnGamePaused += ReceiveGamePaused;
        else Debug.LogError("GameManager is not assigned in the inspector");
        
        gameObject.SetActive(false);
    }

    private void ReceiveGamePaused(bool isPaused)
    {
        gameObject.SetActive(isPaused);
    }
}