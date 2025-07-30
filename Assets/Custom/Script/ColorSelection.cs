using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering;
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

    [SerializeField] private CustomManager manager;
    [SerializeField] private MeshRenderer finalItem;

    private void Start()
    {
        manager.OnReset+=ResetValues;
        input.OnMove += DecalSelected;
        input.OnSelect += Select;
        btnList = new List<ColorBtn>();
        foreach (var color in materials)
        {
            GameObject go = Instantiate(colorPrefab, parentColors.transform);
            ColorBtn btn = go.GetComponent<ColorBtn>();

            if (btn == null)
                Debug.Log("BTN IS NULLLLLLLLLLL");
            else
                btnList.Add(go.GetComponent<ColorBtn>());

            Image image = go.GetComponent<Image>();
            if (image == null)
                return;
            image.material = color;
        }
        DecalSelected(new Vector2Int(0, 0));
    }

    private void ResetValues()
    {
        currentSelectColor = 0;
        DecalSelected(new Vector2Int(0, 0));
        menuImage.material = null;
        finalItem.material = null;

    }
    private void Select()
    {
        menuImage.material = materials[currentSelectColor];
        finalItem.material = materials[currentSelectColor];

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
