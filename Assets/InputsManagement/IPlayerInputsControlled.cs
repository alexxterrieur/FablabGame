using System;
using UnityEngine;
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

        public void ResetInputs();
    }

    public abstract class Controller : MonoBehaviour, IPlayerInputsControlled
    {
        public void ReceiveAInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveBInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMovementDownInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMovementLeftInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMovementRightInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMovementUpInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveSelectInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ReceiveStartInput(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ResetInputs()
        {
            throw new NotImplementedException();
        }
    }
}