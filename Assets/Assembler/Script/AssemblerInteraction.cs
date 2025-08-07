using System;
using UnityEngine;

public class AssemblerInteraction : MonoBehaviour, IHighlight
{
    public Materials assemblerMaterial;
    public Assembler assembler;
    private SO_Order currentOrder;

    public event Action OnOrderCompleted;
    public Action<SO_CollectableItem> OnOrderCraft;
    private bool isObjectCraft = false;

    [SerializeField] private GameObject feedbackCircle;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject miniGamesRulesPanel;

    public FeedbackManager feedbackManager;

    private void Start()
    {
        if (assembler)
            assembler.OnAssembleurActivityExit += EndActivity;
    }

    private void EndActivity(bool isSuccess)
    {
        if (isObjectCraft)
            return;

        if (isSuccess)
        {
            DisableRules();

            isObjectCraft = true;
            OnOrderCraft(currentOrder.finalItem);
            return;
        }

        currentOrder.RemoveDeliveredItem();

        DisableRules();

        if (feedbackManager != null)
        {
            foreach (var material in currentOrder.Materials)
            {
                if (currentOrder.CanAddItem(material.item))
                {
                    var shelfGroup = feedbackManager.shelfGroups.Find(g => g.item == material.item);
                    if (shelfGroup != null)
                    {
                        foreach (var shelf in shelfGroup.shelves)
                        {
                            shelf.ToggleFeedback(true);
                        }
                    }
                }
            }
        }
    }

    public void SetCurrentOrder(SO_Order order)
    {
        currentOrder = order;
        isObjectCraft = false;
    }

    public SO_CollectableItem TryGetCraftItem()
    {
        return null;
    }

    public bool TryDeliverItem(SO_CollectableItem item)
    {
        if (currentOrder.mainMaterial != assemblerMaterial)
        {
            return false;
        }

        if (currentOrder.TryAddItem(item))
        {
            if (currentOrder.IsOrderComplete())
            {
                assembler?.Activate();

                controlPanel.SetActive(false);
                if (miniGamesRulesPanel != null)
                    miniGamesRulesPanel.SetActive(true);

                OnOrderCompleted?.Invoke();
            }

            return true;
        }

        return false;
    }

    public Highlight.HighlightState CanBeUse(SO_CollectableItem item)
    {
        if(item == null)
            return Highlight.HighlightState.NotInteractable;

        if (currentOrder.mainMaterial != assemblerMaterial || !currentOrder.CanAddItem(item))
        {
            return Highlight.HighlightState.NotInteractable;
        }

        return Highlight.HighlightState.Interactable;
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

    public void ToggleFeedback(bool isOn)
    {
        if (feedbackCircle != null)
            feedbackCircle.SetActive(isOn);
    }

    public void DisableRules()
    {
        controlPanel.SetActive(true);
        if (miniGamesRulesPanel != null)
            miniGamesRulesPanel.SetActive(false);
    }
}
