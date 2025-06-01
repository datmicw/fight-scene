using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    public MovementPlayer movement;
    public float mouseSensitivity = 100f;  // Thêm tốc độ chuột
    public float punchDuration = 0.6f;
    [SerializeField] public int punchDamage = 10;
    [SerializeField] private float moveSpeedMultiplier = 0.2f;

    private CharacterController characterMover;

    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<CharacterController>();
        movement = FindObjectOfType<MovementPlayer>();
        InitializeModel(100, 5, punchDamage, 1);
    }

    private void Update()
    {
        if (!model.IsAlive() || isPunching) return;

        HandleRotation();
        HandleMovement();
        HandleInput();
    }

    private void HandleRotation()
    {
        if (Mathf.Abs(movement.MouseX) > 0.01f)
        {
            transform.Rotate(Vector3.up * movement.MouseX * mouseSensitivity * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        if (movement.Vertical > 0.1f)
        {
            Vector3 move = transform.forward;
            float currentSpeed = model.MoveSpeed * moveSpeedMultiplier;

            characterMover.Move(move * currentSpeed * Time.deltaTime);
            view.FaceDirection(move);
            view.SetWalking(true);
        }
        else
        {
            view.SetWalking(false);
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

    private void EndPunch() => isPunching = false;

    public void SetSpeedMultiplier(float multiplier) => moveSpeedMultiplier = multiplier;

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
                    Debug.Log("Tấn công enemy: " + enemy.name);
                }
            }
        }
    }
}
    