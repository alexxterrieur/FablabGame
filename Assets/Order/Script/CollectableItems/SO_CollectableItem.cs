using UnityEngine;

[CreateAssetMenu(fileName = "SO_CollectableItem", menuName = "FabLabGame_SO/SO_CollectableItem")]
public class SO_CollectableItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public Color itemColor;
    public Mesh itemMesh; //Gameobjets pour gerer taille + material
    [Range(0, 1)] public float slowdownMovementFactor = 1;

    public bool IsFinalItem = false;

}
