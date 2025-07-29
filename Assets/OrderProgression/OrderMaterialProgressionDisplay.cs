using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderProgression
{
    public class OrderMaterialProgressionDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemProgressionText;
        [SerializeField] private Image itemStatusIcon;
        
        [Header("Assets")]
        [SerializeField] private Sprite deliveredIcon;
        [SerializeField] private Sprite notDeliveredIcon;
        
        private MaterialAmount currentMaterial;
        
        public void SetMaterialDetails(MaterialAmount material)
        {
            currentMaterial = material;
            itemIcon.sprite = material.item.itemIcon;
            itemNameText.text = material.item.itemName;
            itemNameText.color = material.item.itemColor;
            itemProgressionText.text = $"0 / {material.amount}";
            
            itemStatusIcon.sprite = notDeliveredIcon;
        }
        
        public void UpdateMaterialAmount(int currentAmount, int maxAmount)
        {
            itemProgressionText.text = $"{currentAmount} / {maxAmount}";
            itemStatusIcon.sprite = currentAmount >= maxAmount ? deliveredIcon : notDeliveredIcon;
        }
        
        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
        
        public SO_CollectableItem MaterialDisplay => currentMaterial.item;
    }
}