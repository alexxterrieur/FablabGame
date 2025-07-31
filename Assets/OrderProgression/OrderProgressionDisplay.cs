using System.Collections.Generic;
using DeliveryPoint;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderProgression
{
    public class OrderProgressionDisplay : MonoBehaviour
    {
        [Header("Gameplay References")]
        [SerializeField] private OrderManager orderManager;
        [SerializeField] private DeliveryPointManagement deliveryPointManagement;
        
        [Header("UI References")]
        [SerializeField] private Image orderIcon;
        [SerializeField] private TMP_Text orderNameText;
        [SerializeField] private TMP_Text orderMachineText;
        [SerializeField] private TMP_Text orderScoreText;
        
        [Header("Item Display Prefab")]
        [SerializeField] private OrderMaterialProgressionDisplay orderMaterialDisplay;
        [SerializeField] private Transform itemDisplayContainer;
        
        private SO_Order currentOrder;
        private List<OrderMaterialProgressionDisplay> materialDisplays = new();

        private void Awake()
        {
            if (orderManager) orderManager.OnOrderChanged += ManageNewOrder;
            else Debug.LogError("OrderManager reference is missing in OrderProgressionDisplay.");
            
            if (deliveryPointManagement) deliveryPointManagement.OnItemDelivered += ReceiveItemDelivered;
            else Debug.LogError("DeliveryPointManagement reference is missing in OrderProgressionDisplay.");
            
            if (!orderMaterialDisplay) Debug.LogError("OrderItemDisplay reference is missing in OrderProgressionDisplay.");
            
            SetVisibility(false);
        }
        
        private void ManageNewOrder(SO_Order newOrder)
        {
            if (currentOrder) currentOrder.OnMaterialAmountChanged -= ReceiveMaterialAmountChanged;
            
            if (!newOrder)
            {
                SetVisibility(false);
                Debug.LogError("New order is null in OrderProgressionDisplay.");
                return;
            }
            
            currentOrder = newOrder;
            
            if (currentOrder) currentOrder.OnMaterialAmountChanged += ReceiveMaterialAmountChanged;
            
            SetVisibility(true);
            ManageAmountDisplay(currentOrder);
            SetNewOrderDetails(currentOrder);
        }

        private void SetNewOrderDetails(SO_Order order)
        {
            if (!order) return;

            orderIcon.sprite = order.orderIcon;
            orderNameText.text = order.orderName;
            orderScoreText.text = $"+{order.orderPoints}";
            
            orderMachineText.text = order.mainMaterial switch 
            {
                Materials.Wood => "Fraiseuse",
                Materials.Plastic => "Imprimante 3D",
                Materials.Metal => "Decoupeuse Laser",
                _ => "Unknown Machine"
            };

            
            for (int i = 0; i < order.Materials.Count; i++)
                materialDisplays[i].SetMaterialDetails(order.Materials[i]);
        }
        
        private void ManageAmountDisplay(SO_Order order)
        {
            if (!order) return;
            
            int nbDifferentMaterials = order.Materials.Count;
            
            if (nbDifferentMaterials < materialDisplays.Count)
            {
                for (int i = materialDisplays.Count - 1; i >= nbDifferentMaterials; i--)
                    materialDisplays[i].SetVisibility(false);
            }
            else if (nbDifferentMaterials > materialDisplays.Count)
            {
                foreach (var materialDisplay in materialDisplays)
                    materialDisplay.SetVisibility(true);

                for (int i = materialDisplays.Count; i < nbDifferentMaterials; i++)
                    materialDisplays.Add(Instantiate(orderMaterialDisplay, itemDisplayContainer.position, Quaternion.identity, itemDisplayContainer));
            }
            else
            {
                foreach (var itemDisplay in materialDisplays)
                    itemDisplay.SetVisibility(true);
            }
        }
        
        private void ReceiveMaterialAmountChanged(SO_CollectableItem material, int currentAmount, int maxAmount)
        {
            if (!currentOrder) return;

            foreach (var materialDisplay in materialDisplays)
            {
                if (materialDisplay.MaterialDisplay == material)
                {
                    materialDisplay.UpdateMaterialAmount(currentAmount, maxAmount);
                    return;
                }
            }
        }

        private void ReceiveItemDelivered(int _)
        {
            SetVisibility(false);
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
        
        private void OnDestroy()
        {
            if (orderManager) orderManager.OnOrderChanged -= ManageNewOrder;
            
            if (currentOrder) currentOrder.OnMaterialAmountChanged -= ReceiveMaterialAmountChanged;
        }
    }
}