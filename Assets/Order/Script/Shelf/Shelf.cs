using UnityEngine;

public class Shelf : MonoBehaviour, IHighlight
{
    [SerializeField] private SO_CollectableItem shelfItem;
    public bool isDroppedItem;

    [SerializeField] private GameObject feedbackCircle;

    private void Awake()
    {
        if (shelfItem != null)
            ShelfRegistry.Register(shelfItem, this);
    }

    private void OnDestroy()
    {
        if (shelfItem != null)
            ShelfRegistry.Unregister(shelfItem, this);
    }

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

    public void ToggleFeedback(bool isOn)
    {
        if (feedbackCircle != null)
            feedbackCircle.SetActive(isOn);
    }

}

public interface IHighlight
{
    public Highlight.HighlightState CanBeUse(SO_CollectableItem item);
}
