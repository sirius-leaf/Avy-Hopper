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

    [Header("Double Jump")]
    public int maxJumps = 2;
    public float jumpForce = 12f;
    public float doubleJumpForce = 10f; // Bisa lebih kecil dari jump pertama
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

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool wasGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        jump.action.started += Jump;
    }

    void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();
        wasGrounded = isGrounded;
        isGrounded = IsGrounded();
        jumpBufferCounter -= Time.deltaTime;

        if (!wasGrounded && isGrounded)
            jumpsRemaining = maxJumps;

        // if (isGrounded && jumpsRemaining == 0)
        //     jumpsRemaining = maxJumps;

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

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
    }

    bool IsGrounded()
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

    // Visualisasi ray di Scene view
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

    private void Jump(InputAction.CallbackContext obj)
    {
        jumpBufferCounter = jumpBufferTime;
    }
}
