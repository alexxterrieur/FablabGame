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
        pulseSpeed = 2 * Mathf.PI;
    }

    private void Update()
    {
        if (hasEnded) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;


            if (!isWarningActive && remainingTime <= warningTime)
            {
                isWarningActive = true;
            }

            if (isWarningActive)
            {
                PulseEffect();
            }
            else
                UpdateTimerDisplay();

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

    private float nextUpdateTime = 0f;

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
