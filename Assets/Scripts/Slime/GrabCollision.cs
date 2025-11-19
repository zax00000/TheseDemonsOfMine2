using System;
using UnityEngine;

public class GrabCollision : MonoBehaviour
{
    [SerializeField] private Collider grabLCollider;
    [SerializeField] private Collider grabRCollider;
  
    private SlimeConnection slimeConnection;

    public LayerMask targetLayer;

    private bool grab;

    void Start()
    {
        grab = false;
        grabLCollider.enabled = false;
        grabRCollider.enabled = false;

        slimeConnection = GetComponentInParent<SlimeConnection>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (!grab)
            {
                return;
            }
            Sword sword = player.GetComponent<Sword>();
            
            if (sword != null && sword.isParrying && !sword.parryTime)
            {
                    DeactivateGrab();
                    sword.ParryTime();
                    return;
            }
            else
            {
                slimeConnection?.SuccesfulGrab(player);
                return;
            }
        }
    }

    public void ActivateGrab()
    {
        grab = true;
        grabLCollider.enabled = true;
        grabRCollider.enabled = true;
    }

    public void DeactivateGrab()
    {
        grab = false;
        grabLCollider.enabled = false;
        grabRCollider.enabled = false;
    }
}
