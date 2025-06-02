using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -6);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;
        
        // tính toán vị trí mong muốn của camera dựa trên vị trí của target và offset
        // Sử dụng Quaternion để xoay offset theo hướng của target
        Vector3 desiredPosition = target.position + Quaternion.Euler(0, target.eulerAngles.y, 0) * offset;
        // lerp vị trí camera từ vị trí hiện tại đến vị trí mong muốn
        // Sử dụng Vector3.Lerp để làm mượt chuyển động camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        Vector3 lookDirection = (target.position + Vector3.up * 1.5f) - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f) // kiểm tra xem hướng nhìn có khác biệt không
        {
            transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }
}
