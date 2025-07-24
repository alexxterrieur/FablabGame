using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeliveryPoint
{
    public class DeliveryPointManagement : MonoBehaviour
    {
        private SO_Order currentOrder;
        private List<SO_Order> ordersDelivered = new();
        
        public event Action<int> OnItemDelivered;

        public void SetCurrentOrder(SO_Order order)
        {
            currentOrder = order;
        }

        public void DeliverItem()
        {
            // Logic for delivering an item
            Debug.Log("Item delivered successfully!");

            ordersDelivered.Add(currentOrder);
            OnItemDelivered?.Invoke(currentOrder.orderPoints);
        }
        
        public List<SO_Order> OrdersDelivered => ordersDelivered;
    }
}