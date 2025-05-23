using UnityEngine;

// lớp cơ sở cho controller của nhân vật
public abstract class CharacterControllerBase : MonoBehaviour
{
    protected CharacterModel model; // dữ liệu nhân vật
    protected CharacterView view;   // hiển thị nhân vật

    protected float lastAttackTime; // thời gian tấn công cuối cùng
    protected bool isPunching;      // trạng thái đang đấm
    public CharacterModel Model => model; // trả về model

    // hàm khởi tạo, lấy view từ component
    protected virtual void Awake()
    {
        view = GetComponent<CharacterView>();
    }

    // khởi tạo model với các chỉ số
    public virtual void InitializeModel(float health, float speed, float damage, float cooldown)
    {
        model = new CharacterModel(health, speed, damage, cooldown);
    }

    // nhận sát thương
    public virtual void TakeDamage(float damage)
    {
        if (model == null) return;

        model.TakeDamage(damage);

        // nếu chết thì gọi Die
        if (!model.IsAlive())
        {
            Die();
        }
    }

    // xử lý khi chết
    protected virtual void Die()
    {
        view.SetActive(false); // ẩn view
        gameObject.SetActive(false); // ẩn object, dùng object pooling

        // nếu là enemy thì báo cho GameModeManager
        if (CompareTag("Enemy"))
        {
            var manager = FindObjectOfType<GameModeManager>();
            // gọi hàm khi enemy bị giết
            if (manager != null)
                manager.OnEnemyKilled(gameObject);
        }

        // nếu là player thì báo cho GameModeManager
        if (CompareTag("Player"))
        {
            var manager = FindObjectOfType<GameModeManager>();
            // gọi hàm khi player bị giết
            if (manager != null)
                manager.OnPlayerKilled(gameObject);
        }
    }

    // reset máu khi bắt đầu level mới
    public void ResetHealth()
    {
        if (model == null) return;

        model.Health = model.MaxHealth;
        if (view != null)
            view.SetActive(true);
    }

    // trả về chuỗi máu hiện tại
    internal string GetHealth() => model != null ? $"{model.Health}/{model.MaxHealth}" : "No Model";

    // kiểm tra còn sống không
    public bool IsAlive() => model != null && model.IsAlive();

    // set trạng thái đấm
    public void SetPunching(bool value) => isPunching = value;
    // kiểm tra có đang đấm không
    public bool IsPunching() => isPunching;
}
