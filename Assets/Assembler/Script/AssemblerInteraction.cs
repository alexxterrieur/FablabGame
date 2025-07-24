using System;
using UnityEngine;

public class AssemblerInteraction : MonoBehaviour
{
    public Materials assemblerMaterial;
    public Assembler assembler;
    private SO_Order currentOrder;

    public event Action OnOrderCompleted;
    public Action<SO_CollectableItem> OnOrderCraft;

    private void Start()
    {
        assembler.OnAssembleurActivityEnd += EndActivity;
    }

    private void EndActivity(bool isSuccess)
    {
        if (isSuccess)
        {
            OnOrderCraft(currentOrder.finalItem);
            return;
        }

        currentOrder.RemoveDeliveredItem();
    }

    public void SetCurrentOrder(SO_Order order)
    {
        currentOrder = order;
    }

    public SO_CollectableItem TryGetCraftItem()
    {
        return null;
    }

    public bool TryDeliverItem(SO_CollectableItem item)
    {
        if (currentOrder.mainMaterial != assemblerMaterial)
        {
            return false;
        }

        bool valid = currentOrder.MarkItemDelivered(item);

        if (valid)
        {
            if (currentOrder.IsOrderComplete())
            {
                assembler.Activate();
                OnOrderCompleted?.Invoke();
            }

            return true;
        }

        return false;
    }
}
