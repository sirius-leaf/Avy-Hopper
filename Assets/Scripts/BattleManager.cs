using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public event Action<bool> OnPlayerTurnChanged;

    private bool _isPlayerTurn;

    public bool IsPlayerTurn
    {
        get => _isPlayerTurn;
        set
        {
            _isPlayerTurn = value;

            OnPlayerTurnChanged?.Invoke(_isPlayerTurn);
        }
    }

    public void ToggleTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;
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
        IsPlayerTurn = true;
    }
}
