using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GameManagement;
using UnityEngine.UIElements;

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
    public List<ImageBtn> images= new List<ImageBtn>();

    [Header("Parameters")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private int sequenceLength = 2;
    private int nbrOfValidKey = 0;

    [Header("Debug")]
    [SerializeField] private List<QTEKeys> activeKeys = new();
    private QTEKeys currentKey;
    private QTEKeys lastInstanciateKey;

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

    private IEnumerator QTERoutine()
    {
        while(true)
        {
            yield return null;

            if (currentKey != null)
            {
                currentKey.CheckInputs(inputManager.pressedKey);
                Debug.Log("Check Input ...");
            }
        }
    }

    private void ChangeCurrentKey(QTEKeys _)
    {
        currentKey = currentKey.nextKey;

        currentKey.OnEnterZone += ChangeCurrentKey;
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

        if (currentKey == null)
        {
            currentKey = qte;
            currentKey.OnEnterZone += ChangeCurrentKey;
        }


        if (lastInstanciateKey == null)
            lastInstanciateKey = qte;
        else
        {
            lastInstanciateKey.nextKey = qte;
            lastInstanciateKey = qte;
        }

        float limitX = targetZone.anchoredPosition.x;

        qte.Initialize(keyList,GetSprites(keyList), moveSpeed, limitX);
        qte.OnEnterZone += (QTEKeys k) => StartCoroutine(ValidateKey(k));
        qte.OnExitZone += FailKey;

        activeKeys.Add(qte);

        // Display text or images here
        // go.GetComponentInChildren<Text>().text = string.Join(" + ", keys);
    }

    private List<ImageBtn> GetSprites(List<QTEKey> keyList)
    {
        List<ImageBtn> sprites = new List<ImageBtn>();

        foreach (QTEKey key in keyList)
        {
            int index = GetKeyIndex(key); 
            if (index >= 0 && index < images.Count)
            {
                sprites.Add(images[index]);
            }
        }

        return sprites;
    }

    private int GetKeyIndex(QTEKey key)
    {
        switch (key)
        {
            case QTEKey.Up: return 0;
            case QTEKey.Down: return 1;
            case QTEKey.Left: return 2;
            case QTEKey.Right: return 3;
            case QTEKey.A: return 4;
            case QTEKey.B: return 5;
            default: return -1;
        }
    }

    //private List<QTEKey> GenerateRandomKeys()
    //{

    //    return keyList;
    //}

    private IEnumerator ValidateKey(QTEKeys key)
    {
        while(key != null)
        {
            if (key.RequiredKeys.All(k => inputManager.pressedKey.HasFlag(k)) &&
            Utils.CountPressedFlags(inputManager.pressedKey) == key.RequiredKeys.Count)
            {
                Debug.Log("QTE success!");
                key.MarkAsValidated();
                nbrOfValidKey++;
            }
            yield return null;
        }

        //else
        //{
        //    Debug.Log("Wrong input.");
        //    Debug.Log(key);
        //    FailKey(key);
        //    // Let it exit naturally for now
        //}
        //if(nbrOfValidKey >= sequenceLength)
        //{
        //    UnActivate();
        //    OnAssembleurActivityEnd?.Invoke(true, this);
        //}
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
        StartCoroutine(QTERoutine());
    }

    public override void UnActivate()
    {
        foreach(var a in activeKeys)
        { 
            if(a != null)
                Destroy(a.gameObject);
        }
            
        currentKey = null;
        lastInstanciateKey = null;
        canvas.SetActive(false);
        this.StopAllCoroutines();
    }
}

[Serializable]
public struct ImageBtn
{
    public Sprite red;
    public Sprite green;
}