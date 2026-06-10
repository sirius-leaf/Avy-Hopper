using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private IEnemyMovement _movement;
    private Vector3 _startPos;
    
    void Awake()
    {
        _movement = GetComponent<IEnemyMovement>();
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
            _movement?.Move(transform);
        else return;
    }
}

public interface IEnemyMovement
{
    void Move(Transform projectileTransform);
}
