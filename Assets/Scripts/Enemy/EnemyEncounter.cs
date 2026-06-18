using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private string enemyId;
    [SerializeField] private EncounterData encounterData;
    [SerializeField] private SceneReference battleScene;
    [SerializeField] private GameObject[] enemies = new GameObject[3];

    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;

        if (GameManager.Instance.HasDefeatEnemy(enemyId))
        {
            Debug.Log(GameManager.Instance.HasDefeatEnemy(enemyId));
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            encounterData.enemyPrefabs = enemies;
            GameManager.Instance.lastEnemyEncounteredId = enemyId;
            GameManager.Instance.lastExplorePosition = _playerTransform.position;
            GameManager.Instance.lastExploreSceneId = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(battleScene.BuildIndex);
        }
    }
}
