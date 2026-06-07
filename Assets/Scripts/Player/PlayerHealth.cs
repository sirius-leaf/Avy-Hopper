using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]

[RequireComponent(typeof(PlayerAttack))]
public class PlayerHealth : MonoBehaviour
{
    public event Action<int> OnPlayerHealthChanged;
    public event Action OnPlayerDie;
    
    public int maxHealth = 10;
    public float iframeDuration = 1f;

    private SpriteRenderer _sprite;
    private PlayerAttack _playerAttack;
    private float _iframeTimer;
    private int _health = 10;

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Max(0, value);

            OnPlayerHealthChanged?.Invoke(_health);
            if (_health <= 0) OnPlayerDie?.Invoke();
        }
    }

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    void OnEnable()
    {
        _playerAttack.OnHasHitEnemyChanged += OnHasHitEnemyChanged;
        OnPlayerDie += OnDie;
    }
    void OnDisable()
    {
        _playerAttack.OnHasHitEnemyChanged -= OnHasHitEnemyChanged;
        OnPlayerDie -= OnDie;
    }

    void Start() => Health = maxHealth;

    void Update()
    {
        if (_iframeTimer > 0) _iframeTimer -= Time.deltaTime;
        
        _sprite.color = _iframeTimer > 0 ? Color.red : Color.white;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_playerAttack.HasHitEnemy) return;

        switch (collision.tag)
        {
            case "Hazard":
                TakeDamage(1);
                break;
        }
    }

    private void TakeDamage(int amount)
    {
        if (_iframeTimer > 0) return;

        Health -= amount;
        _iframeTimer = iframeDuration;
    }

    private void OnHasHitEnemyChanged(bool hasHitEnemy)
    {
        gameObject.tag = hasHitEnemy ? "IgnoreCollision" : "Player";
    }

    private void OnDie()
    {
        Debug.Log("You Lose!");
    }
}
