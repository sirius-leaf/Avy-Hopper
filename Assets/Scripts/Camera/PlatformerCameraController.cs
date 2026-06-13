using UnityEngine;

public class PlatformerCameraController : MonoBehaviour
{
    [Header("Basic Config")]
    public Transform cameraFollowTarget;

    [Header("Dynamic Offset")]
    public bool useDynamicOffset = true;
    public Rigidbody2D playerRigidbody;
    public Vector2 offsetSensitivity = new(1f, 2f);

    void LateUpdate()
    {
        Vector3 dynamicOffset = useDynamicOffset
            ? new(playerRigidbody.linearVelocityX / offsetSensitivity.x, playerRigidbody.linearVelocityY / offsetSensitivity.y, 0)
            : new(0f, 0f, 0f);
        Vector3 cameraTargetPosition = cameraFollowTarget.position + dynamicOffset;

        transform.position = new Vector3(cameraTargetPosition.x,cameraTargetPosition.y, 0f);
    }
}
