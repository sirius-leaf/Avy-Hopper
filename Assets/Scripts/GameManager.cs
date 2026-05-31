using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnPlayerTurn;
    public static event Action OnEnemyTurn;

    private static bool _isPlayerTurn;

    public static bool IsPlayerTurn
    {
        get => _isPlayerTurn;
        set
        {
            _isPlayerTurn = value;

            if (value) OnPlayerTurn?.Invoke();
            else OnEnemyTurn?.Invoke();
        }
    }

    public void ToggleTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;

        Debug.Log(IsPlayerTurn);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
