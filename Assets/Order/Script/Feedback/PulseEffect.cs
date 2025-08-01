using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float scaleOffset = 0.4f;
    [SerializeField] private float pulseDuration = 1f;

    private Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float halfDuration = pulseDuration / 2f;
        float t = Mathf.PingPong(Time.time, halfDuration) / halfDuration;
        float scaleFactor = Mathf.Lerp(0f, scaleOffset, t);
        transform.localScale = baseScale + Vector3.one * scaleFactor;
    }
}