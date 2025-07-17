using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<SO_Order> orders = new List<SO_Order>();
    public SO_Order currentOrder;

    void Start()
    {
        currentOrder = orders[0];
    }

    public void GoNextOrder()
    {
        orders.Remove(currentOrder);

        if (orders.Count > 0)
            currentOrder = orders[Random.Range(0, orders.Count)];
        else
            Debug.Log("No more orders");
    }
}