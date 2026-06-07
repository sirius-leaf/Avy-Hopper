using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Player Turn Transform")]
    public Vector3 playerTurnPos;
    public float playerTurnSize;
    
    [Header("Enemy Turn Transform")]
    public Vector3 enemyTurnPos;
    public float enemyTurnSize;

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        transform.position = playerTurnPos;
        _cam.orthographicSize = playerTurnSize;
    }

    void Update()
    {
        Vector3 currentTurnPos = BattleManager.Instance.IsPlayerTurn ? playerTurnPos : enemyTurnPos;
        float currentTurnSize = BattleManager.Instance.IsPlayerTurn ? playerTurnSize : enemyTurnSize;

        float expDecayValue = Utils.ExpDecayT(5f);
        transform.position = Vector3.Lerp(transform.position, currentTurnPos, expDecayValue);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, currentTurnSize, expDecayValue);
    }
}
