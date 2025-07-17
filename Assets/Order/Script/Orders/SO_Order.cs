using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Order", menuName = "FabLabGame_SO/SO_Order")]
public class SO_Order : ScriptableObject
{
    public enum Materials { Wood, Plastic, Metal}

    public string orderName;
    public Sprite orderIcon;
    public Materials mainMaterial;
    public List<SO_CollectableItem> items = new List<SO_CollectableItem>();
    public int orderPoints;
}
