using System;
using UnityEngine;

[Serializable]
public struct MaterialAmount
{
    public SO_CollectableItem item;
    [Min(0)] public int amount;
}