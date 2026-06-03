using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Player Health Events
    public static event Action<int> OnPlayerHealthChanged;
    public static event Action OnPlayerDie;


    // Turn Events
    public static event Action OnPlayerTurn;
    public static event Action OnEnemyTurn;

    private static int _playerHealth;
    private static bool _isPlayerTurn;

    public static int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);

            OnPlayerHealthChanged?.Invoke(_playerHealth);
            if (_playerHealth == 0) OnPlayerDie?.Invoke();
        }
    }

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

        // Set Default Value
        IsPlayerTurn = true;
    }
}
