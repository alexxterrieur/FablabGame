using System;
using UnityEngine;

namespace DeliveryPoint
{
    public class DeliveryPointManagement : MonoBehaviour
    {
        [SerializeField] private SO_Order currentOrder;
        
        public event Action<int> onItemDelivered;
        
        public void DeliverItem()
        {
            // Logic for delivering an item
            Debug.Log("Item delivered successfully!");

            // Trigger the event to notify subscribers
            onItemDelivered?.Invoke(currentOrder.orderPoints);
        }
    }
}