using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float radiusAttack = 0.5f;
    public Transform pointAttack;
    private Animator anim;
    private int attackId;
    private float nextAttack;
    private float attackRate = 0.2f;
    public int damageEnemy = 10;
    public float forceEnemy = 5f;
    //
    public AudioClip attackAudio;
    // Start is called once before the first executzion of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        attackId = Animator.StringToHash("isAttack");
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.R))
        {
            GetKey();
        }
       */ 
        if (IsAttackThisFrame())
        {
            GetKey();
        }

    }
    private bool IsAttackThisFrame()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.rKey.wasPressedThisFrame)
                return true;
        }
        var gp = Gamepad.current;
        if (gp != null)
        {
            if (gp.buttonWest.wasPressedThisFrame) // X button
                return true;
        }
        return false;
    }
    private bool GetKey()
    {
        AudioManager.Instance.PlaySfxPlayer(attackAudio);
        anim.SetTrigger(attackId);
        if(Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackRate;
            Attack();
            return true;
        }
        else
        {
               return false;
        }
    }
    private void Attack()
    {
        
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pointAttack.position, radiusAttack);
        if(hitEnemies.Length > 0)
        {
            foreach (var enemy in hitEnemies)
            {
                ICanTakeDamage canTakeDamage = enemy.GetComponent<ICanTakeDamage>();
                if (canTakeDamage != null)
                {
                    Vector2 force = enemy.transform.position - transform.position;
                    force.Normalize();
                    canTakeDamage.TakeDamage(damageEnemy, force * forceEnemy, gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(pointAttack != null )
           Gizmos.DrawWireSphere(pointAttack.position, radiusAttack);
    }
}
