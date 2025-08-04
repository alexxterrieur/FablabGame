using System;
using System.Collections;
using System.Collections.Generic;
using DeliveryPoint;
using Player.Script;
using PlayerPrefsManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DeliveryPointManagement deliveryPointManagement;
        [SerializeField] private CountdownTimer countdownTimer;
        [SerializeField] private PlayerScore playerScore;
        [SerializeField] private OrderManager orderManager;
        [SerializeField] private List<AssemblerInteraction> assemblers = new();
        [SerializeField] private AssemblerInteraction woodAssembler;
        [SerializeField] private AssemblerInteraction metalAssembler;
        [SerializeField] private AssemblerInteraction plasticAssembler;
        [SerializeField] private CustomManager customManager;
        
        public event Action<bool> OnGameFinished;
        public event Action<bool> OnGamePaused;

        private void Awake()
        {
            if (countdownTimer) countdownTimer.onTimerFinished.AddListener(ReceiveTimerFinished);
            else Debug.LogError("CountdownTimer is not assign in the inspector");
            
            if (deliveryPointManagement) deliveryPointManagement.OnItemDelivered += ReceiveItemDelivered;
            else Debug.LogError("DeliveryPointManagement is not assigned in the inspector");
            
            if (orderManager) orderManager.OnOrderChanged += ReceiveOrderChanged;
            else Debug.LogError("OrderManager is not assigned in the inspector");

            if (!woodAssembler)    Debug.LogError("woodAssembler is not assigned in the inspector");
            if (!metalAssembler)   Debug.LogError("metalAssembler is not assigned in the inspector");
            if (!plasticAssembler) Debug.LogError("plasticAssembler is not assigned in the inspector");
            if (!customManager) Debug.LogError("custom interaction is not assigned in the inspector");
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(true);
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1;
            OnGamePaused?.Invoke(false);
        }
        
        public bool IsGamePaused()
        {
            return Time.timeScale == 0;
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ReceiveTimerFinished()
        {
            StartCoroutine(ReceiveTimerFinishedCoroutine());
        }

        private IEnumerator ReceiveTimerFinishedCoroutine()
        {
            yield return new WaitForEndOfFrame();

            int highScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HighScoreKey);
            int currentScore = playerScore.Score;
            bool betterScore = currentScore > highScore;

            if (betterScore)
                PlayerPrefs.SetInt(PlayerPrefsKeys.HighScoreKey, currentScore);

            OnGameFinished?.Invoke(betterScore);
        }


        private void ReceiveItemDelivered(int orderPoints)
        {
            playerScore.IncreaseScore(orderPoints);
            countdownTimer.SetRunning(false);
        }
        
        private void ReceiveOrderChanged(SO_Order order)
        {
            foreach (var assembler in assemblers)
            {
                assembler.SetCurrentOrder(order);
            }

            woodAssembler.SetCurrentOrder(order);
            metalAssembler.SetCurrentOrder(order);
            plasticAssembler.SetCurrentOrder(order);
            deliveryPointManagement.SetCurrentOrder(order);
            
            countdownTimer.SetRunning(true);
        }
        
        public PlayerScore PlayerScore => playerScore;
        public DeliveryPointManagement DeliveryPoint => deliveryPointManagement;
        public AssemblerInteraction WoodAssembler => woodAssembler;
        public AssemblerInteraction MetalAssembler => metalAssembler;
        public AssemblerInteraction PlasticAssembler => plasticAssembler;
        public CustomManager CustomManager => customManager;

        private void OnDestroy()
        {
            if (countdownTimer)
                countdownTimer.onTimerFinished.RemoveListener(ReceiveTimerFinished);

            if (deliveryPointManagement)
                deliveryPointManagement.OnItemDelivered -= ReceiveItemDelivered;
            
            if (orderManager)
                orderManager.OnOrderChanged -= ReceiveOrderChanged;
        }
    }
}