using DeliveryPoint;
using UnityEngine;

namespace PNJ
{
    public class PNJGameManager : MonoBehaviour
    {
        [Header("Assemblers")]
        [SerializeField] private AssemblerInteraction woodAssembler;
        [SerializeField] private AssemblerInteraction metalAssembler;
        [SerializeField] private AssemblerInteraction plasticAssembler;
        
        [Header("Others")]
        [SerializeField] private PNJManager pnjManager;
        [SerializeField] private DeliveryPointManagement deliveryPointManagement;

        private void Awake()
        {
            if (pnjManager) pnjManager.OnOrderStarted += ReceiveOrderStarted;
            else Debug.LogError("PNJManager is not assigned in the inspector");
            
            if (!woodAssembler) Debug.LogError("Wood Assembler is not assigned in the inspector");
            if (!metalAssembler) Debug.LogError("Metal Assembler is not assigned in the inspector");
            if (!plasticAssembler) Debug.LogError("Plastic Assembler is not assigned in the inspector");
        }

        private void ReceiveOrderStarted(SO_Order order)
        {
            woodAssembler.SetCurrentOrder(order);
            metalAssembler.SetCurrentOrder(order);
            plasticAssembler.SetCurrentOrder(order);
            deliveryPointManagement.SetCurrentOrder(order);
        }
    }
}