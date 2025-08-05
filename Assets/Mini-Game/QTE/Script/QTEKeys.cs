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
    public GameObject imagePrefab;

    public List<QTEKey> RequiredKeys { get; private set; }

    [SerializeField] private TextMeshProUGUI text;

    private float moveSpeed;
    private float limitX;
    private bool enteredZone = false;
    private bool validated = false;

    private Coroutine moveRoutine;

    public void Initialize(List<QTEKey> keys, List<Sprite> sprites ,float speed, float zoneX)
    {
        RequiredKeys = keys;
        moveSpeed = speed;
        limitX = zoneX;

        SetRequiredKeysImage(sprites);

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void SetRequiredKeysImage(List<Sprite> sprites)
    {
        foreach (Sprite sprite in sprites)
        {
            GameObject go = Instantiate(imagePrefab, transform);
            Image img = go.GetComponent<Image>();

            if (img != null)
            {
                img.sprite = sprite;
            }
        }
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
