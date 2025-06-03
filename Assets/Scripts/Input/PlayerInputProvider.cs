using UnityEngine;

// lớp này cung cấp input cho người chơi
public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    // lấy giá trị di chuyển theo trục dọc (vertical)
    public float GetMoveInput()
    {
        return Input.GetAxis("Vertical");
    }

    // lấy giá trị di chuyển của chuột theo trục X
    public float GetMouseX()
    {
        return Input.GetAxis("Mouse X");
    }
}
