using Player.Script;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;

    private void Awake()
    {
        if (playerInteraction) playerInteraction.OnItemEquipped += ReceiveItemEquipped;
        else Debug.LogError("PlayerInteraction component is not assigned in PlayerManager.");
        
        if (!playerMovement) Debug.LogError("PlayerMovement component is not assigned in PlayerManager.");
    }
    
    private void ReceiveItemEquipped(SO_CollectableItem item)
    {
        playerMovement.SlowDownMovement(item?.slowdownMovementFactor ?? 1);
    }

    private void OnDestroy()
    {
        if (playerInteraction) playerInteraction.OnItemEquipped -= ReceiveItemEquipped;
    }
}