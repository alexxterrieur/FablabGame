using UnityEngine;

public class ColorBtn : SelectBtn
{

}

public abstract class SelectBtn : MonoBehaviour
{
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
}
