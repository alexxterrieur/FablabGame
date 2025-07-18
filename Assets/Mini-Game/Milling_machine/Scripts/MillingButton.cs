using System;
using System.Collections;
using UnityEngine;

public class MillingButton : MonoBehaviour
{
    public Action<float> OnStartDrilling;
    public Action<MillingButton> OnStopDrilling;
    private FormPart part;

    [SerializeField] private GameObject filler;
    [SerializeField] private float minAcceptSize = 0.8f;
    [SerializeField] private float maxAcceptSize = 1.0f;

    public bool IsDrilling { get; private set; }

    public void SetPart(FormPart value) => part = value;
    public FormPart GetPart() => part;

    private void Start()
    {
        OnStartDrilling += StartDrilling;
        OnStopDrilling += StopDrilling;
    }

    public enum DrillCondition {NotEnought, Perfect , TooMuch};
    private IEnumerator Drill(float millingDrillSpeed)
    {
        while (IsDrilling)
        {
            Debug.Log("Drilling ...");
            filler.transform.localScale += new Vector3(millingDrillSpeed * Time.deltaTime, millingDrillSpeed * Time.deltaTime, millingDrillSpeed * Time.deltaTime);
            
            if(filler.transform.localScale.x > 1.2f)
                OnStopDrilling?.Invoke(this);

            yield return null;
        }
        Debug.Log("Drill Stoped !!!! ");
    }

    private void StartDrilling(float drillSpeed)
    {
        IsDrilling = true;
        StartCoroutine(Drill(drillSpeed));
    }
    
    /// <summary>
    /// Call when you want to stop Drilling 
    /// </summary>
    /// <returns> the drill condition </returns>
    private void StopDrilling(MillingButton _)
    {
        IsDrilling = false;
        CheckDrillCondition();
    }

    public DrillCondition CheckDrillCondition()
    {
        switch(filler.transform.localScale.x)
        {
            case float size when size < minAcceptSize:
                return DrillCondition.NotEnought;

            case float size when size <= maxAcceptSize:
                return DrillCondition.Perfect;

            default: return DrillCondition.TooMuch; 
        }
    }
}
