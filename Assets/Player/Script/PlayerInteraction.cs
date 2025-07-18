using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public bool isCarrying;
    public SO_CollectableItem heldItem;
    public Shelf collisionShelf;
    public AssemblerInteraction collisionAssembler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Shelf"))
        {
            collisionShelf = other.transform.parent.GetComponent<Shelf>();
        }
        else if (other.transform.parent.CompareTag("Assembler"))
        {
            collisionAssembler = other.transform.parent.GetComponent<AssemblerInteraction>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Shelf"))
        {
            collisionShelf = null;
        }
        else if (other.transform.parent.CompareTag("Assembler"))
        {
            collisionAssembler = null;
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        if (isCarrying)
        {
            if (collisionAssembler != null)
            {
                bool delivered = collisionAssembler.TryDeliverItem(heldItem);

                if (delivered)
                {
                    Debug.Log("Item livré");
                    heldItem = null;
                    isCarrying = false;
                }
                else
                {
                    Debug.Log("item pas bon");
                }
            }
            else
            {
                heldItem = null;
                isCarrying = false;
            }
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
