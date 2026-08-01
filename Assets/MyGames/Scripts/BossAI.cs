using System;
using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    
    public enum BossState
    {
        Idle,
        Chase,
        Attack,
        Dash,
        Rage,
        Dead
    }
    [Header("Player Target")]
    public Transform player;

    [Header("Stats")]
    public float speed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    [Header("Attack Settings")]
    public int attackDamage = 20;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime = 0.3f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 5f;
    public float lastDashTime;

    [Header("Rage Settings")]
    public float rangeMultiplier = 1.5f;
    private bool isRage = false;

    [Header("Animator")]

    private BossState currentState;
    private Vector2 dashDirection;
    //
   
    [Header("Animation")]
    private Animator animator;
    bool isDashing=false;
    private Rigidbody2D rb;
    private Enemy enemy;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = BossState.Idle;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemy= GetComponent<Enemy>();
    }
    private void FixedUpdate()
    {
        if (currentState == BossState.Dead || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if(!isRage && enemy.currentHealth <= enemy.maxHealth / 2)
        {
            EnterRage();
        }
        switch (currentState)
        {
            case BossState.Idle:
                Idle();
                if (distance <= chaseRange)
                    ChangeSate(BossState.Chase);
                break;
            case BossState.Chase:
                chase();
                if (distance <= attackRange)
                {
                    ChangeSate(BossState.Attack);
                }
                if (distance > chaseRange)
                {
                    ChangeSate(BossState.Idle);
                }
                if (Time.time>lastDashTime+dashCooldown)
                {
                    ChangeSate(BossState.Dash);
                }
                break;
            case BossState.Attack:
                Attack(distance);
                break;
             case BossState.Dash:
                 if(!isDashing)
                    StartCoroutine(DoDash());
                break;
             case BossState.Rage: 
                RageBehavior(distance);
                break;
        }

    }

    private void Idle()
    {
       animator?.SetBool("isMoving", false);    
    }

    private void RageBehavior(float distance)
    {
        chase();
        if(Time.time > lastDashTime + dashCooldown/2)
        {
            ChangeSate(BossState.Dash);
        }
         if (distance <= attackRange)
                {
                    ChangeSate(BossState.Attack);
                }
    }

    private void EnterRage()
    {
        isRage = true;
        speed*= rangeMultiplier;
        animator?.SetTrigger("Rage");
        ChangeSate(BossState.Rage);
    }

    private void ChangeSate(BossState rage)
    {
        currentState = rage;
    }
    void chase()
    {
        animator?.SetBool("isMoving", true);
        Vector2 dir = (player.position - transform.position).normalized;
        //transform.position += (Vector3)( dir * speed * Time.deltaTime);
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        Flip(dir.x);
    }

    private void Flip(float x)
    {
       if(x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Attack(float distance)
    {
        //Debug.Log("Boss is attacking!");
        animator?.SetBool("isMoving",false);
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            // Attack logic here (e.g., reduce player health)
            animator?.SetTrigger("Attack");

           // Debug.Log("Boss attacks for " + attackDamage + " damage!");
            lastAttackTime = Time.time;
        }
        if (distance > attackRange)
        {
        ChangeSate(BossState.Chase);
        }
    }
    IEnumerator DoDash()
    {
        isDashing = true;
        ChangeSate(BossState.Dash);
        animator?.SetTrigger("Dash");
        dashDirection = new Vector2((player.position.x - transform.position.x),0).normalized;
        float timer = 0f;
        while (timer < dashTime)
        {
           // transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            timer +=Time.deltaTime;
            yield return null;
        }
        lastDashTime = Time.time;
        isDashing = false;
        ChangeSate (BossState.Chase);
    }
}
