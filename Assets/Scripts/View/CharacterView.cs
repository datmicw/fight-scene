using UnityEngine;

// yêu cầu component animator phải có trên gameobject này
[RequireComponent(typeof(Animator))]
public class CharacterView : MonoBehaviour
{
    // thuộc tính animator, chỉ cho phép đọc bên ngoài
    public Animator Animator { get; private set; }

    // hàm awake, lấy component animator khi khởi tạo
    private void Awake() => Animator = GetComponent<Animator>();

    // đặt trạng thái đi bộ cho animator
    public void SetWalking(bool walking) => Animator.SetBool("isWalking", walking);

    // kích hoạt trigger đấm thường
    public void TriggerPunch() => Animator.SetTrigger("Punching");

    // kích hoạt trigger đấm vào đầu
    public void SetHeadPunch() => Animator.SetTrigger("HeadPunch");

    // bật hoặc tắt gameobject
    public void SetActive(bool active) => gameObject.SetActive(active);

    // xoay nhân vật về hướng chỉ định
    public void FaceDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5f);
    }
}