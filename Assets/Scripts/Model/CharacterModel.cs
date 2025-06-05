using UnityEngine;

// lớp characterModel đại diện cho mô hình nhân vật
public class CharacterModel : ICharacterModel
{
    // máu tối đa của nhân vật
    public float MaxHealth { get; private set; }
    // máu hiện tại của nhân vật
    public float Health { get; set; }
    // tốc độ di chuyển của nhân vật
    public float MoveSpeed { get; set; }
    // sát thương tấn công của nhân vật
    public float AttackDamage { get; set; }
    // thời gian hồi chiêu tấn công
    public float AttackCooldown { get; private set; }

    // hàm khởi tạo nhân vật với các thuộc tính
    public CharacterModel(float health, float speed, float damage, float cooldown)
    {
        MaxHealth = health;
        Health = health;
        MoveSpeed = speed;
        AttackDamage = damage;
        AttackCooldown = cooldown;
    }

    // hàm xử lý khi nhân vật nhận sát thương
    public void TakeDamage(float damage)
    {
        Health = Mathf.Max(Health - damage, 0);
        
    }
    
    // kiểm tra nhân vật còn sống hay không
    public bool IsAlive() => Health > 0;
}
