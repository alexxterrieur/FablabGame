using Player.Script;
using TMPro;
using UnityEngine;

namespace Score
{
    public class PlayerScoreDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerScore playerScore;
        [SerializeField] private TMP_Text scoreText;
        
        private void Awake()
        {
            if (playerScore) playerScore.onScoreChanged += UpdateScoreDisplay;
            else Debug.LogError("PlayerScore is not assigned in the inspector");
            
            if (scoreText) UpdateScoreDisplay(0);
            else Debug.LogError("Score Text is not assigned in the inspector");
        }
        
        private void UpdateScoreDisplay(int newScore)
        {
            if (scoreText)
                scoreText.text = newScore.ToString();
        }
        
        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void OnDestroy()
        {
            if (playerScore)
                playerScore.onScoreChanged -= UpdateScoreDisplay;
        }
    }
}