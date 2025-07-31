using System;
using System.Collections.Generic;
using System.Linq;
using GameManagement;
using InputsManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace OrderChoice
{
    public class OrderChoiceManager : MonoBehaviour, IPlayerInputsControlled
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private OrderManager orderManager;
        
        [SerializeField] private List<OrderDisplay> orderDisplays = new();
        
        private List<SO_Order> availableOrders = new();
        private List<SO_Order> selectedOrders = new();
        private int selectedOrderIndex;
        
        public event Action OnOrderSelected;

        private void Start()
        {
            if (gameManager)
            {
                gameManager.OnGameFinished += ReceiveGameFinished;
                gameManager.DeliveryPoint.OnItemDelivered += ReceiveItemDelivered;
            }
            else Debug.LogError("GameManager is not assigned in the inspector");
            
            ResetAvailableOrders();
            UpdateOrderDisplays();
            orderDisplays[selectedOrderIndex].SetSelected(true);
        }

        private void ReceiveItemDelivered(int _)
        {
            SetVisibility(true);
            //selectedOrderIndex = 0;
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
            
            selectedOrders = availableOrders.OrderBy(_ => Random.value).Take(orderDisplays.Count).ToList();

            for (var i = 0; i < selectedOrders.Count; i++)
            {
                orderDisplays[i].DisplayOrder(selectedOrders[i]);
            }
        }

        private void SelectOrder()
        {
            orderManager.SetCurrentOrder(selectedOrders[selectedOrderIndex]);
            availableOrders.RemoveAt(selectedOrderIndex);
            SetVisibility(false);
            OnOrderSelected?.Invoke();
        }
        
        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
        
        private void ReceiveGameFinished(bool _)
        {
            SetVisibility(false);
        }

        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                orderDisplays[selectedOrderIndex].SetSelected(false);
                selectedOrderIndex = (selectedOrderIndex - 1 + orderDisplays.Count) % orderDisplays.Count;
                orderDisplays[selectedOrderIndex].SetSelected(true);
            }
        }

        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                orderDisplays[selectedOrderIndex].SetSelected(false);
                selectedOrderIndex = (selectedOrderIndex + 1) % orderDisplays.Count;
                orderDisplays[selectedOrderIndex].SetSelected(true);
            }
        }

        public void ReceiveAInput(InputAction.CallbackContext context)
        {
            SelectOrder();
        }

        private void OnDestroy()
        {
            if (gameManager)
                gameManager.DeliveryPoint.OnItemDelivered -= ReceiveItemDelivered;
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context) { }

        public void ReceiveMovementDownInput(InputAction.CallbackContext context) { }

        public void ReceiveBInput(InputAction.CallbackContext context) { }

        public void ReceiveStartInput(InputAction.CallbackContext context) { }

        public void ReceiveSelectInput(InputAction.CallbackContext context) { }
        
        public void ResetInputs() { }
    }
}