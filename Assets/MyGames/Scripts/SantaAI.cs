using UnityEngine;

public class SantaAI : MonoBehaviour
{
    public enum State { Idle, Walk, Attack, Dead }

    [Header("References")]
    [Tooltip("Player transform. If null, will find GameObject tagged 'Player' at Start.")]
    public Transform player;
    public Animator animator;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waitTimeAtPoint = 1f;

    [Header("Detection & Combat")]
    public float detectionRange = 6f;
    public float attackRange = 1.2f;
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("Stats")]
    public int maxHealth = 3;

    // runtime
    State state = State.Walk;
    int currentPatrol = 0;
    float waitTimer = 0f;
    float attackTimer = 0f;
    int currentHealth;
    Vector3 startScale;

    void Start()
    {
        currentHealth = maxHealth;
        startScale = transform.localScale;

        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.transform;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // if no patrol points supplied, create two points relative to start
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GameObject p1 = new GameObject(name + "_patrol_0");
            GameObject p2 = new GameObject(name + "_patrol_1");
            p1.transform.position = transform.position + Vector3.left * 3f;
            p2.transform.position = transform.position + Vector3.right * 3f;
            p1.transform.parent = transform.parent;
            p2.transform.parent = transform.parent;
            patrolPoints = new Transform[] { p1.transform, p2.transform };
        }

        state = State.Walk; // start by patrolling
    }

    void Update()
    {
        if (state == State.Dead) return;

        attackTimer -= Time.deltaTime;

        float distToPlayer = float.PositiveInfinity;
        if (player) distToPlayer = Vector2.Distance(transform.position, player.position);

        // state transitions
        if (player != null && distToPlayer <= attackRange)
        {
            state = State.Attack;
        }
        else if (player != null && distToPlayer <= detectionRange)
        {
            // chase / engage
            state = State.Walk;
        }
        else
        {
            // if not seeing player, patrol
            state = State.Walk;
        }

        // state behaviors
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Walk:
                UpdateWalk(distToPlayer);
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }

        UpdateAnimator();
    }

    void UpdateIdle()
    {
        // simply wait in place
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            state = State.Walk;
        }
    }

    void UpdateWalk(float distToPlayer)
    {
        // if player in detection range but not attack range -> move toward player
        if (player != null && distToPlayer <= detectionRange && distToPlayer > attackRange)
        {
            MoveTowards(player.position);
        }
        else
        {
            // patrol between points
            if (patrolPoints.Length == 0) return;
            Transform target = patrolPoints[currentPatrol];
            float d = Vector2.Distance(transform.position, target.position);
            if (d <= 0.1f)
            {
                // reached point
                currentPatrol = (currentPatrol + 1) % patrolPoints.Length;
                waitTimer = waitTimeAtPoint;
                state = State.Idle;
                return;
            }
            MoveTowards(target.position);
        }
    }

    void UpdateAttack()
    {
        if (player == null)
        {
            state = State.Walk;
            return;
        }

        // face player
        FaceTowards(player.position);

        // perform attack when cooldown ready
        if (attackTimer <= 0f)
        {
            attackTimer = attackCooldown;
            animator?.SetTrigger("Attack");
            DoDamageToPlayer();
        }

        // if player moves away beyond attackRange, go back to walk
        float d = Vector2.Distance(transform.position, player.position);
        if (d > attackRange + 0.2f)
        {
            state = State.Walk;
        }
    }

    void DoDamageToPlayer()
    {
        if (player == null) return;
        // try to call a TakeDamage method on player
        var target = player.GetComponent<MonoBehaviour>();
        if (target != null)
        {
            player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        dir.z = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        Vector3 move = dir.normalized * speed * Time.deltaTime;
        transform.position += move;
        FaceTowards(target);
    }

    void FaceTowards(Vector3 target)
    {
        float dx = target.x - transform.position.x;
        if (dx > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(startScale.x), startScale.y, startScale.z);
        else if (dx < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(startScale.x), startScale.y, startScale.z);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetBool("Dead", state == State.Dead);
        animator.SetBool("Walking", state == State.Walk);
        // Attack trigger is set when attacking
    }

    public void TakeDamage(int amount)
    {
        if (state == State.Dead) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        state = State.Dead;
        animator?.SetBool("Dead", true);
        // disable collider and other components to stop interactions
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
        // optionally destroy after a delay
        Destroy(gameObject, 3f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
