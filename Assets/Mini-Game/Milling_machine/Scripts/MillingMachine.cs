using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MillingMachine : MonoBehaviour
{
    [SerializeField] public MillingMachineManager millingMachineManager;

    [SerializeField] private Transform parent;

    [SerializeField] private FormData data;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private GameObject millingButtonPrefab;
    private List<GameObject> millingButtons = new();
    [SerializeField] private GameObject partPrefab;
    private List<GameObject> parts = new();
    [SerializeField] private Reamer reamer;
    [SerializeField] private MeshRenderer itemPreviewRenderer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float reamerRotationSpeed = 5f;
    [SerializeField] private float reamerDownSpeed = 5f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private (float, float) miMaxReamerYPos = (0f, 5f);
    [SerializeField] private float normalDrillSize = 1f;
    [SerializeField] private float minimalDrillSize = 0.2f;
    [SerializeField] private float waitTimeBeforeValidate = 1f;
    
    private Vector2 moveInput;
    private bool useReamer = false;
    private bool lockMovement = false;
    private Transform _transform;

    private int nbrOfPerfectForm = 0;
    private float currentReamerRatationSpeed = 0f;
    private Vector3 currentScale = Vector3.one;

    private void Awake()
    {
        _transform = transform;
        
        currentScale.Set(normalDrillSize, normalDrillSize, normalDrillSize);
        _transform.localScale = currentScale;

        if (!reamer) Debug.LogError("Reamer is not assigned in MillingMachine script!");
        
        currentReamerRatationSpeed = reamerRotationSpeed;
    }

    public void SetCurrentOrder(SO_Order order)
    {
        if (!order || !order.millingForm)
            return;
        
        data = order.millingForm;
        itemPreviewRenderer.material.mainTexture = data.itemPreview;
        SetUpForm(data);
        
        if (reamer) reamer.ResetReamerSpeed(); 
        else Debug.LogError("Reamer is not assigned in MillingMachine script!");
        
        currentReamerRatationSpeed = reamerRotationSpeed;
    }

    private void SetUpForm(FormData _data)
    {
        nbrOfPerfectForm = 0;

        foreach (FormPart part in _data.forms) 
        {
            MillingButton millingButton = Instantiate(millingButtonPrefab, parent).GetComponent<MillingButton>();
            millingButton.gameObject.transform.localPosition = part.position;
            millingButtons.Add(millingButton.gameObject);
            millingButton.OnStopDrilling += CheckDrill;
            millingButton.SetPart(part);
        }
        
    }

    public void OnMove(Vector2 dir)
    {
        moveInput = dir;
    }

    public void OnMoveReamer(InputAction.CallbackContext context)
    {
        useReamer = context.ReadValueAsButton();
    }
    
    public void ResetMachine()
    {
        moveInput = Vector2.zero;
        useReamer = false;
        lockMovement = false;
        
        currentScale.Set(normalDrillSize, normalDrillSize, normalDrillSize);
        _transform.localScale = currentScale;

        _transform.localPosition = new Vector3(_transform.localPosition.x, miMaxReamerYPos.Item2, _transform.localPosition.z);
    }

    private void FixedUpdate()
    {
        CheckIfCanMove();
        Move();
    }

    private void Move()
    {
        if (!lockMovement)
            _transform.localPosition = new Vector3(_transform.localPosition.x + moveInput.x * Time.fixedDeltaTime * moveSpeed, _transform.localPosition.y, _transform.localPosition.z + moveInput.y * Time.fixedDeltaTime * moveSpeed);

        if (!useReamer && transform.localPosition.y < miMaxReamerYPos.Item2)
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y + Time.fixedDeltaTime * reamerDownSpeed, _transform.position.z);
        }
        else if (useReamer && transform.localPosition.y > miMaxReamerYPos.Item1)
        {
            _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + Time.fixedDeltaTime * -reamerDownSpeed, _transform.localPosition.z);
        }
        else if(useReamer && transform.localPosition.y < miMaxReamerYPos.Item1)
        {
            _transform.localPosition = new Vector3(_transform.localPosition.x, miMaxReamerYPos.Item1, _transform.localPosition.z);
        }
        
        
        float scale = Mathf.Lerp(minimalDrillSize, normalDrillSize, _transform.localPosition.y / miMaxReamerYPos.Item2);
        currentScale.Set(scale, scale, scale);
        _transform.localScale = currentScale;

        if (scale <= minimalDrillSize)
            _transform.Rotate(0, 60 * currentReamerRatationSpeed * Time.deltaTime, 0);
    }

    private void CheckIfCanMove()
    {
        if (transform.localPosition.y >= miMaxReamerYPos.Item2)
            lockMovement = false;
        else
            lockMovement = true;
    }

    private void CheckDrill(MillingButton millingButton)
    {
        switch (millingButton.CheckDrillCondition()) 
        {
            case MillingButton.DrillCondition.Perfect: 
                Destroy(millingButton.gameObject);
                Form form = Instantiate(partPrefab, parent).GetComponent<Form>();
                parts.Add(form.gameObject);
                form.SetImage(millingButton.GetPart().form);
                nbrOfPerfectForm++;

                currentReamerRatationSpeed *= speedMultiplier;
                reamer.IncreaseReamerSpeed(speedMultiplier);

                if (nbrOfPerfectForm >= data.forms.Count)
                {
                    StartCoroutine(WaitBeforeValidate());
                }

                break;

            case MillingButton.DrillCondition.TooMuch:
                /* Destroy the material end Drilling       */
                Destroy(millingButton.gameObject);

                millingMachineManager.OnAssembleurActivityEnd(false, millingMachineManager);
                ResetMillingMachine();
                SetUpForm(data);
                millingMachineManager.UnActivate();

                Debug.Log("Material Is Destroyed");
                break;

            default:
                Debug.Log("Need More Drill");
                break;
        }  
    }
    
    private IEnumerator WaitBeforeValidate()
    {
        yield return new WaitForSeconds(waitTimeBeforeValidate);
        millingMachineManager.OnAssembleurActivityEnd(true, millingMachineManager);
        ResetMillingMachine();
        millingMachineManager.UnActivate();
    }

    private void ResetMillingMachine()
    {
        foreach (var part in parts)
        {
            if(part!=null)
                Destroy(part.gameObject);
        }

        foreach(var button in millingButtons)
        {
            if(button!=null)
                Destroy(button);
        }
    }
}
