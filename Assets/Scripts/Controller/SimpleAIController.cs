using UnityEngine;

[RequireComponent(typeof(CharacterControllerBase))]
public class SimpleAIController : MonoBehaviour
{
    private CharacterControllerBase self;
    private CharacterView view;
    private CharacterControllerBase currentTarget;

    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float punchDuration = 0.6f;

    private float lastAttackTime;

    private void Awake()
    {
        self = GetComponent<CharacterControllerBase>();
        view = GetComponentInChildren<CharacterView>();
    }

    private void Start() => InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    private void EndPunch() => self.SetPunching(false);
    private void UpdateTarget() => currentTarget = FindClosestEnemy();
    private void Update()
    {
        if (!self.IsAlive()) return;

        if (currentTarget == null || !currentTarget.IsAlive())
        {
            view.SetWalking(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, self.Model.MoveSpeed * Time.deltaTime);
            view.FaceDirection(dir);
            view.SetWalking(true);
        }
        else
        {
            view.SetWalking(false);
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < self.Model.AttackCooldown || self.IsPunching()) return;

        lastAttackTime = Time.time;
        self.SetPunching(true);
        view.TriggerPunch();
        Invoke(nameof(DealDamage), punchDuration * 0.5f);
        Invoke(nameof(EndPunch), punchDuration);
    }

    private void DealDamage()
    {
        if (currentTarget != null && currentTarget.IsAlive())
        {
            currentTarget.TakeDamage(self.Model.AttackDamage);
        }
    }

    private CharacterControllerBase FindClosestEnemy()
    {
        string targetTag = gameObject.CompareTag("Player") ? "Enemy" : "Player";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        float minDistance = float.MaxValue;
        CharacterControllerBase closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            CharacterControllerBase ctrl = enemy.GetComponent<CharacterControllerBase>();
            if (ctrl != null && ctrl.IsAlive())
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = ctrl;
                }
            }
        }
        return closest;
    }
}
