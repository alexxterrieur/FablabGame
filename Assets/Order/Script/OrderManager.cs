using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [SerializeField] private List<SO_Order> orders = new List<SO_Order>();
    public SO_Order currentOrder;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentOrder = orders[0];
        currentOrder.InitDeliveryStatus();
    }

    public void GoNextOrder()
    {
        orders.Remove(currentOrder);

        if (orders.Count > 0)
        {
            currentOrder = orders[Random.Range(0, orders.Count)];
            currentOrder.InitDeliveryStatus();
        }
        else
        {
            Debug.Log("No more orders");
        }
    }
}

public enum Materials { Wood, Plastic, Metal }
