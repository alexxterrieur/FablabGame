using System;
using TMPro;
using UnityEngine;

namespace Timer.Scripts
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] private CountdownTimer timer;
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Pulse Effect")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color warningColor = Color.red;
        [SerializeField] private float pulseSpeed = 2f;

        
        private float originalFontSize;
        private bool isWarningActive = false;
        
        private float nextUpdateTime = 0f;

        private void Awake()
        {
            timer.onTimerFinished.AddListener(ReceiveTimerFinished);
        }

        private void Start()
        {
            originalFontSize = timerText.fontSize;
            SetVisibility(false);
            
            pulseSpeed = 2 * Mathf.PI;
        }

        private void Update()
        {
            if (!timer.IsRunning || timer.IsFinished) return;
            
            UpdateTimerDisplay();
            
            if (timer.Warning)
                PulseEffect();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(timer.RemainingTime / 60);
            int seconds = Mathf.FloorToInt(timer.RemainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void PulseEffect()
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

            if (Time.time >= nextUpdateTime)
            {
                UpdateTimerDisplay();
                nextUpdateTime = Time.time + (Mathf.PI / pulseSpeed); // Un pic toutes les ? / pulseSpeed secondes
            }

            timerText.color = Color.Lerp(normalColor, warningColor, t);
            timerText.fontSize = Mathf.Lerp(42f, 52f, t);
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
        
        private void ReceiveTimerFinished()
        {
            isWarningActive = false;
            UpdateTimerDisplay();
            timerText.color = warningColor;
            timerText.fontSize = originalFontSize;
        }
    }
}