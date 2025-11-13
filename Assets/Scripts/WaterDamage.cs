using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    private Collider Collider;

    public LayerMask targetLayer;

    void Start()
    {
        Collider = GetComponent<Collider>();
        Collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                
                playerHealth.Fall();
            }
        }
    }
}
