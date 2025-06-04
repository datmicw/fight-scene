using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    private IInputProvider input;
    public float mouseSensitivity = 100f;
    public float punchDuration = 0.6f;
    public int punchDamage = 10;
    [SerializeField] private float moveSpeedMultiplier = 0.2f;
    private CharacterController characterMover;

    private enum AttackState { None, Punch, HeadPunch }
    private AttackState attackState = AttackState.None;
    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<CharacterController>();
        input = GetComponent<IInputProvider>();

        if (input == null) Debug.LogError("IInputProvider not found.");
        if (view == null) Debug.LogError("View not assigned.");

        InitializeModel(100, 5, punchDamage, 1);
        Debug.Log($"[Player] Initialized: HP={model.Health}, Damage={model.AttackDamage}");
    }
    private void Update()
    {
        if (!model.IsAlive() || attackState != AttackState.None) return;

        HandleRotation();
        HandleMovement();
        HandleInput();
        HandleHeadPunchInput();
    }
    private void HandleRotation()
    {
        float mouseX = input.GetMouseX();
        if (Mathf.Abs(mouseX) > 0.01f)
        {
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);
        }
    }
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
    private void HandleHeadPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastAttackTime > model.AttackCooldown)
        {
            StartHeadPunch();
        }
    }
    private void StartPunch()
    {
        attackState = AttackState.Punch;
        view.TriggerPunch();
        view.SetWalking(false);
        lastAttackTime = Time.time;
        CameraEffectsManager.Instance.Shake();
        Invoke(nameof(EndPunch), punchDuration);
    }

    private void StartHeadPunch()
    {
        attackState = AttackState.HeadPunch;
        view.SetHeadPunch();
        view.SetWalking(false);
        lastAttackTime = Time.time;
        CameraEffectsManager.Instance.Zoom();
        Invoke(nameof(EndHeadPunch), punchDuration);
    }
    private void EndPunch() => attackState = AttackState.None;
    private void EndHeadPunch() => attackState = AttackState.None;
    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 toEnemy = (hit.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, toEnemy) > 0.5f)
                {
                    var enemy = hit.GetComponent<CharacterControllerBase>();
                    if (enemy != null && enemy != this)
                    {
                        enemy.TakeDamage(model.AttackDamage);
                        Debug.Log($"Tấn công enemy: {enemy.name} - Sát thương: {model.AttackDamage}");
                    }
                }
            }
        }
    }
}
