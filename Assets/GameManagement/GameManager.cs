using System;
using Player.Script;
using PlayerPrefsManagement;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CountdownTimer countdownTimer;
        [SerializeField] private PlayerScore playerScore;
        
        public event Action<bool> OnGameFinished;

        private void Awake()
        {
            if (countdownTimer) countdownTimer.onTimerFinished.AddListener(ReceiveTimerFinished);
            else Debug.LogError("CountdownTimer is not assign in the inspector");
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
    }
}