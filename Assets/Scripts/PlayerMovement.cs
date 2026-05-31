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
    public float jumpForce = 12f;

    [Header("Ground Check - Raycast")]
    public float rayLength = 0.1f; // Jarak ray ke bawah
    public LayerMask groundLayer;
    public Vector2 rayOffset = new Vector2(0.2f, 0f); // Offset kiri/kanan

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    private Rigidbody2D rb;
    private InputManager inputActions;
    private Collider2D col;

    private Vector2 moveInput;
    private bool jumpPressed_;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        inputActions = new InputManager();

        // inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        // inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        // inputActions.Player.Jump.performed += ctx => jumpBufferCounter = jumpBufferTime;
    }

    void OnEnable()
    {
        jump.action.started += Jump;
    }

    void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();
        isGrounded = IsGrounded();
        jumpBufferCounter -= Time.deltaTime;

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) {
            Debug.Log("jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
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
