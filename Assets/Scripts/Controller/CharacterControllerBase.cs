using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    protected CharacterModel model;
    [SerializeField] protected CharacterView view;

    protected float lastAttackTime;
    protected bool isPunching;
    protected bool isHeadPunching;
    public CharacterModel Model => model;

    protected virtual void Awake()
    {
        if (view == null) view = GetComponent<CharacterView>();
    }

    public virtual void InitializeModel(float health, float speed, float damage, float cooldown)
    {
        model = new CharacterModel(health, speed, damage, cooldown);
    }

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

        if (!model.IsAlive())
        {
            Debug.Log($"{gameObject.name} has died.");
            Die();
        }
    }

    protected virtual void Die()
    {
        view.SetActive(false);
        Debug.Log($"{gameObject.name} has died.");
    }

    public void ResetHealth()
    {
        if (model == null) return;
        model.Health = model.MaxHealth;
        if (view != null) view.SetActive(true);
    }

    internal string GetHealth() => model != null ? $"{model.Health}/{model.MaxHealth}" : "No Model";

    public bool IsAlive() => model != null && model.IsAlive();

    public void SetPunching(bool value) => isPunching = value;
    public bool IsPunching() => isPunching;
    public void SetHeadPunching(bool value) => isHeadPunching = value;
    public bool IsHeadPunching() => isHeadPunching;
}
