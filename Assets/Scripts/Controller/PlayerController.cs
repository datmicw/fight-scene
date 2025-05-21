using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    public FloatingJoystick joystick;
    public float punchDuration = 0.6f;
    [SerializeField] public int punchDamage = 10;

    [SerializeField] private float moveSpeedMultiplier = 1f;

    private CharacterController characterMover;

    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<CharacterController>();
        joystick = FindObjectOfType<FloatingJoystick>();
        InitializeModel(100, 5, punchDamage, 1); // máu, tốc độ, sát thương, thời gian hồi chiêu
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
            float currentSpeed = model.MoveSpeed * moveSpeedMultiplier;

            characterMover.Move(move * currentSpeed * Time.deltaTime);
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

    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<CharacterControllerBase>();
                if (enemy != null && enemy != this)
                {
                    enemy.TakeDamage(model.AttackDamage);
                    Debug.Log("Attack enemy: " + enemy.name);
                    Debug.Log("Enemy nhận damage: " + model.AttackDamage);
                    Debug.Log("Enemy còn lại: " + enemy.GetHealth());
                }
            }
        }
    }
    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier = multiplier;
    }
}

