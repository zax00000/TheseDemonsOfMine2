using UnityEngine;

public class PunchCollision : MonoBehaviour
{
    [SerializeField] private Collider punchLTrigger;
    [SerializeField] private Collider punchRTrigger;
    [SerializeField] private Collider punchLCollider;
    [SerializeField] private Collider punchRCollider;

    [SerializeField] private LayerMask targetLayer;

    private SlimeConnection slimeConnection;

    private bool hasHitL = false;
    private bool hasHitR = false;

    private void Start()
    {
        punchLTrigger.enabled = false;
        punchRTrigger.enabled = false;
        punchLCollider.enabled = false;
        punchRCollider.enabled = false;

        slimeConnection = GetComponentInParent<SlimeConnection>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        Sword sword = player.GetComponent<Sword>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // Determine which trigger fired
        bool isLeft = other == punchLTrigger;
        bool isRight = other == punchRTrigger;

        // Prevent duplicate hits
        if ((isLeft && hasHitL) || (isRight && hasHitR)) return;

        if (sword != null && sword.isParrying && !sword.parryTime)
        {
            DeactivateLPunch();
            DeactivateRPunch();
            sword.ParryTime();
            return;
        }

        if (playerHealth != null)
        {
            Debug.Log("WTF");
            playerHealth.TakeDamage(30);

            if (isLeft) hasHitL = true;
            if (isRight) hasHitR = true;
        }
    }

    public void ActivateRPunch()
    {
        punchRCollider.enabled = true;
        punchRTrigger.enabled = true;
        hasHitR = false;
    }

    public void DeactivateRPunch()
    {
        punchRCollider.enabled = false;
        punchRTrigger.enabled = false;
        hasHitR = false;
    }

    public void ActivateLPunch()
    {
        punchLCollider.enabled = true;
        punchLTrigger.enabled = true;
        hasHitL = false;
    }

    public void DeactivateLPunch()
    {
        punchLCollider.enabled = false;
        punchLTrigger.enabled = false;
        hasHitL = false;
    }
}
