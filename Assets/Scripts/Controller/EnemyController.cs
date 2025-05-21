using UnityEngine;

public class EnemyController : CharacterControllerBase
{
    [SerializeField] private GameObject player;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float punchDuration = 0.6f;
    [SerializeField] private float moveSpeedFactor = 0.5f;
    [SerializeField] public int punchDamage = 5;


    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        InitializeModel(100, 3, punchDamage, 1); // máu, tốc độ, sát thương, thời gian hồi chiêu

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player)
            playerTransform = player.transform;
    }

    private void Update()
    {
        if (!model.IsAlive() || isPunching || playerTransform == null) return;

        Vector3 toPlayer = playerTransform.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            MoveTowards(toPlayer);
        }
        else if (Time.time - lastAttackTime >= model.AttackCooldown)
        {
            StartPunch();
        }
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 moveDir = direction.normalized;
        transform.position += moveDir * (model.MoveSpeed * moveSpeedFactor * Time.deltaTime);
        view.FaceDirection(moveDir);
        view.SetEnemyWalking(true);
    }

    private void StartPunch()
    {
        isPunching = true;
        lastAttackTime = Time.time;

        view.SetEnemyWalking(false);
        view.TriggerPunch();

        Invoke(nameof(DealDamageToPlayer), punchDuration * 0.5f); // gây sát thương ở giữa đòn
        Invoke(nameof(EndEnemyPunch), punchDuration);
    }

    private void DealDamageToPlayer()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= stopDistance + 0.2f)
        {
            CharacterControllerBase target = playerTransform.GetComponent<CharacterControllerBase>();
            if (target != null && target != this)
            {
                target.TakeDamage(model.AttackDamage);
                Debug.Log($"Enemy dealt {model.AttackDamage} damage to player.");
                Debug.Log($"Player remaining health: {target.GetHealth()}");
            }
        }
    }

    private void EndEnemyPunch()
    {
        isPunching = false;
    }

    public void OverrideStats(float health, float speed)
    {
        model.Health = health;
        model.MoveSpeed = speed;
    }
}
