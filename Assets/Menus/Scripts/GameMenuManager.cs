using DeliveryPoint;
using OrderChoice;
using OrderProgression;
using Score;
using Unity.VisualScripting;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private DeliveryPointManagement deliveryPointManagement;
    
    [Header("UI References")]
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private OrderProgressionDisplay orderProgressionDisplay;
    [SerializeField] private PlayerScoreDisplay playerScoreDisplay;
    [SerializeField] private OrderChoiceManager orderChoiceManager;
    [SerializeField] private GameObject controlsPanel;
    
    private void Awake()
    {
        if (countdownTimer) countdownTimer.onTimerFinished.AddListener(ReceiveGameFinished);
        else Debug.LogError("CountdownTimer is not assigned in the inspector");

        if (orderManager) orderManager.OnOrderChanged += ReceiveOrderChanged;
        else Debug.LogError("OrderManager is not assigned in the inspector");
        
        if (deliveryPointManagement) deliveryPointManagement.OnItemDelivered += ReceiveItemDelivered;
        else Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
        
        if (!orderProgressionDisplay) Debug.LogError("OrderProgressionDisplay is not assigned in the inspector");
        if (!playerScoreDisplay) Debug.LogError("PlayerScoreDisplay is not assigned in the inspector");
        if (!orderChoiceManager) Debug.LogError("OrderChoiceManager is not assigned in the inspector");
    }

    private void ReceiveItemDelivered(int _)
    {
        playerScoreDisplay.SetVisibility(false);
        controlsPanel.SetActive(false);
        countdownTimer.SetVisibility(false);
    }

    private void ReceiveOrderChanged(SO_Order _)
    {
        playerScoreDisplay.SetVisibility(true);
        controlsPanel.SetActive(true);
        countdownTimer.SetVisibility(true);
    }

    private void ReceiveGameFinished()
    {
        countdownTimer.SetVisibility(false);
        orderProgressionDisplay.SetVisibility(false);
        playerScoreDisplay.SetVisibility(false);
        controlsPanel.SetActive(false);
        orderChoiceManager.SetVisibility(false);
    }
    
}
