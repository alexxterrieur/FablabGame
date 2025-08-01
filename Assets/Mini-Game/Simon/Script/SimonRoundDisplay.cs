using UnityEngine;
using UnityEngine.UI;

namespace Mini_Game.Simon.Script
{
    public class SimonRoundDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite validSprite;
        [SerializeField] private Sprite invalidSprite;
        
        private Image renderer;

        private void Awake()
        {
            if (!renderer)
                Initialize();
        }

        private void Initialize()
        {
            if (GetComponent<Image>() is {} spriteRenderer)
            {
                renderer = spriteRenderer;
                renderer.sprite = invalidSprite;
            }
            else Debug.LogError("Image Renderer component is missing on the GameObject.");
        }
        
        public void SetValid(bool isValid)
        {
            if (!renderer)
                Initialize();

            renderer.sprite = isValid ? validSprite : invalidSprite;
        }
    }
}