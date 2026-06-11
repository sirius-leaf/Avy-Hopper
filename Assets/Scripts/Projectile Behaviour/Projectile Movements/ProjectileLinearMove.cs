using UnityEngine;

public class ProjectileLinearMove : MonoBehaviour, IProjectileMovement
{
    public float MoveSpeed { get; set; } = 10f;
    public float direction = 0f;

    public void Move(Transform t)
    {
        float radDir = direction * Mathf.Deg2Rad;
        Vector2 dirVector = new(Mathf.Cos(radDir), Mathf.Sin(radDir));

        t.Translate(MoveSpeed * Time.deltaTime * dirVector);
    }
}
