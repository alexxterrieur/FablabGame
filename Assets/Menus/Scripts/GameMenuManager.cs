using DeliveryPoint;
using OrderChoice;
using OrderProgression;
using Score;
using Timer.Scripts;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private DeliveryPointManagement deliveryPointManagement;
    
    [Header("UI References")]
    [SerializeField] private TimerDisplay timerDisplay;
    [SerializeField] private OrderProgressionDisplay orderProgressionDisplay;
    [SerializeField] private PlayerScoreDisplay playerScoreDisplay;
    [SerializeField] private OrderChoiceManager orderChoiceManager;
    
    private void Awake()
    {
        if (countdownTimer) countdownTimer.onTimerFinished.AddListener(ReceiveGameFinished);
        else Debug.LogError("CountdownTimer is not assigned in the inspector");

        if (orderManager) orderManager.OnOrderChanged += ReceiveOrderChanged;
        else Debug.LogError("OrderManager is not assigned in the inspector");
        
        if (deliveryPointManagement) deliveryPointManagement.OnItemDelivered += ReceiveItemDelivered;
        else Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
        
        if (!orderProgressionDisplay) Debug.LogError("OrderProgressionDisplay is not assigned in the inspector");
        if (!timerDisplay) Debug.LogError("TimerDisplay is not assigned in the inspector");
        if (!playerScoreDisplay) Debug.LogError("PlayerScoreDisplay is not assigned in the inspector");
        if (!orderChoiceManager) Debug.LogError("OrderChoiceManager is not assigned in the inspector");
    }

    private void ReceiveItemDelivered(int _)
    {
        playerScoreDisplay.SetVisibility(false);
        timerDisplay.SetVisibility(false);
    }

    private void ReceiveOrderChanged(SO_Order _)
    {
        playerScoreDisplay.SetVisibility(true);
        timerDisplay.SetVisibility(true);
    }

    private void ReceiveGameFinished()
    {
        timerDisplay.SetVisibility(false);
        orderProgressionDisplay.SetVisibility(false);
        playerScoreDisplay.SetVisibility(false);
        orderChoiceManager.SetVisibility(false);
    }
    
}
