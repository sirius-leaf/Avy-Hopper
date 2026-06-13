using UnityEngine;

public class HazardObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Utils.RestartCurrentScene();
    }
}
