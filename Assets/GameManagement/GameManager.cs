using System;
using DeliveryPoint;
using Player.Script;
using PlayerPrefsManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DeliveryPointManagement deliveryPointManagement;
        [SerializeField] private CountdownTimer countdownTimer;
        [SerializeField] private PlayerScore playerScore;
        
        public event Action<bool> OnGameFinished;
        public event Action<bool> OnGamePaused;

        private void Awake()
        {
            if (countdownTimer) countdownTimer.onTimerFinished.AddListener(ReceiveTimerFinished);
            else Debug.LogError("CountdownTimer is not assign in the inspector");
            
            if (deliveryPointManagement) deliveryPointManagement.onItemDelivered += playerScore.IncreaseScore;
            else Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(true);
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1;
            OnGamePaused?.Invoke(false);
        }
        
        public bool IsGamePaused()
        {
            return Time.timeScale == 0;
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ReceiveTimerFinished()
        {
            int highScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HighScoreKey);
            int currentScore = playerScore.Score;
            bool betterScore = currentScore > highScore;

            if (betterScore)
                PlayerPrefs.SetInt(PlayerPrefsKeys.HighScoreKey, currentScore);
            
            OnGameFinished?.Invoke(betterScore);
        }
        
        public PlayerScore PlayerScore => playerScore;

        private void OnDestroy()
        {
            if (countdownTimer)
                countdownTimer.onTimerFinished.RemoveListener(ReceiveTimerFinished);

            if (deliveryPointManagement)
                deliveryPointManagement.onItemDelivered -= playerScore.IncreaseScore;
        }
    }
}