using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelection : MonoBehaviour
{
    public List<Material> materials;
    private List<ColorBtn> btnList;
    public GameObject colorPrefab;
    private int currentSelectColor = 0;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameObject parentColors;
    [SerializeField] private CustomInput input;

    [SerializeField] private Image menuImage;
    [SerializeField] private Sprite NoneSprite;

    [SerializeField] private CustomManager manager;
    [SerializeField] private MeshRenderer finalItem;
    [SerializeField] private MeshRenderer holdingItem;

    private void Start()
    {
        manager.OnReset+=ResetValues;
        btnList = new List<ColorBtn>();
        menuImage.gameObject.SetActive(false);
        bool first = true;
        foreach (var color in materials)
        {
            GameObject go = Instantiate(colorPrefab, parentColors.transform);
            ColorBtn btn = go.GetComponent<ColorBtn>();

            if (btn == null)
                Debug.Log("BTN IS NULLLLLLLLLLL");
            else
                btnList.Add(btn);
            
            if(first)
            {
                first = false;
                btn.Setup(NoneSprite);
            }
            else
                btn.Setup(color);
        }
        ResetValues();
        DecalSelected(new Vector2Int(0, 0));
    }

    private void OnEnable()
    {
        OpenSelectionColor();
    }

    public void OpenSelectionColor()
    {
        RemoveSelectionColor();
        input.OnMove += DecalSelected;
        input.OnSelect += Select;
    }

    private void RemoveSelectionColor()
    {
        input.OnMove -= DecalSelected;
        input.OnSelect -= Select;
    }

    private void ResetValues()
    {
        btnList[currentSelectColor].Unselect();
        currentSelectColor = 0;
        DecalSelected(new Vector2Int(0, 0));
        menuImage.material = materials[0];
        finalItem.material = materials[0];
        holdingItem.material = materials[0];


    }
    private void Select()
    {
        menuImage.material = materials[currentSelectColor];
        finalItem.material = materials[currentSelectColor];
        holdingItem.material = materials[currentSelectColor];

        if (currentSelectColor != 0)
        {
            manager.additionalScore.Item1 = 50;
            menuImage.gameObject.SetActive(true);
        }

        else
        {
            menuImage.gameObject.SetActive(false);
            manager.additionalScore.Item1 = 0;
        }

        RemoveSelectionColor();
    }

    public void DecalSelected(Vector2Int selectionMovement)
    {
        btnList[currentSelectColor].Unselect();

        DecalSelection(selectionMovement.x, selectionMovement.y, grid.constraintCount);
        btnList[currentSelectColor].Select();
    }

    private void DecalSelection(int moveX, int moveY, int columns)
    {
        int total = btnList.Count;

        int row = currentSelectColor / columns;
        int col = currentSelectColor % columns;

        int maxRow = (total - 1) / columns;

        row = (row + moveY + maxRow + 1) % (maxRow + 1);

        int targetRowLength = (row == maxRow && total % columns != 0) ? total % columns : columns;

        col = (col + moveX + targetRowLength) % targetRowLength;

        int newIndex = row * columns + col;

        if (newIndex >= total)
            newIndex = total - 1;

        currentSelectColor = newIndex;
    }
}
