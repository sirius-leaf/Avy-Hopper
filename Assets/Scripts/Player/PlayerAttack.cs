using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerAttack : MonoBehaviour
{
    public InputActionReference attack;
    public LayerMask enemyLayer;
    public float chargeDuration = 3;

    private Collider2D _col;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        attack.action.started += OnAttackStarted;
        GameManager.OnPlayerTurnChanged += OnPlayerTurnChanged;
    }

    void OnDisable()
    {
        attack.action.started -= OnAttackStarted;
        GameManager.OnPlayerTurnChanged -= OnPlayerTurnChanged;
    }

    void Update()
    {
        if (!GameManager.IsPlayerTurn)
            GameManager.PlayerAttackCharge += Time.deltaTime / chargeDuration;
    }

    private RaycastHit2D HitEnemy()
    {
        Vector2 origin = new(transform.position.x, -5f);

        return Physics2D.Raycast(origin, Vector2.up, 10f, enemyLayer);
    }

    private void OnAttackStarted(InputAction.CallbackContext obj)
    {
        if (GameManager.PlayerAttackCharge >= 1f)
        {
            RaycastHit2D hitEnemy = HitEnemy();

            if (hitEnemy.collider != null)
            {
                Debug.Log("Hit!");
            }
            else
            {
                Debug.Log("Miss!");
            }
            
            GameManager.IsPlayerTurn = true;
        }
    }

    private void OnPlayerTurnChanged(bool isPlayerTurn)
    {
        if (isPlayerTurn) GameManager.PlayerAttackCharge = 0f;
    }
}
