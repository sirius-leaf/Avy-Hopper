using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider playerHealthBar;
    public PlayerHealth playerHealth;

    void OnEnable()
    {
        GameManager.OnPlayerHealthChanged += OnPlayerHealthChanged;
    }

    void OnDisable()
    {
        GameManager.OnPlayerHealthChanged -= OnPlayerHealthChanged;
    }

    void Start()
    {
        playerHealthBar.maxValue = playerHealth.maxHealth;
        playerHealthBar.value = GameManager.PlayerHealth;
    }

    private void OnPlayerHealthChanged(int value)
    {
        playerHealthBar.value = value;
    }
}
