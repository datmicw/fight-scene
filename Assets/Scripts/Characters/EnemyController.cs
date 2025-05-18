using UnityEngine;

public class EnemyController : CharacterBase
{
    [Header("Enemy Settings")]
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float moveSpeedFactor = 0.5f;
    [SerializeField] private GameObject player;

    private Transform playerTransform;
    private bool isPunching;

    public float punchDuration = 0.6f; 

    protected override void Awake()
    {
        base.Awake();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer;
        }

        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (!isAlive || playerTransform == null) return;

        HandleMovementAndPlayerFocus();
    }

    private void HandleMovementAndPlayerFocus()
    {
        if (isPunching) return; //nếu đang đấm thì không di chuyển

        Vector3 toPlayer = playerTransform.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance) // nếu khoảng cách từ enemy đến player lớn hơn stopDistance thì di chuyển đến
        {
            MoveTowards(toPlayer); // focus vào player
        }
        else Attack(playerTransform.GetComponent<CharacterBase>()); // nếu khoảng cách nhỏ hơn stopDistance thì tấn công
    }
// 
    private void MoveTowards(Vector3 direction)
    {
        Vector3 moveDirection = direction.normalized;

        transform.position += moveDirection * (moveSpeed * moveSpeedFactor * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        animator?.SetBool("isEnemyWalking", true);
    }

    public override void Attack(CharacterBase target)
    {
        if (target == null) return;
        TriggerPunch();
    }

    private void TriggerPunch()
    {
        isPunching = true;

        animator.SetBool("isEnemyWalking", false); // dừng animation đi 
        animator.SetTrigger("Punching"); // bật animation đấm

        lastAttackTime = Time.time;

        Invoke(nameof(EndEnemyPunch), punchDuration); 
    }
    // kết thúc animation đấm
    private void EndEnemyPunch()
    {
        isPunching = false;
    }
}
