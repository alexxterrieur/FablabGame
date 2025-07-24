using DeliveryPoint;
using System.Runtime.CompilerServices;
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

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Shelf"))
        {
            collisionShelf = other.transform.parent.GetComponent<Shelf>();
        }
        else if (other.transform.parent.CompareTag("Assembler"))
        {
            collisionAssembler = other.transform.parent.GetComponent<AssemblerInteraction>();
            collisionAssembler.OnOrderCraft = null;
            collisionAssembler.OnOrderCraft += EquipCraftItem;
        }
        else if (other.transform.parent.CompareTag("DeliveryPoint"))
        {
            deliveryPointManagement = other.transform.parent.GetComponent<DeliveryPointManagement>();
        }

        if(other.gameObject.TryGetComponent(out Highlight hl))
        {
            hl.ToggleHighlight(true);
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
        if (other.gameObject.TryGetComponent(out Highlight hl))
        {
            hl.ToggleHighlight(false);
        }
    }
    
    public void EquipCraftItem(SO_CollectableItem item)
    {
        if (isCarrying)
            DropHoldItem();

        EquipItem(item);

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
                DropHoldItem();
            }
        }
        else
        {
            if (collisionShelf != null)
            {
                EquipItem(collisionShelf.TakeItem());
            }
            else if(collisionAssembler != null)
            {
                heldItem = collisionAssembler.TryGetCraftItem();

                if(heldItem == null)
                    return;

                isCarrying = true;
                objectHolding.GetComponent<MeshFilter>().mesh = heldItem.itemMesh;
                objectHolding.SetActive(true);
            }
        }
    }

    private void EquipItem(SO_CollectableItem item)
    {
        heldItem = item;
        isCarrying = true;
        objectHolding.GetComponent<MeshFilter>().mesh = heldItem.itemMesh;
        objectHolding.SetActive(true);
    }

    private void DropHoldItem()
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

