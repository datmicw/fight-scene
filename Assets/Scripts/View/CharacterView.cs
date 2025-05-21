using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    public void SetWalking(bool walking)
    {
        Animator.SetBool("isWalking", walking);
    }

    public void SetEnemyWalking(bool walking)
    {
        Animator.SetBool("isEnemyWalking", walking);
    }

    public void TriggerPunch()
    {
        Animator.SetTrigger("Punching");
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public void FaceDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5f);
    }
}