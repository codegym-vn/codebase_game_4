using System;
using UnityEngine;
[AddComponentMenu("DangSon/Enemy")]
public class Enemy : MonoBehaviour, ICanTakeDamage
{
    [Header("Health Settings")]
    public int maxHealth =100;
    public int currentHealth;
    public float gravityValue = 1.0f;
    public float timeToDestroy = 2.0f;
    private bool isDead = false;
    [Header("Animation")]
    private Animator anim;
    private int isDeadId;
    private EnemyAI enemyAI;
    private BossAI bossAI;
    private Collider2D col;
    private Rigidbody2D rb;

    public float attackRate = 1.0f;
    public float nextAttack = 0.0f;

    public GameObject fxPrefabs;
    public Transform positionFx;
    public int damage = 20;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        isDeadId = Animator.StringToHash("Isdead");
        enemyAI = GetComponent<EnemyAI>();
        bossAI = GetComponent<BossAI>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();   
       
    }
    public void TakeDamage(int damageAmount, Vector2 force, GameObject instigator)
    {
        if(isDead) return;
        currentHealth -= damageAmount;
        
        if (currentHealth <=0)
        {
            isDead = true;
            DeadEnemy();
        }
    }
    private void DeadEnemy()
    {
        Debug.Log("Enemy Dead");
        anim.SetTrigger(isDeadId);
        if(enemyAI != null) 
           enemyAI.enabled = false;
        if (bossAI != null)
            bossAI.enabled = false;
        col.enabled = false;
        rb.gravityScale = gravityValue;
        Destroy(gameObject,timeToDestroy);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDead) return;  
        Player player = collision.GetComponent<Player>();
        if (player!=null && player.GetIdead() == false)
        {
            Instantiate(fxPrefabs, positionFx.position, Quaternion.identity);
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + attackRate;
                
                ICanTakeDamage playerDamage = collision.GetComponent<ICanTakeDamage>();
                playerDamage.TakeDamage(damage, Vector2.zero, gameObject);
            }
        }
    }
    public int GetHealth()
    {
        return currentHealth;
    }
}
