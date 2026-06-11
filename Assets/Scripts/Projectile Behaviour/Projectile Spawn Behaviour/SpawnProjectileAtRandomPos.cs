using UnityEngine;

public class SpawnProjectileAtRandomPos : MonoBehaviour, IProjectileSpawnBehaviour
{
    [Header("Spawn Pos Random Range")]
    public Vector2 minSpawnPos;
    public Vector2 maxSpawnPos;

    [Header("Spawn Pos Center")]
    public Vector2 offset;
    public Transform spawnPosCenter; // null = global position

    [Header("Override Projectile Movement")]
    public bool isOverrideProjectileMovement = false;
    public float projectileAngle = 0f;
    public float moveSpeed = 10f;

    public void Spawn(GameObject prefab)
    {
        Vector3 finalSpawnPos = new(Random.Range(minSpawnPos.x, maxSpawnPos.x), Random.Range(minSpawnPos.y, maxSpawnPos.y), 0f);
        finalSpawnPos += (spawnPosCenter != null ? spawnPosCenter.position : Vector3.zero) + (Vector3)offset;

        GameObject projectile = Instantiate(prefab, finalSpawnPos, Quaternion.identity);
        if (isOverrideProjectileMovement)
        {
            projectile.transform.eulerAngles = new Vector3(0f, 0f, projectileAngle);
            projectile.GetComponent<IProjectileMovement>().MoveSpeed = moveSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = (spawnPosCenter != null ? spawnPosCenter.position : (minSpawnPos + maxSpawnPos) / 2f) + (Vector3)offset;
        Vector3 size = new(
            Mathf.Abs(minSpawnPos.x - maxSpawnPos.x),
            Mathf.Abs(minSpawnPos.y - maxSpawnPos.y),
            0f
        );

        Gizmos.color = Color.green;
        if (size != Vector3.zero) Gizmos.DrawWireCube(center, size);
        else Gizmos.DrawSphere(center, 0.2f);
    }
}
