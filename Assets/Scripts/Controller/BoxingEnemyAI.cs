using UnityEngine;

// Yêu cầu component Rigidbody phải có trên object
[RequireComponent(typeof(Rigidbody))]
public class BoxingEnemyAI : CharacterControllerBase
{
    private Transform player; // Lưu transform của player
    private Rigidbody rb;     // Lưu rigidbody của enemy

    [Header("Boxing Settings")]
    [SerializeField] private float attackRange = 1f; // Khoảng cách tấn công
    [SerializeField] private float moveSpeed = 2f;   // Tốc độ di chuyển

    private bool hasDealtDamageThisPunch = false;    // Đã gây damage trong đòn này chưa

    // Khởi tạo các thành phần cần thiết
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        view = GetComponent<CharacterView>();
        InitializeModel(100, 5, 5, 1); // máu, tốc độ, sát thương, cooldown
    }

    private void Start()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.Player != null)
        {
            player = PlayerManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogError("Player không tồn tại trong PlayerManager.");
        }
    }

    private void FixedUpdate()
    {
        if (player == null || !IsAlive()) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        float distance = direction.magnitude;
        view.FaceDirection(direction);

        if (distance > attackRange)
        {
            if (!isPunching)
            {
                MoveTowards(direction.normalized);
                view.SetWalking(true);
            }
        }
        else
        {
            view.SetWalking(false);
            TryAttack();
        }
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 move = direction * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + move;
        rb.MovePosition(newPosition);
    }

    // Gọi khi đủ điều kiện tấn công
    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= model.AttackCooldown && !isPunching)
        {
            lastAttackTime = Time.time;
            isPunching = true;
            hasDealtDamageThisPunch = false;

            view.TriggerPunch(); // gọi animation đấm, animation sẽ gọi DealDamage()
            Invoke(nameof(EndPunch), 0.6f); // kết thúc đấm sau thời gian phù hợp với animation
        }
    }

    // Hàm kết thúc đấm (reset lại trạng thái)
    private void EndPunch()
    {
        isPunching = false;
        hasDealtDamageThisPunch = false;
    }

    // Gây sát thương nếu player trong phạm vi và ở trước mặt
    // Được gọi qua Animation Event
    private void DealDamage()
    {
        if (!isPunching || hasDealtDamageThisPunch) return;

        hasDealtDamageThisPunch = true;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 toPlayer = (hit.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, toPlayer) > 0.5f)
                {
                    var playerCtrl = hit.GetComponent<CharacterControllerBase>();
                    if (playerCtrl != null && playerCtrl != this && playerCtrl.IsAlive())
                    {
                        playerCtrl.TakeDamage(model.AttackDamage);
                        Debug.Log($"Tấn công player: {playerCtrl.name} - Sát thương: {model.AttackDamage}");
                    }
                }
            }
        }
    }

    // Debug hình cầu va chạm trong Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
