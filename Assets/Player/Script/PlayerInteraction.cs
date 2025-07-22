using DeliveryPoint;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool isCarrying;
    public SO_CollectableItem heldItem;
    public Shelf collisionShelf;
    public AssemblerInteraction collisionAssembler;
    public DeliveryPointManagement deliveryPointManagement;
    
    public GameObject objectHolding;
    public GameObject dropedObjectPrefab;

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
        else if (other.transform.parent.CompareTag("DeliveryPoint"))
        {
            deliveryPointManagement = other.transform.parent.GetComponent<DeliveryPointManagement>();
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
        else if (other.transform.parent.CompareTag("DeliveryPoint"))
        {
            deliveryPointManagement = null;
        }
    }

    public void Interact()
    {
        if (isCarrying)
        {
            if (collisionAssembler != null)
            {
                bool delivered = collisionAssembler.TryDeliverItem(heldItem);

                if (delivered)
                {
                    heldItem = null;
                    isCarrying = false;
                    objectHolding.SetActive(false);
                }
            }
            else if (deliveryPointManagement != null)
            {
                deliveryPointManagement.DeliverItem();

                heldItem = null;
                isCarrying = false;
                objectHolding.SetActive(false);
            }
            else
            {
                //drop item
                isCarrying = false;
                objectHolding.SetActive(false);

                GameObject dropedItem = Instantiate(dropedObjectPrefab, transform.position, Quaternion.identity);
                Shelf dropShelf = dropedItem.GetComponent<Shelf>();
                dropShelf.SetItem(heldItem);
                dropShelf.isDroppedItem = true;

                heldItem = null;
            }
        }
        else
        {
            if (collisionShelf != null)
            {
                heldItem = collisionShelf.TakeItem();
                isCarrying = true;
                objectHolding.GetComponent<MeshFilter>().mesh = heldItem.itemMesh;
                objectHolding.SetActive(true);
            }
        }
    }
}
