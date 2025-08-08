using GameManagement;
using PlayerPrefsManagement;
using System;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreDisplay : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private GameManager gameManager;
        
        [Header("Text Reference")]
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text dailyHighScoreText;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private GameObject newHighScorePanel;
        [SerializeField] private GameObject newDailyHighScorePanel;
        
        [Header("")]
        [SerializeField] private string highScoreTextFormat = "Meilleur Score: {0}";
        [SerializeField] private string dailyHighScoreTextFormat = "Meilleur Score du jour: {0}";

        private void Awake()
        {
            if (gameManager) gameManager.OnGameFinished += DisplayScore;
            else Debug.LogError("GameManager is not assign in the inspector");

            if (newHighScorePanel) newHighScorePanel.SetActive(false);
            else Debug.LogError("GameManager is not assign in the inspector");
        }

        private void DisplayScore(bool betterScore, bool betterDailyScore)
        {
            int highScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HighScoreKey);
            int dailyHighScore = PlayerPrefs.GetInt(PlayerPrefsKeys.DailyScoreKey);
            int currentScore = gameManager.PlayerScore.Score;
            
            highScoreText.text = string.Format(highScoreTextFormat, highScore);
            dailyHighScoreText.text = string.Format(dailyHighScoreTextFormat, dailyHighScore);
            currentScoreText.text = currentScore.ToString();

            newHighScorePanel.SetActive(betterScore);
            newDailyHighScorePanel.SetActive(betterDailyScore);
        }

        private void OnDestroy()
        {
            if (gameManager) gameManager.OnGameFinished -= DisplayScore;
        }

        private void Start()
        {
            if (IsSameDayAsSaved())
                return;

            PlayerPrefs.SetInt(PlayerPrefsKeys.DailyScoreKey, 0);
            SaveTodayDate();
        }
        private void SaveTodayDate()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd"); // format AAAA-MM-JJ
            PlayerPrefs.SetString(PlayerPrefsKeys.LastDateKey, today);
            PlayerPrefs.Save();
        }

        private bool IsSameDayAsSaved()
        {
           if (!PlayerPrefs.HasKey(PlayerPrefsKeys.LastDateKey))
                return false;
    
            string savedDate = PlayerPrefs.GetString(PlayerPrefsKeys.LastDateKey);
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            return savedDate == today;
        }

    }
}