using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeliveryPoint
{
    public class DeliveryPointManagement : MonoBehaviour
    {
        [SerializeField] private Transform entryPoint;
        [SerializeField] private ObjectCapture capture;
        [SerializeField] private Animator animator;
        [SerializeField] private CustomManager customManager;
        
        private SO_Order currentOrder;
        private List<FinalObject> ordersDelivered = new();
        private bool isDelivering;
        
        public event Action<int> OnItemDelivered;
        
        private Coroutine deliveryCoroutine;

        public void SetCurrentOrder(SO_Order order)
        {
            currentOrder = order;
            
            if (capture)
                capture.capturedTexture = null;
        }

        public bool CanDeliver(SO_CollectableItem item, bool inIsDelivering)
        {
            if (inIsDelivering)
                isDelivering = true;
            
            return item == currentOrder.finalItem;
        }

        public void DeliverItem()
        {
            animator.SetTrigger("OpenBox");

            deliveryCoroutine = StartCoroutine(WaitForAnim());
        }

        public void ForceDeliverItem()
        {
            StopCoroutine(deliveryCoroutine);
            ordersDelivered.Add(new FinalObject(currentOrder, capture.capturedTexture));
            OnItemDelivered?.Invoke(GetScoreToAdd());
        }

        private IEnumerator WaitForAnim()
        {
            yield return new WaitForSeconds(3.5f);
            ordersDelivered.Add(new FinalObject(currentOrder, capture.capturedTexture));
            OnItemDelivered?.Invoke(GetScoreToAdd());
            isDelivering = false;
        }
        
        private int GetScoreToAdd()
        {
            return currentOrder.orderPoints + customManager.additionalScore.colorScore + customManager.additionalScore.stickerScore;
        }

        public Highlight.HighlightState CanBeUse(SO_CollectableItem item)
        {
            return CanDeliver(item, false) ? Highlight.HighlightState.Interactable : Highlight.HighlightState.NotInteractable;
        }

        public List<FinalObject> OrdersDelivered => ordersDelivered;
        public Transform EntryPoint => entryPoint;
        
        public bool IsDelivering => isDelivering;
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