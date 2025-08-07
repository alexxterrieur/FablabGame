using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StickersSelection : MonoBehaviour
{
    public List<Texture2D> textures;
    private List<StikerBtn> btnList;
    public GameObject stickerPrefab;
    private int currentSelectSticker = 0;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameObject parentStickers;
    [SerializeField] private CustomInput input;

    [SerializeField] private Image menuImage;
    [SerializeField] private DecalProjector projector;
    [SerializeField] private ProjectorMovement projectorMovement;
    [SerializeField] private CustomManager customManager;

    [SerializeField] private TMP_Text stickerText; 
    public event Action<bool> onStickerSelected; 

    private void Start()
    {
        customManager.OnReset += ResetValues;
        menuImage.gameObject.SetActive(false);
        btnList = new List<StikerBtn>();
        foreach (var tex in textures)
        {
            GameObject go = Instantiate(stickerPrefab, parentStickers.transform);
            StikerBtn btn = go.GetComponent<StikerBtn>();

            if (btn == null)
                Debug.Log("BTN IS NULLLLLLLLLLL");
            else
                btnList.Add(btn);
            
            btn.Setup(Utils.ConvertToSprite(tex));

            /*
            Image image = go.GetComponent<Image>();
            if (image == null)
                return;
            image.sprite = Utils.ConvertToSprite(tex);
        */
        }
        DecalSelected(new Vector2Int(0, 0));
        stickerText.text = "0/1";
    }

    private void OnEnable()
    {
        input.OnMove += DecalSelected;
        input.OnSelect += Select;
    }

    private void ResetValues()
    {
        btnList[currentSelectSticker].Unselect();
        currentSelectSticker = 0;
        SetProjectorTexture(textures[0]);
        DecalSelection(0, 0, grid.constraintCount);
        menuImage.sprite = Utils.ConvertToSprite(textures[0]);
        projector.transform.localPosition = Vector3.zero;
        menuImage.gameObject.SetActive(false);
        projector.gameObject.SetActive(false);
    }

    private void SetUpProjectorMovement()
    {
        RemoveProjectorMovement();
        input.OnMoveDirection += projectorMovement.OnMove;
        input.OnSelect += SelectPosition;
    }

    private void RemoveProjectorMovement()
    {
        input.OnMoveDirection?.Invoke(new Vector2(0,0));
        input.OnMoveDirection -= projectorMovement.OnMove; 
        input.OnSelect -= SelectPosition;
    }

    private void Select()
    {
        menuImage.sprite = Utils.ConvertToSprite(textures[currentSelectSticker]);
        SetProjectorTexture(textures[currentSelectSticker]);
        input.OnMove -= DecalSelected;
        input.OnSelect -= Select;

        if (currentSelectSticker == 0)
        {
            projector.gameObject.SetActive(false);
            menuImage.gameObject.SetActive(false);
            SelectPosition();
            customManager.additionalScore.Item2 = 0;
            stickerText.text = "0/1";
            onStickerSelected?.Invoke(false);
            return;
        }
        stickerText.text = "1/1";
        onStickerSelected?.Invoke(true);
        menuImage.gameObject.SetActive(true);
        customManager.additionalScore.Item2 = 50;
        projector.gameObject.SetActive(true);
        SetUpProjectorMovement();
    }
    
    private void SelectPosition()
    {
        RemoveProjectorMovement();
        customManager.Return();
    }

    private void SetProjectorTexture(Texture2D tex)
    {

        if (projector != null)
        {
            Material decalMaterial = projector.material;

            //Shader shader = decalMaterial.shader;
            //int propertyCount = shader.GetPropertyCount();

            //for (int i = 0; i < propertyCount; i++)
            //{
            //    string propName = shader.GetPropertyName(i);
            //    UnityEngine.Rendering.ShaderPropertyType propType = shader.GetPropertyType(i);

            //    Debug.Log($"[{propType}] {propName}");
            //}

            decalMaterial.SetTexture("Base_Map", tex);

            //Debug.LogError("Shader used: " + decalMaterial.shader.name);
        }
    }

    public void DecalSelected(Vector2Int selectionMovement)
    {
        btnList[currentSelectSticker].Unselect();

        DecalSelection(selectionMovement.x, -selectionMovement.y, grid.constraintCount);
        btnList[currentSelectSticker].Select();
    }

    private void DecalSelection(int moveX, int moveY, int columns)
    {
        int total = btnList.Count;

        int row = currentSelectSticker / columns;
        int col = currentSelectSticker % columns;

        int maxRow = (total - 1) / columns;

        row = (row + moveY + maxRow + 1) % (maxRow + 1);

        int targetRowLength = (row == maxRow && total % columns != 0) ? total % columns : columns;

        col = (col + moveX + targetRowLength) % targetRowLength;

        int newIndex = row * columns + col;

        if (newIndex >= total)
            newIndex = total - 1;

        currentSelectSticker = newIndex;
    }
}
