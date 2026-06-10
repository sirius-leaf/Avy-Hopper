using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAttack : MonoBehaviour
{
    public event Action<float> OnPlayerAttackChargeChanged;

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
    
    private EnemyHealth _enemy;
    private float _playerAttackCharge;

    public float PlayerAttackCharge
    {
        get => _playerAttackCharge;
        set
        {
            _playerAttackCharge = Mathf.Clamp(value, 0f, 1f);

            OnPlayerAttackChargeChanged?.Invoke(_playerAttackCharge);
        }
    }

    void OnEnable()
    {
        attack.action.started += OnAttackStarted;
        BattleManager.Instance.OnCurrentBattleStateChanged += OnCurrentBattleStateChanged;
    }

    void OnDisable()
    {
        attack.action.started -= OnAttackStarted;
        BattleManager.Instance.OnCurrentBattleStateChanged -= OnCurrentBattleStateChanged;
    }

    void Update()
    {
        if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.ENEMY_TURN)
            PlayerAttackCharge += Time.deltaTime / chargeDuration;
        else if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.PLAYER_ATTACK)
        {
            _attackDamageVisualProgress += Time.deltaTime / attackDamageVisualDuration;
            _weightedAttackDamageVisualProgress = Mathf.Pow(_attackDamageVisualProgress, weight);
            
            leftAttackDamageVisual.gameObject.SetActive(true);
            rightAttackDamageVisual.gameObject.SetActive(true);
            

            if (_attackDamageVisualProgress > 1f) BattleManager.Instance.CurrentBattleState = BattleManager.BattleState.PLAYER_TURN;
            
            leftAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, _weightedAttackDamageVisualProgress * -135f);
            rightAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, _weightedAttackDamageVisualProgress * 135f);
        }
    }

    private void OnAttackStarted(InputAction.CallbackContext obj)
    {
        if (PlayerAttackCharge >= 1f)
        {
            if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.ENEMY_TURN)
            {
                Vector2 origin = new(transform.position.x, -5f);
                RaycastHit2D hitEnemy = Physics2D.Raycast(origin, Vector2.up, 10f, enemyLayer);

                if (hitEnemy.collider != null)
                {
                    _enemy = hitEnemy.collider.gameObject.GetComponent<EnemyHealth>();
                    BattleManager.Instance.CurrentBattleState = BattleManager.BattleState.PLAYER_ATTACK;
                }
                else
                {
                    Debug.Log("Miss!");
                    BattleManager.Instance.CurrentBattleState = BattleManager.BattleState.PLAYER_TURN;
                }
            }
            else if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.PLAYER_ATTACK)
            {
                float damageMultiplier = damageMultiplierCurve.Evaluate(_weightedAttackDamageVisualProgress);
                int damageGiven = Mathf.CeilToInt(damage * damageMultiplier);

                _enemy.TakeDamage(damageGiven);
                Debug.Log(damageGiven);
                BattleManager.Instance.CurrentBattleState = BattleManager.BattleState.PLAYER_TURN;
            }
        }
    }

    private void OnCurrentBattleStateChanged(BattleManager.BattleState battleState)
    {
        if (battleState == BattleManager.BattleState.PLAYER_TURN)
        {
            leftAttackDamageVisual.gameObject.SetActive(false);
            rightAttackDamageVisual.gameObject.SetActive(false);

            leftAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, 0f);
            rightAttackDamageVisual.rotation = Quaternion.Euler(0f, 0f, 0f);

            _attackDamageVisualProgress = 0f;
            PlayerAttackCharge = 0f;
        }
    }
}
