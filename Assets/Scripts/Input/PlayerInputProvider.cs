using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    public float GetMoveInput()
    {
        return Input.GetAxis("Vertical");
    }

    public float GetMouseX()
    {
        return Input.GetAxis("Mouse X");
    }
}
