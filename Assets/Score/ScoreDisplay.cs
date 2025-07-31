using GameManagement;
using PlayerPrefsManagement;
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
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private GameObject newHighScorePanel;
        
        [Header("")]
        [SerializeField] private string highScoreTextFormat = "Meilleur Score: {0}";

        private void Awake()
        {
            if (gameManager) gameManager.OnGameFinished += DisplayScore;
            else Debug.LogError("GameManager is not assign in the inspector");

            if (newHighScorePanel) newHighScorePanel.SetActive(false);
            else Debug.LogError("GameManager is not assign in the inspector");
        }

        private void DisplayScore(bool betterScore)
        {
            int highScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HighScoreKey);
            int currentScore = gameManager.PlayerScore.Score;
            
            highScoreText.text = string.Format(highScoreTextFormat, highScore);
            currentScoreText.text = currentScore.ToString();

            newHighScorePanel.SetActive(betterScore);
        }

        private void OnDestroy()
        {
            if (gameManager) gameManager.OnGameFinished -= DisplayScore;
        }
    }
}