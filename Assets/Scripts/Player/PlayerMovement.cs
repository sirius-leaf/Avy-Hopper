using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference move;
    public InputActionReference jump;

    [Header("Movement")]
    public float moveSpeed = 7f;

    [Header("Jump")]
    public int maxJumps = 2;
    public float jumpForce = 12f;
    public float doubleJumpForce = 10f;
    public float jumpCutMultiplier = 0.5f;
    public float minJumpVelocity = 3f;
    private int jumpsRemaining;

    [Header("Ground Check - Raycast")]
    public float rayLength = 0.1f; // Jarak ray ke bawah
    public LayerMask groundLayer;
    public Vector2 rayOffset = new Vector2(0.2f, 0f); // Offset kiri/kanan

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Other")]
    public Vector3 startPos;

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumpReleased;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        jumpsRemaining = maxJumps;
    }

    void OnEnable()
    {
        jump.action.started += OnJumpStart;
        jump.action.canceled += OnJumpStop;

        GameManager.OnPlayerTurn += OnPlayerTurn;
        GameManager.OnEnemyTurn += OnEnemyTurn;
    }

    void OnDisable()
    {
        jump.action.started -= OnJumpStart;
        jump.action.canceled -= OnJumpStop;

        GameManager.OnPlayerTurn -= OnPlayerTurn;
        GameManager.OnEnemyTurn -= OnEnemyTurn;
    }

    void Update()
    {
        if (GameManager.IsPlayerTurn)
        {
            const float MOVE_SMOOTHING = 5;
            transform.position = Vector3.Lerp(transform.position, startPos, Utils.ExpDecayT(MOVE_SMOOTHING));

            return;
        }

        moveInput = move.action.ReadValue<Vector2>();
        wasGrounded = isGrounded;
        isGrounded = IsGrounded();
        if (jumpBufferCounter > 0f) jumpBufferCounter -= Time.deltaTime;

        // Ground Checker
        if (!wasGrounded && isGrounded)
            jumpsRemaining = maxJumps;

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Movement Control
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        Jump();
    }

    void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Vector2 origin = new(transform.position.x, col.bounds.min.y);

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(origin, Vector2.down * rayLength);
        Gizmos.DrawRay(origin - rayOffset, Vector2.down * rayLength);
        Gizmos.DrawRay(origin + rayOffset, Vector2.down * rayLength);
    }

    private bool IsGrounded()
    {
        // Titik awal ray = bawah tengah collider
        Vector2 origin = new(transform.position.x, col.bounds.min.y);

        // 3 ray: tengah, kiri, kanan — lebih akurat di tepi platform
        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin - rayOffset, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + rayOffset, Vector2.down, rayLength, groundLayer);

        return hitCenter.collider != null
	        || hitLeft.collider != null
            || hitRight.collider!= null;
    }

    private void OnPlayerTurn()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }
    
    private void OnEnemyTurn()
    {
        rb.gravityScale = 1f;
        jumpBufferCounter = 0f;
    }

    private void Jump()
    {
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f && jumpsRemaining == maxJumps) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
                jumpsRemaining--;
            }
            else if (jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                jumpsRemaining--;
            }
        }

        if (jumpReleased && rb.linearVelocity.y > minJumpVelocity)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Max(rb.linearVelocity.y * jumpCutMultiplier, minJumpVelocity)
            );
            
        }
        jumpReleased = false;

    }

    private void OnJumpStart(InputAction.CallbackContext obj)
    {
        jumpBufferCounter = jumpBufferTime;
    }

    private void OnJumpStop(InputAction.CallbackContext obj)
    {
        jumpReleased = true;
    }
}
