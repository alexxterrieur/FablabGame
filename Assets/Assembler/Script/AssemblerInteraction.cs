using UnityEngine;

public class AssemblerInteraction : MonoBehaviour
{
    public Materials assemblerMaterial;

    public bool TryDeliverItem(SO_CollectableItem item)
    {
        SO_Order order = OrderManager.instance.currentOrder;

        if (order.mainMaterial != assemblerMaterial)
        {
            Debug.Log("Mauvais type d'assembleur");
            return false;
        }

        bool valid = order.MarkItemDelivered(item);

        if (valid)
        {
            Debug.Log("Item commande validé");
            if (order.IsOrderComplete())
            {
                Debug.Log("play mini game");
            }

            return true;
        }

        Debug.Log("item pas bon ou déja livré");
        return false;
    }
}
