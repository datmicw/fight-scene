using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    protected CharacterModel model;
    protected CharacterView view;

    protected float lastAttackTime;
    protected bool isPunching;
    public CharacterModel Model => model;


    protected virtual void Awake()
    {
        view = GetComponent<CharacterView>();

    }


    public virtual void InitializeModel(float health, float speed, float damage, float cooldown)
    {
        model = new CharacterModel(health, speed, damage, cooldown);
    }

    public virtual void TakeDamage(float damage)
    {
        if (model == null) return;

        model.TakeDamage(damage);

        if (!model.IsAlive())
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log("Die() called");
        if (CompareTag("Enemy"))
        {
            var manager = FindObjectOfType<GameModeManager>();
            if (manager != null)
            {
                manager.OnEnemyKilled(gameObject);
            }
        }
        if (view != null)
            view.SetActive(false);
    }

    public void ResetHealth()
    {
        if (model == null) return;

        model.Health = model.MaxHealth;
        if (view != null)
            view.SetActive(true); 
    }

    internal string GetHealth() => model != null ? $"{model.Health}/{model.MaxHealth}" : "No Model";

    public bool IsAlive() => model != null && model.IsAlive();

    public void SetPunching(bool value) => isPunching = value;
    public bool IsPunching() => isPunching;
}
