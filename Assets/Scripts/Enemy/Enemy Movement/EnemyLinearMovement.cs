using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyLinearMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 2f;

    private const float DELTA_POSITION_TOLERANCE = 0.1f;

    private EnemyController _enemyController;

    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void Move(Transform t)
    {
        if (Mathf.Abs(t.position.x - _enemyController.MoveTarget) > DELTA_POSITION_TOLERANCE)
            t.position = new Vector3(Mathf.MoveTowards(t.position.x, _enemyController.MoveTarget, moveSpeed * Time.deltaTime), t.position.y, t.position.z);
        else
        {
            _enemyController.SetStayState();
        }
    }
}
