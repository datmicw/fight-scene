using UnityEngine;

// bắt buộc phải có component CharacterControllerBase trên object này
[RequireComponent(typeof(CharacterControllerBase))]
public class SimpleAIController : MonoBehaviour
{
    // tham chiếu đến controller của chính mình
    private CharacterControllerBase self;
    // tham chiếu đến view để điều khiển hoạt ảnh
    private CharacterView view;
    // mục tiêu hiện tại mà ai sẽ tấn công
    private CharacterControllerBase currentTarget;

    // khoảng cách dừng lại khi đến gần mục tiêu
    [SerializeField] private float stopDistance = 1.5f;
    // thời gian thực hiện một cú đấm
    [SerializeField] private float punchDuration = 0.6f;

    // thời gian lần tấn công cuối cùng
    private float lastAttackTime;

    // khởi tạo các tham chiếu
    private void Awake()
    {
        self = GetComponent<CharacterControllerBase>();
        view = GetComponentInChildren<CharacterView>();
    }

    // lặp lại việc cập nhật mục tiêu mỗi 0.5 giây
    private void Start() => InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);

    // kết thúc trạng thái đấm
    private void EndPunch() => self.SetPunching(false);

    // cập nhật mục tiêu gần nhất
    private void UpdateTarget() => currentTarget = FindClosestEnemy();

    private void Update()
    {
        // nếu đã chết thì không làm gì cả
        if (!self.IsAlive()) return;

        // nếu không có mục tiêu hoặc mục tiêu đã chết thì dừng đi bộ
        if (currentTarget == null || !currentTarget.IsAlive())
        {
            view.SetWalking(false);
            return;
        }

        // tính khoảng cách và hướng đến mục tiêu
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;

        // nếu còn xa mục tiêu thì di chuyển lại gần
        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, self.Model.MoveSpeed * Time.deltaTime);
            view.FaceDirection(dir);
            view.SetWalking(true);
        }
        else
        {
            // nếu đã đến gần thì dừng lại và thử tấn công
            view.SetWalking(false);
            TryAttack();
        }
    }

    // thử thực hiện tấn công nếu đủ điều kiện
    private void TryAttack()
    {
        // kiểm tra cooldown và trạng thái đấm
        if (Time.time - lastAttackTime < self.Model.AttackCooldown || self.IsPunching()) return;

        lastAttackTime = Time.time;
        self.SetPunching(true);
        view.TriggerPunch();
        // gây sát thương sau một nửa thời gian đấm
        Invoke(nameof(DealDamage), punchDuration * 0.5f);
        // kết thúc trạng thái đấm sau khi hết thời gian đấm
        Invoke(nameof(EndPunch), punchDuration);
    }

    // gây sát thương cho mục tiêu nếu còn sống
    private void DealDamage()
    {
        if (currentTarget != null && currentTarget.IsAlive())
        {
            currentTarget.TakeDamage(self.Model.AttackDamage);
        }
    }

    // tìm kẻ địch gần nhất dựa trên tag
    private CharacterControllerBase FindClosestEnemy()
    {
        // nếu là player thì tìm enemy, ngược lại tìm player
        string targetTag = gameObject.CompareTag("Player") ? "Enemy" : "Player";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        float minDistance = float.MaxValue;
        CharacterControllerBase closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            CharacterControllerBase ctrl = enemy.GetComponent<CharacterControllerBase>();
            if (ctrl != null && ctrl.IsAlive())
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = ctrl;
                }
            }
        }
        return closest;
    }
}
