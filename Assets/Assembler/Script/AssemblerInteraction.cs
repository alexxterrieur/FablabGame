using System;
using UnityEngine;

public class AssemblerInteraction : MonoBehaviour
{
    public Materials assemblerMaterial;
    public Assembler assembler;
    private SO_Order currentOrder;

    public event Action OnOrderCompleted;
    public Action<SO_CollectableItem> OnOrderCraft;
    private bool isObjectCraft = false;

    private void Start()
    {
        assembler.OnAssembleurActivityExit += EndActivity;
    }

    private void EndActivity(bool isSuccess)
    {
        if (isObjectCraft)
            return;

        if (isSuccess)
        {
            isObjectCraft = true;
            OnOrderCraft(currentOrder.finalItem);
            return;
        }

        currentOrder.RemoveDeliveredItem();
    }

    public void SetCurrentOrder(SO_Order order)
    {
        currentOrder = order;
        isObjectCraft = false;
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

        if (currentOrder.TryAddItem(item))
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
