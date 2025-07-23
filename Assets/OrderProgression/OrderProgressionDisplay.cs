using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderProgression
{
    public class OrderProgressionDisplay : MonoBehaviour
    {
        [Header("Gameplay References")]
        [SerializeField] private OrderManager orderManager;
        
        [Header("UI References")]
        [SerializeField] private Image orderIcon;
        [SerializeField] private TMP_Text orderNameText;
        [SerializeField] private TMP_Text orderScoreText;
        
        [Header("Item Display Prefab")]
        [SerializeField] private OrderProgressionItemDisplay orderItemDisplay;
        [SerializeField] private Transform itemDisplayContainer;
        
        private SO_Order currentOrder;
        private List<OrderProgressionItemDisplay> itemDisplays = new();

        private void Awake()
        {
            if (orderManager) orderManager.OnOrderChanged += ManageNewOrder;
            else Debug.LogError("OrderManager reference is missing in OrderProgressionDisplay.");
            
            if (!orderItemDisplay) Debug.LogError("OrderItemDisplay reference is missing in OrderProgressionDisplay.");
        }
        
        private void ManageNewOrder(SO_Order newOrder)
        {
            if (currentOrder) currentOrder.OnItemDeliveryStatusChanged -= ReceiveItemDeliveryStatusChanged;
            
            currentOrder = newOrder;
            
            if (currentOrder) currentOrder.OnItemDeliveryStatusChanged += ReceiveItemDeliveryStatusChanged;
            
            ManageAmountDisplay(currentOrder);
            SetNewOrderDetails(currentOrder);
        }

        private void SetNewOrderDetails(SO_Order order)
        {
            if (!order) return;

            orderIcon.sprite = order.orderIcon;
            orderNameText.text = order.orderName;
            orderScoreText.text = $"Score: {order.orderPoints}";
            
            for (int i = 0; i < order.items.Count; i++)
                itemDisplays[i].SetItemDetails(order.items[i]);
        }
        
        private void ManageAmountDisplay(SO_Order order)
        {
            if (!order) return;
            
            int itemCountInOrder = order.items.Count;
            
            if (itemCountInOrder < itemDisplays.Count)
            {
                for (int i = itemDisplays.Count - 1; i >= itemCountInOrder; i--)
                {
                    itemDisplays[i].SetVisibility(false);
                }
            }
            else if (itemCountInOrder > itemDisplays.Count)
            {
                foreach (var itemDisplay in itemDisplays)
                    itemDisplay.SetVisibility(true);

                for (int i = itemDisplays.Count; i < itemCountInOrder; i++)
                {
                    var newItemDisplay = Instantiate(orderItemDisplay, itemDisplayContainer);
                    itemDisplays.Add(newItemDisplay);
                }
            }
            else
            {
                foreach (var itemDisplay in itemDisplays)
                {
                    itemDisplay.SetVisibility(true);
                }
            }
        }
        
        private void ReceiveItemDeliveryStatusChanged(int index, bool isDelivered)
        {
            if (!currentOrder) return;
            
            itemDisplays[index].UpdateItemStatus(isDelivered);
        }
        
        private void OnDestroy()
        {
            if (orderManager) orderManager.OnOrderChanged -= ManageNewOrder;
            
            if (currentOrder) currentOrder.OnItemDeliveryStatusChanged -= ReceiveItemDeliveryStatusChanged;
        }
    }
}