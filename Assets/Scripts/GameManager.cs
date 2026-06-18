using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Last Exploration Data")]
    public int lastExploreSceneId;
    public string lastEnemyEncounteredId;
    public Vector3 lastExplorePosition;

    private HashSet<string> _defeatedEnemyIds = new();

    public void RegisterEnemyDefeat(string enemyId)
    {
        if (!_defeatedEnemyIds.Contains(enemyId))
            _defeatedEnemyIds.Add(enemyId);
    }

    public bool HasDefeatEnemy(string enemyId)
    {
        return _defeatedEnemyIds.Contains(enemyId);
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
