using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float lifetime = 5f;

    private IProjectileMovement _movement;
    private IProjectileHitEffect _hitEffect;

    void Awake()
    {
        _movement = GetComponent<IProjectileMovement>();
        _hitEffect = GetComponent<IProjectileHitEffect>();
    }

    void OnEnable()
    {
        BattleManager.Instance.OnCurrentBattleStateChanged += OnCurrentBattleStateChanged;
    }

    void OnDisable()
    {
        BattleManager.Instance.OnCurrentBattleStateChanged -= OnCurrentBattleStateChanged;
    }

    void Start() => Destroy(gameObject, lifetime);

    void Update() => _movement?.Move(transform);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("IgnoreCollision")) return;
        
        _hitEffect?.OnHit(other);
        Destroy(gameObject);
    }

    private void OnCurrentBattleStateChanged(BattleManager.BattleState battleState)
    {
        if (battleState != BattleManager.BattleState.ENEMY_TURN) Destroy(gameObject);
    }
}

public interface IProjectileMovement
{
    float MoveSpeed { get; set; }
    
    void Move(Transform projectileTransform);
}

public interface IProjectileHitEffect
{
    void OnHit(Collider2D other);
}
