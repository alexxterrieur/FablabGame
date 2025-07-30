using UnityEngine;
using UnityEngine.UIElements;

public class ProjectorMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Transform _transform;
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        _transform = transform;
    }

    public void OnMove(Vector2 dir)
    {
        moveInput = dir;
    }

    private void Update()
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x + moveInput.x * Time.fixedDeltaTime * moveSpeed, _transform.localPosition.y + moveInput.y * Time.fixedDeltaTime * moveSpeed, _transform.localPosition.z );
    }
}
