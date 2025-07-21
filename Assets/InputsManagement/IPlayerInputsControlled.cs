using UnityEngine.InputSystem;

namespace InputsManagement
{
    public interface IPlayerInputsControlled
    {
        public void ReceiveMovementUpInput(InputAction.CallbackContext context);
        public void ReceiveMovementDownInput(InputAction.CallbackContext context);
        public void ReceiveMovementLeftInput(InputAction.CallbackContext context);
        public void ReceiveMovementRightInput(InputAction.CallbackContext context);
        public void ReceiveAInput(InputAction.CallbackContext context);
        public void ReceiveBInput(InputAction.CallbackContext context);
        public void ReceiveStartInput(InputAction.CallbackContext context);
        public void ReceiveSelectInput(InputAction.CallbackContext context);
    }
}