using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QTEManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private QTEInputManager inputManager;
    [SerializeField] private GameObject qteKeyPrefab;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private RectTransform targetZone;

    [Header("Parameters")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private int sequenceLength = 10;

    [Header("Debug")]
    [SerializeField] private List<QTEKeys> activeKeys = new();

    private void Start()
    {
        StartCoroutine(QTESequenceRoutine());
    }

    private IEnumerator QTESequenceRoutine()
    {
        for (int i = 0; i < sequenceLength; i++)
        {
            SpawnNewQTE();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNewQTE()
    {
        GameObject go = Instantiate(qteKeyPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        QTEKeys qte = go.GetComponent<QTEKeys>();

        List<QTEKey> keys = GenerateRandomKeys();
        float limitX = targetZone.anchoredPosition.x;

        qte.Initialize(keys, moveSpeed, limitX);
        qte.OnEnterZone += ValidateKey;
        qte.OnExitZone += FailKey;

        activeKeys.Add(qte);

        // Display text or images here
        // go.GetComponentInChildren<Text>().text = string.Join(" + ", keys);
    }

    private List<QTEKey> GenerateRandomKeys()
    {
        QTEKey[] allKeys = { QTEKey.Up, QTEKey.Down, QTEKey.Left, QTEKey.Right, QTEKey.A, QTEKey.B };
        int count = Random.Range(1, 4);
        return allKeys.OrderBy(x => Random.value).Take(count).ToList();
    }

    private void ValidateKey(QTEKeys key)
    {
        if (key.RequiredKeys.All(k => inputManager.pressedKey.HasFlag(k)))
        {
            Debug.Log("QTE success!");
            key.MarkAsValidated();
        }
        else
        {
            Debug.Log("Wrong input.");
            // Let it exit naturally for now
        }
    }

    private void FailKey(QTEKeys key)
    {
        Debug.Log("QTE failed (too late)");
        activeKeys.Remove(key);
    }
}
