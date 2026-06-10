using UnityEngine;
using System;

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
}
