using System.Collections.Generic;
using UnityEngine;

public static class ShelfRegistry
{
    private static Dictionary<SO_CollectableItem, List<Shelf>> shelvesByItem = new();

    public static void Register(SO_CollectableItem item, Shelf shelf)
    {
        if (!shelvesByItem.ContainsKey(item))
            shelvesByItem[item] = new List<Shelf>();

        if (!shelvesByItem[item].Contains(shelf))
            shelvesByItem[item].Add(shelf);

        DebugShelves();
    }

    public static void DebugShelves()
    {
        foreach (var pair in shelvesByItem)
        {
            Debug.Log($"Item: {pair.Key.name}, Shelf Count: {pair.Value.Count}");
        }
    }

    public static void Unregister(SO_CollectableItem item, Shelf shelf)
    {
        if (shelvesByItem.TryGetValue(item, out var list))
            list.Remove(shelf);
    }

    public static List<Shelf> GetShelves(SO_CollectableItem item)
    {
        return shelvesByItem.TryGetValue(item, out var list) ? list : new List<Shelf>();
    }

    public static void Clear()
    {
        shelvesByItem.Clear();
    }
}
