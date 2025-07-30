using DeliveryPoint;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject customCanvas;
    [SerializeField] private GameObject custom3DObjects;
    [SerializeField] private GameObject mainCustom;
    [SerializeField] private GameObject ColorsCustom;
    [SerializeField] private GameObject StickersCustom;
    [SerializeField] private ObjectCapture capture;

    [SerializeField] private MeshFilter finalItem;
    [SerializeField] private DeliveryPointManagement delivery;

    [Header("Input")]
    public CustomInput customInput;

    [Header("Button")]
    [SerializeField] Button colorsBtn;
    [SerializeField] Button stickersBtn;
    [SerializeField] Button confirmBtn;
    private int indexBtn = 0;

    public Action<Mesh> OnEnter;
    public Action OnExit;
    public Action OnReset;  

    private bool inMenu = true;

    private void Start()
    {
        delivery.OnItemDelivered += (int _) => OnReset?.Invoke();
        ChangeBtn(new Vector2Int(0, 0));

        OnEnter += OpenMenuCustom;
        OnExit += CloseMenuCustom;
    }

    private void OpenMenuCustom(Mesh obj)
    {
        finalItem.mesh = obj;
        customCanvas.SetActive(true);
        custom3DObjects.SetActive(true);
        Activate();
    }

    private void CloseMenuCustom() 
    {
        customCanvas.SetActive(false);
        custom3DObjects.SetActive(false);   
        capture.CaptureObjectImage();
        Deactivate();

    }


    private void Activate()
    {
        Deactivate();
        mainCustom.SetActive(true);
        customInput.OnMove += ChangeBtn;
        customInput.OnSelect += Select;
    }

    private void Deactivate()
    {
        mainCustom.SetActive(false);
        customInput.OnMove -= ChangeBtn;
        customInput.OnSelect -= Select;
    }

    private void ChangeBtn(Vector2Int move)
    {
        if (!inMenu)
            return;

        indexBtn += move.y;

        if (indexBtn >= 3)
            indexBtn = 0;
        else if (indexBtn < 0)
            indexBtn = 3;

        switch (indexBtn) 
        {
            case 0:
                colorsBtn.interactable = true;
                stickersBtn.interactable = false;
                confirmBtn.interactable = false;
                break; 

            case 1:
                colorsBtn.interactable = false;
                stickersBtn.interactable = true;
                confirmBtn.interactable = false;
                break; 

            case 2:
                colorsBtn.interactable = false;
                stickersBtn.interactable = false;
                confirmBtn.interactable = true;
                break; 
        }
    }

    private void Select()
    {
        if (!inMenu)
            return;

        if (stickersBtn.interactable)
            StickersMode();
        else if (colorsBtn.interactable)
            ColorMode();
        else if (confirmBtn.interactable)
            OnExit?.Invoke();
    }

    public void Interact(SO_CollectableItem item)
    {
        if (item == null)
            return;

        customCanvas.SetActive(true);
        OnEnter?.Invoke(item.itemMesh);
    }

    public void Return()
    {
        ColorsCustom.SetActive(false);
        StickersCustom.SetActive(false);
        Activate();
        customInput.OnSelect -= Return;
    }

    public void Validate()
    {
        customCanvas.SetActive(false);
        OnExit?.Invoke();
    }

    public void ColorMode()
    {
        customInput.OnSelect -= Return;
        customInput.OnSelect += Return;

        ColorsCustom.SetActive(true);
        Deactivate();
    }

    public void StickersMode()
    {
        StickersCustom.SetActive(true);
        Deactivate();
    }

}
