using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MillingMachine : Assembler
{
    [SerializeField] private FormData data;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private GameObject millingButtonPrefab;
    [SerializeField] private GameObject partPrefab;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private (float, float) miMaxReamerYPos = (0f, 5f);
    private Vector2 moveInput;
    private bool useReamer = false;
    private bool lockMovement = false;
    private Transform _transform;

    private void Start()
    {
        SetUpForm(data);

        _transform = transform;
    }

    private void SetUpForm(FormData _data)
    {
        foreach (FormPart part in _data.forms) 
        {
            MillingButton millingButton = Instantiate(millingButtonPrefab, part.position, Quaternion.identity).GetComponent<MillingButton>();
            millingButton.OnStopDrilling += CheckDrill;
            millingButton.SetPart(part);
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
            _transform.position = new Vector3(_transform.position.x + moveInput.x * Time.fixedDeltaTime * moveSpeed, _transform.position.y, _transform.position.z + moveInput.y * Time.fixedDeltaTime * moveSpeed);

        if (!useReamer && transform.position.y < miMaxReamerYPos.Item2)
            _transform.position = new Vector3(_transform.position.x, _transform.position.y + Time.fixedDeltaTime * moveSpeed, _transform.position.z);

        else if (useReamer && transform.position.y > miMaxReamerYPos.Item1)
            _transform.position = new Vector3(_transform.position.x, _transform.position.y + Time.fixedDeltaTime * -moveSpeed, _transform.position.z);
    }

    private void CheckIfCanMove()
    {
        if (transform.position.y >= miMaxReamerYPos.Item2)
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
                Form form = Instantiate(partPrefab).GetComponent<Form>();
                form.SetImage(millingButton.GetPart().form);

                Debug.Log("Perfect");
                break;

            case MillingButton.DrillCondition.TooMuch:
                /* Destroy the material end Drilling       */
                Destroy(millingButton.gameObject);
                Debug.Log("Material Is Destroyed");
                break;

            default:
                Debug.Log("Need More Drill");
                break;
        }  
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void UnActivate()
    {
        throw new System.NotImplementedException();
    }
}
