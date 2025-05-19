using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    protected CharacterModel model;
    protected CharacterView view;

    protected float lastAttackTime;
    protected bool isPunching;

    protected virtual void Awake()
    {
        view = GetComponent<CharacterView>();
    }

    public virtual void InitializeModel(float health, float speed, float damage, float cooldown)
    {
        model = new CharacterModel(health, speed, damage, cooldown);
    }

    public virtual void TakeDamage(float amount)
    {
        model.TakeDamage(amount);
        if (!model.IsAlive())
            Die();
    }

    protected virtual void Die()
    {
        view.SetActive(false); // dùng object pooling
    }

    public abstract void Attack(CharacterControllerBase target);
}
