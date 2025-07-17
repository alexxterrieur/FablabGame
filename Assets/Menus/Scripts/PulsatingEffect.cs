using UnityEngine;

public class PulsatingEffect : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseSize = 1.2f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float scaleMultiplier = Mathf.Lerp(1f, pulseSize, t);
        transform.localScale = originalScale * scaleMultiplier;
    }
}
