using UnityEngine;

// lớp điều khiển người chơi kế thừa từ CharacterControllerBase
public class PlayerController : CharacterControllerBase
{
    private IInputProvider input; // lưu input provider

    public float mouseSensitivity = 100f; // độ nhạy chuột
    public float punchDuration = 0.6f; // thời gian ra đòn

    // thuộc tính punchDamage không cần [SerializeField] vì đã là public
    public int punchDamage = 10;
    [SerializeField] private float moveSpeedMultiplier = 0.2f; // hệ số tốc độ di chuyển

    private CharacterController characterMover; // component di chuyển

    // trạng thái tấn công
    private enum AttackState { None, Punch, HeadPunch }
    private AttackState attackState = AttackState.None;

    // hàm khởi tạo
    protected override void Awake()
    {
        base.Awake();
        characterMover = GetComponent<CharacterController>();
        input = GetComponent<IInputProvider>();

        if (input == null) Debug.LogError("IInputProvider not found.");
        if (view == null) Debug.LogError("View not assigned.");

        InitializeModel(100, 5, punchDamage, 1); // khởi tạo model với máu, tốc độ, damage, cooldown
        Debug.Log($"[Player] Initialized: HP={model.Health}, Damage={model.AttackDamage}");

    }

    // hàm update mỗi frame
    private void Update()
    {
        if (!model.IsAlive() || attackState != AttackState.None) return;

        HandleRotation(); // xử lý xoay
        HandleMovement(); // xử lý di chuyển
        HandleInput(); // xử lý input tấn công thường
        HandleHeadPunchInput(); // xử lý input tấn công đặc biệt
    }

    // xử lý xoay theo chuột
    private void HandleRotation()
    {
        float mouseX = input.GetMouseX();
        if (Mathf.Abs(mouseX) > 0.01f)
        {
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);
        }
    }

    // xử lý di chuyển
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

    // xử lý input tấn công thường
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

    // xử lý input tấn công đặc biệt (head punch)
    private void HandleHeadPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastAttackTime > model.AttackCooldown)
        {
            StartHeadPunch();
        }
    }

    // bắt đầu tấn công thường
    private void StartPunch()
    {
        attackState = AttackState.Punch;
        view.TriggerPunch();
        view.SetWalking(false);
        lastAttackTime = Time.time;
        Invoke(nameof(EndPunch), punchDuration);
    }

    // bắt đầu tấn công đặc biệt
    private void StartHeadPunch()
    {
        attackState = AttackState.HeadPunch;
        view.SetHeadPunch();
        view.SetWalking(false);
        lastAttackTime = Time.time;
        Invoke(nameof(EndHeadPunch), punchDuration);
    }

    // kết thúc tấn công thường
    private void EndPunch() => attackState = AttackState.None;
    // kết thúc tấn công đặc biệt
    private void EndHeadPunch() => attackState = AttackState.None;

    // hàm gây sát thương cho enemy trong bán kính
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
