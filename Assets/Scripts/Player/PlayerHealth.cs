using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

[RequireComponent(typeof(PlayerAttack))]
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float iframeDuration = 1f;

    private SpriteRenderer _sprite;
    private PlayerAttack _playerAttack;
    private float _iframeTimer;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    void OnEnable()
    {
        GameManager.OnPlayerDie += OnPlayerDie;
        _playerAttack.OnHasHitEnemyChanged += OnHasHitEnemyChanged;
    }
    void OnDisable()
    {
        GameManager.OnPlayerDie -= OnPlayerDie;
        _playerAttack.OnHasHitEnemyChanged -= OnHasHitEnemyChanged;
    }

    void Start() => GameManager.PlayerHealth = maxHealth;

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

        GameManager.PlayerHealth -= amount;
        _iframeTimer = iframeDuration;
    }

    private void OnHasHitEnemyChanged(bool hasHitEnemy)
    {
        gameObject.tag = hasHitEnemy ? "IgnoreCollision" : "Player";
    }

    private void OnPlayerDie()
    {
        Debug.Log("You Lose!");
    }
}
