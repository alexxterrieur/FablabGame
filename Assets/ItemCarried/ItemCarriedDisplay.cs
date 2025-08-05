using System;
using UnityEngine;
using UnityEngine.UI;

namespace ItemCarried
{
    public class ItemCarriedDisplay : MonoBehaviour
    {
        [SerializeField] private Image renderer;
        [SerializeField] private PlayerInteraction playerInteraction;

        private void Awake()
        {
            if (playerInteraction) playerInteraction.OnItemEquipped += SetItemSprite;
            else Debug.LogError("PlayerInteraction is not assigned in ItemCarriedDisplay.");
            
            if (!renderer) Debug.LogError("Renderer is not assigned in ItemCarriedDisplay.");
        }

        private void Start()
        {
            SetItemSprite(null);
        }

        private void SetItemSprite(SO_CollectableItem newItem)
        {
            if (newItem)
            {
                renderer.sprite = newItem.itemIcon;
                gameObject.SetActive(true);
            }
            else 
                gameObject.SetActive(false);
        }
    }
}