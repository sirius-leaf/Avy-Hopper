using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Platformer Mode Camera")]
    public bool isPlatformerMode = false;
    public Transform cameraFollowTarget;
    public float cameraFollowSmoothness = 5f;

    [Header("Player Turn Transform")]
    public Vector3 playerTurnPos;
    public float playerTurnSize;
    
    [Header("Enemy Turn Transform")]
    public Vector3 enemyTurnPos;
    public float enemyTurnSize;

    private Camera _cam;

    void Awake()
    {
        if (isPlatformerMode) return;
        
        _cam = GetComponent<Camera>();

        transform.position = playerTurnPos;
        _cam.orthographicSize = playerTurnSize;
    }

    void Update()
    {
        if (isPlatformerMode)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(cameraFollowTarget.position.x, cameraFollowTarget.position.y, -10f), Utils.ExpDecayT(cameraFollowSmoothness));
        }
        else
        {
            bool isPlayerTurn = BattleManager.Instance.CurrentBattleState == BattleManager.BattleState.PLAYER_TURN;
            Vector3 currentTurnPos = isPlayerTurn ? playerTurnPos : enemyTurnPos;
            float currentTurnSize = isPlayerTurn ? playerTurnSize : enemyTurnSize;

            float expDecayValue = Utils.ExpDecayT(5f);
            transform.position = Vector3.Lerp(transform.position, currentTurnPos, expDecayValue);
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, currentTurnSize, expDecayValue);
        }
    }
}
