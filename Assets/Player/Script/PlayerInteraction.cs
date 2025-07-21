using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public bool isCarrying;
    public SO_CollectableItem heldItem;
    public Shelf collisionShelf;
    public AssemblerInteraction collisionAssembler;
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

    public void Interact()
    {
        if (isCarrying)
        {
            if (collisionAssembler != null)
            {
                bool delivered = collisionAssembler.TryDeliverItem(heldItem);

                if (delivered)
                {
                    Debug.Log("Item livr�");
                    heldItem = null;
                    isCarrying = false;
                    objectHolding.SetActive(false);
                }
                else
                {
                    Debug.Log("item pas bon");
                }
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
