using UnityEngine;

public class FearHitbox : MonoBehaviour
{
    [Header("Hitboxes")]
    [SerializeField] private Collider hitbox;
    [SerializeField] private Collider hitbox2;

    [Header("Damage Settings")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage = 50f;

    public bool dead = false;

    private void Start()
    {
        if (hitbox != null)
        {
            hitbox.isTrigger = true;
            hitbox.enabled = false;
        }

        if (hitbox2 != null)
        {
            hitbox2.isTrigger = true;
            hitbox2.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead) return;
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Sword sword = player.GetComponent<Sword>();

            if (sword != null && sword.isParrying)
            {
                FOFF();
                sword.ParryTime();
                return;
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Collision();
            }
        }
    }

    public void FON()
    {
        if (hitbox != null) hitbox.enabled = true;
        if (hitbox2 != null) hitbox2.enabled = true;
    }

    public void FOFF()
    {
        if (hitbox != null) hitbox.enabled = false;
        if (hitbox2 != null) hitbox2.enabled = false;
    }

    private void Collision()
    {
        if (hitbox != null) hitbox.enabled = false;
        if (hitbox2 != null) hitbox2.enabled = false;
    }
}
