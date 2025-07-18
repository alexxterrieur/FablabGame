using UnityEngine;

public class Form : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;

    public void SetImage(Texture2D texture)
    {
        renderer.material.mainTexture = texture;
    }

}
