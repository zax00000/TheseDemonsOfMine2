using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FearConnection : MonoBehaviour
{
    private FearDamage fearDamage;

    private FearMovement fearMovement;

    private FearHitbox fearHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        fearDamage = GetComponentInParent<FearDamage>();
        fearMovement = GetComponentInParent<FearMovement>();
        fearHitbox = GetComponentInChildren<FearHitbox>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void F()
    {
        fearHitbox?.FON();
    }

    public void FE()
    {
        fearHitbox?.FOFF();
    }

    public void Death()
    {
        //unlock wall or do nothing
    }

    public void ResetCd()
    { 
    fearDamage?.ResetCooldown();
    }

    public void Track()
    {
        fearMovement?.ResumeTracking();
    }

    public void Step()
    {
        fearMovement?.PlayStepSound();
    }

    public void Attack()
    { 
    fearDamage?.AttackSound();
    }

    public void HBOFF()
    { 
    fearHitbox.dead = true;
    }
}
