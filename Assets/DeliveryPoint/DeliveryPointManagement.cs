using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeliveryPoint
{
    public class DeliveryPointManagement : MonoBehaviour, IHighlight
    {
        [SerializeField] private Transform entryPoint;
        [SerializeField] private ObjectCapture capture;
        [SerializeField] private Animator animator;
        [SerializeField] private CustomManager customManager;
        
        private SO_Order currentOrder;
        private List<FinalObject> ordersDelivered = new();
        
        public event Action<int> OnItemDelivered;

        public void SetCurrentOrder(SO_Order order)
        {
            currentOrder = order;
            capture.capturedTexture = null;
        }

        public bool CanDeliver(SO_CollectableItem item)
        {
            return item == currentOrder.finalItem;
        }

        public void DeliverItem()
        {
            // Logic for delivering an item
            Debug.Log("Item delivered successfully!");

            animator.SetTrigger("OpenBox");

            ordersDelivered.Add(new FinalObject(currentOrder, capture.capturedTexture)) ;
            OnItemDelivered?.Invoke(currentOrder.orderPoints + customManager.additionalScore.colorScore + customManager.additionalScore.stickerScore);
        }

        public Highlight.HighlightState CanBeUse(SO_CollectableItem item)
        {
            return CanDeliver(item) ? Highlight.HighlightState.Interactable : Highlight.HighlightState.NotInteractable;
        }

        public List<FinalObject> OrdersDelivered => ordersDelivered;
        public Transform EntryPoint => entryPoint;
    }
}

public struct FinalObject
{
    public SO_Order order;
    public Texture2D customTex;

    public FinalObject(SO_Order _order, Texture2D _customTex)
    {
        order = _order;
        customTex = _customTex;
    }
}