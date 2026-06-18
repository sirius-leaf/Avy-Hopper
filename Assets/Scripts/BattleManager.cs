using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public enum BattleState
    {
        PLAYER_TURN = 0,
        ENEMY_TURN = 1,
        PLAYER_ATTACK = 2,
    }

    public event Action<BattleState> OnCurrentBattleStateChanged;

    private BattleState _currentBattleState;
    private UiManager _ui;

    public BattleState CurrentBattleState
    {
        get => _currentBattleState;
        set
        {
            _currentBattleState = value;

            OnCurrentBattleStateChanged?.Invoke(_currentBattleState);
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Set Default Value
        CurrentBattleState = BattleState.PLAYER_TURN;
    }

    void Start()
    {
        _ui = FindFirstObjectByType<UiManager>();
    }

    void Update()
    {
        int nullEnemyCount = _ui.enemyHealth.Count(item => item == null);

        if (nullEnemyCount >= 3)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterEnemyDefeat(GameManager.Instance.lastEnemyEncounteredId);
            }
            
            SceneManager.LoadScene(GameManager.Instance.lastExploreSceneId);
        }
    }
}
