using UnityEngine;

public class EnemyController : CharacterControllerBase
{
    [SerializeField] private GameObject player;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float punchDuration = 0.6f;
    [SerializeField] private float moveSpeedFactor = 0.5f;

    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        InitializeModel(50, 3, 5, 1);

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player)
            playerTransform = player.transform;
    }

    private void Update()
    {
        if (!model.IsAlive() || isPunching || !playerTransform) return;
        Vector3 toPlayer = playerTransform.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            MoveTowards(toPlayer);
        }
        else
        {
            Attack(playerTransform.GetComponent<CharacterControllerBase>());
        }
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 moveDir = direction.normalized;
        transform.position += moveDir * (model.MoveSpeed * moveSpeedFactor * Time.deltaTime);
        view.FaceDirection(moveDir);
        view.SetEnemyWalking(true);
    }

    public override void Attack(CharacterControllerBase target)
    {
        if (Time.time - lastAttackTime < model.AttackCooldown) return;
        if (target == null) return;

        isPunching = true;
        view.SetEnemyWalking(false);
        view.TriggerPunch();
        lastAttackTime = Time.time;
        Invoke(nameof(EndEnemyPunch), punchDuration);
    }

    private void EndEnemyPunch()
    {
        isPunching = false;
    }
}
