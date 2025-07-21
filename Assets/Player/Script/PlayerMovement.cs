using UnityEngine;

namespace Player.Script
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;

        private Vector2 moveInput;
        private Vector3 direction;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetDirection(Vector2 input)
        {
            moveInput += input;
            
            direction.Set(moveInput.x, 0f, moveInput.y);
        }
        
        public void ResetDirection()
        {
            SetDirection(-moveInput);
        }

        private void FixedUpdate()
        {
            direction.Set(moveInput.x, 0f, moveInput.y);

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);

                rb.MovePosition(rb.position + direction.normalized * (moveSpeed * Time.fixedDeltaTime));
            } 
        }
    }

}