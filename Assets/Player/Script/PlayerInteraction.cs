using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public bool isCarrying;
    public SO_CollectableItem heldItem;
    public Shelf collisionShelf;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.CompareTag("Shelf"))
        {
            collisionShelf = other.transform.parent.GetComponent<Shelf>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Shelf"))
        {
            collisionShelf = null;
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (isCarrying)
            {
                isCarrying = false;
                heldItem = null;
            }
            else
            {
                if (collisionShelf != null)
                {
                    heldItem = collisionShelf.shelfItem;
                    isCarrying = true;
                }
            }
        }
    }

}
