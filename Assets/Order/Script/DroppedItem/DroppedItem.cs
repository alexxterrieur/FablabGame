using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private float groundCheckInterval = 0.1f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private Transform groundCheckTransform;
    
    private SO_CollectableItem item;
    private Rigidbody rb;
    private WaitForSeconds groundCheck;

    private void Awake()
    {
        groundCheck = new WaitForSeconds(groundCheckInterval);
    }

    public SO_CollectableItem TakeItem()
    {
        Destroy(gameObject);
        return item;
    }
    
    public void SetItem(SO_CollectableItem newItem)
    {
        item = newItem;
        GetComponent<MeshFilter>().mesh = item.itemMesh;
    }
    
    public void DropItem()
    {
        if (GetComponent<Rigidbody>() is { } rbody)
        {
            rb = rbody;
            rb.mass = 1000;
            rb.isKinematic = false;
        }
        else
            Debug.LogError($"Rigidbody component is missing on the dropped item {gameObject.name}.");
        
        StartCoroutine(CheckGround());
    }

    public void ThrowItem(Vector3 throwDirection, float throwForce)
    {
        if (GetComponent<Rigidbody>() is { } rbody)
        {
            rb = rbody;
            rb.isKinematic = false;
            rb.AddForce(throwDirection * (throwForce * item.slowdownMovementFactor), ForceMode.Impulse);
        }
        else
            Debug.LogError($"Rigidbody component is missing on the dropped item {gameObject.name}.");
        
        StartCoroutine(CheckGround());
    }

    private IEnumerator CheckGround()
    {
        while (rb.isKinematic == false)
        {
            yield return groundCheck;

            if (Physics.Raycast(groundCheckTransform.position, Vector3.down, out RaycastHit hit, groundCheckDistance))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    rb.isKinematic = true;
                }
            }
        }
    }
}