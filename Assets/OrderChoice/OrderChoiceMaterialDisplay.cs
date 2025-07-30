using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderChoice
{
    public class OrderChoiceMaterialDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;
        
        public void SetMaterial(Sprite materialIcon, int amount)
        {
            if (icon)
                icon.sprite = materialIcon;

            if (amountText)
                amountText.text = amount.ToString();
        }
        
        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}