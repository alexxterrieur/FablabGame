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

        private void Awake()
        {
            if (gameManager) gameManager.OnGameFinished += DisplayOrdersCompleted;
            else Debug.LogError("GameManager is not assigned in the inspector");
            
            if (!deliveryPointManagement) Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
            
            if (!orderCompletedDisplayPrefab) Debug.LogError("OrderCompletedDisplayPrefab is not assigned in the inspector");
        }
        
        private void DisplayOrdersCompleted(bool _, bool __)
        {
            gameObject.SetActive(true);
            
            foreach (FinalObject order in deliveryPointManagement.OrdersDelivered)
                Instantiate(orderCompletedDisplayPrefab, transform).DisplayOrder(order);
        }

        private void OnDestroy()
        {
            if (gameManager) gameManager.OnGameFinished -= DisplayOrdersCompleted;
        }
    }
}