using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<SO_Order> orders = new();
    
    public SO_Order currentOrder;
    
    public event Action<SO_Order> OnOrderChanged; 

    public void SetCurrentOrder(SO_Order order)
    {
        if (!order)
        {
            Debug.LogWarning("Attempted to set current order to null.");
            return;
        }

        currentOrder = order;
        currentOrder.InitDeliveryStatus();
        
        OnOrderChanged?.Invoke(currentOrder);
    }

    public List<SO_Order> Orders => orders;
}

public enum Materials { Wood, Plastic, Metal }
