using System.Collections;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -6);
    public float smoothSpeed = 10f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;

    private Vector3 originalOffset;
    private bool isZooming = false;

    void Start()
    {
        originalOffset = offset;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 currentOffset = isZooming ? offset : originalOffset;

        if (shakeDuration > 0)
        {
            currentOffset += Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
        }

        Vector3 desiredPosition = target.position + Quaternion.Euler(0, target.eulerAngles.y, 0) * currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        Vector3 lookDirection = (target.position + Vector3.up * 1.5f) - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void TriggerZoom(float zoomMultiplier = 0.7f, float duration = 0.5f)
    {
        if (isZooming) return;
        StartCoroutine(ZoomInOut(zoomMultiplier, duration));
    }

    private IEnumerator ZoomInOut(float zoomMultiplier, float duration)
    {
        isZooming = true;
        Vector3 zoomedOffset = originalOffset * zoomMultiplier;

        // Zoom in
        float t = 0f;
        while (t < 1f)
        {
            offset = Vector3.Lerp(originalOffset, zoomedOffset, t);
            t += Time.deltaTime / (duration / 2f);
            yield return null;
        }

        // Wait briefly
        yield return new WaitForSeconds(0.1f);

        // Zoom out
        t = 0f;
        while (t < 1f)
        {
            offset = Vector3.Lerp(zoomedOffset, originalOffset, t);
            t += Time.deltaTime / (duration / 2f);
            yield return null;
        }

        offset = originalOffset;
        isZooming = false;
    }
}
