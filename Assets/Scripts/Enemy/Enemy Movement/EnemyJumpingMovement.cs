using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyJumpingMovement : MonoBehaviour, IEnemyMovement
{
    public float jumpingDuration = 1f;
    public float jumpHeight = 3f;

    private EnemyController _enemyController;
    private BoxCollider2D _collider;
    private Vector3 _lastPos;
    private float _jumpProgress = 0f;
    private bool _hasSaveLastPos = false;

    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _collider = GetComponent<BoxCollider2D>();
    }
    
    public void Move(Transform t)
    {
        if (!_hasSaveLastPos)
        {
            _lastPos = t.position;
            _hasSaveLastPos = true;
        }

        if (_jumpProgress < 1f)
        {
            _collider.enabled = false;
            _jumpProgress = Mathf.Min(_jumpProgress + Time.deltaTime * (1f / jumpingDuration), 1f);

            t.position = new Vector3(Mathf.Lerp(_lastPos.x, _enemyController.MoveTarget, _jumpProgress), _lastPos.y + Mathf.Sin(_jumpProgress * Mathf.PI) * jumpHeight, 0f);
        }
        else
        {
            _jumpProgress = 0f;
            _hasSaveLastPos = false;
            _collider.enabled = true;
            _enemyController.SetStayState();
        }
    }
}
