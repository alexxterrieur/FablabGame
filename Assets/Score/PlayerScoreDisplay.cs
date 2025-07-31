using Player.Script;
using TMPro;
using UnityEngine;

namespace Score
{
    public class PlayerScoreDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerScore playerScore;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private PulseOnce pulseEffect;
        
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
            {
                scoreText.text = newScore.ToString();
                if (pulseEffect)
                {
                    pulseEffect.TriggerPulse();
                }
            }
                
        }

        private void OnDestroy()
        {
            if (playerScore)
                playerScore.onScoreChanged -= UpdateScoreDisplay;
        }
    }
}