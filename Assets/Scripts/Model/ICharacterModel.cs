// giao diện cho mô hình nhân vật
public interface ICharacterModel
{
    // máu tối đa của nhân vật
    float MaxHealth { get; }
    // máu hiện tại của nhân vật
    float Health { get; set; }
    // tốc độ di chuyển của nhân vật
    float MoveSpeed { get; set; }
    // sát thương tấn công của nhân vật
    float AttackDamage { get; set; }
    // thời gian hồi chiêu tấn công
    float AttackCooldown { get; }

    // hàm nhận sát thương
    void TakeDamage(float damage);
    // kiểm tra nhân vật còn sống không
    bool IsAlive();
}
