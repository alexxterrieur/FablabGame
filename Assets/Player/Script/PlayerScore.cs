using UnityEngine;

namespace Player.Script
{
    public class PlayerScore : MonoBehaviour
    {
        private int score;
        
        public void IncreaseScore(int amount)
        {
            score += amount;
        }
        
        public int Score => score;
    }
}