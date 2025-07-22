using System;
using UnityEngine;

public class AssemblerInteraction : MonoBehaviour
{
    public Materials assemblerMaterial;
    public Assembler assembler;
    private SO_Order currentOrder;

    public event Action OnOrderCompleted;

    private void Start()
    {
        assembler.OnAssembleurActivityEnd += EndActivity;
    }

    private void EndActivity(bool isSuccess)
    {
        if (isSuccess)
            return;

       currentOrder.RemoveDeliveredItem();

        Debug.Log("Remove item");

    }

    public void SetCurrentOrder(SO_Order order)
    {
        currentOrder = order;
    }

    public bool TryDeliverItem(SO_CollectableItem item)
    {
        if (currentOrder.mainMaterial != assemblerMaterial)
        {
            Debug.Log("Mauvais type d'assembleur");
            return false;
        }

        bool valid = currentOrder.MarkItemDelivered(item);

        if (valid)
        {
            Debug.Log("Item commande valid�");
            if (currentOrder.IsOrderComplete())
            {
                Debug.Log("play mini game");
                assembler.Activate();
                OnOrderCompleted?.Invoke();
            }

            return true;
        }

        Debug.Log("item pas bon ou d�ja livr�");
        return false;
    }
}
