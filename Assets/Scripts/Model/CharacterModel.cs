using UnityEngine;

public class CharacterModel : ICharacterModel
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
        Health = Mathf.Max(Health - damage, 0);
    }

    public bool IsAlive() => Health > 0;
}
