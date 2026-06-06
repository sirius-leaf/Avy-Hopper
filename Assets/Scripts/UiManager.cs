using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHealthBar;
    public Slider playerChargeBar;
    public PlayerHealth playerHealth;

    [Header("Enemy UI")]
    public Slider[] enemyHealthBar = new Slider[3];
    public EnemyHealth[] enemyHealth = new EnemyHealth[3];

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

        for (int i = 0; i < 3; i++)
        {
            if (enemyHealth[i] != null)
            {
                enemyHealthBar[i].maxValue = enemyHealth[i].maxHealth;
                enemyHealthBar[i].value = enemyHealth[i].health;
            }
            else
            {
                enemyHealthBar[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemyHealth[i] == null || !enemyHealthBar[i].gameObject.activeSelf) continue;

            enemyHealthBar[i].value = enemyHealth[i].health;
        }
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
