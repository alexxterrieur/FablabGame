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
    public Vector3 zoomOffset = Vector3.up * 1f; // Permet un léger décalage

    private bool isZooming = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalFOV;

    private void Start()
    {
        originalPosition = mainCam.transform.position;
        originalRotation = mainCam.transform.rotation;
        originalFOV = mainCam.fieldOfView;
    }

    public void ZoomToPlayer()
    {
        if (!isZooming)
        {
            Vector3 direction = (mainCam.transform.position - player.position).normalized;
            Vector3 targetPos = player.position + direction * distanceFromPlayer + zoomOffset;
            StartCoroutine(ZoomCoroutine(targetPos, originalRotation, zoomedInFOV));
        }
    }

    public void ZoomOut()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomCoroutine(originalPosition, originalRotation, originalFOV));
        }
    }

    private IEnumerator ZoomCoroutine(Vector3 targetPos, Quaternion fixedRot, float targetFOV)
    {
        isZooming = true;

        Vector3 startPos = mainCam.transform.position;
        float startFOV = mainCam.fieldOfView;

        float time = 0f;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;

            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCam.transform.rotation = fixedRot;
            mainCam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            time += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = fixedRot;
        mainCam.fieldOfView = targetFOV;

        isZooming = false;
    }
}
