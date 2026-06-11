using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum MoveState
    {
        STAY,
        MOVE,
    }

    public Vector2 stayDurationRange = new(0.5f, 2f);
    public Vector2 posXMoveRange = new(-11f, 11f);

    public MoveState CurrentState { get; private set; }
    public float MoveTarget { get; private set; }
    
    private IEnemyMovement _movement;
    private Vector3 _startPos;
    private float _stayTimer;
    
    void Awake()
    {
        _movement = GetComponent<IEnemyMovement>();
        SetStayState();
    }

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.PLAYER_TURN)
            transform.position = Vector3.Lerp(transform.position, _startPos, Utils.ExpDecayT(5f));
        else if (BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.ENEMY_TURN)
            Move();
        else return;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = new((posXMoveRange.x + posXMoveRange.y) / 2f, transform.position.y, 0f);
        Vector3 size = new(
            Mathf.Abs(posXMoveRange.x - posXMoveRange.y),
            0.5f,
            0f
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }

    public void SetStayState()
    {
        _stayTimer = Random.Range(stayDurationRange.x, stayDurationRange.y);
        CurrentState = MoveState.STAY;
    }
    public void SetMoveTarget() => MoveTarget = Random.Range(posXMoveRange.x, posXMoveRange.y);

    private void Move()
    {
        switch (CurrentState)
        {
            case MoveState.STAY:
                if (_stayTimer > 0f) _stayTimer -= Time.deltaTime;
                else
                {
                    SetMoveTarget();
                    CurrentState = MoveState.MOVE;
                }

                break;
            case MoveState.MOVE:
                _movement?.Move(transform);

                break;
        }
    }
}

public interface IEnemyMovement
{
    void Move(Transform projectileTransform);
}
