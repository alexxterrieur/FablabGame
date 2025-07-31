using UnityEngine;

public class PulseOnce : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseSize = 1.2f;

    private Vector3 originalScale;
    private float pulseTime;
    private bool isPulsing;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!isPulsing) return;

        pulseTime += Time.deltaTime * pulseSpeed;
        float t = Mathf.Sin(pulseTime * Mathf.PI); // one full pulse
        float scaleMultiplier = Mathf.Lerp(1f, pulseSize, t);
        transform.localScale = originalScale * scaleMultiplier;

        if (pulseTime >= 1f) // end of pulse
        {
            isPulsing = false;
            transform.localScale = originalScale;
        }
    }

    public void TriggerPulse()
    {
        pulseTime = 0f;
        isPulsing = true;
    }
}