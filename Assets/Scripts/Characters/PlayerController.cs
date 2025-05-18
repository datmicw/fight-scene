using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : CharacterBase
{
    [Header("References")]
    public FloatingJoystick joystick;

    [Header("Settings")]
    public float punchDuration = 0.6f;
    private bool isPunching;
    protected override void Awake()
    {
        base.Awake();
        if (!joystick)
        {
            joystick = FindObjectOfType<FloatingJoystick>();
            if (!joystick)
                Debug.LogWarning("No FloatingJoystick found in scene!");
        }
    }

    void Update()
    {
        if (!isAlive || isPunching) return;

        HandleMovement();
        HandlePunchInput();
    }

    void HandleMovement()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        bool walking = input.sqrMagnitude > 0.01f;

        if (animator.GetBool("isWalking") != walking)
            animator.SetBool("isWalking", walking);

        if (walking)
        {
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = Camera.main.transform.TransformDirection(move);
            move.y = 0;
            move.Normalize();

            characterController.Move(move * moveSpeed * Time.deltaTime);
            transform.forward = move;
        }
    }

    void HandlePunchInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackCooldown)
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Time.time - lastAttackTime > attackCooldown)
#endif
        {
            TriggerPunch();
        }
    }

    void TriggerPunch()
    {
        isPunching = true;
        animator.SetTrigger("Punching");
        animator.SetBool("isWalking", false);
        lastAttackTime = Time.time;
        Invoke(nameof(EndPunch), punchDuration);
    }

    public override void Attack(CharacterBase target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
        {
            target.TakeDamage(attackDamage);
        }
    }

    void EndPunch()
    {
        isPunching = false;
    }
}