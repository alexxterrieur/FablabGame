using System.Collections;
using UnityEngine;

public class CameraCraftMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCam;
    public Transform player;
    public float zoomDuration = 1.5f;
    public float zoomedInFOV = 40f;
    public float zoomedOutFOV = 5f;
    public float distanceFromPlayer = 3f;
    public Vector3 zoomOffset = Vector3.up * 1f;

    public bool isZooming = false;

    private Quaternion originalRotation;
    private float originalFOV;

    private CameraMovement camMovement;

    private void Start()
    {
        originalRotation = mainCam.transform.rotation;
        originalFOV = mainCam.fieldOfView;
        camMovement = GetComponent<CameraMovement>();
    }

    public void ZoomToPlayer()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomInCoroutine());
        }
    }

    public void ZoomOut()
    {
        if (!isZooming)
        {
            Vector3 targetPos = camMovement != null
                ? camMovement.GetTargetCameraPosition()
                : mainCam.transform.position;

            StartCoroutine(ZoomOutCoroutine(targetPos));
        }
    }

    private IEnumerator ZoomInCoroutine()
    {
        isZooming = true;
        if (camMovement != null) camMovement.enabled = false;

        Vector3 startPos = mainCam.transform.position;
        float startFOV = mainCam.fieldOfView;

        Vector3 direction = (mainCam.transform.position - player.position).normalized;
        Vector3 targetPos = player.position + direction * distanceFromPlayer + zoomOffset;

        float time = 0f;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            mainCam.fieldOfView = Mathf.Lerp(startFOV, zoomedInFOV, smoothT);

            time += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.fieldOfView = zoomedInFOV;

        isZooming = false;
    }

    private IEnumerator ZoomOutCoroutine(Vector3 targetPos)
    {
        isZooming = true;
        if (camMovement != null) camMovement.enabled = false;

        Vector3 startPos = mainCam.transform.position;
        float startFOV = mainCam.fieldOfView;

        float time = 0f;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            mainCam.fieldOfView = Mathf.Lerp(startFOV, zoomedOutFOV, smoothT);

            time += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.fieldOfView = zoomedOutFOV;

        if (camMovement != null) camMovement.enabled = true;
        isZooming = false;
    }

    // Debug input (optionnel)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ZoomToPlayer();
        if (Input.GetKeyDown(KeyCode.X)) ZoomOut();
    }
}
