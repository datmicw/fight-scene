using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    public float Vertical { get; private set; }
    public float MouseX { get; private set; }

    void Update()
    {
        Vertical = Input.GetAxis("Vertical"); // W/S
        MouseX = Input.GetAxis("Mouse X");     // chuột ngang
    }
}
