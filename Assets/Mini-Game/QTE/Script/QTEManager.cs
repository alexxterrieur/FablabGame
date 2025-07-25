using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QTEManager : Assembler
{
    [Header("Dependencies")]
    [SerializeField] private QTEInputManager inputManager;
    [SerializeField] private GameObject qteKeyPrefab;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private RectTransform targetZone;

    [Header("Parameters")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private int sequenceLength = 2;

    [Header("Debug")]
    [SerializeField] private List<QTEKeys> activeKeys = new();

    [Header("Canvas")]
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        OnAssembleurActivityEnd += (bool p, Assembler a) => { inputManager?.OnActivityEnd.Invoke(p, this); };
    }

    private IEnumerator QTESequenceRoutine()
    {
        for (int i = 0; i < sequenceLength; i++)
        {
            SpawnNewQTE();
            yield return new WaitForSeconds(spawnInterval);
        }
        OnAssembleurActivityEnd?.Invoke(true, this);
        UnActivate();
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
        if (key.RequiredKeys.All(k => inputManager.pressedKey.HasFlag(k)) /*Add Lenght verif*/ )
        {
            Debug.Log("QTE success!");
            key.MarkAsValidated();
        }
        else
        {
            Debug.Log("Wrong input.");
            Debug.Log(key);
            FailKey(key);
            // Let it exit naturally for now
        }
    }

    private void FailKey(QTEKeys _)
    {
        Debug.Log("QTE failed (too late)");
        OnAssembleurActivityEnd?.Invoke(false, this);
        UnActivate();
    }

    public override void Activate()
    {
        canvas.SetActive(true);
        this.StopAllCoroutines();
        StartCoroutine(QTESequenceRoutine());
    }

    public override void UnActivate()
    {
        foreach(var a in activeKeys)
        { 
            if(a != null)
                Destroy(a.gameObject);
        }
            

        canvas.SetActive(false);
        this.StopAllCoroutines();
    }
}
