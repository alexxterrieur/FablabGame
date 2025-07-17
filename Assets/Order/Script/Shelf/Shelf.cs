using UnityEngine;

public class Shelf : MonoBehaviour
{
    public SO_CollectableItem shelfItem;

    public SO_CollectableItem TakeItem()
    {
        return shelfItem;
    }
}
