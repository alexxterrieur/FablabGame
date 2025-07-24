using System;
using DeliveryPoint;
using GameManagement;
using UnityEngine;

namespace OrderCompleted
{
    public class OrderCompletedDisplayManager : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DeliveryPointManagement deliveryPointManagement;
        
        [Header("UI Reference")]
        [SerializeField] private OrderCompletedDisplay orderCompletedDisplayPrefab;
        [SerializeField] private Transform orderCompletedDisplayContainer;

        private void Awake()
        {
            if (gameManager) gameManager.OnGameFinished += DisplayOrdersCompleted;
            else Debug.LogError("GameManager is not assigned in the inspector");
            
            if (!deliveryPointManagement) Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
            
            if (!orderCompletedDisplayPrefab) Debug.LogError("OrderCompletedDisplayPrefab is not assigned in the inspector");
            
            gameObject.SetActive(false);
        }
        
        private void DisplayOrdersCompleted(bool _)
        {
            gameObject.SetActive(true);
            
            foreach (SO_Order order in deliveryPointManagement.OrdersDelivered)
                Instantiate(orderCompletedDisplayPrefab, orderCompletedDisplayContainer).DisplayOrder(order);
        }

        private void OnDestroy()
        {
            if (gameManager) gameManager.OnGameFinished -= DisplayOrdersCompleted;
        }
    }
}