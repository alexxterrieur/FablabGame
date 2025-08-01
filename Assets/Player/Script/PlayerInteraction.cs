using System;
using DeliveryPoint;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Item")]
    public bool isCarrying;
    public SO_CollectableItem heldItem;
    [SerializeField] private float throwForce;
    
    [Header("Collision References")]
    public Shelf collisionShelf;
    public AssemblerInteraction collisionAssembler;
    public DeliveryPointManagement deliveryPointManagement;
    public DroppedItem collisionDroppedItem;
    public CustomManager collisionCustom;
    
    [Header("Prefabs References")]
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
        Transform parent = other.transform.parent;
        if (parent == null) return;

        Highlight.HighlightState state = Highlight.HighlightState.Disabled;

        switch (parent.tag)
        {
            case "Shelf":
                collisionShelf = parent.GetComponent<Shelf>();
                state = collisionShelf.CanBeUse(heldItem);
                break;

            case "Assembler":
                collisionAssembler = parent.GetComponent<AssemblerInteraction>();
                collisionAssembler.OnOrderCraft = null;
                collisionAssembler.OnOrderCraft += EquipCraftItem;
                state = collisionAssembler.CanBeUse(heldItem);
                break;

            case "DeliveryPoint":
                deliveryPointManagement = parent.GetComponent<DeliveryPointManagement>();
                state = deliveryPointManagement.CanBeUse(heldItem);
                break;

            case "Custom":
                collisionCustom = parent.GetComponent<CustomManager>();
                if(heldItem == null)
                    state = Highlight.HighlightState.NotInteractable;
                else
                    state = heldItem.IsFinalItem ? Highlight.HighlightState.Interactable : Highlight.HighlightState.NotInteractable;
                break;

            case "DroppedItem":
                collisionDroppedItem = other.transform.parent.GetComponent<DroppedItem>();
                break;

        }

        if (other.gameObject.TryGetComponent(out Highlight hl))
        {
            hl.ToggleHighlight(state);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        Transform parent = other.transform.parent;
        if (parent == null) return;

        switch (parent.tag)
        {
            case "Shelf":
                collisionShelf = null;
                break;

            case "Assembler":
                collisionAssembler = null;
                break;

            case "DeliveryPoint":
                deliveryPointManagement = null;
                break;

            case "Custom":
                collisionCustom = null;
                break;

            case "DroppedItem":
                collisionDroppedItem = null;
                break;
        }
        

        if (other.gameObject.TryGetComponent(out Highlight hl))
        {
            hl.ToggleHighlight(Highlight.HighlightState.Disabled);
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
                    animator.SetTrigger("Interact");

                    UnequipItem(null);
                }
            }
            else if (deliveryPointManagement != null)
            {
                if (deliveryPointManagement.CanDeliver(heldItem))
                {
                    SO_CollectableItem formerHeldItem = heldItem;
                    GameObject finalItem = DropHoldItem(finalItemPrefab);
                    FinalItem finalItemComponent = finalItem.GetComponent<FinalItem>();
                    finalItemComponent.SetDeliveryPointManagement(deliveryPointManagement);
                    finalItemComponent.SetMesh(formerHeldItem.itemMesh);
                }
            }

            else if(collisionCustom != null && heldItem.IsFinalItem)
            {
                collisionCustom.Interact(heldItem);
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
            else if (collisionDroppedItem != null)
            {
                //Play interaction animation
                animator.SetTrigger("Interact");
                EquipItem(collisionDroppedItem.TakeItem());
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
    
    private GameObject UnequipItem(GameObject itemPrefab)
    {
        if (!isCarrying) return null;

        isCarrying = false;
        objectHolding.SetActive(false);

        GameObject unequippedItem = null;
        
        if (itemPrefab)
        {
            unequippedItem = Instantiate(itemPrefab, objectHolding.transform.position, Quaternion.identity);
            unequippedItem.GetComponent<DroppedItem>()?.SetItem(heldItem);
        }

        heldItem = null;
        OnItemEquipped?.Invoke(null);
        return unequippedItem;
    }

    private GameObject DropHoldItem(GameObject itemPrefab)
    {
        animator.SetTrigger("Drop");

        if (UnequipItem(itemPrefab) is {} item)
        {
            if (item.GetComponent<DroppedItem>() is { } droppedItemComponent)
                droppedItemComponent.DropItem();
            
            return item;
        }
        return null;
    }

    public void ThrowItem()
    {
        if (!isCarrying) return;
        
        animator.SetTrigger("Throw");
        
        SO_CollectableItem formerHeldItem = heldItem;

        if (UnequipItem(dropedObjectPrefab).GetComponent<DroppedItem>() is {} droppedItemComponent)
        {
            droppedItemComponent.SetItem(formerHeldItem);
            droppedItemComponent.ThrowItem(transform.forward + Vector3.up * 0.5f, throwForce);
        }
    }
}

