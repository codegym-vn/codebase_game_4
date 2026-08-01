using UnityEngine;
[AddComponentMenu("DangSon/EnemyAI")]
public class EnemyAI : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;
    public float speedMove = 2;
    public float minDistance = 0.2f;

    private Animator anim;
    private Transform Target;
    private Rigidbody2D rb;
    private int isIdleId;

    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        isIdleId = Animator.StringToHash("isIdle");
        Target = pointB;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }
     private void Patrol()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("IdleCat"))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = (Target.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, Target.position) < minDistance)
        {
            anim.SetTrigger(isIdleId);
            Target = Target == pointB ? pointA : pointB;
            
            
          //  Vector2 localScale = transform.localScale;
          //  localScale.x *= -1;
           // transform.localScale = localScale;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        rb.linearVelocity = direction * speedMove;
    }
}
