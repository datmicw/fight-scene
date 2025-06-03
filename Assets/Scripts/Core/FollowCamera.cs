using UnityEngine;

// lớp này dùng để camera đi theo một đối tượng (target)
public class FollowCamera : MonoBehaviour
{
    public Transform target; // đối tượng mà camera sẽ theo dõi
    public Vector3 offset = new Vector3(0, 5, -6); // khoảng cách giữa camera và target
    public float smoothSpeed = 10f; // tốc độ làm mượt chuyển động camera

    void LateUpdate()
    {
        if (target == null) return; // nếu không có target thì không làm gì
        
        // tính vị trí mong muốn của camera dựa trên vị trí của target và offset đã xoay theo hướng của target
        Vector3 desiredPosition = target.position + Quaternion.Euler(0, target.eulerAngles.y, 0) * offset;
        // di chuyển camera từ vị trí hiện tại đến vị trí mong muốn một cách mượt mà
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        // xác định hướng nhìn của camera, nhìn về phía target và cao hơn 1.5 đơn vị
        Vector3 lookDirection = (target.position + Vector3.up * 1.5f) - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f) // kiểm tra xem hướng nhìn có khác biệt không
        {
            // xoay camera để nhìn về phía target
            transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }
}
