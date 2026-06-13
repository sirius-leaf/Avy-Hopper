using UnityEngine;

public class ProjectlleHomingMove : MonoBehaviour, IProjectileMovement
{
    public float stayTime = 0f;
    public float stayRotateSpeed = -1f;
    public float chargeTime = 0f;
    public bool isContinousFollow = true;
    public float MoveSpeed { get; set; } = 10f;

    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }
    public void Move(Transform t)
    {
        if (stayTime > 0f)
        {
            if (stayRotateSpeed < 0)
            {
                LookAtPlayer(t);
            }
            else
                RotateTowardPlayer(t, stayRotateSpeed);

            stayTime -= Time.deltaTime;
        } else if (chargeTime > 0f)
            chargeTime -= Time.deltaTime;
        else
        {
            t.Translate(MoveSpeed * Time.deltaTime * Vector3.right);
        } 
    }

    private void LookAtPlayer(Transform t)
    {
        Vector3 direction = _playerTransform.position - t.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        t.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private void RotateTowardPlayer(Transform t, float rotateSpeed)
    {
        Vector3 direction = _playerTransform.position - t.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        t.rotation = Quaternion.RotateTowards(t.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
