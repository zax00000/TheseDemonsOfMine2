using System.Collections;
using UnityEngine;

public class StressCollision : MonoBehaviour
{
    public Collider LTrigger;
    public Collider RTrigger;

    private StressConnection stressConnection;

    public LayerMask targetLayer;

    private float damage = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LTrigger.enabled = false;
        RTrigger.enabled = false;

        stressConnection = GetComponentInParent<StressConnection>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Sword sword = player.GetComponent<Sword>();

            if (sword != null && sword.isParrying)
            {
                    A1END();
                    A2END();
                    A3END();
                    sword.ParryTime();
                    return;
            }

            else
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }

    public void A1()
    {
        damage = 10;
        RTrigger.enabled = true;
    }

    public void A1END()
    {
        RTrigger.enabled = false;
    }

    public void A2()
    {
        damage = 15;
        LTrigger.enabled = true;
    }

    public void A2END()
    {
        LTrigger.enabled = false;
    }

    public void A3()
    {
        damage = 20;
        RTrigger.enabled = true;
        LTrigger.enabled = true;
    }

    public void A3END()
    {
        RTrigger.enabled = false;
        LTrigger.enabled = false;
    }
}
