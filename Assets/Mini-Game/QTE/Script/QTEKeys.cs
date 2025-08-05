using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEKeys : MonoBehaviour
{
    public event Action<QTEKeys> OnEnterZone;
    public event Action<QTEKeys> OnExitZone;
    public GameObject imagePrefab;
    public float distanceBeforeLoose = 1000f;

    List<ImageBtn> sprites = new List<ImageBtn>();
    List<Image> images = new List<Image>();

    public QTEKeys nextKey;

    public List<QTEKey> RequiredKeys { get; private set; }

    [SerializeField] private TextMeshProUGUI text;

    private float moveSpeed;
    private float limitX;
    private bool enteredZone = false;
    private bool validated = false;

    private Coroutine moveRoutine;

    public void Initialize(List<QTEKey> keys, List<ImageBtn> sprites ,float speed, float zoneX)
    {
        RequiredKeys = keys;
        moveSpeed = speed;
        limitX = zoneX;

        SetRequiredKeysImage(sprites);

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void CheckInputs(QTEKey pressedKey)
    {
        // Check 1 : Trop d'inputs (plus que nécessaire)
        bool tooManyInputs = Utils.CountPressedFlags(pressedKey) > RequiredKeys.Count;

        // Check 2 : Tous les inputs actuels sont bons (même s'il en manque)
        bool allInputsAreValid = Enum.GetValues(typeof(QTEKey))
            .Cast<QTEKey>()
            .Where(k => k != QTEKey.None && pressedKey.HasFlag(k))
            .All(k => RequiredKeys.Contains(k));

        // Check 3 : Il y a au moins un input invalide
        bool hasWrongInput = !allInputsAreValid || tooManyInputs;

        if (hasWrongInput)
        {
            Debug.Log("Trop d'inputs ou certains inputs sont incorrects ! !");

            for (int i = 0; i<images.Count; i++)
            {
                images[i].sprite = sprites[i].red;
            }
        }
        else
        {
            Debug.Log("Pas assez d'inputs, mais tous sont bons.");
            for (int i = 0; i < images.Count; i++)
            {
                if (pressedKey.HasFlag(RequiredKeys[i]))
                {
                    images[i].sprite = sprites[i].green;
                }
                else
                    images[i].sprite = sprites[i].red;
            }
        }
        //else
        //{
        //    Debug.Log("Inputs parfaits !");
        //    for (int i = 0; i < images.Count; i++)
        //    {
        //        images[i].sprite = sprites[i].green;
        //    }
        //}

    }

    public void SetRequiredKeysImage(List<ImageBtn> _sprites)
    {
        sprites = _sprites;
        foreach (ImageBtn sprite in _sprites)
        {
            GameObject go = Instantiate(imagePrefab, transform);
            Image img = go.GetComponent<Image>();

            images.Add(img);

            if (img != null)
            {
                img.sprite = sprite.red;
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

            if (enteredZone && rect.anchoredPosition.x > limitX + distanceBeforeLoose && !validated)
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
