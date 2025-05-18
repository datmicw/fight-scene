using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Character Stats")]
    public float health = 100f;
    public float moveSpeed = 5f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    protected CharacterController characterController;
    protected Animator animator;
    protected bool isAlive = true;
    protected float lastAttackTime;

    protected virtual void Awake()
    {
        TryGetComponent(out characterController);
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        // animator.SetTrigger("Die");
        gameObject.SetActive(false); // Sử dụng object pooling
    }

    public abstract void Attack(CharacterBase target);
}