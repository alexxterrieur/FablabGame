using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrderChoice
{
    public class OrderChoiceManager : MonoBehaviour
    {
        [SerializeField] private OrderManager orderManager;
        
        [SerializeField] private List<OrderDisplay> orderDisplays = new();
        
        private List<SO_Order> availableOrders = new();
        private int selectedOrderIndex = 0;

        private void Start()
        {
            ResetAvailableOrders();
            UpdateOrderDisplays();
        }
        
        private void ResetAvailableOrders()
        {
            availableOrders = new List<SO_Order>(orderManager.Orders);
        }

        private void UpdateOrderDisplays()
        {
            if (availableOrders.Count < orderDisplays.Count)
                ResetAvailableOrders();
            
            List<SO_Order> selectedOrders = availableOrders.OrderBy(_ => Random.value).Take(orderDisplays.Count).ToList();

            for (var i = 0; i < selectedOrders.Count; i++)
            {
                orderDisplays[i].DisplayOrder(selectedOrders[i]);
            }
        }

        public void SelectOrder()
        {
            orderManager.SetCurrentOrder(availableOrders[selectedOrderIndex]);
            availableOrders.RemoveAt(selectedOrderIndex);
        }
    }
}