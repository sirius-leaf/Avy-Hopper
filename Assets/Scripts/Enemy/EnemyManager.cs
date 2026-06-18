using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EncounterData encounterData;
    [SerializeField] private GameObject[] enemies = new GameObject[3];

    private UiManager _ui;

    void Awake()
    {
        _ui = FindFirstObjectByType<UiManager>();

        if (encounterData.enemyPrefabs.Any(item => item != null))
        {
            enemies = encounterData.enemyPrefabs;
            encounterData.ClearData();
        }

        int enemyPos = 0;
        for (int i = 0; i < 3; i++)
        {
            if (enemies[i] == null) continue;

            GameObject enemy = Instantiate(enemies[i], new Vector3(enemyPos * 3f, 0f, 0f), Quaternion.identity, transform);
            _ui.enemyHealth[i] = enemy.GetComponent<EnemyHealth>();
            
            enemyPos++;
        }

        transform.position = new Vector3((2 - (transform.childCount - 1)) * 1.5f, -2f, 0f);
    }
}
