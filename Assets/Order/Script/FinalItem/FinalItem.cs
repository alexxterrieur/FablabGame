using DeliveryPoint;
using UnityEngine;

public class FinalItem : MonoBehaviour
{
    [SerializeField] private DeliveryPointManagement deliveryPointManagement;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float movementDuration = 1f;
    [SerializeField] private float normalizedStartScaleTime = 0.75f;
    
    private float startTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 newPosition;
    
    private Vector3 startScale;
    private Vector3 endScale = Vector3.zero;
    private float startScaleTime;
    private bool isScaleTimeSet;
    private float remainingTimeToScale;

    private void Awake()
    {
        startTime = Time.time;
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    public void SetDeliveryPointManagement(DeliveryPointManagement inDeliveryPointManagement)
    {
        deliveryPointManagement = inDeliveryPointManagement;
        endPosition = deliveryPointManagement.EntryPoint.position;
    }

    private void Update()
    {
        if (deliveryPointManagement == null)
            return;
        
        float t = Mathf.Clamp01((Time.time - startTime) / movementDuration);
        newPosition = Vector3.Lerp(startPosition, endPosition, t);
        newPosition.y = startPosition.y + movementCurve.Evaluate(t);
        
        transform.position = newPosition;
        
        if (t >= normalizedStartScaleTime)
        {
            if (!isScaleTimeSet)
            {
                startScaleTime = Time.time;
                remainingTimeToScale = movementDuration - (Time.time - startTime);
                isScaleTimeSet = true;
            }
            
            float scaleT = Mathf.Clamp01((Time.time - startScaleTime) / remainingTimeToScale);
            transform.localScale = Vector3.Lerp(startScale, endScale, scaleT);
        }
        
        if (Vector3.Distance(transform.position, endPosition) < 0.1f)
        {
            deliveryPointManagement.DeliverItem();
            Destroy(gameObject);
        }
    }
}