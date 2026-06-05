using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float iframeDuration = 1f;

    private SpriteRenderer _sprite;
    private float _iframeTimer;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable() => GameManager.OnPlayerDie += OnPlayerDie;
    void OnDisable() => GameManager.OnPlayerDie -= OnPlayerDie;

    void Start() => GameManager.PlayerHealth = maxHealth;

    void Update()
    {
        if (_iframeTimer > 0) _iframeTimer -= Time.deltaTime;
        
        _sprite.color = _iframeTimer > 0 ? Color.red : Color.white;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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

    private void OnPlayerDie()
    {
        Debug.Log("You Lose!");
    }
}
