using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Script
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        
        public void ReceiveMovementInput(InputAction.CallbackContext context)
        {
            playerMovement.Move(context.ReadValue<Vector2>());
        }
    }
}