using UnityEngine;
using UnityEngine.UI;

namespace OrderCompleted
{
    public class OrderCompletedDisplay : MonoBehaviour
    {
        [SerializeField] private Image orderImage;

        public void DisplayOrder(FinalObject order)
        {
            if(order.customTex == null)
                orderImage.sprite = order.order.orderIcon;
            else
                orderImage.sprite = Utils.ConvertToSprite(order.customTex);
        }
    }
}