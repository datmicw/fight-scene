public class CharacterModel
{
    public float MaxHealth { get; private set; }
    public float Health { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackDamage { get; set; }
    public float AttackCooldown { get; private set; }

    public CharacterModel(float health, float speed, float damage, float cooldown)
    {
        MaxHealth = health;
        Health = health;
        MoveSpeed = speed;
        AttackDamage = damage;
        AttackCooldown = cooldown;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = UnityEngine.Mathf.Max(Health, 0);
    }

    public bool IsAlive() => Health > 0;
}
