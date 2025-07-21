using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEKeys : MonoBehaviour
{
    public event Action<QTEKeys> OnEnterZone;
    public event Action<QTEKeys> OnExitZone;

    public List<QTEKey> RequiredKeys { get; private set; }

    [SerializeField] private TextMeshProUGUI text;

    private float moveSpeed;
    private float limitX;
    private bool enteredZone = false;
    private bool validated = false;

    private Coroutine moveRoutine;

    public void Initialize(List<QTEKey> keys, float speed, float zoneX)
    {
        RequiredKeys = keys;
        moveSpeed = speed;
        limitX = zoneX;

        SetRequiredKeysText(RequiredKeys);

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void SetRequiredKeysText(List<QTEKey> keys)
    {
        text.text = string.Join("\n", keys);
    }

    private IEnumerator MoveRoutine()
    {
        RectTransform rect = GetComponent<RectTransform>();

        while (true)
        {
            rect.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;

            float distToLimit = Mathf.Abs(rect.anchoredPosition.x - limitX);

            if (!enteredZone && distToLimit < 10f)
            {
                enteredZone = true;
                OnEnterZone?.Invoke(this);
            }

            if (enteredZone && rect.anchoredPosition.x > limitX + 20f && !validated)
            {
                OnExitZone?.Invoke(this);
                break;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    public void MarkAsValidated()
    {
        validated = true;
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        Destroy(gameObject);
    }
}
