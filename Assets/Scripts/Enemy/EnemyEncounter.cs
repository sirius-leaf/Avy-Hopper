using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class EnemyEncounter : MonoBehaviour
{
    public EncounterData encounterData;
    public SceneReference battleScene;
    public GameObject[] enemies = new GameObject[3];

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            encounterData.enemyPrefabs = enemies;

            SceneManager.LoadScene(battleScene.BuildIndex);
        }
    }
}
