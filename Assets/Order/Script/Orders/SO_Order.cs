using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SO_Order", menuName = "FabLabGame_SO/SO_Order")]
public class SO_Order : ScriptableObject
{
    public string orderName;
    public Sprite orderIcon;
    public Materials mainMaterial;
    public SO_CollectableItem finalItem;
    public int orderPoints;
    
    [SerializeField] private List<MaterialAmount> materials = new();
    public FormData millingForm;
    private Dictionary<SO_CollectableItem, int> itemDeliveryCount = new();
    public event Action<SO_CollectableItem, int, int> OnMaterialAmountChanged; 

    public void InitDeliveryStatus()
    {
        itemDeliveryCount.Clear();
    }

    public bool CanAddItem(SO_CollectableItem item)
    {
        if (GetItemCount(item) >= GetItemMaxAmount(item))
            return false;
        return true;
    }

    public bool TryAddItem(SO_CollectableItem item)
    {
        if (!CanAddItem(item))
            return false;

        itemDeliveryCount.TryAdd(item, 0);
        itemDeliveryCount[item]++;
        
        NotifyItemAmountChanged(item);
        return true;
    }

    public void RemoveDeliveredItem()
    {
        if (!IsOrderComplete())
            return;
        
        int index = Random.Range(0, materials.Count);
        SO_CollectableItem item = materials[index].item;
        if (itemDeliveryCount.TryGetValue(item, out int itemCount) && itemCount > 0)
        {
            itemDeliveryCount[item]--;
            NotifyItemAmountChanged(item);
        }
    }

    public bool IsOrderComplete()
    {
        bool isComplete = true;
        
        foreach (var material in materials)
        {
            if (itemDeliveryCount.TryGetValue(material.item, out int count))
            {
                if (count < material.amount)
                    isComplete = false;
            }
            else
                isComplete = false;
        }

        return isComplete;
    }
    
    private void NotifyItemAmountChanged(SO_CollectableItem item)
    {
        OnMaterialAmountChanged?.Invoke(item, GetItemCount(item), GetItemMaxAmount(item));
    }
    
    private int GetItemCount(SO_CollectableItem item)
    {
        return itemDeliveryCount.GetValueOrDefault(item, 0);
    }
    
    private int GetItemMaxAmount(SO_CollectableItem item)
    {
        foreach (MaterialAmount material in materials)
        {
            if (material.item == item)
            {
                return material.amount;
            }
        }
        return 0;
    }
    
    public int GetAllNeededMaterialsCount()
    {
        return materials.Sum(material => material.amount);
    }
    
    public List<MaterialAmount> Materials => materials;
}


