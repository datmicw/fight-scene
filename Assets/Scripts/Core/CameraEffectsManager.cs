using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    public static CameraEffectsManager Instance { get; private set; }

    private FollowCamera followCam;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        followCam = FindObjectOfType<FollowCamera>();
    }

    public void Shake(float duration = 1f, float magnitude = 1f)
    {
        if (followCam != null)
        {
            followCam.TriggerShake(duration, magnitude);
        }
    }

    public void Zoom(float zoomMultiplier = 0.7f, float duration = 0.5f)
    {
        if (followCam != null)
        {
            followCam.TriggerZoom(zoomMultiplier, duration);
        }
    }
}
