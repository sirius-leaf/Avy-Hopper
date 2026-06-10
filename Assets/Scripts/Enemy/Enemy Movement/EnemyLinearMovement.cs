using Unity.VisualScripting;
using UnityEngine;

public class EnemyLinearMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 2f;
    public Vector2 stayDurationRange;
    public Vector2 posXMoveRange;

    private enum MoveState
    {
        STAY,
        MOVE,
    }

    private const float DELTA_POSITION_TOLERANCE = 0.1f;

    private MoveState _currentState;
    private float _stayTimer;
    private float _moveTarget;

    void Awake()
    {
        _currentState = MoveState.STAY;
        SetStayTimer();
    }

    public void Move(Transform t)
    {
        switch (_currentState)
        {
            case MoveState.STAY:
                if (_stayTimer > 0f) _stayTimer -= Time.deltaTime;
                else
                {
                    SetMoveTarget();
                    _currentState = MoveState.MOVE;
                }

                break;
            case MoveState.MOVE:
                if (Mathf.Abs(t.position.x - _moveTarget) > DELTA_POSITION_TOLERANCE)
                    t.position = new Vector3(Mathf.MoveTowards(t.position.x, _moveTarget, moveSpeed * Time.deltaTime), t.position.y, t.position.z);
                else
                {
                    SetStayTimer();
                    _currentState = MoveState.STAY;
                }

                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3((posXMoveRange.x + posXMoveRange.y) / 2f, transform.position.y, 0f);
        Vector3 size = new(
            Mathf.Abs(posXMoveRange.x - posXMoveRange.y),
            0.5f,
            0f
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }

    private void SetStayTimer() => _stayTimer = Random.Range(stayDurationRange.x, stayDurationRange.y);
    private void SetMoveTarget() => _moveTarget = Random.Range(posXMoveRange.x, posXMoveRange.y);
}
