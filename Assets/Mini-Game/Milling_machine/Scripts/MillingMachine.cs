using InputsManagement;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private (float, float) miMaxReamerYPos = (0f, 5f);
    private Vector2 moveInput;
    private bool useReamer = false;
    private bool lockMovement = false;
    private Transform _transform;

    private int nbrOfPerfectForm = 0;


    private void OnEnable()
    {
        SetUpForm(data);
    }
    private void Awake()
    {
        _transform = transform;
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
            _transform.position = new Vector3(_transform.position.x, _transform.position.y + Time.fixedDeltaTime * moveSpeed, _transform.position.z);

        else if (useReamer && transform.localPosition.y > miMaxReamerYPos.Item1)
            _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + Time.fixedDeltaTime * -moveSpeed, _transform.localPosition.z);
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
                Debug.Log("Perfect");

                if (nbrOfPerfectForm >= data.forms.Count)
                {
                    millingMachineManager.OnAssembleurActivityEnd(true);
                    ResetMillingMachine();
                    millingMachineManager.UnActivate();
                }

                break;

            case MillingButton.DrillCondition.TooMuch:
                /* Destroy the material end Drilling       */
                Destroy(millingButton.gameObject);

                millingMachineManager.OnAssembleurActivityEnd(false);
                ResetMillingMachine();
                millingMachineManager.UnActivate();

                Debug.Log("Material Is Destroyed");
                break;

            default:
                Debug.Log("Need More Drill");
                break;
        }  
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
