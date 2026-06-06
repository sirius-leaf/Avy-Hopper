using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider playerHealthBar;
    public Slider playerChargeBar;
    public PlayerHealth playerHealth;

    void OnEnable()
    {
        GameManager.OnPlayerHealthChanged += OnPlayerHealthChanged;
        GameManager.OnPlayerAttackChargeChanged += OnPlayerAttackChargeChanged;
    }

    void OnDisable()
    {
        GameManager.OnPlayerHealthChanged -= OnPlayerHealthChanged;
        GameManager.OnPlayerAttackChargeChanged -= OnPlayerAttackChargeChanged;
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
    
    private void OnPlayerAttackChargeChanged(float value)
    {
        playerChargeBar.value = value;
    }
}
