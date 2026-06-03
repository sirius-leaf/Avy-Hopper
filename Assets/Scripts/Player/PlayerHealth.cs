using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float iframeDuration = 1f;

    private SpriteRenderer sprite;
    private float iframeTimer;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable() => GameManager.OnPlayerDie += OnPlayerDie;
    void OnDisable() => GameManager.OnPlayerDie -= OnPlayerDie;

    void Start() => GameManager.PlayerHealth = maxHealth - 5;

    void Update()
    {
        if (iframeTimer > 0) iframeTimer -= Time.deltaTime;
        
        sprite.color = iframeTimer > 0 ? Color.red : Color.white;
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
        if (iframeTimer > 0) return;

        GameManager.PlayerHealth -= amount;
        iframeTimer = iframeDuration;
    }

    private void OnPlayerDie()
    {
        Debug.Log("You Lose!");
    }
}
