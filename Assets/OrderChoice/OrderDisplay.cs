using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderChoice
{
    public class OrderDisplay : MonoBehaviour
    {
        [SerializeField] private Image orderIcon;
        [SerializeField] private TMP_Text orderNameText;
        [SerializeField] private TMP_Text orderScoreText;
        [SerializeField] private TMP_Text orderMainMaterialText;
        [SerializeField] private TMP_Text orderNbItemsRequiredText;
        
        [SerializeField] private Image selectionIndicator;

        private void Awake()
        {
            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            selectionIndicator.enabled = isSelected;
        }
        
        public void DisplayOrder(SO_Order order)
        {
            if (!order)
            {
                Debug.LogWarning("Order is null, cannot display.");
                return;
            }

            orderIcon.sprite = order.orderIcon;
            orderNameText.text = order.orderName;
            orderScoreText.text = $"Points: {order.orderPoints}";
            orderMainMaterialText.text = $"Main Material: {order.mainMaterial}";
            orderNbItemsRequiredText.text = $"Items Required: {order.items.Count}";
        }
    }
}