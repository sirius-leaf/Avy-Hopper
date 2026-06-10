using UnityEngine;

[RequireComponent(typeof(IProjectileHitEffect))]
public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnInterval = 1f;
    public float spawnIntervalRandAdd;

    private float _spawnTimer;
    private float _currentSpawnInterval;

    private IProjectileSpawnBehaviour _projectileSpawnBehaviour;

    void Awake()
    {
        _projectileSpawnBehaviour = GetComponent<IProjectileSpawnBehaviour>();

        GenerateRandomInterval();
    }

    void Update()
    {
        if (BattleManager.Instance.CurrentBattleState != BattleManager.BattleState.ENEMY_TURN) return;

        if (_spawnTimer >= _currentSpawnInterval)
        {
            _projectileSpawnBehaviour.Spawn(projectilePrefab);

            GenerateRandomInterval();
            _spawnTimer = 0f;
        }

        _spawnTimer += Time.deltaTime;
    }

    private void GenerateRandomInterval()
    {
        _currentSpawnInterval =  spawnInterval + Random.Range(0f, spawnIntervalRandAdd);
    }
}

public interface IProjectileSpawnBehaviour
{
    void Spawn(GameObject prefab);
}
