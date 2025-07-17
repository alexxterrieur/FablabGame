using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float totalTime = 300f;
    [SerializeField] private float warningTime = 30f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Pulse Effect")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float pulseSpeed = 2f;

    [Header("Events")]
    public UnityEvent onTimerFinished;

    private float remainingTime;
    private bool isWarningActive = false;
    private bool hasEnded = false;
    private float originalFontSize;

    private void Start()
    {
        remainingTime = totalTime;
        originalFontSize = timerText.fontSize;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (hasEnded) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (!isWarningActive && remainingTime <= warningTime)
            {
                isWarningActive = true;
            }

            if (isWarningActive)
            {
                PulseEffect();
            }

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                TimerFinished();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void PulseEffect()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        timerText.color = Color.Lerp(normalColor, warningColor, t);
        timerText.fontSize = Mathf.Lerp(42f, 52f, t);
    }

    private void TimerFinished()
    {
        Debug.Log("Timer Finished!");
        hasEnded = true;
        UpdateTimerDisplay();
        timerText.color = warningColor;
        timerText.fontSize = originalFontSize;
        onTimerFinished?.Invoke();
    }
}
