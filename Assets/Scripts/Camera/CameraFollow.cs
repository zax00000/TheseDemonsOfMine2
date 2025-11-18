using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Offset Settings")]
    public float heightOffset = 5f;
    public float depthOffsetX = -10f;
    public float followSpeed = 5f;
    public float sideLerpSpeed = 3f;

    [Header("Side Offset Z")]
    public float forwardOffsetZ = -2f;  // When moving forward
    public float backwardOffsetZ = 2f;  // When moving backward
    private float currentSideOffsetZ = 0f;

    public bool lookAtTarget = true;

    private Vector3 previousTargetPosition;

    void Start()
    {
        if (target != null)
        {
            previousTargetPosition = target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 🧠 Detect Z movement direction
        float zVelocity = target.position.z - previousTargetPosition.z;
        float targetSideOffsetZ = 0f;

        if (Mathf.Abs(zVelocity) > 0.01f)
        {
            targetSideOffsetZ = zVelocity > 0 ? forwardOffsetZ : backwardOffsetZ;
        }

        // Smoothly interpolate side offset toward target
        currentSideOffsetZ = Mathf.Lerp(currentSideOffsetZ, targetSideOffsetZ, sideLerpSpeed * Time.unscaledDeltaTime);

        // Apply offsets
        Vector3 sideOffset = Vector3.forward * currentSideOffsetZ;
        Vector3 depthOffset = Vector3.right * depthOffsetX;
        Vector3 heightOffsetVec = Vector3.up * heightOffset;

        Vector3 desiredPosition = target.position + sideOffset + depthOffset + heightOffsetVec;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.unscaledDeltaTime);

        if (lookAtTarget)
        {
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }

        previousTargetPosition = target.position;
    }
}
