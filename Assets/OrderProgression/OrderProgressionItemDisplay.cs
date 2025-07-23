using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderProgression
{
    public class OrderProgressionItemDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private Image itemStatusIcon;
        
        [Header("Assets")]
        [SerializeField] private Sprite deliveredIcon;
        [SerializeField] private Sprite notDeliveredIcon;
        
        public void SetItemDetails(SO_CollectableItem item)
        {
            if (!item) return;

            itemIcon.sprite = item.itemIcon;
            itemNameText.text = item.itemName;
            
            itemStatusIcon.sprite = notDeliveredIcon;
        }
        
        public void UpdateItemStatus(bool isDelivered)
        {
            itemStatusIcon.sprite = isDelivered ? deliveredIcon : notDeliveredIcon;
        }
        
        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}