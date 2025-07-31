using UnityEngine;

public class Shelf : MonoBehaviour, IHighlight
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

    public Highlight.HighlightState CanBeUse(SO_CollectableItem item)
    {
        return Highlight.HighlightState.Interactable;
    }
}

public interface IHighlight
{
    public Highlight.HighlightState CanBeUse(SO_CollectableItem item);
}
