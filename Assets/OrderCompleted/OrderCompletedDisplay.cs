using UnityEngine;
using UnityEngine.UI;

namespace OrderCompleted
{
    public class OrderCompletedDisplay : MonoBehaviour
    {
        [SerializeField] private Image orderImage;

        public void DisplayOrder(SO_Order order)
        {
            orderImage.sprite = order.orderIcon;
        }
    }
}