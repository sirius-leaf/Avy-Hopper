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
    private int _jumpsRemaining;

    [Header("Ground Check - Raycast")]
    public float rayLength = 0.1f; // Jarak ray ke bawah
    public LayerMask groundLayer;
    public Vector2 rayOffset = new Vector2(0.2f, 0f); // Offset kiri/kanan

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    [Header("Other")]
    public Vector3 startPos;

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 _moveInput;
    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _jumpReleased;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        _jumpsRemaining = maxJumps;
    }

    void OnEnable()
    {
        jump.action.started += OnJumpStart;
        jump.action.canceled += OnJumpStop;

        GameManager.OnPlayerTurnChanged += OnPlayerTurnChanged;
    }

    void OnDisable()
    {
        jump.action.started -= OnJumpStart;
        jump.action.canceled -= OnJumpStop;
    }

    void Update()
    {
        if (GameManager.IsPlayerTurn)
        {
            const float MOVE_SMOOTHING = 5;
            transform.position = Vector3.Lerp(transform.position, startPos, Utils.ExpDecayT(MOVE_SMOOTHING));

            return;
        }

        _moveInput = move.action.ReadValue<Vector2>();
        _wasGrounded = _isGrounded;
        _isGrounded = _IsGrounded();
        if (_jumpBufferCounter > 0f) _jumpBufferCounter -= Time.deltaTime;

        // Ground Checker
        if (!_wasGrounded && _isGrounded)
            _jumpsRemaining = maxJumps;

        if (_isGrounded)
            _coyoteTimeCounter = coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;

        // Movement Control
        rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, rb.linearVelocity.y);

        Jump();
    }

    void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Vector2 origin = new(transform.position.x, col.bounds.min.y);

        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(origin, Vector2.down * rayLength);
        Gizmos.DrawRay(origin - rayOffset, Vector2.down * rayLength);
        Gizmos.DrawRay(origin + rayOffset, Vector2.down * rayLength);
    }

    private bool _IsGrounded()
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

    private void OnPlayerTurnChanged(bool playerTurn)
    {
        if (playerTurn)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 1f;
            _jumpBufferCounter = 0f;
        }
    }

    private void Jump()
    {
        if (_jumpBufferCounter > 0f)
        {
            if (_coyoteTimeCounter > 0f && _jumpsRemaining == maxJumps) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                _jumpBufferCounter = 0f;
                _coyoteTimeCounter = 0f;
                _jumpsRemaining--;
            }
            else if (_jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                _jumpsRemaining--;
            }
        }

        if (_jumpReleased && rb.linearVelocity.y > minJumpVelocity)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Max(rb.linearVelocity.y * jumpCutMultiplier, minJumpVelocity)
            );
            
        }
        _jumpReleased = false;

    }

    private void OnJumpStart(InputAction.CallbackContext obj)
    {
        _jumpBufferCounter = jumpBufferTime;
    }

    private void OnJumpStop(InputAction.CallbackContext obj)
    {
        _jumpReleased = true;
    }
}
