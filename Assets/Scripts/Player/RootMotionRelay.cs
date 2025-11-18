using UnityEngine;

public class RootMotionRelay : MonoBehaviour
{
    public Sword sword;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        if (sword == null)
            sword = GetComponentInParent<Sword>();

        rb = sword?.GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        if (sword != null && sword.ShouldApplyRootMotion() && animator != null && rb != null)
        {
            Vector3 deltaPosition = animator.deltaPosition;
            Quaternion deltaRotation = animator.deltaRotation;

            Vector3 velocity = deltaPosition / Time.deltaTime;

            // ✅ Correct gravity preservation
            velocity.y = rb.linearVelocity.y;

            sword.ApplyRootMotionFromChild(velocity * Time.deltaTime, deltaRotation);
        }
    }
}
