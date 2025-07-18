using UnityEngine;

public class Reamer : MonoBehaviour
{
    public bool isReamerDown = false;
    private MillingButton currentMillingButton;
    [SerializeField] private float drillingSpeed = 5f;

    public void StartDrilling(float _drillingSpeed)
    {
        drillingSpeed = _drillingSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MillingButton millingButton) && !isReamerDown)
        {
            currentMillingButton = millingButton;
            millingButton?.OnStartDrilling(drillingSpeed);
            Debug.Log("StartDrilling");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MillingButton millingButton) && millingButton == currentMillingButton)
        {
            currentMillingButton?.OnStopDrilling(currentMillingButton);
            currentMillingButton = null;
            Debug.Log("StopDrilling");
        }
    }
}
