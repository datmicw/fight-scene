using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : CharacterControllerBase
{
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float punchDuration = 0.6f;
    [SerializeField] private float moveSpeedFactor = 0.5f;

    private Transform targetTransform;
    private CharacterControllerBase targetController;

    protected override void Awake()
    {
        base.Awake();
        InitializeModel(100, 3, 5, 1); // health, speed, damage, cooldown
    }

    private void Update()
    {
        if (!model.IsAlive() || isPunching || targetTransform == null || targetController == null || !targetController.IsAlive())
            return;

        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (distance > stopDistance)
        {
            MoveTowardsTarget();
        }
        else if (Time.time - lastAttackTime >= model.AttackCooldown)
        {
            StartPunch();
        }
    }
    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        Vector3 targetPosition = transform.position + direction * model.MoveSpeed * moveSpeedFactor * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, model.MoveSpeed * moveSpeedFactor * Time.deltaTime);

        view.FaceDirection(direction);
        view.SetEnemyWalking(true);
    }

    private void StartPunch()
    {
        isPunching = true;
        lastAttackTime = Time.time;

        view.SetEnemyWalking(false);
        view.TriggerPunch();

        Invoke(nameof(DealDamage), punchDuration * 0.5f);
        Invoke(nameof(EndPunch), punchDuration);
    }

    private void DealDamage()
    {
        if (targetController == null || !targetController.IsAlive()) return;

        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance <= stopDistance + 0.2f)
        {
            targetController.TakeDamage(model.AttackDamage);
            Debug.Log($"Enemy dealt {model.AttackDamage} damage to {targetController.name}");
            Debug.Log($"Target remaining health: {targetController.GetHealth()}");
            WinLoseUI winLoseUI = FindObjectOfType<WinLoseUI>();
            if (!targetController.IsAlive())
            {
                Debug.Log($"{targetController.name} has been defeated!");
                var manager = FindObjectOfType<GameModeManager>();
                if (manager != null)
                    manager.OnPlayerKilled(targetController.gameObject);

                targetTransform = null;
                targetController = null;
            }

        }

    }

    private void EndPunch() => isPunching = false;

    public void SetTarget(Transform target)
    {
        targetTransform = target;
        targetController = target != null ? target.GetComponent<CharacterControllerBase>() : null;
    }
}
