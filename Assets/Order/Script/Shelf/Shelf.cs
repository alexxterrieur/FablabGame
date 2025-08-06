using UnityEngine;

public class Shelf : MonoBehaviour, IHighlight
{
    [SerializeField] private SO_CollectableItem shelfItem;
    public bool isDroppedItem;

    [SerializeField] private GameObject feedbackCircle;

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

    public void UpdateFeedbackColor(Highlight.HighlightState state)
    {
        if (feedbackCircle != null)
        {
            var renderer = feedbackCircle.GetComponent<SpriteRenderer>();

            if (state == Highlight.HighlightState.Interactable)
                renderer.color = Color.green;
            else
                renderer.color = Color.white;
        }
    }
}

public interface IHighlight
{
    public Highlight.HighlightState CanBeUse(SO_CollectableItem item);
    void UpdateFeedbackColor(Highlight.HighlightState state);
}
