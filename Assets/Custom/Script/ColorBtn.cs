using UnityEngine;
using UnityEngine.UI;

public class ColorBtn : SelectBtn
{

}

public abstract class SelectBtn : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject selection;

    private void Awake()
    {
        Unselect();
    }
    public void Select()
    {
        selection.SetActive(true);
    }

    public void Unselect()
    {
        selection.SetActive(false);
    }

    public void Setup(Material color)
    {
        image.color = color.color;
    }

    public void Setup(Sprite sprite)
    {
        image.sprite = sprite;
        image.material = null;
    }
}
