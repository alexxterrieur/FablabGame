using UnityEngine;

public class Reamer : MonoBehaviour
{
    public bool isReamerDown = false;
    private MillingButton currentMillingButton;
    [SerializeField] private float drillingSpeed = 5f;
    
    private float currentDrillingSpeed = 0f;

    private void Start()
    {
        currentDrillingSpeed = drillingSpeed;
    }

    public void StartDrilling(float _drillingSpeed)
    {
        drillingSpeed = _drillingSpeed;
    }
    
    public void IncreaseReamerSpeed(float speedMultiplier)
    {
        currentDrillingSpeed *= speedMultiplier;
    }

    public void ResetReamerSpeed()
    {
        currentDrillingSpeed = drillingSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MillingButton millingButton) && !isReamerDown)
        {
            currentMillingButton = millingButton;
            millingButton?.OnStartDrilling(currentDrillingSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MillingButton millingButton) && millingButton == currentMillingButton)
        {
            currentMillingButton?.OnStopDrilling(currentMillingButton);
            currentMillingButton = null;
        }
    }
}
