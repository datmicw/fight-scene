using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 input;
    public Vector2 Direction => input;

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        // kiểm tra xem có kéo không
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform, // chuyển đổi
            eventData.position, // vị trí kéo
            eventData.pressEventCamera, // camera
            out Vector2 pos)) // kiểm tra xem có kéo không
        {
            input = Vector2.ClampMagnitude(pos / 100f, 1f); //100f là bán kính của joystick, 1f là độ lớn tối đa
        }
    }

    public void OnPointerUp(PointerEventData eventData) => input = Vector2.zero;
}
