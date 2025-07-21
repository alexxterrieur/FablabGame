using System;
using UnityEngine;

namespace Player.Script
{
    public class PlayerScore : MonoBehaviour
    {
        private int score;
        
        public event Action<int> onScoreChanged;
        
        public void IncreaseScore(int amount)
        {
            score += amount;
            onScoreChanged?.Invoke(score);
        }
        
        public int Score => score;
    }
}