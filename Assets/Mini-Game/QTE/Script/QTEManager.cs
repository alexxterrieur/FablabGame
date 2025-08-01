using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class QTEManager : Assembler
{
    [Header("Dependencies")]
    [SerializeField] private QTEInputManager inputManager;
    [SerializeField] private GameObject qteKeyPrefab;
    [SerializeField] private RectTransform spawnPointA;
    [SerializeField] private RectTransform spawnPointB;
    [SerializeField] private RectTransform targetZone;

    private static QTEKey[] PlayerAKeys = { QTEKey.Up, QTEKey.Down, QTEKey.Left, QTEKey.Right};
    private static QTEKey[] PlayerBKeys = { QTEKey.A, QTEKey.B };

    [Header("Parameters")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private int sequenceLength = 2;
    private int nbrOfValidKey = 0;

    [Header("Debug")]
    [SerializeField] private List<QTEKeys> activeKeys = new();

    [Header("Canvas")]
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        base.Start();
        OnAssembleurActivityEnd += (bool p, Assembler a) => { inputManager?.OnActivityEnd.Invoke(p, this); };
    }

    private IEnumerator QTESequenceRoutine()
    {
        nbrOfValidKey = 0;
        for (int i = 0; i < sequenceLength; i++)
        {
            SpawnNewQTE();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNewQTE()
    {
        int count = UnityEngine.Random.Range(0, 3);
        Transform spawnPoint;
        List<QTEKey> keyList = new List<QTEKey>();
        switch (count)
        {
            case 0:
                keyList.Add(PlayerAKeys[UnityEngine.Random.Range(0, PlayerAKeys.Length)]);
                spawnPoint = spawnPointA;
                break;

            case 1:
                keyList.Add(PlayerBKeys[UnityEngine.Random.Range(0, PlayerBKeys.Length)]);
                spawnPoint = spawnPointB;
                break;

            default:
                keyList.Add(PlayerAKeys[UnityEngine.Random.Range(0, PlayerAKeys.Length)]);
                keyList.Add(PlayerBKeys[UnityEngine.Random.Range(0, PlayerBKeys.Length)]);
                spawnPoint = spawnPointA;
                break;
        }

        GameObject go = Instantiate(qteKeyPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        QTEKeys qte = go.GetComponent<QTEKeys>();

        float limitX = targetZone.anchoredPosition.x;

        qte.Initialize(keyList, moveSpeed, limitX);
        qte.OnEnterZone += ValidateKey;
        qte.OnExitZone += FailKey;

        activeKeys.Add(qte);

        // Display text or images here
        // go.GetComponentInChildren<Text>().text = string.Join(" + ", keys);
    }

    //private List<QTEKey> GenerateRandomKeys()
    //{

    //    return keyList;
    //}

    private int CountPressedFlags(QTEKey flags)
    {
        int count = 0;
        foreach (QTEKey value in Enum.GetValues(typeof(QTEKey)))
        {
            if (flags.HasFlag(value) && Convert.ToInt32(value) != 0)
                count++;
        }
        return count;
    }

    private void ValidateKey(QTEKeys key)
    {
        if (key.RequiredKeys.All(k => inputManager.pressedKey.HasFlag(k)) &&
            CountPressedFlags(inputManager.pressedKey) == key.RequiredKeys.Count)
        {
            Debug.Log("QTE success!");
            key.MarkAsValidated();
            nbrOfValidKey++;
        }
        else
        {
            Debug.Log("Wrong input.");
            Debug.Log(key);
            FailKey(key);
            // Let it exit naturally for now
        }
        if(nbrOfValidKey >= sequenceLength)
        {
            UnActivate();
            OnAssembleurActivityEnd?.Invoke(true, this);
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
