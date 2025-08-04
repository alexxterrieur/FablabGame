using System.Collections.Generic;
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
        
        [SerializeField] private List<OrderChoiceMaterialDisplay> materialDisplays;
        
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
            orderScoreText.text = $"+{order.orderPoints}";
            orderMainMaterialText.text = order.mainMaterial switch 
            {
                Materials.Wood => "Fraiseuse",
                Materials.Plastic => "Imprimante 3D",
                Materials.Metal => "Decoupeuse Laser",
                _ => "Unknown Machine"
            };
            
            
            for (int i = 0; i < materialDisplays.Count; i++)
            {
                var currentDisplay = materialDisplays[i];
                
                if (i < order.Materials.Count)
                {
                    var material = order.Materials[i];
                    currentDisplay.SetMaterial(material.item.itemIcon, material.amount);
                    currentDisplay.SetVisibility(true);
                }
                else
                {
                    currentDisplay.SetVisibility(false);
                }
            }
        }
    }
}