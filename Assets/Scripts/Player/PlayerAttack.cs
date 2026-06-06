using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Properties")]
    public InputActionReference attack;
    public LayerMask enemyLayer;
    public float chargeDuration = 3;
    public int damage = 10;
    public float weight = 1f;

    [Header("Attack Damage Visual")]
    public Transform leftAttackDamageVisual;
    public Transform rightAttackDamageVisual;
    public AnimationCurve damageMultiplierCurve;
    public float attackDamageVisualDuration = 1.5f;
    private float _attackDamageVisualProgress = 0f;
    private float _weightedAttackDamageVisualProgress = 0f;
    
    public event Action<bool> OnHasHitEnemyChanged;
    private bool _hasHitEnemy;
    public bool HasHitEnemy
    {
        get => _hasHitEnemy;
        private set
        {
            _hasHitEnemy = value;

            OnHasHitEnemyChanged?.Invoke(_hasHitEnemy);
        }
    }

    private EnemyHealth _enemy;

    void Awake()
    {
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
        if (GameManager.IsPlayerTurn) return;
        else GameManager.PlayerAttackCharge += Time.deltaTime / chargeDuration;
        
        if(HasHitEnemy)
        {
            leftAttackDamageVisual.gameObject.SetActive(true);
            rightAttackDamageVisual.gameObject.SetActive(true);
            
            _attackDamageVisualProgress += Time.deltaTime / attackDamageVisualDuration;
            _weightedAttackDamageVisualProgress = Mathf.Pow(_attackDamageVisualProgress, weight);

            if (_attackDamageVisualProgress > 1f) GameManager.IsPlayerTurn = true;
            
            leftAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, _weightedAttackDamageVisualProgress * -135f);
            rightAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, _weightedAttackDamageVisualProgress * 135f);
        }
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
            if (!HasHitEnemy)
            {
                RaycastHit2D hitEnemy = HitEnemy();

                if (hitEnemy.collider != null)
                {
                    _enemy = hitEnemy.collider.gameObject.GetComponent<EnemyHealth>();
                    HasHitEnemy = true;
                }
                else
                {
                    Debug.Log("Miss!");
                    GameManager.IsPlayerTurn = true;
                }
            }
            else
            {
                float damageMultiplier = damageMultiplierCurve.Evaluate(_weightedAttackDamageVisualProgress);
                int damageGiven = Mathf.CeilToInt(damage * damageMultiplier);

                _enemy.TakeDamage(damageGiven);
                Debug.Log(damageGiven);
                GameManager.IsPlayerTurn = true;
            }
        }
    }

    private void OnPlayerTurnChanged(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            leftAttackDamageVisual.gameObject.SetActive(false);
            rightAttackDamageVisual.gameObject.SetActive(false);

            leftAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, 0f);
            rightAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, 0f);

            HasHitEnemy = false;
            _attackDamageVisualProgress = 0f;
            GameManager.PlayerAttackCharge = 0f;
        }
    }
}
