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
    public GameObject finalItemPrefab;

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
    
    public void EquipCraftItem(SO_CollectableItem item)
    {
        if (isCarrying)
            DropHoldItem(dropedObjectPrefab);

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
                if (deliveryPointManagement.CanDeliver(heldItem))
                {
                    //deliveryPointManagement.DeliverItem();
                    
                    DropHoldItem(finalItemPrefab).GetComponent<FinalItem>().SetDeliveryPointManagement(deliveryPointManagement);
                }
            }
            else
            {
                DropHoldItem(dropedObjectPrefab);
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

    private GameObject DropHoldItem(GameObject itemPrefab)
    {
        //drop item
        isCarrying = false;
        objectHolding.SetActive(false);

        GameObject droppedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Shelf dropShelf = droppedItem.GetComponent<Shelf>();
        dropShelf.SetItem(heldItem, true);

        heldItem = null;
        return droppedItem;
    }
}

