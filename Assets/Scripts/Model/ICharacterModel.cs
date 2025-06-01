public interface ICharacterModel
{
    float MaxHealth { get; }
    float Health { get; set; }
    float MoveSpeed { get; set; }
    float AttackDamage { get; set; }
    float AttackCooldown { get; }

    void TakeDamage(float damage);
    bool IsAlive();
}
