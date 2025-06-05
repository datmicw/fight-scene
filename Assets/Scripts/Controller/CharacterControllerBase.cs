using UnityEngine;

// lớp cơ sở cho controller của nhân vật
public abstract class CharacterControllerBase : MonoBehaviour
{
    protected CharacterModel model; // lưu trữ model của nhân vật
    [SerializeField] protected CharacterView view; // tham chiếu đến view của nhân vật

    protected float lastAttackTime; // thời gian lần tấn công cuối
    protected bool isPunching; // trạng thái đang đấm
    protected bool isHeadPunching; // trạng thái đang đấm vào đầu
    public CharacterModel Model => model; // trả về model

    // hàm khởi tạo, gán view nếu chưa có
    protected virtual void Awake()
    {
        if (view == null) view = GetComponent<CharacterView>();
    }

    // khởi tạo model với các chỉ số
    public virtual void InitializeModel(float health, float speed, float damage, float cooldown)
    {
        model = new CharacterModel(health, speed, damage, cooldown);
    }

    // nhận sát thương
    public virtual void TakeDamage(float damage)
    {
        if (model == null)
        {
            Debug.LogError($"{gameObject.name} has no model.");
            return;
        }

        float before = model.Health;
        model.TakeDamage(damage);
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {before} → {model.Health}");

        // kiểm tra nếu đã chết thì gọi Die
        if (!model.IsAlive())
        {
            Debug.Log($"{gameObject.name} has died.");
            Die();
        }
    }

    // xử lý khi chết
    protected virtual void Die()
    {
        view.SetActive(false);
        Debug.Log($"{gameObject.name} has died.");
    }

    // hồi lại máu đầy
    public void ResetHealth()
    {
        if (model == null) return;
        model.Health = model.MaxHealth;
        if (view != null) view.SetActive(true);
    }

    // trả về chuỗi máu hiện tại
    internal string GetHealth() => model != null ? $"{model.Health}/{model.MaxHealth}" : "No Model";
    // trả về sát thương làm tròn
    public int GetDamage() => model != null ? Mathf.RoundToInt(model.AttackDamage) : 0;

    // kiểm tra còn sống không
    public bool IsAlive() => model != null && model.IsAlive();

    // set trạng thái đang đấm
    public void SetPunching(bool value) => isPunching = value;
    // kiểm tra có đang đấm không
    public bool IsPunching() => isPunching;
    // set trạng thái đang đấm đầu
    public void SetHeadPunching(bool value) => isHeadPunching = value;
    // kiểm tra có đang đấm đầu không
    public bool IsHeadPunching() => isHeadPunching;
}
