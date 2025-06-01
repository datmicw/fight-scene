using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    private IInputProvider input;
    public float mouseSensitivity = 100f;  // Thêm tốc độ chuột
    public float punchDuration = 0.6f;
    [SerializeField] public int punchDamage = 10;
    [SerializeField] private float moveSpeedMultiplier = 0.2f;

    private CharacterController characterMover;

    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<CharacterController>();
        input = GetComponent<IInputProvider>();
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
        float mouseX = input.GetMouseX();
        if (Mathf.Abs(mouseX) > 0.01f)
        {
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);
        }
    }

    private void EndPunch() => isPunching = false;

    public void SetSpeedMultiplier(float multiplier) => moveSpeedMultiplier = multiplier;
    private void HandleMovement()
    {
        float move = input.GetMoveInput();
        if (move > 0.1f)
        {
            Vector3 direction = transform.forward;
            float speed = model.MoveSpeed * moveSpeedMultiplier;
            characterMover.Move(direction * speed * Time.deltaTime);
            view.FaceDirection(direction);
            view.SetWalking(true);
        }
        else view.SetWalking(false);
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
