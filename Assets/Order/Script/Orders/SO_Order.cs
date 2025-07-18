using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Order", menuName = "FabLabGame_SO/SO_Order")]
public class SO_Order : ScriptableObject
{
    public string orderName;
    public Sprite orderIcon;
    public Materials mainMaterial;
    public List<SO_CollectableItem> items = new List<SO_CollectableItem>();
    public int orderPoints;

    [HideInInspector]
    public List<bool> delivered = new List<bool>();

    public void InitDeliveryStatus()
    {
        delivered.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            delivered.Add(false);
        }
    }

    public bool MarkItemDelivered(SO_CollectableItem item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item && !delivered[i])
            {
                delivered[i] = true;
                return true;
            }
        }
        return false;
    }

    public bool IsOrderComplete()
    {
        foreach (bool isDelivered in delivered)
        {
            if (!isDelivered)
                return false;
        }
        return true;
    }
}


