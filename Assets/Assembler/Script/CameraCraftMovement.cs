using UnityEngine;

public class CameraCraftMovement : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    private Camera camera;

    [Header("Paramètres")]
    public float distanceToPlayer;
    public float transitionSpeed;
    public float transitionbackSpeed;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool movingToCraft = false;
    private bool returning = false;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        camera = GetComponent<Camera>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (movingToCraft)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
                movingToCraft = false;
        }
        else if (returning)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * transitionbackSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * transitionbackSpeed);

            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
                returning = false;
        }
    }

    public void MoveToCraftView()
    {
        if (player == null)
            return;
        

        camera.orthographic = false;

        Vector3 flatForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        targetPosition = player.position - flatForward * distanceToPlayer;

        Vector3 newEuler = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        targetRotation = Quaternion.Euler(newEuler);

        movingToCraft = true;
        returning = false;
    }


    public void ReturnToOriginalView()
    {
        camera.orthographic = true;

        returning = true;
        movingToCraft = false;
    }
}
