using System.Collections.Generic;
using System.Linq;
using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace OrderChoice
{
    public class OrderChoiceManager : MonoBehaviour, IPlayerInputsControlled
    {
        [SerializeField] private OrderManager orderManager;
        
        [SerializeField] private List<OrderDisplay> orderDisplays = new();
        
        private List<SO_Order> availableOrders = new();
        private int selectedOrderIndex = 0;

        private void Start()
        {
            ResetAvailableOrders();
            UpdateOrderDisplays();
            orderDisplays[selectedOrderIndex].SetSelected(true);
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

        private void SelectOrder()
        {
            orderManager.SetCurrentOrder(availableOrders[selectedOrderIndex]);
            availableOrders.RemoveAt(selectedOrderIndex);
        }

        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                orderDisplays[selectedOrderIndex].SetSelected(false);
                selectedOrderIndex = (selectedOrderIndex - 1 + (availableOrders.Count - 1)) % (availableOrders.Count - 1);
                orderDisplays[selectedOrderIndex].SetSelected(true);
            }
        }

        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                orderDisplays[selectedOrderIndex].SetSelected(false);
                selectedOrderIndex = (selectedOrderIndex + 1) % (availableOrders.Count - 1);
                orderDisplays[selectedOrderIndex].SetSelected(true);
            }
        }

        public void ReceiveAInput(InputAction.CallbackContext context)
        {
            SelectOrder();
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context) { }

        public void ReceiveMovementDownInput(InputAction.CallbackContext context) { }

        public void ReceiveBInput(InputAction.CallbackContext context) { }

        public void ReceiveStartInput(InputAction.CallbackContext context) { }

        public void ReceiveSelectInput(InputAction.CallbackContext context) { }
    }
}