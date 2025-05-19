using UnityEngine;

public class CharacterModel
{
    public float Health { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackDamage { get; private set; }
    public float AttackCooldown { get; private set; }

    public CharacterModel(float health, float moveSpeed, float attackDamage, float attackCooldown)
    {
        Health = health;
        MoveSpeed = moveSpeed;
        AttackDamage = attackDamage;
        AttackCooldown = attackCooldown;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
    }
}