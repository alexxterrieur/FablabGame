using System;
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
    public GameObject finalItemPrefab;

    [SerializeField] private Animator animator;
    
    public event Action<SO_CollectableItem> OnItemEquipped; 

    private void Start()
    {
        if (animator == null)
            Debug.LogWarning("Animator not assigned to PlayerInteraction.");
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
                    //Play interaction animation
                    animator.SetTrigger("Interact");
                    OnItemEquipped?.Invoke(null);
                }
            }
            else if (deliveryPointManagement != null)
            {
                if (deliveryPointManagement.CanDeliver(heldItem))
                {
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
                //Play interaction animation
                animator.SetTrigger("Interact");
                EquipItem(collisionShelf.TakeItem());
            }
            else if (collisionAssembler != null)
            {
                heldItem = collisionAssembler.TryGetCraftItem();

                if (heldItem == null)
                    return;

                //Play interaction animation
                animator.SetTrigger("Interact");
                //equip item
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
        
        OnItemEquipped?.Invoke(item);
    }

    private GameObject DropHoldItem(GameObject itemPrefab)
    {
        animator.SetTrigger("Drop");

        //drop item
        isCarrying = false;
        objectHolding.SetActive(false);

        GameObject droppedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        droppedItem.GetComponent<Shelf>()?.SetItem(heldItem, true);

        heldItem = null;
        OnItemEquipped?.Invoke(null);
        return droppedItem;
    }
}

