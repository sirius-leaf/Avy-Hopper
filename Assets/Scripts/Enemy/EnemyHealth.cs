using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
