using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float totalTime = 300f;
    [SerializeField] private float warningTime = 30f;
    
    [Header("Events")]
    public UnityEvent onTimerFinished;

    private float startTime;
    private float remainingTime;
    private bool hasEnded = false;
    
    private bool isRunning = true;

    private void Start()
    {
        remainingTime = totalTime;
        
        SetRunning(false);
    }

    private void Update()
    {
        if (hasEnded || !isRunning) return;
        if (remainingTime < 0) return;
         
        remainingTime -= Time.deltaTime;
        
        if (remainingTime <= 0)
            TimerFinished();
    }


    private void TimerFinished()
    {
        Debug.Log("Timer Finished!");
        hasEnded = true;
        onTimerFinished?.Invoke();
    }

    public void SetRunning(bool inIsRunning)
    {
        isRunning = inIsRunning;
    }
    
    public float RemainingTime => remainingTime;
    public bool Warning => remainingTime - warningTime <= 0;
    public bool IsRunning => isRunning;
    public bool IsFinished => hasEnded;
}
