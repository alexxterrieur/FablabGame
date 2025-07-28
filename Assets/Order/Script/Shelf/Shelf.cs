using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private SO_CollectableItem shelfItem;
    public bool isDroppedItem;

    public SO_CollectableItem TakeItem()
    {
        if (isDroppedItem)
        {
            Destroy(gameObject);
        }

        return shelfItem;
    }
}
