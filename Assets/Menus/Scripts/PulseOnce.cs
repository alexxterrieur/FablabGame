using UnityEngine;
using System.Collections;

public class PulseOnce : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseSize = 1.2f;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void TriggerPulse()
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        pulseCoroutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        float time = 0f;
        float duration = 1f / pulseSpeed;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Sin((time / duration) * Mathf.PI);
            float scaleMultiplier = Mathf.Lerp(1f, pulseSize, t);
            transform.localScale = originalScale * scaleMultiplier;
            yield return null;
        }

        transform.localScale = originalScale;
        pulseCoroutine = null;
    }
}