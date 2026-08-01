using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("DangSon/PlayerController")]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    [Tooltip("Smaller = snappier. Typical: 0.02 - 0.1")]
    public float movementSmoothing = 0.05f;

    [Header("Jump / Ground")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float jumpForce = 7f;
    public float radius = 0.2f;

    private Rigidbody2D rb;
    private Animator anim;
    private int isWalkId;
    private int isJumpId;

    bool facingRight = true;

    // input captured in Update, applied in FixedUpdate
    private float horizontalInput;
    private bool jumpRequested;
    
    // used by SmoothDamp
    private Vector2 velocityRef = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        isWalkId = Animator.StringToHash("isWalk");
        isJumpId = Animator.StringToHash("isJump");
      
    }

    void Update()
    {
        // capture raw input for crisp direction (smoothing happens on physics side)
       // horizontalInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = ReadHorizontalInput();

       /* if (Input.GetButtonDown("Jump"))
        {
            // request jump, actual physics applied in FixedUpdate
            jumpRequested = true;
        }
       */ 
        if (IsJumPressedThisFrame())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        MovePhysics();
        if (jumpRequested)
        {
            TryJump();
            jumpRequested = false;
        }
    }
    private float ReadHorizontalInput()
    {
        float keyboardDir = 0f;
        var kb = Keyboard.current;
        if (kb != null)
        {
         if(kb.leftArrowKey.isPressed||kb.aKey.isPressed)
            {
                keyboardDir -= 1f;
            }
            if(kb.rightArrowKey.isPressed||kb.dKey.isPressed)
            {
                keyboardDir += 1f;
            }
        }
        //
        var gp = Gamepad.current;
        if (gp != null)
        {
        float stickX = gp.leftStick.ReadValue().x;
            if(Mathf.Abs(stickX) > 0.1f) // deadzone
                      return stickX;
        }
        return keyboardDir;
    }
    private bool IsJumPressedThisFrame()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.spaceKey.wasPressedThisFrame)
                return true;
        }
        var gp = Gamepad.current;
        if (gp != null)
        {
            if (gp.buttonSouth.wasPressedThisFrame) // A button
                return true;
        }
        return false;
    }


    private void MovePhysics()
    {
        // target velocity based on input
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // smooth velocity change for nicer movement
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocityRef, movementSmoothing);

        // flip sprite based on input direction
        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();

        // driving animation: use actual body velocity for consistent animation state
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            anim.SetBool(isWalkId, true);
        else
            anim.SetBool(isWalkId, false);
    }

    private void TryJump()
    {
        if (IsGrounded())
        {
            // set vertical velocity directly for an instant jump feel
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger(isJumpId);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
       // spriteRenderer.flipX = !facingRight;
    }

    bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
    public bool GetFaceRight()
    {
        return facingRight;
    }
}
