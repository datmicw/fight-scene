using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate() // lateupdate được gọi sau update, dùng để xử lý camera
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (!target) return;
        var desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target, Vector3.up);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
