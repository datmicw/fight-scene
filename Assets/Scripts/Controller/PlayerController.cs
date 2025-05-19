using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    public FloatingJoystick joystick;
    public float punchDuration = 0.6f;
    private UnityEngine.CharacterController characterMover;

    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<UnityEngine.CharacterController>();
        joystick = FindObjectOfType<FloatingJoystick>();
        InitializeModel(100, 5, 10, 1);
    }

    private void Update()
    {
        if (!model.IsAlive() || isPunching) return;

        HandleMovement();
        HandleInput();
    }

    private void HandleMovement()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        bool walking = input.sqrMagnitude > 0.01f;
        view.SetWalking(walking);

        if (walking)
        {
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = Camera.main.transform.TransformDirection(move);
            move.y = 0;
            move.Normalize();

            characterMover.Move(move * model.MoveSpeed * Time.deltaTime);
            view.FaceDirection(move);
        }
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > model.AttackCooldown)
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Time.time - lastAttackTime > model.AttackCooldown)
#endif
        {
            StartPunch();
        }
    }

    private void StartPunch()
    {
        isPunching = true;
        view.TriggerPunch();
        view.SetWalking(false);
        lastAttackTime = Time.time;
        Invoke(nameof(EndPunch), punchDuration);
    }

    private void EndPunch()
    {
        isPunching = false;
    }

    public override void Attack(CharacterControllerBase target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
        {
            target.TakeDamage(model.AttackDamage);
        }
    }
}
