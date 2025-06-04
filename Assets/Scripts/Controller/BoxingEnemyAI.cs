using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BoxingEnemyAI : CharacterControllerBase
{
    private Transform player;
    private Rigidbody rb;
    [Header("Boxing Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float moveSpeed = 2f;
    private bool hasDealtDamageThisPunch = false;
    public System.Action onDeath;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        view = GetComponent<CharacterView>();
        InitializeModel(100, 5, 5, 1);
    }
    private IEnumerator Start()
    {
        while (player == null)
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.Player != null)
            {
                player = PlayerManager.Instance.Player.transform;
                Debug.Log("Player assigned to Enemy in Start: " + player.name);
            }
            yield return null;
        }
    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Enemy has no target player!");
            return;
        }
        if (!IsAlive())
        {
            Debug.Log("Enemy is dead, no action.");
            return;
        }
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;
        view.FaceDirection(direction);
        if (distance > attackRange)
        {
            if (!isPunching)
            {
                MoveTowards(direction.normalized);
                view.SetWalking(true);
            }
            else
            {
                view.SetWalking(false);
            }
        }
        else
        {
            view.SetWalking(false);
            TryAttack();
        }
    }
    private void MoveTowards(Vector3 direction)
    {
        Vector3 move = direction * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + move;
        rb.MovePosition(newPosition);
    }
    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= model.AttackCooldown && !isPunching)
        {
            lastAttackTime = Time.time;
            isPunching = true;
            hasDealtDamageThisPunch = false;
            view.TriggerPunch();
            Invoke(nameof(EndPunch), 0.6f);
        }
    }
    private void EndPunch()
    {
        isPunching = false;
        hasDealtDamageThisPunch = false;
    }
    private void DealDamage()
    {
        if (!isPunching || hasDealtDamageThisPunch) return;
        hasDealtDamageThisPunch = true;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 toPlayer = (hit.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, toPlayer) > 0.5f)
                {
                    var playerCtrl = hit.GetComponent<CharacterControllerBase>();
                    if (playerCtrl != null && playerCtrl != this && playerCtrl.IsAlive())
                    {
                        playerCtrl.TakeDamage(model.AttackDamage);
                    }
                }
            }
        }
    }
    protected override void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }
    public void SetTarget(Transform target)
    {
        player = target;
        Debug.Log("Enemy assigned target via GameManager: " + (target != null ? target.name : "null"));
    }
}
