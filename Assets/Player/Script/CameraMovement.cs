using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float playerZMin = 0f;
    [SerializeField] private float playerZMax = 8f;
    [SerializeField] private float camZMin = -19f;
    [SerializeField] private float camZMax = -12.7f;

    private float fixedX;
    private float fixedY;

    private CameraCraftMovement cameraCraftMovement;

    private void Start()
    {
        fixedX = transform.position.x;
        fixedY = transform.position.y;

        cameraCraftMovement = GetComponent<CameraCraftMovement>();
    }

    private void LateUpdate()
    {
        if (cameraCraftMovement != null && cameraCraftMovement.isZooming)
        {
            return; // Bloquer le mouvement auto pendant un zoom
        }

        transform.position = GetTargetCameraPosition();
    }

    public Vector3 GetTargetCameraPosition()
    {
        float currentPlayerZ = target.position.z;
        float clampedPlayerZ = Mathf.Clamp(currentPlayerZ, playerZMin, playerZMax);
        float t = (clampedPlayerZ - playerZMin) / (playerZMax - playerZMin);
        float targetCamZ = Mathf.Lerp(camZMin, camZMax, t);

        return new Vector3(fixedX, fixedY, targetCamZ);
    }
}
