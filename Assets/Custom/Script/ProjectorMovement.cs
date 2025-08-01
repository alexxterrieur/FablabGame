using UnityEngine;
using UnityEngine.UIElements;

public class ProjectorMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Transform _transform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float MaxMove = 7.3f;

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
        Vector3 nextPos = new Vector3(
        _transform.localPosition.x + moveInput.x * Time.fixedDeltaTime * moveSpeed,
        _transform.localPosition.y + moveInput.y * Time.fixedDeltaTime * moveSpeed,
        _transform.localPosition.z);

        if (Mathf.Abs(nextPos.x) > MaxMove)
        {
            nextPos.x = _transform.localPosition.x;
        }

        if (Mathf.Abs(nextPos.y) > MaxMove)
        {
            nextPos.y = _transform.localPosition.y;
        }
        _transform.localPosition = nextPos;  
    }
}
